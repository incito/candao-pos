using System.Windows.Input;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using Keyboard = CanDaoCD.Pos.Common.Operates.Keyboard;

namespace CanDaoCD.Pos.Common.Controls.CSystem
{
    public class DigitalInputKeyboardVm : ViewModelBase
    {
        public DigitalInputKeyboardVm()
        {
            InputCmd = new RelayCommand<string>(Input);
        }

        /// <summary>
        /// 输入命令。
        /// </summary>
        public ICommand InputCmd { get; private set; }

        /// <summary>
        /// 输入命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void Input(string param)
        {
            if (param == null)
                return;

            switch (param)
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
                case ".":
                {
                    Keyboard.Press(Key.Decimal);
                    Keyboard.Release(Key.Decimal);
                    break;
                }
                default:
                    Keyboard.Type(param);
                    break;
            }
        }

    }
}