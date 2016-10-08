namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 餐道会员消费接口返回类。
    /// </summary>
    public class CanDaoMemberSaleResponse : CanDaoMemberBaseResponse
    {
        /// <summary>
        /// 交易号。
        /// </summary>
        public string TraceCode { get; set; }

        /// <summary>
        /// 存储卡余额。
        /// </summary>
        public decimal StoreCardBalance { get; set; }

        /// <summary>
        /// 积分总额。
        /// </summary>
        public decimal IntegralOverall { get; set; }

        /// <summary>
        /// 增长的积分。
        /// </summary>
        public decimal AddIntegral { get; set; }

        /// <summary>
        /// 减少的积分。
        /// </summary>
        public decimal DecIntegral { get; set; }


        public decimal InflatedRate { get; set; }

        public decimal NetAmount { get; set; }
    }
}