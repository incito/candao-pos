using System.Windows.Input;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 输入更多信息窗口Vm。
    /// </summary>
    public class InputMoreInfoWndVm : NormalWindowViewModel
    {
        private string _inputInfo;

        public InputMoreInfoWndVm(string title, string info)
        {
            Title = title;
            MaxInputLetterCount = 20;
            RemainLetterNum = MaxInputLetterCount;
            InputInfo = string.IsNullOrEmpty(info) ? "" : info;
        }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 剩余可输入字数。
        /// </summary>
        public int RemainLetterNum { get; set; }

        /// <summary>
        /// 最大可输入数量。
        /// </summary>
        public int MaxInputLetterCount { get; set; }

        /// <summary>
        /// 输入的信息。
        /// </summary>
        public string InputInfo
        {
            get { return _inputInfo; }
            set
            {
                _inputInfo = value;
                RemainLetterNum = MaxInputLetterCount - value.Length;
                RaisePropertyChanged("RemainLetterNum");
                RaisePropertyChanged("InputInfo");
            }
        }

        /// <summary>
        /// 清空命令。
        /// </summary>
        public ICommand ClearCmd { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void Clear(object arg)
        {
            InputInfo = "";
        }

        private bool CanClear(object arg)
        {
            return InputInfo.Length > 0;
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            ClearCmd = CreateDelegateCommand(Clear, CanClear);
        }
    }
}