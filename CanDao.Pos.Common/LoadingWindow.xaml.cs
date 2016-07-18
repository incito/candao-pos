using System.Windows;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 正在加载窗口。
    /// </summary>
    public partial class LoadingWindow
    {
        public LoadingWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Properties

        /// <summary>
        /// 提示文字
        /// </summary>
        public string InfoMsg
        {
            get { return (string)GetValue(InfoMsgProperty); }
            set { SetValue(InfoMsgProperty, value); }
        }

        public static readonly DependencyProperty InfoMsgProperty =
            DependencyProperty.Register("InfoMsg", typeof(string), typeof(LoadingWindow), new PropertyMetadata("处理中..."));

        #endregion
    }
}
