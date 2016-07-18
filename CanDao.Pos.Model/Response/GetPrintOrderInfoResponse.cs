namespace CanDao.Pos.Model.Response
{
    public class GetPrintOrderInfoResponse : RestOrderListJsResponse<PrintOrderInfoResponse, PayDetailResponse, OrderDishDataResponse>
    {

    }

    public class PayDetailResponse
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 结算类别。
        /// </summary>
        public string itemDesc { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        public int incometype { get; set; }

        /// <summary>
        /// 付款金额。
        /// </summary>
        public decimal payamount { get; set; }

        /// <summary>
        /// 付款方式。
        /// </summary>
        public int payway { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        public int couponNum { get; set; }

        /// <summary>
        /// 会员号。
        /// </summary>
        public string membercardno { get; set; }

        /// <summary>
        /// 银行卡号/优惠名称。
        /// </summary>
        public string bankcardno { get; set; }
    }
}