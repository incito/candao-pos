namespace CanDao.Pos.Model
{
    /// <summary>
    /// 雅座储值信息。
    /// </summary>
    public class YaZuoStorageInfo
    {
        /// <summary>
        /// 会员卡号。
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 交易流水号。
        /// </summary>
        public string TradeCode { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredBalance { get; set; }

        /// <summary>
        /// 积分余额。
        /// </summary>
        public decimal IntegralBalance { get; set; }

    }
}