namespace Models
{
    /// <summary>
    /// 菜品简略信息。
    /// </summary>
    public class DishSimpleInfo
    {
        /// <summary>
        /// 菜品姓名。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 菜品价格。
        /// </summary>
        public decimal DishPrice { get; set; }

        /// <summary>
        /// 菜品单位。
        /// </summary>
        public string DishUnit { get; set; }

        /// <summary>
        /// 菜品数量。
        /// </summary>
        public decimal DishNum { get; set; }
    }
}