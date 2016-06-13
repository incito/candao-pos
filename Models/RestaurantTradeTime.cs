using System;
using Models.Enum;

namespace Models
{
    /// <summary>
    /// 店铺营业时间。
    /// </summary>
    public class RestaurantTradeTime
    {
        public RestaurantTradeTime(string beginTime, string endTime, string dateType)
        {
            BeginTime = DateTime.ParseExact(beginTime, "HH:mm", null).AddDays(1);
            EndTime = DateTime.ParseExact(endTime, "HH:mm", null);
            TradeTimeType = (EnumTradeTimeType)System.Enum.Parse(typeof(EnumTradeTimeType), dateType);
            if (TradeTimeType == EnumTradeTimeType.N)
                EndTime = EndTime.AddDays(1);
        }

        /// <summary>
        /// 开业时间。
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结业时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 结业时间类型。
        /// </summary>
        public EnumTradeTimeType TradeTimeType { get; set; }
    }
}