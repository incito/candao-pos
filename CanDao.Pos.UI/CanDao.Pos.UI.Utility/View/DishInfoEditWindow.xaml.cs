using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 菜品信息编辑窗口。
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
            get { return ((DishInfoEditWndVm)DataContext).DishNum; }
        }

        private void DishInfoEditWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbDishNum.Focus();
        }
    }
}
