namespace Models.Request
{
    /// <summary>
    /// 大数据注册设备请求类。
    /// </summary>
    public class BigDataRegisterRequest : BigDataRequest<BigDataRegisterBody>
    {
        public BigDataRegisterRequest(int branchId, string branchName, string appkey)
        {
            head.method = "appsList";
            var body = new BigDataRegisterBody
            {
                appkey = appkey,
                appname = branchName,
                stores_id = branchId
            };
            data.Add(body);
        }
    }

    /// <summary>
    /// 大数据注册设备数据体。
    /// </summary>
    public class BigDataRegisterBody
    {
        public BigDataRegisterBody()
        {
            type = 4;
            appname = "餐道";
        }

        public int stores_id { get; set; }

        public string appkey { get; set; }

        public string appname { get; set; }

        /// <summary>
        /// 1:老板app  2:pad 3:手环  4:pos
        /// </summary>
        public int type { get; set; }
    }
}