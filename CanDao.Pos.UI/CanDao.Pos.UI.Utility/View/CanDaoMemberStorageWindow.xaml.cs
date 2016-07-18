using System.Windows;
using System.Windows.Input;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 餐道会员储值窗口。
    /// </summary>
    public partial class CanDaoMemberStorageWindow
    {
        public CanDaoMemberStorageWindow(string mobile, string cardNo)
        {
            InitializeComponent();
            DataContext = new CanDaoMemberStorageWndVm(mobile, cardNo) { OwnerWindow = this };
        }

        private void CanDaoMemberStorageWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((CanDaoMemberStorageWndVm) DataContext).StoreAmountAsync();
        }

        private void CanDaoMemberStorageWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbStore.Focus();
            TbStore.SelectionStart = 1;
        }
    }
}
