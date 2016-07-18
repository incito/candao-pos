namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 小费统计数据返回类。
    /// </summary>
    public class ReportTipInfoResponse
    {
        public string waiterName { get; set; }

        public int serviceCount { get; set; }

        public decimal tipMoney { get; set; } 
    }
}