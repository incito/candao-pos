using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;
using Keyboard = CanDao.Pos.Common.Keyboard;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 餐台操作窗口。
    /// </summary>
    public partial class TableOperWindow
    {
        public TableOperWindow(TableInfo tableInfo)
        {
            InitializeComponent();
            DataContext = new TableOperNormalWndVm(tableInfo) { OwnerWindow = this };
        }

        public TableOperWindow(string tableName)
        {
            InitializeComponent();
            DataContext = new TableOperTakeoutWndVm(tableName) { OwnerWindow = this };
        }

        public void DishListPageUp()
        {
            DishListGc.Focus();
            Keyboard.Press(Key.PageUp);
            Keyboard.Release(Key.PageUp);
        }

        public void DishListPageDowm()
        {
            DishListGc.Focus();
            Keyboard.Press(Key.PageDown);
            Keyboard.Release(Key.PageDown);
        }

        public void CouponListPageUp()
        {
            CouponListGc.Focus();
            Keyboard.Press(Key.PageUp);
            Keyboard.Release(Key.PageUp);
        }

        public void CouponListPageDown()
        {
            CouponListGc.Focus();
            Keyboard.Press(Key.PageDown);
            Keyboard.Release(Key.PageDown);
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((TableOperWndVm)DataContext).OperCmd.Execute("PayBill");
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
