using System.Windows;
using System.Windows.Input;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 输入零找金窗口。
    /// </summary>
    public partial class PettyCashWindow
    {
        public PettyCashWindow()
        {
            InitializeComponent();
            DataContext = new PettyCashWndVm { OwnerWindow = this };
        }
    }
}
