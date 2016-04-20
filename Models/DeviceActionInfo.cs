using System;
using Models.Enum;

namespace Models
{
    /// <summary>
    /// 设备行为信息。
    /// </summary>
    public class DeviceActionInfo
    {
        public DeviceActionInfo(EnumDeviceAction action, string orderId, string key = "")
        {
            DeviceAction = action;
            Time = DateTime.Now;
            OrderId = orderId ?? "";
            Key = key;
        }

        public EnumDeviceAction DeviceAction { get; set; }

        public DateTime Time { get; set; }

        public string OrderId { get; set; }

        public string Key { get; set; }
    }
}