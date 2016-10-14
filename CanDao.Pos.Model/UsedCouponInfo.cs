using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 使用的优惠券信息。 
    /// </summary>
    public class UsedCouponInfo
    {
        /// <summary>
        /// 使用优惠券与账单关系ID
        /// </summary>
        public string RelationId { set; get; }

        /// <summary>
        /// 使用的优惠券。
        /// </summary>
        public CouponInfo CouponInfo { get; set; }

        /// <summary>
        /// 使用的优惠券类型。
        /// </summary>
        public EnumUsedCouponType UsedCouponType { get; set; }

        /// <summary>
        /// 是否是折扣类优惠券。
        /// </summary>
        public bool IsDiscount { get; set; }

        /// <summary>
        /// 券名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 优免金额。
        /// </summary>
        public decimal FreeAmount { get; set; }

        /// <summary>
        /// 挂账金额。
        /// </summary>
        public decimal DebitAmount { get; set; }

        /// <summary>
        /// 账单金额。
        /// </summary>
        public decimal BillAmount { get; set; }
    }
}