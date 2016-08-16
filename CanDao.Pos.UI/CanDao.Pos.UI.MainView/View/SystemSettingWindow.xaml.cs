using System;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 系统设置窗口。
    /// </summary>
    public partial class SystemSettingWindow
    {
        public SystemSettingWindow()
        {
            InitializeComponent();
        }

        private void SystemSettingWindow_OnClosed(object sender, EventArgs e)
        {
            InfoLog.Instance.I("系统窗口关闭了。");
            SysPrinterList.DisposeTimer();
        }
    }
}
