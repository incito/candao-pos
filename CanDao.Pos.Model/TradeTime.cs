using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 店铺营业时间。
    /// </summary>
    public class TradeTime
    {

        public TradeTime(string beginTime, string endTime, string dateType)
        {
            BeginTime = DateTime.ParseExact(beginTime, "HH:mm", null).AddDays(1);
            EndTime = DateTime.ParseExact(endTime, "HH:mm", null);

            if (string.IsNullOrEmpty(dateType))//考虑兼容以前版本没有传这个字段。
                TradeTimeType = EnumTradeTimeType.T;
            else
                TradeTimeType = (EnumTradeTimeType)System.Enum.Parse(typeof(EnumTradeTimeType), dateType);

            if (TradeTimeType == EnumTradeTimeType.N)//当标记为次日的话，结业时间加一天。
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