namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 账单付款方式枚举。
    /// </summary>
    public enum EnumBillPayType
    {
        /// <summary>
        /// 现金。
        /// </summary>
        Cash = 0,

        /// <summary>
        /// 银行卡。
        /// </summary>
        BankCard = 1,

        /// <summary>
        /// 会员储值。
        /// </summary>
        MemberSotred = 2,

        /// <summary>
        /// 优免券。（没地方用）
        /// </summary>
        Coupon = 3,

        /// <summary>
        /// 折扣券。
        /// </summary>
        Discount = 4,

        /// <summary>
        /// 挂账。（团购类优惠券挂账）
        /// </summary>
        OnAccount = 5,

        /// <summary>
        /// 优免。
        /// </summary>
        FreeAmount = 6,

        /// <summary>
        /// 抹零。
        /// </summary>
        RemoveOdd = 7,

        /// <summary>
        /// 会员卡。
        /// </summary>
        MemberCard = 8,

        /// <summary>
        /// 会员积分。
        /// </summary>
        MemberIntegral = 11,

        /// <summary>
        /// 雅座会员优惠券。
        /// </summary>
        YazuoMemberCoupon = 12,

        /// <summary>
        /// 挂账到单位。
        /// </summary>
        OnCompanyAccount = 13,

        /// <summary>
        /// 微信。
        /// </summary>
        Wechat = 17,

        /// <summary>
        /// 支付宝。
        /// </summary>
        Alipay = 18,

        /// <summary>
        /// 四舍五入调整。
        /// </summary>
        Rounding = 20,
    }
}