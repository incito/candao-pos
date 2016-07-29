using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    public class PropertyChangedMessage<T> : PropertyChangedMessageBase
    {
        /// <summary>
        /// Gets the value that the property has after the change.
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the value that the property had before the change.
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GalaSoft.MvvmLight.Messaging.PropertyChangedMessage`1" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
            : base(sender, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GalaSoft.MvvmLight.Messaging.PropertyChangedMessage`1" /> class.
        /// </summary>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GalaSoft.MvvmLight.Messaging.PropertyChangedMessage`1" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.</param>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(object sender, object target, T oldValue, T newValue, string propertyName)
            : base(sender, target, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}
