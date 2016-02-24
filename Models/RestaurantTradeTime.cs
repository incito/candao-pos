using System;

namespace Models
{
    /// <summary>
    /// 店铺营业时间。
    /// </summary>
    public class RestaurantTradeTime
    {
        public RestaurantTradeTime(string beginTime, string endTime)
        {
            BeginTime = DateTime.ParseExact(beginTime, "HH:mm", null).AddDays(1);
            EndTime = DateTime.ParseExact(endTime, "HH:mm", null);
        }

        /// <summary>
        /// 开业时间。
        /// </summary>
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}