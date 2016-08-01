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
        public SelectGiftDishWndVm(TableFullInfo tableFullInfo)
        {
            GiftDishes = new ObservableCollection<GiftDishInfo>();
            ParseTableFullInfo(tableFullInfo);
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
            var giftDishInfo = (GiftDishInfo) arg;
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
        /// <param name="data"></param>
        private void ParseTableFullInfo(TableFullInfo data)
        {
            var dic = GetGiftDishCouponDic(data.UsedCouponInfos);
            foreach (var orderDishInfo in data.DishInfos)
            {
                if (orderDishInfo.PayAmount == 0)//价格为0的不赠送
                    continue;

                //赠菜可选你数量需要减去已经赠菜的数量。
                var dishNum = orderDishInfo.DishNum;
                if (dic.ContainsKey(orderDishInfo.DishName))
                {
                    dishNum -= dic[orderDishInfo.DishName];
                    if (dishNum == 0)
                        continue;
                }

                GiftDishes.Add(new GiftDishInfo
                {
                    DishName = orderDishInfo.DishName,
                    DishPrice = orderDishInfo.Price,
                    DishNum = dishNum,
                });
            }
        }

        /// <summary>
        /// 获取赠菜类优惠券字段。
        /// </summary>
        /// <param name="coupons">优惠券集合。</param>
        /// <returns></returns>
        private Dictionary<string, int> GetGiftDishCouponDic(IEnumerable<UsedCouponInfo> coupons)
        {
            var dic = new Dictionary<string, int>();
            foreach (var usedCouponInfo in coupons)
            {
                if (!usedCouponInfo.Name.StartsWith("赠菜："))
                    continue;

                var array = usedCouponInfo.Name.Split('：');
                if (array.Count() < 2)
                    continue;

                var dishName = array[1];
                var count = usedCouponInfo.Count;
                if (dic.ContainsKey(dishName))
                    dic[dishName] += count;
                else
                    dic.Add(dishName, count);
            }

            return dic;
        }
    }
}