using System;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 会员储值信息打印数据类。
    /// </summary>
    public class PrintMemberStoredInfo
    {
        /// <summary>
        /// 单据标题。
        /// </summary>
        public string ReportTitle { get; set; }

        /// <summary>
        /// 卡号。
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 商户号。
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 消费商户。
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 交易号。
        /// </summary>
        public string TraceCode { get; set; }

        /// <summary>
        /// 交易时间。
        /// </summary>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 储值金额。
        /// </summary>
        public decimal StoredAmount { get; set; }

        /// <summary>
        /// 积分余额。
        /// </summary>
        public decimal ScoreBalance { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredBalance { get; set; }
    }
}