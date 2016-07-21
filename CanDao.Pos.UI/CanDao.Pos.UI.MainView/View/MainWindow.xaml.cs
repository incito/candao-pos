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
    }
}
