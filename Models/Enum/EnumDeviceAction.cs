namespace Models.Enum
{
    /// <summary>
    /// 设备行为枚举。
    /// </summary>
    public enum EnumDeviceAction
    {
        None = 0,

        /// <summary>
        /// 开台。
        /// </summary>
        OpenTable = 101,

        /// <summary>
        /// 外卖台开始。
        /// </summary>
        //TakeoutBegin = 102,

        /// <summary>
        /// 外卖台结账完成。
        /// </summary>
        //TakeoutEnd = 103,

        /// <summary>
        /// 堂食结账开始。
        /// </summary>
        DineSettleBegin = 104,

        /// <summary>
        /// 堂食结账结束。
        /// </summary>
        DineSettleEnd = 105,

        /// <summary>
        /// 堂食下单开始。(在开台确认后开始）
        /// </summary>
        DineOrderBegin = 106,

        /// <summary>
        /// 堂食下单结束。
        /// </summary>
        DineOrderEnd = 107,

        /// <summary>
        /// 应用启动。
        /// </summary>
        ApplicationStart = 108,

        /// <summary>
        /// 预结单按钮点击时。
        /// </summary>
        PresettleClicking = 201,

        /// <summary>
        /// 反结按钮点击时。
        /// </summary>
        ResettleClicking = 202,

        /// <summary>
        /// 点菜按钮点击时。
        /// </summary>
        OrderDishClicking = 203,

        /// <summary>
        /// 开钱箱按钮点击时。
        /// </summary>
        OpenCashBoxClicking = 204,

        /// <summary>
        /// 重印结账单按钮点击时。
        /// </summary>
        ReprintSettleBill = 205,

        /// <summary>
        /// 重印客用单按钮时。
        /// </summary>
        ReprintCustBill = 206,

        /// <summary>
        /// 重印交易凭条按钮时。
        /// </summary>
        RepringMemberBill = 207,

        /// <summary>
        /// 取消订单按钮点击时。
        /// </summary>
        CancelOrder = 208,

        /// <summary>
        /// 保留零钱按钮点击时。
        /// </summary>
        KeepSamlChange = 209,

        /// <summary>
        /// 团购按钮点击时。
        /// </summary>
        GroupBuyClicking = 301,

        /// <summary>
        /// 特价券按钮点击时。
        /// </summary>
        SpecialOfferClicking = 302,

        /// <summary>
        /// 折扣券按钮点击时。
        /// </summary>
        DiscountClicking = 303,

        /// <summary>
        /// 代金券按钮点击时。
        /// </summary>
        VouchersClicking = 304,

        /// <summary>
        /// 礼品券按钮点击时。
        /// </summary>
        GiftCouponClicking = 305,

        /// <summary>
        /// 会员优惠券按钮点击时。
        /// </summary>
        MemberCouponClicking = 306,

        /// <summary>
        /// 其他优惠按钮点击时。
        /// </summary>
        OtherCouponClicking = 308,

        /// <summary>
        /// 合作单位按钮就点击时。
        /// </summary>
        CooperationCompanyClicking = 309,

        /// <summary>
        /// 现金按钮点击时。
        /// </summary>
        SettleCashClicking = 401,

        /// <summary>
        /// 银行卡按钮点击时。
        /// </summary>
        SettleBankClicking = 402,
        
        /// <summary>
        /// 会员卡按钮点击时。
        /// </summary>
        SettleMemberClicking = 403,

        /// <summary>
        /// 挂账按钮点击时。
        /// </summary>
        SettleOnAccountClicking = 404,

        /// <summary>
        /// 支付宝按钮点击时。
        /// </summary>
        SettleAlipayClicking = 405,

        /// <summary>
        /// 微信支付按钮点击时。
        /// </summary>
        SettleWechatClicking = 406,

        /// <summary>
        /// 报表按钮点击时。
        /// </summary>
        ReportClicking = 407,
    }
}