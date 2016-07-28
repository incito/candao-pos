using System.Collections.Generic;
using System.Collections.ObjectModel;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 订单菜品信息。
    /// </summary>
    public class OrderDishInfo : BaseNotifyObject
    {
        public OrderDishInfo()
        {
            Diet = "";
            Taste = "";
        }

        /// <summary>
        /// 下单用户Id。
        /// </summary>
        public string UserName { get; set; }

        private decimal _dishNum;
        private decimal _payAmount;

        /// <summary>
        /// 菜品数量。
        /// </summary>
        public decimal DishNum
        {
            get { return _dishNum; }
            set
            {
                _dishNum = value;
                RaisePropertyChanged("DishNum");
            }
        }

        /// <summary>
        /// 菜品单位。
        /// </summary>
        public string DishUnit { get; set; }

        /// <summary>
        /// 菜品原始单位。（For 国际化）
        /// </summary>
        public string SrcDishUnit { get; set; }

        /// <summary>
        /// 菜品Id。
        /// </summary>
        public string DishId { get; set; }

        /// <summary>
        /// 关键菜品Id。套餐和鱼锅内部的菜以这个字段标识它们的主体菜。
        /// </summary>
        public string RelateDishId { get; set; }

        /// <summary>
        /// 唯一标示。
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 菜品状态。
        /// </summary>
        public EnumDishStatus DishStatus { get; set; }

        /// <summary>
        /// 菜品类型。
        /// </summary>
        public EnumDishType DishType { get; set; }
        /// <summary>
        /// 临时菜名称
        /// </summary>
        private string _tempDishName;
        /// <summary>
        /// 临时菜名称。
        /// </summary>
        public string TempDishName
        {
            get { return _tempDishName; }
            set
            {
                _tempDishName = value;
                RaisePropertyChanged("TempDishName");
            }
        }

        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 菜品描述。
        /// </summary>
        public string DishDesc { get; set; }

        /// <summary>
        /// 金额。
        /// </summary>
        public decimal PayAmount
        {
            get { return _payAmount; }
            set
            {
                _payAmount = value;
                RaisePropertyChanged("PayAmount");
            }
        }

        /// <summary>
        /// 菜的单价。等于原价或者会员价。
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 菜的原价。
        /// </summary>
        public decimal PriceSource { get; set; }

        /// <summary>
        /// 会员价。
        /// </summary>
        public decimal MemberPrice { get; set; }

        /// <summary>
        /// 菜品所属分组ID。
        /// </summary>
        public string MenuGroupId { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 下单类型。
        /// </summary>
        public EnumOrderType OrderType { get; set; }

        /// <summary>
        /// 是否是主菜。
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 是否是套餐内的菜。
        /// </summary>
        public bool IsComboDish { get; set; }

        /// <summary>
        /// 是否是鱼锅内的菜。
        /// </summary>
        public bool IsFishPotDish { get; set; }

        /// <summary>
        /// 是否是锅底。
        /// </summary>
        public bool IsPot { get; set; }

        /// <summary>
        /// 菜品选择的口味。
        /// </summary>
        private string _taste;
        /// <summary>
        /// 菜品选择的口味。
        /// </summary>
        public string Taste
        {
            get { return _taste; }
            set
            {
                _taste = value;
                RaisePropertyChanged("Taste");
            }
        }

        /// <summary>
        /// 菜品设置的忌口。
        /// </summary>
        private string _diet;
        /// <summary>
        /// 菜品设置的忌口。
        /// </summary>
        public string Diet
        {
            get { return _diet; }
            set
            {
                _diet = value;
                RaisePropertyChanged("Diet");
            }
        }

        /// <summary>
        /// 赠菜人ID。（收银员）
        /// </summary>
        public string FreeUserId { get; set; }

        /// <summary>
        /// 赠菜授权人ID。
        /// </summary>
        public string FreeAuthorizeId { get; set; }

        /// <summary>
        /// 赠菜原因。
        /// </summary>
        public string FreeReason { get; set; }

        /// <summary>
        /// 套餐的内部菜集合。
        /// </summary>
        public List<OrderDishInfo> DishInfos { get; set; }
    }
}