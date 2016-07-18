using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取系统设置返回类。
    /// </summary>
    public class GetSystemSetDataResponse
    {
        public List<SystemSetDataResponse> rows { get; set; }
    }

    /// <summary>
    /// 系统设置值。
    /// </summary>
    public class SystemSetDataResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public string item_value { get; set; }
        public string itemSort { get; set; }
        public string typename { get; set; }
        public string itemDesc { get; set; }
        public string itemid { get; set; }
        public string type { get; set; }
    }
}