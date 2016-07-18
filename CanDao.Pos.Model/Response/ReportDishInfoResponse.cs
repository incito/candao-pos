namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 品项销售统计返回类。
    /// </summary>
    public class ReportDishInfoResponse
    {
        public string dishName { get; set; }

        public decimal dishCount { get; set; }

        public decimal? totlePrice { get; set; } 
    }
}