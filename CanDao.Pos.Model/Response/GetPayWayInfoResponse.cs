namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取支付方式信息的返回类。
    /// </summary>
    public class GetPayWayInfoResponse : NewHttpBaseListResponse<PayWayInfoResponse>
    {

    }

    /// <summary>
    /// 支付方式信息。
    /// </summary>
    public class PayWayInfoResponse
    {
        /// <summary>
        /// 支付方式ID。
        /// </summary>
        public int itemId { get; set; }
        /// <summary>
        /// 支付方式名称。
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 支付方式状态，0隐藏/1显示。
        /// </summary>
        public byte status { get; set; }
        /// <summary>
        /// 是否计入实收，0不计入/1计入。
        /// </summary>
        public byte chargeStatus { get; set; }
    }
}