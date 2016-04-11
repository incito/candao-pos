namespace Models
{
    /// <summary>
    /// 小费信息。
    /// </summary>
    public class TipInfo
    {
        /// <summary>
        /// 序号。
        /// </summary>
        public int Index { get; set; }

        public string WaiterName { get; set; }

        /// <summary>
        /// 小费次数。
        /// </summary>
        public int TipCount { get; set; }

        /// <summary>
        /// 小费金额。
        /// </summary>
        public decimal TipAmount { get; set; }
    }
}