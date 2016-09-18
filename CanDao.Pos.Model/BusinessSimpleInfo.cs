namespace CanDao.Pos.Model
{
    /// <summary>
    /// 店铺营业简单信息。
    /// </summary>
    public class BusinessSimpleInfo
    {
        /// <summary>
        /// 未结金额。
        /// </summary>
        public decimal UnpaiedAmount { get; set; }

        /// <summary>
        /// 已结金额。
        /// </summary>
        public decimal PaiedAmount { get; set; }

        /// <summary>
        /// 已结单数。
        /// </summary>
        public int PaiedOrderCount { get; set; }

        /// <summary>
        /// 总金额。
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总人数。
        /// </summary>
        public int TotalDinnerCount { get; set; }
    }
}