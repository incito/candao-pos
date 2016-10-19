namespace CanDao.Pos.Model.Response
{
    public class GetSystemConfigInfoResponse : NewHttpBaseResponse<SystemConfigInfoResponse>
    {
        
    }

    public class SystemConfigInfoResponse
    {
        public bool vipstatus { get; set; }
        /// <summary>
        /// 会员类型。1：餐道，2：雅座。
        /// </summary>
        public byte viptype { get; set; }

        /// <summary>
        /// 餐道会员地址。
        /// </summary>
        public string vipcandaourl { get; set; }

        /// <summary>
        /// 雅座会员地址。
        /// </summary>
        public string vipotherurl { get; set; }
    }
}