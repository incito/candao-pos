using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Common;
using Keyboard = CanDao.Pos.Common.Keyboard;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class InputHelperControlVm : BaseViewModel
    {
        public InputHelperControlVm()
        {
            InitCommand();
            IsNumber = true;
        }

        private bool _isNumber;

        public bool IsNumber
        {
            get { return _isNumber; }
            set
            {
                _isNumber = value;
                IsLetter = !value;
                RaisePropertiesChanged("IsNumber");
                RaisePropertiesChanged("IsLetter");
            }
        }

        public bool IsLetter { get; set; }

        /// <summary>
        /// 输入的焦点控件。即当点击本输入控件时，先将焦点控件设置成焦点。
        /// </summary>
        public UIElement FocusElement { get; set; }

        /// <summary>
        /// 输入命令。
        /// </summary>
        public ICommand InputCmd { get; private set; }

        /// <summary>
        /// 输入命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void Input(object param)
        {
            if(param == null)
                return;

            if (FocusElement != null)
                FocusElement.Focus();

            string keyString = (string) param;
            switch (keyString)
            {
                case "Clear":
                    Keyboard.Press(Key.LeftCtrl);
                    Keyboard.Press(Key.A);
                    Keyboard.Release(Key.LeftCtrl);
                    Keyboard.Release(Key.A);
                    Keyboard.Press(Key.Delete);
                    Keyboard.Release(Key.Delete);
                    break;
                case "Back":
                    Keyboard.Press(Key.Back);
                    Keyboard.Release(Key.Back);
                    break;
                case "Tab":
                    Keyboard.Press(Key.Tab);
                    Keyboard.Release(Key.Tab);
                    break;
                case "Enter":
                    //Keyboard.Press(Key.Tab);
                    //Keyboard.Release(Key.Tab);
                    Keyboard.Press(Key.Enter);
                    Keyboard.Release(Key.Enter);
                    break;
                case "Letter":
                    IsNumber = false;
                    break;
                case "Num":
                    IsNumber = true;
                    break;
                default:
                    Keyboard.Type(keyString);
                    break;
            }
        }

        /// <summary>
        /// 初始化命令。
        /// </summary>
        private void InitCommand()
        {
            InputCmd = CreateDelegateCommand(Input);
        }
    }
}