namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 账单的状态。
    /// </summary>
    public enum EnumOrderStatus
    {
        /// <summary>
        /// 已下单。
        /// </summary>
        Ordered = 0,

        /// <summary>
        /// 单桌已结清。
        /// </summary>
        SingleTableSettle = 1,

        /// <summary>
        /// 联桌号已结清。
        /// </summary>
        RelatedTableSettle = 2,

        /// <summary>
        /// 内部结算。
        /// </summary>
        InternalSettle = 3,

        /// <summary>
        /// 正在下单。
        /// </summary>
        Ordering = 4,

        /// <summary>
        /// 已经取消。
        /// </summary>
        Canceled = 5,
    }
}