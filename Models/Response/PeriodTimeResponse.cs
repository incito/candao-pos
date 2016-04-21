namespace Models.Response
{
    /// <summary>
    /// 统计周期返回类。
    /// </summary>
    public class PeriodTimeResponse
    {
        /// <summary>
        /// 统计起始时间。
        /// </summary>
        public string startTime { get; set; }

        /// <summary>
        /// 统计结束时间。
        /// </summary>
        public string endTime { get; set; }
    }
}