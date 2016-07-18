using CanDao.Pos.Common;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 报表窗口。
    /// </summary>
    public partial class ReportViewWindow
    {
        public ReportViewWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
        }
    }
}
