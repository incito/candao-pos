using System.Collections.Generic;

namespace Models.Response
{
    /// <summary>
    /// Java接口的返回基类。
    /// </summary>
    public class JavaResponse
    {
        /// <summary>
        /// 结果。0表示成功。（!!!!有的地方1是成功）
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 错误消息。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 提示信息。（跟msg的区别是有的接口返回msg，有的返回info，吐槽吧。）
        /// </summary>
        public string info { get; set; }

        /// <summary>
        /// 是否操作成功。
        /// </summary>
        public bool IsSuccess
        {
            get { return result.Equals("0"); }
        }
    }

    public class RowsResponse<T>
    {
        public List<T> rows { get; set; }
    }
}