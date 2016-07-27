namespace CanDao.Pos.Model
{
    /// <summary>
    /// 退菜的组合信息。
    /// </summary>
    public class BackDishComboInfo
    {
        public string OrderId { get; set; }

        public string TableNo { get; set; }

        public string AuthorizerUser { get; set; }

        public string Waiter { get; set; }

        public OrderDishInfo DishInfo { get; set; }

        public decimal BackDishNum { get; set; }

        public string BackDishReason { get; set; }
    }
}