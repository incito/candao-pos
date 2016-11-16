namespace CanDao.Pos.Model.Response
{
    public class TableFullInfoResponse : TableInfoResponse
    {
        public int? womanNum { get; set; }

        public int? childNum { get; set; }

        public int mannum { get; set; }

        public int custnum { get; set; }

        public decimal ssamount { get; set; }

        /// <summary>
        /// 订单状态。
        /// </summary>
        public int orderstatus { get; set; }

        public decimal dueamount { get; set; }

        public string begintime { get; set; }

        public string userid { get; set; }

        public string memberno { get; set; }

        public int befprintcount { get; set; }

        public int printcount { get; set; }

        public string endtime { get; set; }

        public decimal? discountamount { get; set; }

        public decimal? zdAmount { get; set; }

        /// <summary>
        /// 小费金额。
        /// </summary>
        public decimal? tipAmount { get; set; }
    }

}