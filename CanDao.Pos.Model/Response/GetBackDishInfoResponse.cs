using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取退菜信息返回类。
    /// </summary>
    public class GetBackDishInfoResponse : DataResponse<BackDishInfoDataResponse>
    {
    }

    /// <summary>
    /// 退菜信息主体返回类。
    /// </summary>
    public class BackDishInfoDataResponse
    {
        public string orderid { get; set; }
        public string superkey { get; set; }
        public decimal discountamount { get; set; }
        public string discarduserid { get; set; }
        public int islatecooke { get; set; }
        public int dishstatus { get; set; }
        public int ismaster { get; set; }
        public string userName { get; set; }
        public decimal payamount { get; set; }
        public int status { get; set; }
        public decimal dishnum { get; set; }
        public string dishunit { get; set; }
        public int dishtype { get; set; }
        public string disuserid { get; set; }
        public int ordertype { get; set; }
        public int ispot { get; set; }
        public string begintime { get; set; }
        public int orderseq { get; set; }
        public string parentkey { get; set; }
        public string sperequire { get; set; }
        public decimal discountrate { get; set; }
        public decimal predisamount { get; set; }
        public string couponid { get; set; }
        public string primarykey { get; set; }
        public int isadddish { get; set; }
        public string dishid { get; set; }
        public string endtime { get; set; }
        public int childdishtype { get; set; }
        public string relatedishid { get; set; }
        public int pricetype { get; set; }
        public string orderdetailid { get; set; }
        public decimal orignalprice { get; set; }
        public decimal? debitamount { get; set; }
        public string fishcode { get; set; }
        public decimal? orderprice { get; set; }
        public string relateorderid { get; set; }
        public string discardreason { get; set; }
    }
}