using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    public class GetOrderInfoResponse : NewHttpBaseResponse
    {
        public OrderInfos data { set; get; }
    }

    public class OrderInfos
    {
        public preferentialInfoList preferentialInfo { set; get; }
        public DishInfoList rows { set; get; }
    }

    public class preferentialInfoList
    {
        public string amount { set; get; }
        public string menuAmount { set; get; }
        public string payamount { set; get; }
        public string tipAmount { set; get; }
        public string freeamount { set; get; }
        public GetpreferentialDetails detailPreferentials { set; get; }
    }

    public class GetpreferentialDetails
    {
        public string id { set; get; }
        public string deAmount { set; get; }
        public PreferentialDetails activity { set; get; }
    }

    public class PreferentialDetails
    {
        public string name { set; get; }
    }

    public class DishInfoList
    {
        public string orderprice { set; get; }
        public string dishstatus { set; get; }
        public string dishname { set; get; }
        public string dishunit { set; get; }
        public string sperequire { set; get; }
        public string islatecooke { set; get; }
        public string primarykey { set; get; }
        public string dishid { set; get; }
        public string userName { set; get; }
        public string pricetype { set; get; }
        public string orderid { set; get; }
        public string ispot { set; get; }
        public string dishtype { set; get; }
        public string orderseq { set; get; }
        public string dishnum { set; get; }
    }
}
