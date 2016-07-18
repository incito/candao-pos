namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 更新发票信息请求类。
    /// </summary>
    public class UpdateInvoiceRequest
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 发票金额。
        /// </summary>
        public decimal invoice_amount { get; set; }

        /// <summary>
        /// 会员号。
        /// </summary>
        public string cardno { get; set; }
    }
}