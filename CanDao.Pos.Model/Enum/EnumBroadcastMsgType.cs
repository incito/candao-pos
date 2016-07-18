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
        Settlement2Pad = 1002,//结算指令广播给PAD。

        /// <summary>
        /// 清台指令。
        /// </summary>
        ClearTable = 1005, //清台指令。

        /// <summary>
        /// 下单指令。
        /// </summary>
        OrderDish = 2201,//下单指令。

        /// <summary>
        /// 结算指令（手环）
        /// </summary>
        Settlement2Wristband = 2002, //结算指令广播给手环
    }
}