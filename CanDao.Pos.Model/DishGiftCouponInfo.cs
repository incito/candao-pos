namespace CanDao.Pos.Model
{
    /// <summary>
    /// 赠菜优惠券信息类。
    /// </summary>
    public class DishGiftCouponInfo
    {
        /// <summary>
        /// 赠菜的菜品ID。
        /// </summary>
        public string DishId { get; set; }

        /// <summary>
        /// 赠菜的菜品单位。
        /// </summary>
        public string DishUnit { get; set; }

        /// <summary>
        /// 使用的优惠券数量。
        /// </summary>
        public decimal UsedCouponCount { get; set; }

    }
}