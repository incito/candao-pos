using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取未清机信息返回类。
    /// </summary>
    public class GetUnclearnPosInfoResponse
    {
        public string result { get; set; }

        public List<UnclearPosResponse> detail { get; set; }
    }

    /// <summary>
    /// 未清机POS信息。
    /// </summary>
    public class UnclearPosResponse
    {
        public string id { get; set; }

        public string username { get; set; }

        public string ipaddress { get; set; }

        public string opendate { get; set; }

        public string cashamount { get; set; }

        public int status { get; set; }
    }
}