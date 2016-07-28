namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取菜单明细返回类。
    /// </summary>
    public class GetOrderDishListResponse : RestOrderAndJsResponse<TableFullInfoResponse, OrderDishDataResponse>
    {
    }

    /// <summary>
    /// 订单菜品信息返回类。
    /// </summary>
    public class OrderDishDataResponse
    {
        public double discountamount { get; set; }

        /// <summary>
        /// 服务员编号。
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 菜品状态。0:正常，1：待称重。鱼锅本身时该项为null。
        /// </summary>
        public int? dishstatus { get; set; }

        /// <summary>
        /// 金额。鱼锅本身时该项为null。
        /// </summary>
        public decimal? payamount { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        public decimal dishnum { get; set; }

        /// <summary>
        /// 单位。
        /// </summary>
        public string dishunit { get; set; }

        /// <summary>
        /// 菜的类型。0：普通菜  1：鱼锅  2：套餐 
        /// </summary>
        public int dishtype { get; set; }

        public string begintime { get; set; }
        public double discountrate { get; set; }
        public double? predisamount { get; set; }
        public string primarykey { get; set; }
        public string dishid { get; set; }
        /// <summary>
        /// 菜的名称。
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 菜的描述。
        /// </summary>
        public string itemdesc { get; set; }
        /// <summary>
        /// 单价。鱼锅本身时该项为null。
        /// </summary>
        public decimal? orderprice { get; set; }
        /// <summary>
        /// 套餐鱼锅内部的菜为0，普通菜，套餐以及鱼锅主菜为1.
        /// </summary>
        public int ismaster { get; set; }
        /// <summary>
        /// 关联菜品id。套餐和鱼锅内部的菜以这个字段标识它们的主体菜。
        /// </summary>
        public string relatedishid { get; set; }
        /// <summary>
        /// 主体菜的primarykey。
        /// </summary>
        public string parentkey { get; set; }
        /// <summary>
        /// 是否是鱼锅锅底。
        /// </summary>
        public int ispot { get; set; }

        /// <summary>
        /// 临时菜
        /// </summary>
        public string avoid { set; get; }
    }
}