using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Common;
using Models;

namespace Library
{
    /// <summary>
    /// 选择赠送菜品窗口。
    /// </summary>
    public partial class SelectGiftDishWindow
    {
        public SelectGiftDishWindow(DataTable dbTable, DataTable couponTb, List<string> giftReasons = null)
        {
            InitializeComponent();
            GiftDishes = new ObservableCollection<GiftDishInfo>();
            GiftReasons = new ObservableCollection<string>();

            ParseDbTable(dbTable, couponTb);

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
            ((GiftDishInfo)((System.Windows.Controls.Border)sender).DataContext).SelectGiftNum++;
        }

        private void ParseDbTable(DataTable dbTable, DataTable couponDb)
        {
            var dic = GetGiftDishDic(couponDb);
            var dinnerWareName = Globals.cjFood.Count > 0 ? Globals.cjFood[0]["dishname"].ToString() : "";
            foreach (DataRow row in dbTable.Rows)
            {
                var dishName = row["title"].ToString();
                var amount = row["amount"].ToString();
                if (string.IsNullOrEmpty(amount) || amount.Equals("0") || dishName.Equals(dinnerWareName))
                    continue;

                //赠菜可选数量需要减去已经赠菜的优惠数量。
                var dishNum = Convert.ToDecimal(row["dishnum"].ToString());
                if (dic.ContainsKey(dishName))
                {
                    dishNum -= dic[dishName];
                    if (dishNum == 0)
                        continue;
                }

                var item = new GiftDishInfo
                {
                    DishName = dishName,
                    DishPrice = Convert.ToDecimal(row["orderprice"].ToString()),
                    DishNum = dishNum,
                    Amount = Convert.ToDecimal(row["amount"].ToString())
                };
                GiftDishes.Add(item);
            }
        }

        private Dictionary<string, decimal> GetGiftDishDic(DataTable couponDb)
        {
            var dic = new Dictionary<string, decimal>();

            foreach (DataRow row in couponDb.Rows)
            {
                var couponName = row["yhname"].ToString();
                if (!couponName.StartsWith("赠菜"))
                    continue;

                var array = couponName.Split('：');
                if (array.Count() < 2)
                    continue;

                var dishName = array[1];
                var count = Convert.ToDecimal(row["num"]);
                if (dic.ContainsKey(dishName))
                    dic[dishName] += count;
                else
                    dic.Add(dishName, count);
            }

            return dic;
        }
    }
}
