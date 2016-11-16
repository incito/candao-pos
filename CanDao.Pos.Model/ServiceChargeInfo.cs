namespace CanDao.Pos.Model
{
    /// <summary>
    /// 服务费信息。
    /// </summary>
    public class ServiceChargeInfo
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 收费是否开启，开启为true，否则为false。
        /// </summary>
        public bool IsChargeOn { get; set; }

        /// <summary>
        /// 是否是自定义服务费。
        /// </summary>
        public bool IsCustomSetting { get; set; }

        /// <summary>
        /// 服务费修改授权人。
        /// </summary>
        public string ChargeAuthor { get; set; }

        /// <summary>
        /// 服务费金额。
        /// </summary>
        public decimal ServiceAmount { get; set; }
    }
}