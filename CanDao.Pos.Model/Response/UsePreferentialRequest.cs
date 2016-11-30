namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 优惠券操作信息返回类。（使用和删除）
    /// </summary>
    public class PreferentialOperResponse : NewHttpBaseResponse<PreferentialResponse>
    {
    }

    public class PreferentialResponse
    {
        public PreferentialInfoResponse preferentialInfo { set; get; }
        public ServiceChargeInfoResponse serviceCharge { get; set; }
    }
}
