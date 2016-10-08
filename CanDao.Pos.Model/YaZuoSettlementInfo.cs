namespace CanDao.Pos.Model
{
    /// <summary>
    /// 雅座结算信息。
    /// </summary>
    public class YaZuoSettlementInfo
    {
        public YaZuoSettlementInfo()
        {
            TransType = "1";
        }

        /// <summary>
        /// 用户ID。
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 会员卡号。
        /// </summary>
        public string MemberCardNo { get; set; }

        /// <summary>
        /// 现金金额。
        /// </summary>
        public decimal CashAmount { get; set; }

        /// <summary>
        /// 积分金额。
        /// </summary>
        public decimal IntegralValue { get; set; }

        /// <summary>
        /// 交易类型。
        /// </summary>
        public string TransType { get; set; }

        /// <summary>
        /// 储值消费金额。
        /// </summary>
        public decimal StoredPayAmount { get; set; }

        /// <summary>
        /// 优惠券使用信息。
        /// </summary>
        public string CouponUsedInfo { get; set; }

        /// <summary>
        /// 会员卡密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 优惠券优惠总金额
        /// </summary>
        public decimal CouponTotalAmount { get; set; }

        public string CheckDataValid()
        {
            return null;
        }
    }
}