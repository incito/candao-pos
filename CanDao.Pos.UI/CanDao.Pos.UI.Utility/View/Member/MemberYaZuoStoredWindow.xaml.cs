using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 会员储值窗口。
    /// </summary>
    public partial class MemberYaZuoStoredWindow
    {
        public MemberYaZuoStoredWindow()
        {
            InitializeComponent();
            DataContext = new MemberYaZuoStoredWndVm { OwnerWindow = this };
        }
    }
}
