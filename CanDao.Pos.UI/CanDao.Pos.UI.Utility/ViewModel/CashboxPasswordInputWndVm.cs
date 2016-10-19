using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 钱箱密码输入窗口的VM。
    /// </summary>
    public class CashboxPasswordInputWndVm : NormalWindowViewModel<CashboxPasswordInputWindow>
    {
        /// <summary>
        /// 默认密码。
        /// </summary>
        private const string DefaultPsw = "518818";

        /// <summary>
        /// 钱箱密码。
        /// </summary>
        public string CashPassword { get; set; }

        protected override void OnWindowLoaded(object param)
        {
            OwnerWnd.PbBox.Focus();
        }

        protected override void Confirm(object param)
        {
            if (string.IsNullOrEmpty(CashPassword))
            {
                MessageDialog.Warning("密码不能为空。");
                return;
            }

            if (!CashPassword.Equals(DefaultPsw))
            {
                MessageDialog.Warning("密码验证错误，请输入正确密码。");
            }
            else
            {
                CloseWindow(true);
            }
        }
    }
}