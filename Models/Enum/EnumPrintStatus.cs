namespace Models.Enum
{
    /// <summary>
    /// 打印状态枚举。
    /// </summary>
    public enum EnumPrintStatus
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 打印纸用尽。
        /// </summary>
        NoPaper,
        /// <summary>
        /// 上盖打开。
        /// </summary>
        CoverOpen,
        /// <summary>
        /// 切刀异常。
        /// </summary>
        CutterError,
        /// <summary>
        /// 连接断开。
        /// </summary>
        NotReachable,
    }
}