using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 选择银行窗口。
    /// </summary>
    public partial class SelectBankWindow
    {
        public SelectBankWindow(BankInfo bankInfo)
        {
            InitializeComponent();

            DataContext = new SelectBankWndVm(bankInfo) { OwnerWindow = this };
        }
    }
}
