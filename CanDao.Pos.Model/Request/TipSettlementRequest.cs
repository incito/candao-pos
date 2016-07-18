namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 小费结算请求类。
    /// </summary>
    public class TipSettlementRequest
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 小费实付金额。
        /// </summary>
        public decimal paid { get; set; }
    }
}