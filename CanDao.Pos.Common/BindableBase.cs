using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CanDao.Pos.Common
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("expression");
            }
            return memberExpression.Member.Name;
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName, Action changedCallback)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;
            if (changedCallback != null)
            {
                changedCallback();
            }
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName)
        {
            return SetProperty(ref storage, value, propertyName, null);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged()
        {
            RaisePropertiesChanged(null);
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            RaisePropertyChanged(GetPropertyName(expression));
        }

        protected void RaisePropertiesChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertiesChanged<T1, T2>(Expression<Func<T1>> expression1, Expression<Func<T2>> expression2)
        {
            RaisePropertyChanged(expression1);
            RaisePropertyChanged(expression2);
        }

        protected void RaisePropertiesChanged<T1, T2, T3>(Expression<Func<T1>> expression1, Expression<Func<T2>> expression2, Expression<Func<T3>> expression3)
        {
            RaisePropertyChanged(expression1);
            RaisePropertyChanged(expression2);
            RaisePropertyChanged(expression3);
        }

        protected void RaisePropertiesChanged<T1, T2, T3, T4>(Expression<Func<T1>> expression1, Expression<Func<T2>> expression2, Expression<Func<T3>> expression3, Expression<Func<T4>> expression4)
        {
            RaisePropertyChanged(expression1);
            RaisePropertyChanged(expression2);
            RaisePropertyChanged(expression3);
            RaisePropertyChanged(expression4);
        }

        protected void RaisePropertiesChanged<T1, T2, T3, T4, T5>(Expression<Func<T1>> expression1, Expression<Func<T2>> expression2, Expression<Func<T3>> expression3, Expression<Func<T4>> expression4, Expression<Func<T5>> expression5)
        {
            RaisePropertyChanged(expression1);
            RaisePropertyChanged(expression2);
            RaisePropertyChanged(expression3);
            RaisePropertyChanged(expression4);
            RaisePropertyChanged(expression5);
        }

        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression, Action changedCallback)
        {
            string propertyName = GetPropertyName(expression);
            return SetProperty(ref storage, value, propertyName, changedCallback);
        }

        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression)
        {
            return SetProperty(ref storage, value, expression, null);
        }
    }
}