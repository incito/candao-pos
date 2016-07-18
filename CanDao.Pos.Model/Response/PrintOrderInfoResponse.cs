namespace CanDao.Pos.Model.Response
{
    public class PrintOrderInfoResponse
    {
        public string areaname { get; set; }

        public string tableName { get; set; }

        public string begintime { get; set; }

        public string endtime { get; set; }

        public int custnum { get; set; }

        public string orderid { get; set; }

        public string userid { get; set; }

        public string fullname { get; set; }

        public int printcount { get; set; }

        public int befprintcount { get; set; }

        /// <summary>
        /// 优惠。（现在不晓得为啥没值了）
        /// </summary>
        public decimal? discountamount { get; set; }

        /// <summary>
        /// 赠送。
        /// </summary>
        public decimal? zdAmount { get; set; }

        /// <summary>
        /// 合计。
        /// </summary>
        public decimal dueamount { get; set; }

        /// <summary>
        /// 抹零。
        /// </summary>
        public decimal? payamount { get; set; }

        /// <summary>
        /// 四舍五入。
        /// </summary>
        public decimal payamount2 { get; set; }


        public decimal ssamount { get; set; }
    }
}