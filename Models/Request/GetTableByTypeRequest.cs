using System.Collections.Generic;

namespace Models.Request
{
    /// <summary>
    /// 根据餐台类型获取餐台集合的请求类。
    /// </summary>
    public class GetTableByTypeRequest
    {
        /// <summary>
        /// 餐台类型集合。
        /// </summary>
        public List<string> tableType { get; set; } 
    }
}