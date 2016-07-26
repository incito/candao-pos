using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace CanDao.Pos.Common
{
    public class BaseViewModel : BindableBase
    {
        private static readonly object NotSetParameter = new object();

        private object _parameter = NotSetParameter;

        private static bool? _isInDesignMode;

        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                    _isInDesignMode = (bool)dependencyPropertyDescriptor.Metadata.DefaultValue;
                }
                return _isInDesignMode.Value;
            }
        }

        protected BaseViewModel()
        {
            if (IsInDesignMode)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(OnInitializeInDesignMode));
                return;
            }
            OnInitializeInRuntime();
        }

        protected virtual void OnParameterChanged(object parameter)
        {
        }

        protected virtual void OnParentViewModelChanged(object parentViewModel)
        {
        }

        protected virtual void OnInitializeInDesignMode()
        {
            OnParameterChanged(null);
        }

        protected virtual void OnInitializeInRuntime()
        {
        }

        private readonly List<DelegateCommand<object>> _commands = new List<DelegateCommand<object>>();

        public void UpdateCanExecute()
        {
            _commands.ForEach(t => t.RaiseCanExecuteChanged());
        }

        protected DelegateCommand CreateDelegateCommand(Action executeMethod)
        {
            return CreateDelegateCommand(executeMethod, null);
        }

        protected DelegateCommand CreateDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            DelegateCommand command = new DelegateCommand(executeMethod, canExecuteMethod);
            _commands.Add(command);
            return command;
        }

        protected DelegateCommand<object> CreateDelegateCommand(Action<object> executeMethod)
        {
            return CreateDelegateCommand(executeMethod, null);
        }

        protected DelegateCommand<object> CreateDelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            DelegateCommand<object> command = new DelegateCommand<object>(executeMethod, canExecuteMethod);
            _commands.Add(command);
            return command;
        }
    }
}