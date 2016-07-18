using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 餐道会员注册窗口。
    /// </summary>
    public partial class CanDaoMemberRegistrationWindow
    {
        public CanDaoMemberRegistrationWindow()
        {
            InitializeComponent();
            DataContext = new CanDaoMemberRegistrationWndVm { OwnerWindow = this };
        }
    }
}
