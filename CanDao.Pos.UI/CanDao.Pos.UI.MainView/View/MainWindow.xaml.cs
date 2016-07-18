using System.ComponentModel;
using CanDao.Pos.Common;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 主界面窗口。
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(bool isForcedEndWork)
        {
            InitializeComponent();
            DataContext = new MainWndVm(isForcedEndWork) { OwnerWindow = this };
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!MessageDialog.Quest("确定要退出系统吗？"))
                e.Cancel = true;
        }
    }
}
