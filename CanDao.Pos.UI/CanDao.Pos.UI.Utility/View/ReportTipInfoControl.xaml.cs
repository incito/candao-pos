using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 小费报表控件。
    /// </summary>
    public partial class ReportTipInfoControl
    {
        public ReportTipInfoControl()
        {
            InitializeComponent();
            DataContext = new ReportTipInfoControlViewModel { GsCtrl = GsTipInfoes };
        }
    }
}
