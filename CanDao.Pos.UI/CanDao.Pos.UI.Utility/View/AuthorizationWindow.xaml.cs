using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.ViewModel;
using DevExpress.Xpf.Editors;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 授权窗口。
    /// </summary>
    public partial class AuthorizationWindow
    {
        public AuthorizationWindow(EnumRightType rightType, string userName = null)
        {
            InitializeComponent();
            DataContext = new AuthorizationWndVm(rightType, userName) { OwnerWindow = this };
        }

        private void AuthorizationWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ((AuthorizationWndVm)DataContext).ConfirmCmd.Execute(null);
                    break;
            }
        }
    }
}
