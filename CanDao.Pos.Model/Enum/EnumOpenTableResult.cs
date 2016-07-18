namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 开台状态枚举。
    /// </summary>
    public enum EnumOpenTableResult
    {
        /// <summary>
        /// 开台成功。
        /// </summary>
        Success = 0,

        /// <summary>
        /// 餐台被占用。
        /// </summary>
        Occupied = 1,

        /// <summary>
        /// 没有这个餐台。
        /// </summary>
        NoTableName = 2,

        /// <summary>
        /// 没有开业。
        /// </summary>
        NoOpenUp = 3,
    }
}