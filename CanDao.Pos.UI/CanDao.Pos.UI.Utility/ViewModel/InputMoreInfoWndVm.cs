using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 输入更多信息窗口Vm。
    /// </summary>
    public class InputMoreInfoWndVm : NormalWindowViewModel<InputMoreInfoWindow>
    {
        private string _inputInfo;

        /// <summary>
        /// 是否允许设置输入框的焦点在文本最后，只有一次有效。
        /// </summary>
        private bool _allowSetCaretIndex2Last;


        public InputMoreInfoWndVm(string title, string info)
        {
            Title = title;
            MaxInputLetterCount = 20;
            RemainLetterNum = MaxInputLetterCount;
            InputInfo = string.IsNullOrEmpty(info) ? "" : info;
            _allowSetCaretIndex2Last = !string.IsNullOrWhiteSpace(info);
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

        protected override void OnWindowLoaded(object param)
        {
            OwnerWnd.TbMoreInfo.Focus();
            OwnerWnd.TbMoreInfo.TextChanged += TbMoreInfo_TextChanged;
        }

        protected override void OnWindowClosed(object param)
        {
            OwnerWnd.TbMoreInfo.TextChanged -= TbMoreInfo_TextChanged;
        }

        void TbMoreInfo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_allowSetCaretIndex2Last)
                OwnerWnd.TbMoreInfo.CaretIndex = OwnerWnd.TbMoreInfo.Text.Length;
            _allowSetCaretIndex2Last = false;
        }
    }
}