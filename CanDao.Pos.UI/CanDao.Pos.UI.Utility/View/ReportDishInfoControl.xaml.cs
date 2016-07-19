using System.Windows.Controls;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 品项报表明细。
    /// </summary>
    public partial class ReportDishInfoControl
    {
        public ReportDishInfoControl()
        {
            InitializeComponent();
            DataContext = new ReportDishInfoControlViewModel { GsCtrl = GsDish };
        }
    }
}
