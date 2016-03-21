namespace Models
{
    /// <summary>
    /// 菜品销售信息。
    /// </summary>
    public class DishSaleInfo
    {
        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 销售数量。
        /// </summary>
        public int SalesCount { get; set; }

        /// <summary>
        /// 销售金额。
        /// </summary>
        public decimal SalesAmount { get; set; }
    }
}