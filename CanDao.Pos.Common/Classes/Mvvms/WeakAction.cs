using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    public class WeakAction
    {
        private Action _staticAction;

        /// <summary>
        /// Gets or sets the <see cref="T:System.Reflection.MethodInfo" /> corresponding to this WeakAction's
        /// method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        /// Gets the name of the method that this WeakAction represents.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                if (this._staticAction != null)
                    return this._staticAction.Method.Name;
                return this.Method.Name;
            }
        }

        /// <summary>
        /// Gets or sets a WeakReference to this WeakAction's action's target.
        /// This is not necessarily the same as
        /// <see cref="P:GalaSoft.MvvmLight.Helpers.WeakAction.Reference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference ActionReference { get; set; }

        /// <summary>
        /// Gets or sets a WeakReference to the target passed when constructing
        /// the WeakAction. This is not necessarily the same as
        /// <see cref="P:GalaSoft.MvvmLight.Helpers.WeakAction.ActionReference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        /// Gets a value indicating whether the WeakAction is static or not.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return this._staticAction != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Action's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (this._staticAction == null && this.Reference == null)
                    return false;
                if (this._staticAction != null && this.Reference == null)
                    return true;
                return this.Reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets the Action's owner. This object is stored as a
        /// <see cref="T:System.WeakReference" />.
        /// </summary>
        public object Target
        {
            get
            {
                if (this.Reference == null)
                    return (object)null;
                return this.Reference.Target;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected object ActionTarget
        {
            get
            {
                if (this.ActionReference == null)
                    return (object)null;
                return this.ActionReference.Target;
            }
        }

        /// <summary>
        /// Initializes an empty instance of the <see cref="T:GalaSoft.MvvmLight.Helpers.WeakAction" /> class.
        /// </summary>
        protected WeakAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GalaSoft.MvvmLight.Helpers.WeakAction" /> class.
        /// </summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action action)
            : this(action.Target, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GalaSoft.MvvmLight.Helpers.WeakAction" /> class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(object target, Action action)
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
        /// is still alive.
        /// </summary>
        public void Execute()
        {
            if (this._staticAction != null)
            {
                this._staticAction();
            }
            else
            {
                if (!this.IsAlive || (!(this.Method != (MethodInfo)null) || this.ActionReference == null || this.ActionTarget == null))
                    return;
                this.Method.Invoke(this.ActionTarget, (object[])null);
            }
        }

        /// <summary>Sets the reference that this instance stores to null.</summary>
        public void MarkForDeletion()
        {
            this.Reference = (WeakReference)null;
            this.ActionReference = (WeakReference)null;
            this.Method = (MethodInfo)null;
            this._staticAction = (Action)null;
        }
    }
}
