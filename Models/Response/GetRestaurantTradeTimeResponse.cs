namespace Models.Response
{
    /// <summary>
    /// 获取店铺营业时间返回值。
    /// </summary>
    public class GetRestaurantTradeTimeResponse : JavaResponse
    {
        public RestaurantTradeTimeRespons detail { get; set; }
    }

    /// <summary>
    /// 店铺营业时间返回值。
    /// </summary>
    public class RestaurantTradeTimeRespons
    {
        /// <summary>
        /// 开业时间。
        /// </summary>
        public string begintime { get; set; }

        /// <summary>
        /// 结业时间。
        /// </summary>
        public string endtime { get; set; }
    }
}