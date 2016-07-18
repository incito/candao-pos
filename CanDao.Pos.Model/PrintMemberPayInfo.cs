using System;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 会员支付信息打印数据类。
    /// </summary>
    public class PrintMemberPayInfo
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
        /// 交易类型。
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 商户号。
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 消费商户。
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 终端号。
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 收银员。
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 主机流水号。
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 交易时间。
        /// </summary>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 积分增减。
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 储值消费。
        /// </summary>
        public decimal StoredPay { get; set; }

        /// <summary>
        /// 券消费。
        /// </summary>
        public decimal Coupons { get; set; }

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