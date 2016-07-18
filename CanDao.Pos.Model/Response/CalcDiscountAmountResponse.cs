namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 计算折扣类金额返回类。
    /// </summary>
    public class CalcDiscountAmountResponse : JavaResponse
    {
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 计算后折扣金额。
        /// </summary>
        public decimal amount { get; set; }
    }
}