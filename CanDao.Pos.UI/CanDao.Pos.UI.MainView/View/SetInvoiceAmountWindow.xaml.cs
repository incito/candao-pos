using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 设置发票金额窗口。
    /// </summary>
    public partial class SetInvoiceAmountWindow
    {
        public SetInvoiceAmountWindow(TableFullInfo tableFullInfo)
        {
            InitializeComponent();
            DataContext = new SetInvoiceAmountWndVm(tableFullInfo) { OwnerWindow = this };
        }
    }
}
