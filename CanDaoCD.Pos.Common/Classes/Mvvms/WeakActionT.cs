using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CanDaoCD.Pos.Common.Classes.Mvvms
{
    public class WeakAction<T> : WeakAction
    {
        private Action<T> _staticAction;

        /// <summary>
        /// Gets the name of the method that this WeakAction represents.
        /// </summary>
        public override string MethodName
        {
            get
            {
                if (this._staticAction != null)
                    return this._staticAction.Method.Name;
                return this.Method.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Action's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (this._staticAction == null && this.Reference == null)
                    return false;
                if (this._staticAction == null)
                    return this.Reference.IsAlive;
                if (this.Reference != null)
                    return this.Reference.IsAlive;
                return true;
            }
        }

        /// <summary>Initializes a new instance of the WeakAction class.</summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action<T> action)
            : this(action.Target, action)
        {
        }

        /// <summary>Initializes a new instance of the WeakAction class.</summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(object target, Action<T> action)
        {
            if (action.Method.IsStatic)
            {
                this._staticAction = action;
                if (target == null)
                    return;
                this.Reference = new WeakReference(target);
            }
            else
            {
                this.Method = action.Method;
                this.ActionReference = new WeakReference(action.Target);
                this.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive. The action's parameter is set to default(T).
        /// </summary>
        public new void Execute()
        {
            this.Execute(default(T));
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (this._staticAction != null)
            {
                this._staticAction(parameter);
            }
            else
            {
                if (!this.IsAlive || (!(this.Method != (MethodInfo)null) || this.ActionReference == null))
                    return;
                this.Method.Invoke(this.ActionTarget, new object[1]
        {
          (object) parameter
        });
            }
        }

        /// <summary>
        /// Executes the action with a parameter of type object. This parameter
        /// will be casted to T. This method implements <see cref="M:GalaSoft.MvvmLight.Helpers.IExecuteWithObject.ExecuteWithObject(System.Object)" />
        /// and can be useful if you store multiple WeakAction{T} instances but don't know in advance
        /// what type T represents.
        /// </summary>
        /// <param name="parameter">The parameter that will be passed to the action after
        /// being casted to T.</param>
        public void ExecuteWithObject(object parameter)
        {
            this.Execute((T)parameter);
        }

        /// <summary>
        /// Sets all the actions that this WeakAction contains to null,
        /// which is a signal for containing objects that this WeakAction
        /// should be deleted.
        /// </summary>
        public new void MarkForDeletion()
        {
            this._staticAction = (Action<T>)null;
            base.MarkForDeletion();
        }
    }
}
