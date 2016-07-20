using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 优惠券信息。
    /// </summary>
    public class CouponInfo : ICloneable
    {
        /// <summary>
        /// 优惠券名称。对应name，当name为空时对应company_name，company_name为空时对应free_reason。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 合作单位。
        /// </summary>
        public string PartnerName { get; set; }

        /// <summary>
        /// 优惠劵明细Id。对应id。
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 优惠券主表Id。对应preferential
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// 最大使用优惠劵数量。
        /// </summary>
        public int MaxCouponCount { get; set; }

        /// <summary>
        ///  适用菜品的Id。对应dish。
        /// </summary>
        public string DishId { get; set; }

        /// <summary>
        /// 优惠劵类型。
        /// </summary>
        public EnumCouponType CouponType { get; set; }

        /// <summary>
        /// 账单金额。
        /// </summary>
        public decimal? BillAmount { get; set; }

        /// <summary>
        /// 实际金额。
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 优免金额。
        /// </summary>
        public decimal? FreeAmount { get; set; }

        /// <summary>
        /// 挂账金额。
        /// </summary>
        public decimal DebitAmount
        {
            get { return BillAmount.HasValue ? (Amount ?? 0) : 0; }
        }

        /// <summary>
        /// 优惠券设置的颜色。
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 折扣率。
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 手工优惠的优免类型枚举。
        /// </summary>
        public EnumHandCouponType? HandCouponType { get; set; }

        /// <summary>
        /// 是否是不常用优惠。
        /// </summary>
        public bool IsUncommonlyUsed { get; set; }

        /// <summary>
        /// 是否是折扣类优惠券。
        /// </summary>
        public bool IsDiscount
        {
            get { return CouponType == EnumCouponType.Discounts || CouponType == EnumCouponType.Special || Discount > 0; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}