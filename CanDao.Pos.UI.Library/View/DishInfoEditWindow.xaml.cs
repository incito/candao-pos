using System.Windows;
using CanDao.Pos.UI.Library.ViewModel;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// DishInfoEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DishInfoEditWindow
    {
        public DishInfoEditWindow(string dishName, decimal dishPrice, bool allowEditDishName = false, bool allowEditDishPrice = false)
        {
            InitializeComponent();
            DataContext = new DishInfoEditWndVm
            {
                OwnerWindow = this,
                DishName = dishName,
                DishPrice = dishPrice,
                AllowEditDishName = allowEditDishName,
                AllowEditDishPrice = allowEditDishPrice,
                WndTitle = (!allowEditDishName && !allowEditDishPrice) ? "菜品数量设置窗口" : "菜品信息编辑窗口",
            };
        }

        public decimal DishNum
        {
            get { return ((DishInfoEditWndVm) DataContext).DishNum; }
        }

        private void DishInfoEditWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbDishNum.Focus();
        }
    }
}
