using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public static void RegisterPosAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                IBigDataService bigDataService = new BigDataServiceImpl();
                var bigResult = bigDataService.RegisterPos();
                if (!string.IsNullOrEmpty(bigResult))
                    AllLog.Instance.E("设备注册时失败：{0}", bigResult);
            });
        }

        public static void DeviceActionAsync(EnumDeviceAction deviceAction)
        {
            var actionInfo = new DeviceActionInfo(deviceAction, "");
            DeviceActionAsync(actionInfo);
        }

        /// <summary>
        /// 记录设备行为。
        /// </summary>
        /// <param name="actionInfo">设备行为数据。</param>
        public static void DeviceActionAsync(DeviceActionInfo actionInfo)
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                IBigDataService bigDataService = new BigDataServiceImpl();
                var bigResult = bigDataService.DeviceAction(actionInfo);
                if (!string.IsNullOrEmpty(bigResult))
                    AllLog.Instance.E("{0} 大数据错误：{1}", actionInfo.DeviceAction.ToString("G"), bigResult);
            });
        }

        public static void DeviceActionAsync(List<DeviceActionInfo> actionInfoList)
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                IBigDataService bigDataService = new BigDataServiceImpl();
                var bigResult = bigDataService.DeviceAction(actionInfoList);
                if (!string.IsNullOrEmpty(bigResult))
                    AllLog.Instance.E("{0} 大数据错误：{1}", actionInfoList.First().DeviceAction.ToString("G"), bigResult);
            });
        }
    }
}