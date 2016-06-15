using System.Collections.Generic;

namespace Models.Response
{
    public class GetPrinterStatusResponse
    {
        public bool isSuccess { get; set; }

        public string errorMsg { get; set; }

        public List<PrinterStatusInfoResponse> data { get; set; }
    }
}