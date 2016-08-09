using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CanDao.Pos.Model;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 套餐选择控件。
    /// </summary>
    public partial class ComboSelectorControl
    {

        public ComboSelectorControl()
        {
            InitializeComponent();
        }

        public MenuComboDishInfo Data
        {
            get { return (MenuComboDishInfo)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(MenuComboDishInfo), typeof(ComboSelectorControl), new PropertyMetadata(null));

        /// <summary>
        /// 套餐选择信息。
        /// </summary>
        public string ComboSelectInfo
        {
            get { return (string)GetValue(ComboSelectInfoProperty); }
            set { SetValue(ComboSelectInfoProperty, value); }
        }

        public static readonly DependencyProperty ComboSelectInfoProperty =
            DependencyProperty.Register("ComboSelectInfo", typeof(string), typeof(ComboSelectorControl), new PropertyMetadata(""));

        private void ComboSelectorControl_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Data = e.NewValue as MenuComboDishInfo;
            UpdateSelectedCount();
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox) sender).SelectAll();
            var dishInfo = ((TextBox)sender).DataContext as MenuDishInfo;
            if (dishInfo == null)
                return;

            if (Data.SelectCount != 1)
                return;

            SetOtherDishSelectNone(dishInfo);
            dishInfo.SelectedCount = 1;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var dishInfo = (MenuDishInfo)((TextBox)sender).DataContext;
            var totalSelectedCount = Data.SourceDishes.Sum(t => t.SelectedCount);
            if (totalSelectedCount > Data.SelectCount)//当总数超出了可选数量时，将其他选择的菜品选择数量设置为0。
                SetOtherDishSelectNone(dishInfo);

            dishInfo.SelectedCount = Math.Min(dishInfo.SelectedCount, Data.SelectCount);
            UpdateSelectedCount();
        }

        /// <summary>
        /// 设定指定菜品之外的菜品选择数量为0。
        /// </summary>
        /// <param name="dishInfo"></param>
        private void SetOtherDishSelectNone(MenuDishInfo dishInfo)
        {
            var otherDishInfoes = Data.SourceDishes.Where(t => !t.Equals(dishInfo)).ToList();
            otherDishInfoes.ForEach(t => t.SelectedCount = 0);
        }

        /// <summary>
        /// 更新已选个数。
        /// </summary>
        private void UpdateSelectedCount()
        {
            var totalSelectedCount = Data.SourceDishes.Sum(t => t.SelectedCount);
            ComboSelectInfo = string.Format("{0}（{1}选{2}）  已选{3}", Data.ComboName, Data.SourceCount, Data.SelectCount, totalSelectedCount);
            BdName.Background = Convert2Brush(totalSelectedCount >= Data.SelectCount ? "Green" : "Coral");
        }

        private Brush Convert2Brush(string colorName)
        {
            return (Brush)TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString(colorName);
        }
    }
}
