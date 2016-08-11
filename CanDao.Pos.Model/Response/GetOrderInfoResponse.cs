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
        public OrderInfos ()
        {
            preferentialInfo = new preferentialInfoResponse();
            rows = new List<DishInfosResponse>();
            
        }

        public UserOrderInfo userOrderInfo { set; get; }
        public preferentialInfoResponse preferentialInfo { set; get; }
        public List<DishInfosResponse> rows { set; get; }
    }

    public class preferentialInfoResponse
    {

        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal amount { set; get; }
        /// <summary>
        /// 实际价格
        /// </summary>
        public decimal menuAmount { set; get; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal payamount { set; get; }
        /// <summary>
        /// 小费总计
        /// </summary>
        public decimal tipAmount { set; get; }
        /// <summary>
        /// 优免金额
        /// </summary>
        public decimal freeamount { set; get; }
        public List<GetpreferentialDetails> detailPreferentials { set; get; }
    }

    public class GetpreferentialDetails
    {
        public string id { set; get; }
        /// <summary>
        /// 优惠券优惠金额
        /// </summary>
        public decimal deAmount { set; get; }
        public PreferentialDetails activity { set; get; }
    }

    public class PreferentialDetails
    {
        public string name { set; get; }
    }

    public class DishInfosResponse:DishGroupInfo
    {
        /// <summary>
        /// 套餐、鱼锅
        /// </summary>
        public List<DishGroupInfo> dishes { set; get; }
    }

    public class DishGroupInfo
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
        public int dishtype { set; get; }
        public string orderseq { set; get; }
        public decimal dishnum { set; get; }
   
    }

    public class UserOrderInfo
    {
        public string memberno { set; get; }
        public int tableStatus { set; get; }
        public string orderInvoiceTitle { set; get; }
        public int customerNumber { set; get; }
        public string orderid { set; get; }
        public int orderStatus { set; get; }
    }
}
