using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取统计信息返回类基类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetReportStatisticInfoBase<T> : JavaResponse where T : class
    {

        public string msg { get; set; }

        public PeriodTimeResponse time { get; set; }

        public List<T> data { get; set; }
    }

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