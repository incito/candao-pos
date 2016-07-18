namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取营业时间返回类。
    /// </summary>
    public class GetTradeTimeResponse : JavaResponse
    {
        public TradeTimeRespons detail { get; set; }
    }

    /// <summary>
    /// 营业时间返回值。
    /// </summary>
    public class TradeTimeRespons
    {
        /// <summary>
        /// 开业时间。
        /// </summary>
        public string begintime { get; set; }

        /// <summary>
        /// 结业时间。
        /// </summary>
        public string endtime { get; set; }
    }
}