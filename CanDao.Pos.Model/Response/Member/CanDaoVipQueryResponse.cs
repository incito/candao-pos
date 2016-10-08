using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 查到查询返回值
    /// </summary>
    public class CanDaoVipQueryResponse : CanDaoVipBaseResponse
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string member_address { get; set; }
        public string createtime { get; set; }
        public string mobile { get; set; }
        public List<CanDaoVipCardInfo> result { get; set; }
    }

    public class CanDaoVipCardInfo
    {
        public string MCard { get; set; }
        public string StoreCardBalance { get; set; }
        public string IntegralOverall { get; set; }
        public string CouponsOverall { get; set; }
        public string TraceCode { get; set; }
        public string card_type { get; set; }
        public string level { get; set; }

        public string level_name { get; set; }
        public string status { get; set; }

    }
}
