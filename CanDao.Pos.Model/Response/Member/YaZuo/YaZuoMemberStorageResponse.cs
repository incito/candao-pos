namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 雅座会员储值返回类。
    /// </summary>
    public class YaZuoMemberStorageResponse : YaZuoMemberBaseResponse
    {
        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal psStoreCardBalance { get; set; }

        /// <summary>
        /// 会员卡编号。
        /// </summary>
        public string pszPan { get; set; }

        /// <summary>
        /// 没用的字段。
        /// </summary>
        public string pszRefnum { get; set; }

        /// <summary>
        /// 没用的字段。
        /// </summary>
        public string pszRestInfo { get; set; }

        /// <summary>
        /// 没用的字段。
        /// </summary>
        public string pszRetcode { get; set; }

        /// <summary>
        /// 交易流水号。
        /// </summary>
        public string pszTrace { get; set; }
    }
}