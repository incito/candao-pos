using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Models;

namespace Library
{
    /// <summary>
    /// 选择赠送菜品窗口。
    /// </summary>
    public partial class SelectGiftDishWindow
    {
        public SelectGiftDishWindow(DataTable dbTable, List<string> giftReasons = null)
        {
            InitializeComponent();
            GiftDishes = new ObservableCollection<GiftDishInfo>();
            GiftReasons = new ObservableCollection<string>();

            ParseDbTable(dbTable);

            if (giftReasons != null)
            {
                giftReasons.ForEach(GiftReasons.Add);
            }
            else
            {
                TbGiftReason.Visibility = Visibility.Collapsed;
                LbGiftReason.Visibility = Visibility.Collapsed;
            }

            DataContext = this;
        }

        /// <summary>
        /// 赠菜集合。
        /// </summary>
        public ObservableCollection<GiftDishInfo> GiftDishes { get; private set; }

        public ObservableCollection<string> GiftReasons { get; private set; }

        public List<GiftDishInfo> SelectedGiftDishInfos
        {
            get { return GiftDishes.Where(t => t.IsSelected).ToList(); }
        }

        public string SelectedReason { get; set; }

        /// <summary>
        /// 确定按钮点击时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDishes = SelectedGiftDishInfos;
            if (!selectedDishes.Any())
            {
                frmBase.Warning("请选择一个赠菜。");
                return;
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// 取消按钮点击时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Bd_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((GiftDishInfo) ((System.Windows.Controls.Border) sender).DataContext).SelectGiftNum++;
        }

        private void ParseDbTable(DataTable dbTable)
        {
            foreach (DataRow row in dbTable.Rows)
            {
                var amount = row["amount"].ToString();
                if (string.IsNullOrEmpty(amount) || amount.Equals("0"))
                    continue;

                var item = new GiftDishInfo
                {
                    DishName = row["title"].ToString(),
                    DishPrice = Convert.ToDecimal(row["orderprice"].ToString()),
                    DishNum = Convert.ToDecimal(row["dishnum"].ToString()),
                    Amount = Convert.ToDecimal(row["amount"].ToString())
                };
                GiftDishes.Add(item);
            }
        }

    }
}
