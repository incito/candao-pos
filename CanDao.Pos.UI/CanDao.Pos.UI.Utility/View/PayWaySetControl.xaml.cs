using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 支付方式设置控件。
    /// </summary>
    public partial class PayWaySetControl
    {
        public PayWaySetControl()
        {
            InitializeComponent();
            DataContext = new PayWaySetControlVm { SetControl = this };
        }
    }
}
