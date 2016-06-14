namespace Models.Enum
{
    /// <summary>
    /// 打印状态枚举。
    /// </summary>
    public enum EnumPrintStatus
    {
        /// <summary>
        /// 未知。
        /// </summary>
        None,

        /// <summary>
        /// 较差。
        /// </summary>
        Bad,

        /// <summary>
        /// 良好
        /// </summary>
        Good,

        /// <summary>
        /// 连接断开。
        /// </summary>
        NotReachable,
    }
}