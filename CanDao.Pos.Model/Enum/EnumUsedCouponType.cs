namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 使用的优惠券类型枚举。
    /// </summary>
    public enum EnumUsedCouponType
    {
        /// <summary>
        /// 普通使用的优惠。
        /// </summary>
        NormalUsed = 0,

        /// <summary>
        /// 服务员优惠。
        /// </summary>
        WaiterUsed = 1,

        /// <summary>
        /// 自动使用优惠。
        /// </summary>
        AutoUsed = 2,

        /// <summary>
        /// 赠菜优惠。
        /// </summary>
        FreeDish = 4,

        /// <summary>
        /// 雅座优惠。
        /// </summary>
        YaZuo = 5,
    }
}