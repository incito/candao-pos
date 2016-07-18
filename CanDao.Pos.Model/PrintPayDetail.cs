namespace CanDao.Pos.Model
{
    /// <summary>
    /// 结算明细。
    /// </summary>
    public class PrintPayDetail
    {
        /// <summary>
        /// 结算方式。
        /// </summary>
        public string PayWay { get; set; }

        /// <summary>
        /// 结算金额。
        /// </summary>
        public decimal Payamount { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Remark { get; set; }
    }
}