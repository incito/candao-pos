using System.Windows;

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

        public MessageBoxButton BoxButton { get; set; }

        public static bool? Show(string message, MessageBoxButton btn, Window ownerWindow = null)
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
            return dialog.DialogResult;
        }

        /// <summary>
        /// 询问。
        /// </summary>
        /// <param name="message">询问消息。</param>
        /// <param name="ownerWindow">所属窗口。</param>
        /// <returns>确定返回true，取消返回false。</returns>
        public static bool Quest(string message, Window ownerWindow = null)
        {
            return Show(message, MessageBoxButton.OKCancel, ownerWindow) == true;
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

        private void MessageDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _cancelBtn.Visibility = BoxButton == MessageBoxButton.OKCancel ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
