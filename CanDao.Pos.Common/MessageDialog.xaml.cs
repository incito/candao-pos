using System.Windows;
using System.Windows.Input;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 消息提示框。 
    /// </summary>
    public partial class MessageDialog
    {
        public MessageDialog()
        {
            InitializeComponent();
            MessageDialogResult = MessageBoxResult.Cancel;
            DataContext = this;
        }

        /// <summary>
        /// 提示文字
        /// </summary>
        public string InfoMsg
        {
            get { return (string)GetValue(InfoMsgProperty); }
            set { SetValue(InfoMsgProperty, value); }
        }

        public static readonly DependencyProperty InfoMsgProperty =
            DependencyProperty.Register("InfoMsg", typeof(string), typeof(MessageDialog), new PropertyMetadata("处理中..."));

        /// <summary>
        /// 消息提示框结果。
        /// </summary>
        public MessageBoxResult MessageDialogResult { get; set; }

        private MessageBoxButton _boxButton;

        public MessageBoxButton BoxButton
        {
            get { return _boxButton; }
            set
            {
                _boxButton = value;
                BtnCancel.Visibility = Visibility.Collapsed;
                switch (value)
                {
                    case MessageBoxButton.OKCancel:
                        BtnCancel.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        public static MessageBoxResult Show(string message, MessageBoxButton btn, Window ownerWindow = null)
        {
            var dialog = new MessageDialog { InfoMsg = message, BoxButton = btn };
            if (!Equals(Application.Current.MainWindow, dialog))
            {
                dialog.Owner = ownerWindow ?? (Application.Current.MainWindow.IsLoaded ? Application.Current.MainWindow : null);
            }
            else
            {
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.ShowInTaskbar = true;
            }
            dialog.ShowDialog();
            return dialog.MessageDialogResult;
        }

        /// <summary>
        /// 询问。
        /// </summary>
        /// <param name="message">询问消息。</param>
        /// <param name="ownerWindow">所属窗口。</param>
        /// <returns>确定返回true，取消返回false。</returns>
        public static bool Quest(string message, Window ownerWindow = null)
        {
            return Show(message, MessageBoxButton.OKCancel, ownerWindow) != MessageBoxResult.Cancel;
        }

        /// <summary>
        /// 显示警告信息。
        /// </summary>
        /// <param name="message">显示消息。</param>
        /// <param name="ownerWindow">所属窗口。</param>
        public static void Warning(string message, Window ownerWindow = null)
        {
            Show(message, MessageBoxButton.OK, ownerWindow);
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogResult = MessageBoxResult.OK;
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDialogResult = MessageBoxResult.Cancel;
            Close();
        }

        private void MessageDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    MessageDialogResult = MessageBoxResult.OK;
                    Close();
                    break;
                case Key.Escape:
                    MessageDialogResult = MessageBoxResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}
