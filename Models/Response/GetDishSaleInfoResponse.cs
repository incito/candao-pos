using System.Collections.Generic;

namespace Models.Response
{
    /// <summary>
    /// 获取品项销售信息返回类。
    /// </summary>
    public class GetDishSaleInfoResponse : JavaResponse
    {
        public string msg { get; set; }

        public PeriodTimeResponse time { get; set; }

        public List<DishSaleInfoDataResponse> data { get; set; }
    }

    /// <summary>
    /// 品项销售信息。
    /// </summary>
    public class DishSaleInfoDataResponse
    {
        public string dishName { get; set; }

        public int dishCount { get; set; }

        public decimal totlePrice { get; set; }
    }

    public class PeriodTimeResponse
    {
        public string startTime { get; set; }

        public string endTime { get; set; }
    }
}