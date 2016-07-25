using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        private static bool? _isInDesignMode;
        private IMessenger _messengerInstance;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running under Blend or Visual Studio).
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return ViewModelBase.IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!ViewModelBase._isInDesignMode.HasValue)
                    ViewModelBase._isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
                return ViewModelBase._isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Gets or sets an instance of a <see cref="T:GalaSoft.MvvmLight.Messaging.IMessenger" /> used to
        /// broadcast messages to other objects. If null, this class will
        /// attempt to broadcast using the Messenger's default instance.
        /// </summary>
        protected IMessenger MessengerInstance
        {
            get
            {
                return this._messengerInstance ?? Messenger.Default;
            }
            set
            {
                this._messengerInstance = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        public ViewModelBase()
            : this((IMessenger)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        /// <param name="messenger">An instance of a <see cref="T:GalaSoft.MvvmLight.Messaging.Messenger" />
        /// used to broadcast messages to other objects. If null, this class
        /// will attempt to broadcast using the Messenger's default
        /// instance.</param>
        public ViewModelBase(IMessenger messenger)
        {
            this.MessengerInstance = messenger;
        }

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public virtual void Cleanup()
        {
            this.MessengerInstance.Unregister((object)this);
        }

        /// <summary>
        /// Broadcasts a PropertyChangedMessage using either the instance of
        /// the Messenger that was passed to this class (if available)
        /// or the Messenger's default instance.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="oldValue">The value of the property before it
        /// changed.</param>
        /// <param name="newValue">The value of the property after it
        /// changed.</param>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            this.MessengerInstance.Send<PropertyChangedMessage<T>>(new PropertyChangedMessage<T>((object)this, oldValue, newValue, propertyName));
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            this.RaisePropertyChanged(propertyName);
            if (!broadcast)
                return;
            this.Broadcast<T>(oldValue, newValue, propertyName);
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="oldValue">The property's value before the change
        /// occurred.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        protected virtual void RaisePropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> propertyExpression, T oldValue, T newValue, bool broadcast)
        {
            PropertyChangedEventHandler propertyChangedHandler = this.PropertyChangedHandler;
            if (propertyChangedHandler == null && !broadcast)
                return;
            string propertyName = this.GetPropertyName<T>(propertyExpression);
            if (propertyChangedHandler != null)
                propertyChangedHandler((object)this, new PropertyChangedEventArgs(propertyName));
            if (broadcast)
                this.Broadcast<T>(oldValue, newValue, propertyName);
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        protected bool Set<T>(System.Linq.Expressions.Expression<Func<T>> propertyExpression, ref T field, T newValue, bool broadcast)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;
            this.RaisePropertyChanging<T>(propertyExpression);
            T oldValue = field;
            field = newValue;
            this.RaisePropertyChanged<T>(propertyExpression, oldValue, field, broadcast);
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the
        /// PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        protected bool Set<T>(string propertyName, ref T field, T newValue, bool broadcast)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;
            this.RaisePropertyChanging(propertyName);
            T oldValue = field;
            field = newValue;
            this.RaisePropertyChanged<T>(propertyName, oldValue, field, broadcast);
            return true;
        }
    }
}
