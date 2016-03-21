namespace Models.Request
{
    /// <summary>
    /// 获取品项销售请求类。
    /// </summary>
    public class GetDishSaleRequest
    {
        /// <summary>
        /// 请求品项销售周期标识。1：今天，2：本周，3：本月，4：上月。
        /// </summary>
        public int flag { get; set; }
    }
}