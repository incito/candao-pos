namespace Models.Response
{
    /// <summary>
    /// 设置优惠券常用或不尝用返回类。
    /// </summary>
    public class SetCouponFavorResponse : JavaResponse
    {
        public string error_code { get; set; }
    }
}