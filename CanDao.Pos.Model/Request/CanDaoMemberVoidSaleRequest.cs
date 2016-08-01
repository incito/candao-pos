namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员反结算请求类。
    /// </summary>
    public class CanDaoMemberVoidSaleRequest : CanDaoMemberQueryRequest
    {
        public CanDaoMemberVoidSaleRequest(string orderId)
        {
            Serial = orderId;
            SUPERPWD = "";
        }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// 交易号。
        /// </summary>
        public string Tracecode { get; set; }

        /// <summary>
        /// 超级密码。
        /// </summary>
        public string SUPERPWD { get; set; }
    }
}