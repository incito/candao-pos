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
        /// 已取消。
        /// </summary>
        CanceledOrder = 2,

        /// <summary>
        /// 已结算。
        /// </summary>
        InternalSettle = 3,
    }
}