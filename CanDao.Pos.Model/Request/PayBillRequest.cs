using System.Collections.Generic;

namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 账单付款请求类。
    /// </summary>
    public class PayBillRequest
    {
        public List<PayBillDetailRequest> payDetail { get; set; }

        public string userName { get; set; }

        public string orderNo { get; set; }
    }

    /// <summary>
    /// 付款明细类。
    /// </summary>
    public class PayBillDetailRequest
    {
        public string payWay { get; set; }

        public decimal payAmount { get; set; }

        public string memerberCardNo { get; set; }

        public string bankCardNo { get; set; }

        public string couponnum { get; set; }

        public string couponid { get; set; }

        public string coupondetailid { get; set; }
    }
}