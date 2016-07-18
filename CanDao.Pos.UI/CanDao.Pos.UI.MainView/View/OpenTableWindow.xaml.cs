using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 开台窗口。 
    /// </summary>
    public partial class OpenTableWindow
    {
        public OpenTableWindow(TableInfo tableInfo)
        {
            InitializeComponent();
            DataContext = new OpenTableWndVm(tableInfo) { OwnerWindow = this };
        }
    }
}
