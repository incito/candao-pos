using System.Windows.Controls;
using CanDao.Pos.SystemConfig.ViewModels;

namespace CanDao.Pos.SystemConfig.Views
{
    /// <summary>
    /// 打印机状态列表显示控件。
    /// </summary>
    public partial class UcPrinterListView
    {
        public UcPrinterListView()
        {
            InitializeComponent();
            ((UcPrinterListViewModel) DataContext).OwnerCtrl = this;
        }
    }
}
