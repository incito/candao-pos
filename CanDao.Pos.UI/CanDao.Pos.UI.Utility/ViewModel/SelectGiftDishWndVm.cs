using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Model;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 赠菜选择窗口的VM。
    /// </summary>
    public class SelectGiftDishWndVm : NormalWindowViewModel
    {
        public SelectGiftDishWndVm(IEnumerable<OrderDishInfo> data, List<DishGiftCouponInfo> dishGiftCouponInfos)
        {
            GiftDishes = new ObservableCollection<GiftDishInfo>();
            ParseTableFullInfo(data, dishGiftCouponInfos);
        }

        /// <summary>
        /// 所有可赠送菜品集合。
        /// </summary>
        public ObservableCollection<GiftDishInfo> GiftDishes { get; private set; }

        /// <summary>
        /// 选择的赠菜集合。
        /// </summary>
        public List<GiftDishInfo> SelectedGiftDishInfos
        {
            get { return GiftDishes.Where(t => t.IsSelected).ToList(); }
        }

        /// <summary>
        /// 选择一个赠菜命令。
        /// </summary>
        public ICommand SelectDishCmd { get; private set; }

        /// <summary>
        /// 选择一个赠菜命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void SelectDish(object arg)
        {
            var giftDishInfo = (GiftDishInfo)arg;
            if (giftDishInfo != null)
                giftDishInfo.SelectGiftNum++;
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            SelectDishCmd = CreateDelegateCommand(SelectDish);
        }

        protected override bool CanConfirm(object param)
        {
            return SelectedGiftDishInfos.Any();
        }

        /// <summary>
        /// 解析餐桌的菜品列表和优惠列表，生成可赠菜集合。
        /// </summary>
        /// <param name="data">菜品集合。</param>
        /// <param name="dishGiftCouponInfos">赠菜优惠券的使用信息。</param>
        private void ParseTableFullInfo(IEnumerable<OrderDishInfo> data, List<DishGiftCouponInfo> dishGiftCouponInfos)
        {
            foreach (var orderDishInfo in data)
            {
                if (orderDishInfo.PayAmount == 0)//价格为0的不赠送
                    continue;

                //赠菜可选的数量需要减去已经赠菜的数量。
                var dishNum = orderDishInfo.DishNum;
                var item = dishGiftCouponInfos.FirstOrDefault(t => t.DishId.Equals(orderDishInfo.DishId));
                if (item != null)
                {
                    var tempNum = Math.Min(dishNum, item.UsedCouponCount);
                    dishNum -= tempNum;

                    item.UsedCouponCount -= tempNum;
                    if (item.UsedCouponCount == 0)
                        dishGiftCouponInfos.Remove(item);

                    if (dishNum <= item.UsedCouponCount)
                        continue;
                }

                GiftDishes.Add(new GiftDishInfo
                {
                    DishId = orderDishInfo.DishId,
                    DishName = orderDishInfo.DishName,
                    DishPrice = orderDishInfo.Price,
                    DishNum = dishNum,
                    DishUnit = orderDishInfo.DishUnit,
                });
            }
        }
    }
}