namespace Models.Enum
{
    /// <summary>
    /// 品项销售周期类型。
    /// </summary>
    public enum EnumStatisticsPeriodsType
    {
        None,

        /// <summary>
        /// 今天。
        /// </summary>
        Today = 1,

        /// <summary>
        /// 本周。
        /// </summary>
        ThisWeek,

        /// <summary>
        /// 本月。
        /// </summary>
        ThisMonth,

        /// <summary>
        /// 上月。
        /// </summary>
        LastMonth,
    }
}