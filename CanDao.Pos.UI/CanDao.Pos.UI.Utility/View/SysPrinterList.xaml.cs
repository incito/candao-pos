using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 系统设置里打印机列表展示控件
    /// </summary>
    public partial class SysPrinterList
    {
        public SysPrinterList()
        {
            InitializeComponent();
            DataContext = new SysPrinterListVm { OwnerCtrl = this };
        }
    }
}
