namespace CanDao.Pos.Model
{
    /// <summary>
    /// 报表数据基类。
    /// </summary>
    public class ReportDataBase
    {
        /// <summary>
        /// 序号。
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// 金额。
        /// </summary>
        public decimal Amount { get; set; }
    }
}