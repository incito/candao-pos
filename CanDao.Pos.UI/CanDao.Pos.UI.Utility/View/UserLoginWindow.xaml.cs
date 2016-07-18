using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 用户登录窗口。
    /// </summary>
    public partial class UserLoginWindow
    {
        public UserLoginWindow()
        {
            InitializeComponent();
            DataContext = new UserLoginWndVm { OwnerWindow = this };
        }
    }
}
