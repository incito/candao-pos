using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 授权窗口。
    /// </summary>
    public partial class AuthorizationWindow
    {
        public AuthorizationWindow(EnumRightType rightType, string userName = "002")
        {
            InitializeComponent();
            DataContext = new AuthorizationWndVm(rightType, userName) { OwnerWindow = this };
        }
    }
}
