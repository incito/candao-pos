using System;
using System.Globalization;
using System.Windows.Input;

namespace CanDao.Pos.Common
{
    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action executeMethod, bool useCommandManager = true)
            : this(executeMethod, null, useCommandManager)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool useCommandManager = true)
            : base(GenerateExecuteMethod(executeMethod), GenerateCanExecuteMethod(canExecuteMethod), useCommandManager)
        {
        }

        private static Action<object> GenerateExecuteMethod(Action action)
        {
            if (action == null)
                return null;

            return delegate { action(); };
        }

        private static Func<object, bool> GenerateCanExecuteMethod(Func<bool> canExecuteMethod)
        {
            if (canExecuteMethod == null)
                return null;

            return delegate { return canExecuteMethod(); };
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod;

        private readonly Func<T, bool> _canExecuteMethod;

        private readonly bool _useCommandManager;

        private event EventHandler _canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_useCommandManager)
                {
                    CommandManager.RequerySuggested += value;
                    return;
                }
                _canExecuteChanged += value;
            }
            remove
            {
                if (_useCommandManager)
                {
                    CommandManager.RequerySuggested -= value;
                    return;
                }
                _canExecuteChanged -= value;
            }
        }

        public DelegateCommand(Action<T> executeMethod, bool useCommandManager = true)
            : this(executeMethod, null, useCommandManager)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool useCommandManager = true)
        {
            _useCommandManager = useCommandManager;
            if (executeMethod == null && canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }
            if (_executeMethod == null)
            {
                return;
            }
            _executeMethod(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(GetGenericParameter(parameter));
        }

        void ICommand.Execute(object parameter)
        {
            Execute(GetGenericParameter(parameter));
        }

        private static T GetGenericParameter(object parameter)
        {
            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (type.IsEnum && parameter is string)
            {
                parameter = Enum.Parse(type, (string)parameter, false);
            }
            else if (parameter is IConvertible && !typeof(T).IsAssignableFrom(parameter.GetType()))
            {
                parameter = Convert.ChangeType(parameter, type, CultureInfo.InvariantCulture);
            }
            if (parameter != null)
            {
                return (T)parameter;
            }
            return default(T);
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (_canExecuteChanged != null)
            {
                _canExecuteChanged(this, EventArgs.Empty);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (_useCommandManager)
            {
                CommandManager.InvalidateRequerySuggested();
                return;
            }
            OnCanExecuteChanged();
        }
    }
}