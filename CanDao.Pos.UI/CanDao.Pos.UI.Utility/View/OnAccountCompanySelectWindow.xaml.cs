using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 挂账单位的选择窗口。
    /// </summary>
    public partial class OnAccountCompanySelectWindow
    {
        public OnAccountCompanySelectWindow()
        {
            InitializeComponent();
            DataContext = new OnAccountCompanySelectWndVm { OwnerWindow = this };
        }

        /// <summary>
        /// 选择的挂账单位。
        /// </summary>
        public OnCompanyAccountInfo SelectedCompany
        {
            get { return ((OnAccountCompanySelectWndVm) DataContext).SelectedCompany; }
        }
    }
}
