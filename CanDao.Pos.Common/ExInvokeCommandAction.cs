using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Reflection;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 扩展的InvokeCommandAction
    /// </summary>
    public class ExInvokeCommandAction : TriggerAction<DependencyObject>
    {

        private string _commandName;
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ExInvokeCommandAction), null);
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ExInvokeCommandAction), null);

        /// <summary>
        /// 获得或设置此操作应调用的命令的名称。
        /// </summary>
        /// <value>此操作应调用的命令的名称。</value>
        /// <remarks>如果设置了此属性和 Command 属性，则此属性将被后者所取代。</remarks>
        public string CommandName
        {
            get
            {
                ReadPreamble();
                return _commandName;
            }
            set
            {
                if (CommandName != value)
                {
                    WritePreamble();
                    _commandName = value;
                    WritePostscript();
                }
            }
        }

        /// <summary>
        /// 获取或设置此操作应调用的命令。这是依赖属性。
        /// </summary>
        /// <value>要执行的命令。</value>
        /// <remarks>如果设置了此属性和 CommandName 属性，则此属性将优先于后者。</remarks>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// 获得或设置命令参数。这是依赖属性。
        /// </summary>
        /// <value>命令参数。</value>
        /// <remarks>这是传递给 ICommand.CanExecute 和 ICommand.Execute 的值。</remarks>
        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// 调用操作。
        /// </summary>
        /// <param name="parameter">操作的参数。如果操作不需要参数，则可以将参数设置为空引用。</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                ICommand command = ResolveCommand();

                /*
                 * ★★★★★★★★★★★★★★★★★★★★★★★★
                 * 注意这里添加了事件触发源和事件参数
                 * ★★★★★★★★★★★★★★★★★★★★★★★★
                 */
                ExCommandParameter exParameter = new ExCommandParameter
                {
                    Sender = AssociatedObject,
                    Parameter = GetValue(CommandParameterProperty),
                    EventArgs = parameter as EventArgs

                };

                if (command != null && command.CanExecute(exParameter))
                {
                    /*
                     * ★★★★★★★★★★★★★★★★★★★★★★★★
                     * 注意将扩展的参数传递到Execute方法中
                     * ★★★★★★★★★★★★★★★★★★★★★★★★
                     */
                    command.Execute(exParameter);
                }
            }
        }

        private ICommand ResolveCommand()
        {
            ICommand result = null;
            if (Command != null)
            {
                result = Command;
            }
            else
            {
                if (AssociatedObject != null)
                {
                    Type type = AssociatedObject.GetType();
                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropertyInfo[] array = properties;
                    foreach (PropertyInfo propertyInfo in array)
                    {
                        if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
                        {
                            result = (ICommand)propertyInfo.GetValue(AssociatedObject, null);
                        }
                    }
                }
            }
            return result;
        }

    }
}