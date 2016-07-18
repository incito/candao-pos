using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取订单发票返回类。
    /// </summary>
    public class GetOrderInvoiceResponse : JavaResponse
    {
        public List<OrderInvoiceResponse> data { get; set; }
    }

    /// <summary>
    /// 订单发票返回类。
    /// </summary>
    public class OrderInvoiceResponse
    {
        /// <summary>
        /// 发票抬头。
        /// </summary>
        public string invoice_title { get; set; }
    }
}