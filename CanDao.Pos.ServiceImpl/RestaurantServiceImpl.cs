using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using JunLan.Common.Base;
using HttpHelper = CanDao.Pos.Common.HttpHelper;

namespace CanDao.Pos.ServiceImpl
{
    /// <summary>
    /// 店铺相关服务实现。
    /// </summary>
    public class RestaurantServiceImpl : IRestaurantService
    {
        public Tuple<string, bool> CheckRestaurantOpened()
        {
            string addr = ServiceAddrCache.GetServiceAddr("RestaurantOpened");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, bool>("开业接口地址为空。", false);
            List<string> param = new List<string> { "", "", PCInfoHelper.LocalIPAddr, "0" };//最后一个"0"是接口的CallType为1时是开业，为0时是检测是否开业。
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, bool>(result.Item1, false);
            return new Tuple<string, bool>(null, result.Item2.IsSuccess);
        }

        public string RestaurantOpening(string userName, string password)
        {
            string addr = ServiceAddrCache.GetServiceAddr("RestaurantOpened");
            if (string.IsNullOrEmpty(addr))
                return "开业接口地址为空。";

            List<string> param = new List<string> { userName, password, PCInfoHelper.LocalIPAddr, "1" };//最后一个"1"是接口的CallType为1时是开业，为0时是检测是否开业。
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return result.Item1;
            return result.Item2.IsSuccess ? null : result.Item2.Info ?? "开业失败！";
        }

        public Tuple<string, bool> CheckPettyCashInput(string userName)
        {
            string addr = ServiceAddrCache.GetServiceAddr("PettyCashInput");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, bool>("零找金接口地址为空。", false);

            List<string> param = new List<string> { userName, PCInfoHelper.MACAddr, "0", "0" };//参数顺序：当前用户，机器标识，零找金额，查询或输入（0查询，1输入）
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, bool>(result.Item1, false);
            return new Tuple<string, bool>(null, result.Item2.IsSuccess);
        }

        public string InputPettyCash(string userName, decimal amount)
        {
            string addr = ServiceAddrCache.GetServiceAddr("PettyCashInput");
            if (string.IsNullOrEmpty(addr))
                return "零找金接口地址为空。";

            List<string> param = new List<string> { userName, PCInfoHelper.MACAddr, amount.ToString(CultureInfo.InvariantCulture), "1" };
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return result.Item1;
            return result.Item2.IsSuccess ? null : result.Item2.Info;
        }

        public Tuple<string, List<SystemSetData>> GetSystemSetData(EnumSystemDataType type)
        {
            string addr = ServiceAddrCache.GetServiceAddr("GetSystemSetData");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<SystemSetData>>("获取系统设置接口地址为空。", null);

            try
            {
                var request = new GetSystemSetDataRequest { type = type.ToString("G") };
                var result = HttpHelper.HttpPost<GetSystemSetDataResponse>(addr, request);
                if (result == null || !result.rows.Any())
                    return new Tuple<string, List<SystemSetData>>("返回系统设置值为空。", null);

                var dataList = result.rows.Select(DataConverter.ToSystemSetData).ToList();
                return new Tuple<string, List<SystemSetData>>(null, dataList);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, List<SystemSetData>>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, OrderDishInfo> GetDinnerWareInfo(string userName)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetDinnerWareInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, OrderDishInfo>("获取餐具信息地址为空。", null);

            var param = new List<string> { userName };
            var result = RestHttpHelper.HttpGet<GetDinnerWareInfoResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, OrderDishInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, OrderDishInfo>(result.Item2.Info ?? "服务器内部错误，获取餐具信息失败。", null);

            var dishList = result.Item2.OrderJson.Data;
            if (dishList == null || !dishList.Any())
                return new Tuple<string, OrderDishInfo>("获取的餐具信息为空。", null);

            var dinnerWareInfo = DataConverter.ToDishInfo(dishList.First());
            return new Tuple<string, OrderDishInfo>(null, dinnerWareInfo);
        }

        public Tuple<string, List<TableInfo>> GetAllTableInfoes()
        {
            string addr = ServiceAddrCache.GetServiceAddr("GetAllTableInfos");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<TableInfo>>("获取所有餐桌信息接口地址为空。", null);

            try
            {
                var result = HttpHelper.HttpPost<List<TableInfoResponse>>(addr, null);
                var dataList = new List<TableInfo>();
                if (result != null && result.Any())
                    dataList = result.Select(DataConverter.ToTableInfo).ToList();
                return new Tuple<string, List<TableInfo>>(null, dataList);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, List<TableInfo>>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, List<TableInfo>> GetTableInfoByType(List<EnumTableType> tableTypes)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetTableInfoByTableType");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<TableInfo>>("获取所有餐桌信息接口地址为空。", null);

            try
            {
                var request = new GetTableByTypeRequest { tableType = tableTypes.Select(t => ((int)t).ToString()).ToList() };
                var result = HttpHelper.HttpPost<List<TableInfoResponse>>(addr, request);
                var dataList = new List<TableInfo>();
                if (result != null && result.Any())
                    dataList = result.Select(DataConverter.ToTableInfo).ToList();
                return new Tuple<string, List<TableInfo>>(null, dataList);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, List<TableInfo>>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, string> OpenTable(OpenTableRequest request)
        {
            string addr = ServiceAddrCache.GetServiceAddr("OpenTable");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, string>("开台地址为空。", null);

            try
            {
                var result = HttpHelper.HttpPost<OpenTableResponse>(addr, request);
                if (result.IsSuccess)
                    return new Tuple<string, string>(null, result.orderid);

                string errMsg;
                switch (result.OpenTableResult)
                {
                    case EnumOpenTableResult.Occupied:
                        errMsg = "餐台被占用。";
                        break;
                    case EnumOpenTableResult.NoTableName:
                        errMsg = string.Format("餐台名称：\"{0}\"不存在。", request.tableNo);
                        break;
                    case EnumOpenTableResult.NoOpenUp:
                        errMsg = "餐厅未开业。";
                        break;
                    default:
                        errMsg = "其他未知错误";
                        break;
                }
                return new Tuple<string, string>(errMsg, null);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, string>(ex.MyMessage(), null);
            }
        }

        public string Clearner(ClearnerRequest request)
        {
            string addr = ServiceAddrCache.GetServiceAddr("Clearner");
            if (string.IsNullOrEmpty(addr))
                return "清机地址为空。";

            var mac = request.Mac ?? PCInfoHelper.MACAddr;
            var posId = request.PosId ?? SystemConfigCache.PosId;
            var param = new List<string> { request.UserId, request.UserName, mac, posId, request.Authorizer };
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return "清机失败：" + result.Item1;

            return result.Item2.IsSuccess ? null : result.Item2.Info ?? "清机失败";
        }

        public Tuple<string, List<UnclearMachineInfo>> GetUnclearnPosInfo()
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetAllUnclearnPosInfoes");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<UnclearMachineInfo>>("获取所有未清机地址为空。", null);

            try
            {
                var result = HttpHelper.HttpGet<GetUnclearnPosInfoResponse>(addr);
                if (!result.result.Equals("0"))
                    return new Tuple<string, List<UnclearMachineInfo>>("服务器内部错误，获取未清机列表失败。", null);

                var list = result.detail.Select(DataConverter.ToUnclearMachineInfo).ToList();
                return new Tuple<string, List<UnclearMachineInfo>>(null, list);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("获取所有未清机时异常。", exp);
                return new Tuple<string, List<UnclearMachineInfo>>(exp.MyMessage(), null);
            }
        }

        public string EndWork(EndWorkRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("EndWork");
            if (string.IsNullOrEmpty(addr))
                return "结业地址为空。";

            var ipAddr = request.IpAddress ?? PCInfoHelper.IPAddr;
            var param = new List<string> { request.UserId, ipAddr };
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return "结业失败：" + result.Item1;

            return result.Item2.IsSuccess ? null : result.Item2.Info ?? "结业失败";
        }

        public string EndWorkSyncData()
        {
            var addr = ServiceAddrCache.GetServiceAddr("EndWorkSyncData");
            if (string.IsNullOrEmpty(addr))
                return "结业后通知后台同步数据地址为空。";

            try
            {
                var request = new EndWorkSyncDataRequest();
                var result = HttpHelper.HttpPost<JavaResponse>(addr, request);
                return result.IsSuccess ? null : "通知后台同步数据失败。";
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return "结业后同步数据失败：" + ex.MyMessage();
            }
        }

        public string BroadcastMessage(int msgId, string msg)
        {
            var addr = ServiceAddrCache.GetServiceAddr("BroadcastMsg");
            if (string.IsNullOrEmpty(addr))
                return "广播消息地址为空。";

            var param = new List<string> { Globals.UserInfo.UserName, msgId.ToString(), msg };
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return "广播消息失败：" + result.Item1;

            return result.Item2.IsSuccess ? null : string.IsNullOrEmpty(result.Item2.Info) ? "广播消息失败" : result.Item2.Info;
        }

        public Tuple<string, List<BankInfo>> GetAllBankInfos()
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetAllBankInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<BankInfo>>("广播消息地址为空。", null);

            try
            {
                var response = HttpHelper.HttpGet<List<GetBankInfoResponse>>(addr);
                var result = response != null ? response.Select(DataConverter.ToBankInfo).ToList() : new List<BankInfo>();
                return new Tuple<string, List<BankInfo>>(null, result.OrderBy(t => t.SortIndex).ToList());
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, List<BankInfo>>("获取银行信息异常。" + ex.Message, null);
            }
        }

        public Tuple<string, List<OnCompanyAccountInfo>> GetAllOnAccountCompany()
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetAllOnCpyAccountInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<OnCompanyAccountInfo>>("获取挂账单位地址为空。", null);

            try
            {
                var response = HttpHelper.HttpPost<List<GetOnCompanyAccountResponse>>(addr, null);
                var result = response != null ? response.Select(DataConverter.ToOnCpyAccInfo).ToList() : new List<OnCompanyAccountInfo>();
                return new Tuple<string, List<OnCompanyAccountInfo>>(null, result);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(exp);
                return new Tuple<string, List<OnCompanyAccountInfo>>("获取挂账单位异常。" + exp.Message, null);
            }
        }

        public Tuple<string, TradeTime> GetTradeTime()
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetTradeTime");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, TradeTime>("获取营业时间地址为空。", null);

            try
            {
                var response = HttpHelper.HttpGet<GetTradeTimeResponse>(addr);
                var result = new TradeTime(response.detail.begintime, response.detail.endtime);
                return new Tuple<string, TradeTime>(null, result);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(exp);
                return new Tuple<string, TradeTime>("获取营业时间异常。" + exp.Message, null);
            }
        }

        public Tuple<string, bool> CheckWhetherTheLastEndWork()
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("CheckTheLastEndWork");
                var result = HttpHelper.HttpGet<CheckWhetherEndWorkResponse>(addr);
                if (!result.IsSuccess)
                    return new Tuple<string, bool>(result.msg ?? "检测是否结业失败。", false);

                return new Tuple<string, bool>(null, result.detail);
            }
            catch (Exception ex)
            {
                return new Tuple<string, bool>(ex.Message, false);
            }
        }

        public Tuple<string, GetClearPosInfoResponse> GetClearPosInfo(string userId)
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetClearPosInfo");
                var list = new List<string> { userId, " ", SystemConfigCache.PosId };
                return RestHttpHelper.HttpGet<GetClearPosInfoResponse>(addr, list);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("获取清机单信息时异常。", exp);
                return new Tuple<string, GetClearPosInfoResponse>(exp.MyMessage(), null);
            }
        }

        public Tuple<string, ReportStatisticInfo> GetReportTipInfo(EnumStatisticsPeriodsType periodsType)
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetReportTipInfo");
                var request = new Dictionary<string, string> { { "flag", ((int)periodsType).ToString() } };
                var result = HttpHelper.HttpGet<GetReportStatisticInfoBase<ReportDishInfoResponse>>(addr, request);
                if (!result.IsSuccess)
                    return new Tuple<string, ReportStatisticInfo>(string.IsNullOrEmpty(result.msg) ? "获取品项销售统计信息失败。" : result.msg, null);

                return new Tuple<string, ReportStatisticInfo>(null, DataConverter.ToReportStatisticInfo(result));
            }
            catch (Exception ex)
            {
                return new Tuple<string, ReportStatisticInfo>(ex.Message, null);
            }
        }

        public Tuple<string, ReportStatisticInfo> GetReportDishInfo(EnumStatisticsPeriodsType periodsType)
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetReportDishInfo");
                var request = new Dictionary<string, string> { { "flag", ((int)periodsType).ToString() } };
                var result = HttpHelper.HttpGet<GetReportStatisticInfoBase<ReportDishInfoResponse>>(addr, request);
                if (!result.IsSuccess)
                    return new Tuple<string, ReportStatisticInfo>(string.IsNullOrEmpty(result.msg) ? "获取小费统计信息失败。" : result.msg, null);

                return new Tuple<string, ReportStatisticInfo>(null, DataConverter.ToReportStatisticInfo(result));
            }
            catch (Exception ex)
            {
                return new Tuple<string, ReportStatisticInfo>(ex.Message, null);
            }
        }

        public Tuple<string, BranchInfo> GetBranchInfo()
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetBranchInfo");
                var result = HttpHelper.HttpGet<GetBranchInfoResponse>(addr);
                if (result.IsSuccess)//这个接口1是成功，0是失败。
                    return new Tuple<string, BranchInfo>(string.IsNullOrEmpty(result.msg) ? "获取分店信息失败。" : result.msg, null);

                var data = DataConverter.ToBranchInfo(result.data);
                return new Tuple<string, BranchInfo>(null, data);
            }
            catch (Exception ex)
            {
                return new Tuple<string, BranchInfo>(ex.MyMessage(), null);
            }
        }

        public string OpenCash(string cashIpAddr)
        {
            var addr = ServiceAddrCache.GetServiceAddr("OpenCash");
            if (string.IsNullOrEmpty(addr))
                return "打开钱箱的地址为空。";

            try
            {
                var param = new List<string> { cashIpAddr };
                var response = RestHttpHelper.HttpGet(addr, param);
                if (!string.IsNullOrEmpty(response.Item1))
                    return string.IsNullOrEmpty(response.Item1) ? "打开钱箱失败。" : response.Item1;

                var jObj = response.Item2.DeserializeJObject();
                var result = jObj["result"].ToString();
                return !result.Trim('[', ']').Trim().Equals("\"1\"") ? "打开钱箱失败。" : null;//这里1标示成功。
            }
            catch (Exception ex)
            {
                return ex.MyMessage();
            }
        }

        public Tuple<string, List<QueryOrderInfo>> QueryOrderInfos(string userId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("QueryOrderInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<QueryOrderInfo>>("账单查询地址为空。", null);

            try
            {
                var param = new List<string> { userId };
                var result = RestHttpHelper.HttpGet<QueryOrderInfoResponse>(addr, param);
                if (!string.IsNullOrEmpty(result.Item1))
                    return new Tuple<string, List<QueryOrderInfo>>(string.Format("账单查询失败：{0}", result.Item1), null);

                List<QueryOrderInfo> list = new List<QueryOrderInfo>();
                if (result.Item2 != null && result.Item2.OrderJson != null && result.Item2.OrderJson.Data != null)
                    list = result.Item2.OrderJson.Data.Select(DataConverter.ToQueryOrderInfo).ToList();
                return new Tuple<string, List<QueryOrderInfo>>(null, list);
            }
            catch (Exception ex)
            {
                return new Tuple<string, List<QueryOrderInfo>>(string.Format("账单查询失败：{0}", ex.MyMessage()), null);
            }
        }

        public string SetCouponFavor(string couponId, bool isCommonlyUsed)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SetCouponFavor");
            if (string.IsNullOrEmpty(addr))
                return "账单查询地址为空。";

            try
            {
                var request = new SetCouponFavorRequest { preferential = couponId, operationtype = isCommonlyUsed ? "0" : "1" };
                var result = HttpHelper.HttpPost<SetCouponFavorResponse>(addr, request);
                if (!result.IsSuccess)
                    return !string.IsNullOrEmpty(result.msg) ? result.msg : "设置优惠券偏好失败。";
                return null;
            }
            catch (Exception ex)
            {
                return string.Format("设置优惠券偏好异常：{0}", ex.MyMessage());
            }
        }
    }
}