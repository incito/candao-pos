namespace Models.Request
{
    /// <summary>
    /// 结算小费请求类。
    /// </summary>
    public class BillingTipRequest
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 小费实付金额。
        /// </summary>
        public float paid { get; set; }
    }
}