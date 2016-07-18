using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 餐道会员修改密码窗口。
    /// </summary>
    public partial class CanDaoMemberModifyPwdWindow
    {
        public CanDaoMemberModifyPwdWindow(MemberInfo memberInfo)
        {
            InitializeComponent();
            DataContext = new CanDaoMemberModifyPwdWndVm(memberInfo) { OwnerWindow = this };
        }
    }
}
