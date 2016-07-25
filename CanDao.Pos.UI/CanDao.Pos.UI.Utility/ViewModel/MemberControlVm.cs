using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.VIPManage.ViewModels;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class MemberControlVm : BaseViewModel
    {
        public MemberControlVm()
        {
            InitCommand();
        }

        public ICommand OperCmd { get; private set; }

        public UserControl OwnerCtrl { get; set; }

        private void OperMethod(object arg)
        {
          
            switch (arg as string)
            {
                case "Query":
                    var query = new UcVipSelectViewModel();
                    OWindowManage.ShowPopupWindow(query.GetUserCtl());
                    break;
                case "Storage":
                    var recharge = new UcVipRechargeViewModel();
                    OWindowManage.ShowPopupWindow(recharge.GetUserCtl());
                    break;
                case "Regist":
                    var regist = new UcVipRegViewModel();
                    OWindowManage.ShowPopupWindow(regist.GetUserCtl());
                    break;
                case "Active":
                    MessageDialog.Warning("激活");
                    break;
            }
        }

        private void InitCommand()
        {
            OperCmd = CreateDelegateCommand(OperMethod);
        }
    }
}