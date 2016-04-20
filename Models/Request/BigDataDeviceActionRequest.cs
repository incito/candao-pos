using System;
using System.Collections.Generic;
using Models.Enum;

namespace Models.Request
{
    public class BigDataDeviceActionRequest : BigDataRequest<BigDataDeviceActionBody>
    {
        public BigDataDeviceActionRequest()
        {
            head.method = "deviceAction";
        }
    }

    public class BigDataDeviceActionBody
    {
        public string appkey { get; set; }

        public string bill_code { get; set; }

        public string event_code { get; set; }

        public string time { get; set; }

        /// <summary>
        /// 1：单个；2：一对。
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 当type为单个类型时，该值为null；当type为一对类型时，该值代表开始与结束配对。
        /// </summary>
        public string corrkey { get; set; }

    }
}