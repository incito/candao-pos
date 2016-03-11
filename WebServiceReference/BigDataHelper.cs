using System.Collections.Generic;
using Common;
using Models;
using Models.Enum;
using Models.Request;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace WebServiceReference
{
    /// <summary>
    /// 大数据接口辅助类。
    /// </summary>
    public class BigDataHelper
    {
        /// <summary>
        /// 大数据注册POS。
        /// </summary>
        public static void RegisterPos()
        {
            IBigDataService bigDataService = new BigDataServiceImpl();
            var bigResult = bigDataService.RegisterPos();
            if (!string.IsNullOrEmpty(bigResult))
                AllLog.Instance.E(bigResult);
        }

        public static void DeviceAction(EnumDeviceAction deviceAction)
        {
            var actionInfo = new DeviceActionInfo(deviceAction, "");
            DeviceAction(actionInfo);
        }

        /// <summary>
        /// 记录设备行为。
        /// </summary>
        /// <param name="actionInfo">设备行为数据。</param>
        public static void DeviceAction(DeviceActionInfo actionInfo)
        {
            IBigDataService bigDataService = new BigDataServiceImpl();
            var bigResult = bigDataService.DeviceAction(actionInfo);
            if (!string.IsNullOrEmpty(bigResult))
                AllLog.Instance.E(bigResult);
        }

        public static void DeviceAction(List<DeviceActionInfo> actionInfoList)
        {
            IBigDataService bigDataService = new BigDataServiceImpl();
            var bigResult = bigDataService.DeviceAction(actionInfoList);
            if (!string.IsNullOrEmpty(bigResult))
                AllLog.Instance.E(bigResult);
        }
    }
}