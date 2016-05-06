using System.Collections.Generic;
using Models;
using Models.Enum;

namespace WebServiceReference.IService
{
    /// <summary>
    /// 大数据服务。
    /// </summary>
    public interface IBigDataService
    {
        /// <summary>
        /// 注册POS。
        /// </summary>
        /// <returns></returns>
        string RegisterPos();

        /// <summary>
        /// 记录设备行为。
        /// </summary>
        /// <param name="actionInfo">设备行为信息。</param>
        /// <returns></returns>
        string DeviceAction(DeviceActionInfo actionInfo);

        /// <summary>
        /// 记录设备行为。
        /// </summary>
        /// <param name="actionInfoList">设备行为信息集合。</param>
        /// <returns></returns>
        string DeviceAction(List<DeviceActionInfo> actionInfoList);
    }
}