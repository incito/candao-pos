namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取赠菜优惠券返回类。
    /// </summary>
    public class GetDishGiftCouponInfoResponse : NewHttpBaseListResponse<DishGiftCouponInfoResponse>
    {
        
    }

    public class DishGiftCouponInfoResponse
    {
        /// <summary>
        /// 菜品ID。
        /// </summary>
        public string dishid { get; set; }

        /// <summary>
        /// 菜品单位。
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 优惠券使用数量。
        /// </summary>
        public decimal count { get; set; }
    }
}