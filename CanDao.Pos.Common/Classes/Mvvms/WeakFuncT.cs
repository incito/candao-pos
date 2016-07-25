using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    public class WeakFunc<T, TResult> : WeakFunc<TResult>
    {
        private Func<T, TResult> _staticFunc;

        /// <summary>
        /// Gets or sets the name of the method that this WeakFunc represents.
        /// </summary>
        public override string MethodName
        {
            get
            {
                if (this._staticFunc != null)
                    return this._staticFunc.Method.Name;
                return this.Method.Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Func's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (this._staticFunc == null && this.Reference == null)
                    return false;
                if (this._staticFunc == null)
                    return this.Reference.IsAlive;
                if (this.Reference != null)
                    return this.Reference.IsAlive;
                return true;
            }
        }

        /// <summary>Initializes a new instance of the WeakFunc class.</summary>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(Func<T, TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>Initializes a new instance of the WeakFunc class.</summary>
        /// <param name="target">The func's owner.</param>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<T, TResult> func)
        {
            if (func.Method.IsStatic)
            {
                this._staticFunc = func;
                if (target == null)
                    return;
                this.Reference = new WeakReference(target);
            }
            else
            {
                this.Method = func.Method;
                this.FuncReference = new WeakReference(func.Target);
                this.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Executes the func. This only happens if the func's owner
        /// is still alive. The func's parameter is set to default(T).
        /// </summary>
        public new TResult Execute()
        {
            return this.Execute(default(T));
        }

        /// <summary>
        /// Executes the func. This only happens if the func's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public TResult Execute(T parameter)
        {
            if (this._staticFunc != null)
                return this._staticFunc(parameter);
            if (!this.IsAlive || (!(this.Method != (MethodInfo)null) || this.FuncReference == null))
                return default(TResult);
            return (TResult)this.Method.Invoke(this.FuncTarget, new object[1]
      {
        (object) parameter
      });
        }

        /// <summary>
        /// Executes the func with a parameter of type object. This parameter
        /// will be casted to T. This method implements <see cref="M:GalaSoft.MvvmLight.Helpers.IExecuteWithObject.ExecuteWithObject(System.Object)" />
        /// and can be useful if you store multiple WeakFunc{T} instances but don't know in advance
        /// what type T represents.
        /// </summary>
        /// <param name="parameter">The parameter that will be passed to the func after
        /// being casted to T.</param>
        /// <returns>The result of the execution as object, to be casted to T.</returns>
        public object ExecuteWithObject(object parameter)
        {
            return (object)this.Execute((T)parameter);
        }

        /// <summary>
        /// Sets all the funcs that this WeakFunc contains to null,
        /// which is a signal for containing objects that this WeakFunc
        /// should be deleted.
        /// </summary>
        public new void MarkForDeletion()
        {
            this._staticFunc = (Func<T, TResult>)null;
            base.MarkForDeletion();
        }
    }
}
