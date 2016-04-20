using Models.Request;

namespace Models.Response
{
    /// <summary>
    /// 大数据返回类。
    /// </summary>
    public class BigDataResponse
    {
        public BigDataHeader head { get; set; }

        public BigDataResultResponse result { get; set; }
    }

    public class BigDataResultResponse
    {
        public string status { get; set; }
    }
}