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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CanDaoCD.Pos.Common.Classes.Mvvms;

namespace Library
{
    /// <summary>
    /// 菜品信息修改窗口。
    /// </summary>
    public partial class DishInfoEditWindow
    {
        /// <summary>
        /// 实例化一个菜品信息修改窗口类。
        /// </summary>
        /// <param name="dishName">菜品名称。</param>
        /// <param name="dishPrice">菜品价格。</param>
        /// <param name="allowEditDishName">是否允许修改菜名名称。</param>
        /// <param name="allowEditDishPrice">是否允许修改菜名价格。</param>
        public DishInfoEditWindow(string dishName, decimal dishPrice, bool allowEditDishName = false, bool allowEditDishPrice = false)
        {
            InitializeComponent();
            var vm = new DishInfoEditWndVm
            {
                OwnerWnd = this,
                DishName = dishName,
                DishPrice = dishPrice,
                AllowEditDishName = allowEditDishName,
                AllowEditDishPrice = allowEditDishPrice
            };
            DataContext = vm;
        }

        /// <summary>
        /// 获取设置的菜品数量。
        /// </summary>
        public decimal DishNum
        {
            get { return ((DishInfoEditWndVm)DataContext).DishNum; }
        }

        public string DishName
        {
            get { return ((DishInfoEditWndVm)DataContext).DishName; }
        }

        /// <summary>
        /// 获取设置的菜品价格。
        /// </summary>
        public decimal DishPrice
        {
            get { return ((DishInfoEditWndVm)DataContext).DishPrice; }
        }

        private void DishInfoEditWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbDishNum.Focus();
        }
    }

    public class DishInfoEditWndVm : ViewModelBase
    {
        public DishInfoEditWndVm()
        {
            ConfirmCmd = new RelayCommand(Confirm, CanConfirm);
            CancelCmd = new RelayCommand(Cancel);
        }

        public Window OwnerWnd { get; set; }

        public bool AllowEditDishName { get; set; }

        public bool AllowEditDishPrice { get; set; }

        public string DishName { get; set; }

        public decimal DishPrice { get; set; }

        public decimal DishNum { get; set; }

        public ICommand ConfirmCmd { get; private set; }

        public ICommand CancelCmd { get; private set; }

        private void Confirm()
        {
            if (OwnerWnd != null)
                OwnerWnd.DialogResult = true;
        }

        private bool CanConfirm()
        {
            return DishNum > 0;
        }

        private void Cancel()
        {
            if (OwnerWnd != null)
                OwnerWnd.DialogResult = false;
        }
    }
}
