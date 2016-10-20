using System.Collections.Generic;

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
            preferentialInfo = new PreferentialInfoResponse();
            rows = new List<DishInfosResponse>();
            
        }

        public UserOrderInfo userOrderInfo { set; get; }
        public PreferentialInfoResponse preferentialInfo { set; get; }
        public List<DishInfosResponse> rows { set; get; }
    }

    public class PreferentialInfoResponse
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
        public decimal toalFreeAmount { set; get; }
        /// <summary>
        /// 四舍五入或抹零金额
        /// </summary>
        public decimal moneyWipeAmount { set; get; }
        /// <summary>
        /// 0:未设置，1：四舍五入，2：抹零
        /// </summary>
        public int moneyDisType { set; get; }
        /// <summary>
        /// 总挂账
        /// </summary>
        public decimal toalDebitAmount { set; get; }
        /// <summary>
        /// 总挂账多收
        /// </summary>
        public decimal toalDebitAmountMany { set; get; }

        /// <summary>
        /// 调整金额
        /// </summary>
        public decimal adjAmout { set; get; }

        public List<GetPreferentialDetails> detailPreferentials { set; get; }
    }

    /// <summary>
    /// 优惠券明细信息。
    /// </summary>
    public class GetPreferentialDetails
    {
        public string id { set; get; }
        /// <summary>
        /// 优惠券优惠金额
        /// </summary>
        public decimal deAmount { set; get; }
        /// <summary>
        /// 优免
        /// </summary>
        public decimal toalFreeAmount { set; get; }
        /// <summary>
        /// 挂账
        /// </summary>
        public decimal toalDebitAmount { set; get; }
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public string preferential { set; get; }
        /// <summary>
        /// 优惠券明细Id
        /// </summary>
        public string coupondetailid { set; get; }

        /// <summary>
        /// 优惠券的使用类型。0：使用优惠，1：服务员优惠；2：系统自动查找优惠；4：赠送菜优惠；5：雅座优惠。
        /// </summary>
        public int isCustom { get; set; }

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

        public string taste { set; get; }
   
    }

    public class UserOrderInfo
    {
        public string memberno { set; get; }
        public int tableStatus { set; get; }
        public string orderInvoiceTitle { set; get; }
        public int customerNumber { set; get; }
        public string orderid { set; get; }
        public int orderStatus { set; get; }
        /// <summary>
        /// 餐具是否收费。0不收费/1收费。
        /// </summary>
        public byte isFree { get; set; }
        /// <summary>
        /// 餐具人数。
        /// </summary>
        public int numOfMeals { get; set; }
    }
}
