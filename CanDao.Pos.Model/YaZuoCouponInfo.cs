namespace CanDao.Pos.Model
{
    /// <summary>
    /// 雅座会员优惠券信息。
    /// </summary>
    public class YaZuoCouponInfo
    {
        /// <summary>
        /// 券 ID。
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// 券名称。
        /// </summary>
        public string CouponName { get; set; }

        /// <summary>
        /// 券数量。
        /// </summary>
        public int CouponCount { get; set; }

        /// <summary>
        /// 券金额。
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 券类型。
        /// </summary>
        public string CouponType { get; set; }
    }
}