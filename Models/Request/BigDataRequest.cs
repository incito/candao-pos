using System.Collections.Generic;

namespace Models.Request
{
    /// <summary>
    /// 大数据请求基类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BigDataRequest<T> where T : new()
    {
        public BigDataRequest()
        {
            head = new BigDataRequstHeader();
            data = new List<T>();
        }

        public BigDataRequstHeader head { get; set; }

        public List<T> data { get; set; }
    }

    /// <summary>
    /// 请求类头部分。
    /// </summary>
    public class BigDataRequstHeader : BigDataHeader
    {
        public BigDataRequstHeader()
        {
            comer = "POS";
            type = "report";
        }
    }
}