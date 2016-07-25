namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 广播指令枚举。
    /// </summary>
    public enum EnumBroadcastMsgType
    {
        None = 0,

        /// <summary>
        /// 结算指令（PAD）
        /// </summary>
        Settlement2Pad = 1002,

        /// <summary>
        /// 清台指令。
        /// </summary>
        ClearTable = 1005, 

        /// <summary>
        /// 下单指令。
        /// </summary>
        OrderDish = 2201,

        /// <summary>
        /// 结算指令（手环）
        /// </summary>
        Settlement2Wristband = 2002, 

        /// <summary>
        /// 咖啡模式下结账指定（PAD）
        /// </summary>
        Settlement2PadCf = 2501,
    }
}