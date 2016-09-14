namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取赠菜优惠券返回类。
    /// </summary>
    public class GetDishGiftCouponInfoResponse : NewHttpBaseResponse<DishGiftCouponInfoResponse>
    {
        
    }

    public class DishGiftCouponInfoResponse
    {
        /// <summary>
        /// 菜品ID。
        /// </summary>
        public string dishid { get; set; }

        /// <summary>
        /// 优惠券使用数量。
        /// </summary>
        public int count { get; set; }
    }
}