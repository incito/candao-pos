using System.Collections.Generic;

namespace CanDao.Pos.Model.Request
{
    public class DishDetailRequest
    {

        public DishDetailRequest()
        {
            printtype = "0";
            orderseq = "1";
            sperequire = "";
            ispot = 0;
        }

        public string printtype { get; set; }

        /// <summary>
        /// 1是赠菜，0是正常。
        /// </summary>
        public string pricetype { get; set; }

        /// <summary>
        /// 菜的价格。
        /// </summary>
        public decimal orderprice { get; set; }

        /// <summary>
        /// 菜的原价。
        /// </summary>
        public decimal orignalprice { get; set; }

        public string dishid { get; set; }

        public string userName { get; set; }

        public string dishunit { get; set; }

        public string orderid { get; set; }

        public int dishtype { get; set; }

        public string orderseq { get; set; }

        public decimal dishnum { get; set; }

        /// <summary>
        /// 忌口。
        /// </summary>
        public string sperequire { get; set; }

        /// <summary>
        /// 唯一标示，判断是否重复下单用。（一次点菜应生成唯一一个标示）
        /// </summary>
        public string primarykey { get; set; }

        /// <summary>
        /// 是否称重。0不称重或已称重，1未称重。
        /// </summary>
        public string dishstatus { get; set; }

        /// <summary>
        /// 0非锅底，1锅底。
        /// </summary>
        public int ispot { get; set; }

        /// <summary>
        /// 口味。
        /// </summary>
        public string taste { get; set; }

        /// <summary>
        /// 赠菜收银员ID。
        /// </summary>
        public string freeuser { get; set; }

        /// <summary>
        /// 赠菜授权人ID。
        /// </summary>
        public string freeauthorize { get; set; }

        /// <summary>
        /// 赠菜原因。
        /// </summary>
        public string freereason{ get; set; }

        public List<DishDetailRequest> dishes { get; set; }
    }
}