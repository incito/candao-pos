namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取餐具信息返回类。
    /// </summary>
    public class GetDinnerWareInfoResponse : RestOrderResponse<DinnerWareInfoResponse>
    {
    }

    public class DinnerWareInfoResponse
    {
        public string dishid { get; set; }
        public string dishname { get; set; }
        public int dishtype { get; set; }
        public string unit { get; set; }
        public decimal? price { get; set; }
        public decimal? vipprice { get; set; }
        public string source { get; set; }

    }
}