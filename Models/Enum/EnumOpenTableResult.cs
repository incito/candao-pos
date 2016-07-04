namespace Models.Enum
{
    /// <summary>
    /// 开台接口结果枚举。
    /// </summary>
    public enum EnumOpenTableResult
    {
        /// <summary>
        /// 成功。
        /// </summary>
        Success = 0,

        /// <summary>
        /// 餐台被占用。
        /// </summary>
        Occupied = 1,

        /// <summary>
        /// 未找到餐台。
        /// </summary>
        TableNotExist = 2,

        /// <summary>
        /// 未开业。
        /// </summary>
        NoOpenUp = 3,
    }
}