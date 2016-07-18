namespace CanDao.Pos.Model
{
    /// <summary>
    /// 打印用订单菜品信息。
    /// </summary>
    public class PrintOrderDishInfo
    {
        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 菜品数量和单位。
        /// </summary>
        public string DishNumUnit { get; set; }

        /// <summary>
        /// 菜品价格。
        /// </summary>
        public decimal DishPrice { get; set; }

        /// <summary>
        /// 菜品价格小计。
        /// </summary>
        public decimal DishAmount { get; set; }
    }
}