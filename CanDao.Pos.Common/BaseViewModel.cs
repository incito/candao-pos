using System;
using System.Collections.Generic;
using DevExpress.Xpf.Mvvm;

namespace CanDao.Pos.Common
{
    public class BaseViewModel : ViewModelBase
    {
        private readonly List<DelegateCommand<object>> _commands = new List<DelegateCommand<object>>();

        public void UpdateCanExecute()
        {
            _commands.ForEach(t => t.RaiseCanExecuteChanged());
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