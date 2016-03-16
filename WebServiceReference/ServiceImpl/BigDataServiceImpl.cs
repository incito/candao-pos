using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models;
using Models.Request;
using Models.Response;
using WebServiceReference.IService;

namespace WebServiceReference.ServiceImpl
{
    public class BigDataServiceImpl : IBigDataService
    {
        public string RegisterPos()
        {
            var addr = "http://" + RestClient.BigDataServer;
            var request = new BigDataRegisterRequest(Globals.BranchInfo.BranchId, Globals.BranchInfo.BranchName, RestClient.Mac);
            try
            {
                var result = HttpHelper.HttpPost<BigDataResponse>(addr, request);
                return ParseResultStatus(result.result.status);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeviceAction(DeviceActionInfo actionInfo)
        {
            var addr = "http://" + RestClient.BigDataServer;
            var request = new BigDataDeviceActionRequest();
            var body = Cvt2ActionBody(actionInfo);
            request.data.Add(body);
            try
            {
                var result = HttpHelper.HttpPost<BigDataResponse>(addr, request);
                return ParseResultStatus(result.result.status);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeviceAction(List<DeviceActionInfo> actionInfoList)
        {
            var addr = "http://" + RestClient.BigDataServer;
            var request = new BigDataDeviceActionRequest();
            var bodyList = actionInfoList.Select(Cvt2ActionBody);
            request.data.AddRange(bodyList);
            try
            {
                var result = HttpHelper.HttpPost<BigDataResponse>(addr, request);
                return ParseResultStatus(result.result.status);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private BigDataDeviceActionBody Cvt2ActionBody(DeviceActionInfo actionInfo)
        {
            return new BigDataDeviceActionBody
            {
                appkey = RestClient.Mac,
                bill_code = actionInfo.OrderId,
                corrkey = actionInfo.Key,
                type = string.IsNullOrEmpty(actionInfo.Key) ? "1" : "2",
                event_code = "pos000" + (int) actionInfo.DeviceAction,
                time = actionInfo.Time.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        private string ParseResultStatus(string status)
        {
            switch (status)
            {
                case "00":
                    return null;
                case "01":
                    return "异常错误";
                case "02":
                    return "数据格式不对";
                default:
                    return "其他未知错误";
            }
        }
    }
}