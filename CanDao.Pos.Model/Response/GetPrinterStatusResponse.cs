using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取打印机状态列表返回类。
    /// </summary>
    public class GetPrinterStatusResponse
    {
        public bool isSuccess { get; set; }

        public string errorMsg { get; set; }

        public List<PrinterStatusInfoResponse> data { get; set; }
    }

    /// <summary>
    /// 每个打印机列表状态信息类。
    /// </summary>
    public class PrinterStatusInfoResponse
    {
        public string ip { get; set; }

        public string name { get; set; }

        public int status { get; set; }

        public string statusTitle { get; set; }
    }
}