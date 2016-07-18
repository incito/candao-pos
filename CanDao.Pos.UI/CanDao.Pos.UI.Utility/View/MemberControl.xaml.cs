using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 会员控件。
    /// </summary>
    public partial class MemberControl
    {
        public MemberControl()
        {
            InitializeComponent();
            ((MemberControlVm)DataContext).OwnerCtrl = this;
        }
    }
}
