using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CanDaoCD.Pos.Common.Classes.Mvvms
{
    public class RelayCommand<T> : ICommand
    {
        private readonly WeakAction<T> _execute;
        private readonly WeakFunc<T, bool> _canExecute;

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

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that
        /// can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute)
            : this(execute, (Func<T, bool>)null)
        {
        }

        /// <summary>Initializes a new instance of the RelayCommand class.</summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            this._execute = new WeakAction<T>(execute);
            if (canExecute == null)
                return;
            this._canExecute = new WeakFunc<T, bool>(canExecute);
        }

        /// <summary>
        /// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand`1.CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data
        /// to be passed, this object can be set to a null reference</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
                return true;
            if (!this._canExecute.IsStatic && !this._canExecute.IsAlive)
                return false;
            if (parameter == null && typeof(T).IsValueType)
                return this._canExecute.Execute(default(T));
            return this._canExecute.Execute((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data
        /// to be passed, this object can be set to a null reference</param>
        public virtual void Execute(object parameter)
        {
            object parameter1 = parameter;
            if (parameter != null && parameter.GetType() != typeof(T) && parameter is IConvertible)
                parameter1 = Convert.ChangeType(parameter, typeof(T), (IFormatProvider)null);
            if (!this.CanExecute(parameter1) || this._execute == null || !this._execute.IsStatic && !this._execute.IsAlive)
                return;
            if (parameter1 == null)
            {
                if (typeof(T).IsValueType)
                    this._execute.Execute(default(T));
                else
                    this._execute.Execute((T)parameter1);
            }
            else
                this._execute.Execute((T)parameter1);
        }
    }
}
