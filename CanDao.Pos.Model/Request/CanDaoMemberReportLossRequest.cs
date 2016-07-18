namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员挂失请求类。
    /// </summary>
    public class CanDaoMemberReportLossRequest
    {
        public CanDaoMemberReportLossRequest(string branchId, string cardNo)
        {
            branch_id = branchId;
            cardno = cardNo;
            securityCode = "";
            password = "";
            FMemo = "";
        }

        public string branch_id { get; set; }

        public string securityCode { get; set; }

        public string cardno { get; set; }

        public string password { get; set; }

        public string FMemo { get; set; }
    }
}