namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 保存服务费的请求类。
    /// </summary>
    public class SaveServiceChargeRequest
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 授权人编号。
        /// </summary>
        public string autho { get; set; }

        /// <summary>
        /// 是否开启账单服务费。0关闭，1开启。
        /// </summary>
        public int chargeOn { get; set; }

        /// <summary>
        /// 手动设置金额。
        /// </summary>
        public decimal chargeAmount { get; set; }
    }
}