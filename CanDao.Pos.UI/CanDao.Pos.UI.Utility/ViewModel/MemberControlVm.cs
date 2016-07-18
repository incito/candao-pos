using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

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
            var wnd = WindowHelper.FindVisualTreeRoot(OwnerCtrl) as Window;
            switch (arg as string)
            {
                case "Query":
                    WindowHelper.ShowDialog(new CanDaoMemberQueryWindow(), wnd);
                    break;
                case "Storage":
                    WindowHelper.ShowDialog(new CanDaoMemberQueryWindow(), wnd);
                    break;
                case "Regist":
                    WindowHelper.ShowDialog(new CanDaoMemberRegistrationWindow(), wnd);
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