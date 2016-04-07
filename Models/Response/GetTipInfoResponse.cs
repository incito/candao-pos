using System.Collections.Generic;

namespace Models.Response
{
    /// <summary>
    /// 获取小费统计信息返回类。
    /// </summary>
    public class GetTipInfoResponse : JavaResponse
    {
        public string msg { get; set; }

        public PeriodTimeResponse time { get; set; }

        public List<TipInfoDataResponse> data { get; set; }
    }

    /// <summary>
    /// 小费具体数据返回类。
    /// </summary>
    public class TipInfoDataResponse
    {
        public string username { get; set; }

        public int count { get; set; }

        public decimal amount { get; set; }
    }
}