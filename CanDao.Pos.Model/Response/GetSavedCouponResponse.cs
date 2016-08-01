
namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 保存的优惠券信息。
    /// </summary>
    public class GetSavedCouponResponse : DataResponse<SavedCouponInfoResponse>
    {

    }

    public class SavedCouponInfoResponse
    {
        public string orderid { get; set; }

        public string yhname { get; set; }

        public string partnername { get; set; }

        public decimal couponrate { get; set; }

        public decimal freeamount { get; set; }

        public decimal debitamount { get; set; }

        public string memo { get; set; }

        public string couponsno { get; set; }

        public int num { get; set; }

        public int ftype { get; set; }

        public string banktype { get; set; }

        public string ruleid { get; set; }

        public string couponid { get; set; }

        public decimal amount { get; set; }
    }
}