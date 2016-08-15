using System.Windows;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// InputMoreInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputMoreInfoWindow
    {
        /// <summary>
        /// 是否允许设置输入框的焦点在文本最后，只有一次有效。
        /// </summary>
        private bool _allowSetCaretIndex2Last;

        public InputMoreInfoWindow(string title, string info)
        {
            InitializeComponent();
            DataContext = new InputMoreInfoWndVm(title, info) { OwnerWindow = this };
            _allowSetCaretIndex2Last = !string.IsNullOrWhiteSpace(info);
            TbMoreInfo.TextChanged += TbMoreInfo_TextChanged;
        }

        void TbMoreInfo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_allowSetCaretIndex2Last)
                TbMoreInfo.CaretIndex = TbMoreInfo.Text.Length;
            _allowSetCaretIndex2Last = false;
        }

        public string InputInfo
        {
            get { return ((InputMoreInfoWndVm)DataContext).InputInfo; }
        }

        private void InputMoreInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbMoreInfo.Focus();
        }

        public void SetInfoCaretIndexToLast()
        {
            TbMoreInfo.CaretIndex = TbMoreInfo.Text.Length;
        }
    }
}
