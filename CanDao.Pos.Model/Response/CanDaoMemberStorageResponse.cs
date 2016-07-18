namespace CanDao.Pos.Model.Response
{
    public class CanDaoMemberStorageResponse : CanDaoMemberBaseResponse
    {
        public decimal StoreCardBalance { get; set; }

        public string TraceCode { get; set; }

        public decimal GiftAmount { get; set; }
    }
}