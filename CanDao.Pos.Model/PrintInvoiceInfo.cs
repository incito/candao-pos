using System;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 打印发票单信息。
    /// </summary>
    public class PrintInvoiceInfo
    {
        public PrintInvoiceInfo()
        {
            PrintTime = DateTime.Now;
        }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }
        
        /// <summary>
        /// 桌号。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 发票金额。
        /// </summary>
        public decimal InvoiceAmount { get; set; }

        /// <summary>
        /// 发票抬头。
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 打印时间。
        /// </summary>
        public DateTime PrintTime { get; set; }
    }
}