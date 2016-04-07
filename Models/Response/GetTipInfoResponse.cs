using System.Collections.Generic;

namespace Models.Response
{
    /// <summary>
    /// 获取小费统计信息返回类。
    /// </summary>
    public class GetTipInfoResponse
    {
        /// <summary>
        /// 结果。1表示成功，2标识失败。
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 是否登录成功。
        /// </summary>
        public bool IsSuccess
        {
            get { return code.Equals("1"); }
        }

        public string msg { get; set; }

        public List<TipInfoDataResponse> resultList { get; set; }
    }

    /// <summary>
    /// 小费具体数据返回类。
    /// </summary>
    public class TipInfoDataResponse
    {
        public string waiterName { get; set; }

        public int serviceCount { get; set; }

        public decimal tipMoney { get; set; }
    }
}