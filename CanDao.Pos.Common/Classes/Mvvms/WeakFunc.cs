using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    /// <summary>
    /// Stores a Func&lt;T&gt; without causing a hard reference
    /// to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    public class WeakFunc<TResult>
    {
        private Func<TResult> _staticFunc;

        /// <summary>
        /// Gets or sets the <see cref="T:System.Reflection.MethodInfo" /> corresponding to this WeakFunc's
        /// method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        /// Get a value indicating whether the WeakFunc is static or not.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return this._staticFunc != null;
            }
        }

        /// <summary>
        /// Gets the name of the method that this WeakFunc represents.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                if (this._staticFunc != null)
                    return this._staticFunc.Method.Name;
                return this.Method.Name;
            }
        }

        /// <summary>
        /// Gets or sets a WeakReference to this WeakFunc's action's target.
        /// This is not necessarily the same as
        /// <see cref="P:GalaSoft.MvvmLight.Helpers.WeakFunc`1.Reference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference FuncReference { get; set; }

        /// <summary>
        /// Gets or sets a WeakReference to the target passed when constructing
        /// the WeakFunc. This is not necessarily the same as
        /// <see cref="P:GalaSoft.MvvmLight.Helpers.WeakFunc`1.FuncReference" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        /// Gets a value indicating whether the Func's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (this._staticFunc == null && this.Reference == null)
                    return false;
                if (this._staticFunc != null && this.Reference == null)
                    return true;
                return this.Reference.IsAlive;
            }
        }

        /// <summary>
        /// Gets the Func's owner. This object is stored as a
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
        /// Gets the owner of the Func that was passed as parameter.
        /// This is not necessarily the same as
        /// <see cref="P:GalaSoft.MvvmLight.Helpers.WeakFunc`1.Target" />, for example if the
        /// method is anonymous.
        /// </summary>
        protected object FuncTarget
        {
            get
            {
                if (this.FuncReference == null)
                    return (object)null;
                return this.FuncReference.Target;
            }
        }

        /// <summary>Initializes an empty instance of the WeakFunc class.</summary>
        protected WeakFunc()
        {
        }

        /// <summary>Initializes a new instance of the WeakFunc class.</summary>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(Func<TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>Initializes a new instance of the WeakFunc class.</summary>
        /// <param name="target">The func's owner.</param>
        /// <param name="func">The func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<TResult> func)
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
        /// Executes the action. This only happens if the func's owner
        /// is still alive.
        /// </summary>
        public TResult Execute()
        {
            if (this._staticFunc != null)
                return this._staticFunc();
            if (this.IsAlive && (this.Method != (MethodInfo)null && this.FuncReference != null))
                return (TResult)this.Method.Invoke(this.FuncTarget, (object[])null);
            return default(TResult);
        }

        /// <summary>Sets the reference that this instance stores to null.</summary>
        public void MarkForDeletion()
        {
            this.Reference = (WeakReference)null;
            this.FuncReference = (WeakReference)null;
            this.Method = (MethodInfo)null;
            this._staticFunc = (Func<TResult>)null;
        }
    }
}
