namespace CanDao.Pos.Model.Response
{
    public class QueryOrderInfoResponse : RestOrderResponse<QueryOrderInfoDataResponse>
    {

    }

    public class QueryOrderInfoDataResponse
    {
        public string orderid { get; set; }
        public decimal discountamount { get; set; }
        public int shiftid { get; set; }
        public int? womanNum { get; set; }
        public string areaNo { get; set; }
        public decimal ssamount { get; set; }
        public decimal? payamount { get; set; }
        public string areaname { get; set; }
        public string couponname { get; set; }
        public string disuserid { get; set; }
        public decimal wipeamount { get; set; }
        public int? childNum { get; set; }
        public int? mannum { get; set; }
        public int befprintcount { get; set; }
        public string couponname3 { get; set; }
        public string tableName { get; set; }
        public string ordertype { get; set; }
        public string payway { get; set; }
        public int? custnum { get; set; }
        public string begintime { get; set; }
        public string orderseq { get; set; }
        public string fullname { get; set; }
        public string specialrequied { get; set; }
        public string branchid { get; set; }
        public string memberno { get; set; }
        public string gift_status { get; set; }
        public decimal freeamount { get; set; }
        public string invoice_id { get; set; }
        public int closeshiftid { get; set; }
        public decimal ymamount { get; set; }
        public string gzuser { get; set; }
        public string gztele { get; set; }
        public string gzname { get; set; }
        public string gzcode { get; set; }
        public int printcount { get; set; }
        public string userid { get; set; }
        public string ageperiod { get; set; }
        public int orderstatus { get; set; }
        public string endtime { get; set; }
        public string currenttableid { get; set; }
        public string tableids { get; set; }
        public string partername { get; set; }
        public decimal fulldiscountrate { get; set; }
        public string pnum { get; set; }
        public string meid { get; set; }
        public string workdate { get; set; }
        public decimal dueamount { get; set; }
        public string relateorderid { get; set; }
        public decimal gzamount { get; set; }
    }
}