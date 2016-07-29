using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 选择外卖挂单窗口。
    /// </summary>
    public partial class SelectTakeoutOnAccountCompany
    {
        public SelectTakeoutOnAccountCompany(TableInfo tableInfo)
        {
            InitializeComponent();
            DataContext = new SelectTakeoutOnAccountCompanyVm(tableInfo) { OwnerWindow = this };
        }
    }
}
