namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 反结算账单请求类。
    /// </summary>
    public class AntiSettlementOrderRequest
    {
        public string userName { get; set; }

        public string orderNo { get; set; }

        public string reason { get; set; }
    }
}