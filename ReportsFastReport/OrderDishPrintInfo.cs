namespace ReportsFastReport
{
    /// <summary>
    /// 订单品项打印信息。
    /// </summary>
    public class OrderDishPrintInfo
    {
        /// <summary>
        /// 菜名。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 数量和单位联合字符串。
        /// </summary>
        public string DishNumUnit { get; set; }

        /// <summary>
        /// 单价。
        /// </summary>
        public decimal DishPrice { get; set; }

        /// <summary>
        /// 小计。
        /// </summary>
        public decimal Amount { get; set; }
    }
}