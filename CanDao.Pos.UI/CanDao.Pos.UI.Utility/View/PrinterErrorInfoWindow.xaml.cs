namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 打印机错误信息提示窗口。
    /// </summary>
    public partial class PrinterErrorInfoWindow
    {
        public PrinterErrorInfoWindow(string info, int nextWarningMinute)
        {
            InitializeComponent();
            TbInfo.Text = info;
            CbNoWarning.Content = string.Format("{0}分钟以内不再提醒。", nextWarningMinute);
        }

        /// <summary>
        /// 是否选择10分钟以内不再提醒。
        /// </summary>
        public bool IsCheckedNoWarning
        {
            get { return CbNoWarning.IsChecked != null && CbNoWarning.IsChecked.Value; }
        }
    }
}
