using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CanDaoCD.Pos.Common.Classes.Mvvms
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Provides access to the PropertyChanged event handler to derived classes.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get { return this.PropertyChanged; }
        }

        /// <summary>
        /// Provides access to the PropertyChanging event handler to derived classes.
        /// </summary>
        protected PropertyChangingEventHandler PropertyChangingHandler
        {
            get { return this.PropertyChanging; }
        }

        /// <summary>Occurs after a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Occurs before a property value changes.</summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Verifies that a property name exists in this ViewModel. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        /// <para>This method is only active in DEBUG mode.</para>
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            Type type = this.GetType();
            if (string.IsNullOrEmpty(propertyName) || !(type.GetProperty(propertyName) == (PropertyInfo) null))
                return;
            ICustomTypeDescriptor customTypeDescriptor = this as ICustomTypeDescriptor;
            if (customTypeDescriptor == null ||
                !customTypeDescriptor.GetProperties()
                    .Cast<PropertyDescriptor>()
                    .Any<PropertyDescriptor>(
                        (Func<PropertyDescriptor, bool>) (property => property.Name == propertyName)))
                throw new ArgumentException("Property not found", propertyName);
        }

        /// <summary>Raises the PropertyChanging event if needed.</summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        protected virtual void RaisePropertyChanging(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangingEventHandler changingEventHandler = this.PropertyChanging;
            if (changingEventHandler == null)
                return;
            changingEventHandler((object) this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>Raises the PropertyChanged event if needed.</summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
            if (changedEventHandler == null)
                return;
            changedEventHandler((object) this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the PropertyChanging event if needed.</summary>
        /// <typeparam name="T">The type of the property that
        /// changes.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changes.</param>
        protected virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangingEventHandler changingEventHandler = this.PropertyChanging;
            if (changingEventHandler == null)
                return;
            string propertyName = this.GetPropertyName<T>(propertyExpression);
            changingEventHandler((object) this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>Raises the PropertyChanged event if needed.</summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
            if (changedEventHandler == null)
                return;
            string propertyName = this.GetPropertyName<T>(propertyExpression);
            changedEventHandler((object) this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Extracts the name of a property from an expression.</summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyExpression">An expression returning the property's name.</param>
        /// <returns>The name of the property returned by the expression.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the expression is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the expression does not represent a property.</exception>
        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Invalid argument", "propertyExpression");
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == (PropertyInfo) null)
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            return propertyInfo.Name;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <returns>True if the PropertyChanged event has been raised,
        /// false otherwise. The event is not raised if the old
        /// value is equal to the new value.</returns>
        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;
            this.RaisePropertyChanging<T>(propertyExpression);
            field = newValue;
            this.RaisePropertyChanged<T>(propertyExpression);
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <returns>True if the PropertyChanged event has been raised,
        /// false otherwise. The event is not raised if the old
        /// value is equal to the new value.</returns>
        protected bool Set<T>(string propertyName, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;
            this.RaisePropertyChanging(propertyName);
            field = newValue;
            this.RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
