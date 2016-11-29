using System;
using System.ComponentModel;

namespace CanDao.Pos.Model
{
    public class BaseNotifyObject : INotifyPropertyChanged
    {
        protected void RaisePropertyChanged(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentException(propertyName + " doesn't exist in " + GetType().Name + " type");
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}