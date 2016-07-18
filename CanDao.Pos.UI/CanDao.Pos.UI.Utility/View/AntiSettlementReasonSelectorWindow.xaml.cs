using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 反结算原因选择窗口。
    /// </summary>
    public partial class AntiSettlementReasonSelectorWindow
    {
        public AntiSettlementReasonSelectorWindow()
        {
            InitializeComponent();
            DataContext = new AntiSettlementReasonSelectorWndVm { OwnerWindow = this };
        }

        /// <summary>
        /// 选择的反结原因。
        /// </summary>
        public string SelectedReason
        {
            get { return ((AntiSettlementReasonSelectorWndVm)DataContext).SelectedReason; }
        }
    }
}
