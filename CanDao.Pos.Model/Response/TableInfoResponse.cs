namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 餐台信息返回类。
    /// </summary>
    public class TableInfoResponse
    {
        public string position { get; set; }
        public int? custnum { get; set; }
        public int personNum { get; set; }

        public string areaname { get; set; }

        public string isVip { get; set; }

        public int status { get; set; }

        public string areaid { get; set; }

        public string isavailable { get; set; }

        public string tableNo { get; set; }

        public string buildingNo { get; set; }

        public string iscompartment { get; set; }

        public decimal fixprice { get; set; }

        public string custPrinter { get; set; }

        public string tableid { get; set; }

        public string tableName { get; set; }

        public decimal minprice { get; set; }

        public int tabletype { get; set; }

        public string orderid { get; set; }

        public string restaurantId { get; set; }

        /// <summary>
        /// 开台时间，未开台为null。
        /// </summary>
        public string begintime { get; set; }

        /// <summary>
        /// 应收金额。
        /// </summary>
        public decimal? amount { get; set; }
    }
}