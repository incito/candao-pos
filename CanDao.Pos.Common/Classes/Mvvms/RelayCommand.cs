using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CanDao.Pos.Common.Classes.Mvvms
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {

        #region 字段

        private readonly WeakAction _execute;
        private readonly WeakFunc<bool> _canExecute;

        #endregion

        #region 事件

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecute == null)
                    return;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (this._canExecute == null)
                    return;
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that
        /// can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute)
            : this(execute, (Func<bool>) null)
        {
        }

        /// <summary>Initializes a new instance of the RelayCommand class.</summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            this._execute = new WeakAction(execute);
            if (canExecute == null)
                return;
            this._canExecute = new WeakFunc<bool>(canExecute);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand.CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ||
                   (this._canExecute.IsStatic || this._canExecute.IsAlive) && this._canExecute.Execute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public virtual void Execute(object parameter)
        {
            if (!this.CanExecute(parameter) || this._execute == null ||
                !this._execute.IsStatic && !this._execute.IsAlive)
                return;
            this._execute.Execute();
        }

        #endregion
    }
}
