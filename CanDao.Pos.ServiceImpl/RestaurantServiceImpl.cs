using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Reports;
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
            List<string> param = new List<string> { "", "", PCInfoHelper.LocalIPAddr, "0" };
            //最后一个"0"是接口的CallType为1时是开业，为0时是检测是否开业。
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

            List<string> param = new List<string> { userName, password, PCInfoHelper.LocalIPAddr, "1" };
            //最后一个"1"是接口的CallType为1时是开业，为0时是检测是否开业。
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

            List<string> param = new List<string> { userName, MachineManage.GetMachineId(), "0", "0" };
            //参数顺序：当前用户，机器标识，零找金额，查询或输入（0查询，1输入）
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, bool>(string.Format("检测是否输入零找金是错误：{0}", result.Item1), false);
            return new Tuple<string, bool>(null, result.Item2.IsSuccess);
        }

        public string InputPettyCash(string userName, decimal amount)
        {
            string addr = ServiceAddrCache.GetServiceAddr("PettyCashInput");
            if (string.IsNullOrEmpty(addr))
                return "零找金接口地址为空。";

            List<string> param = new List<string>
            {
                userName,
                MachineManage.GetMachineId(),
                amount.ToString(CultureInfo.InvariantCulture),
                "1"
            };
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

            var dishList = result.Item2.OrderJson;
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
                var result = HttpHelper.HttpPost<GetAllTableInfoesResponse>(addr, null);
                var dataList = new List<TableInfo>();
                if (result.IsSuccess)
                {
                    dataList = result.data.Select(DataConverter.ToTableInfo).ToList();
                    return new Tuple<string, List<TableInfo>>(null, dataList);
                }
                else
                {
                    return new Tuple<string, List<TableInfo>>(result.msg, null);
                }
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
                var request = new GetTableByTypeRequest
                {
                    tableType = tableTypes.Select(t => ((int)t).ToString()).ToList()
                };
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
                    return new Tuple<string, string>(null, result.data.orderid);

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

            var mac = request.Mac ?? MachineManage.GetMachineId();
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

        public string EndWork()
        {
            var addr = ServiceAddrCache.GetServiceAddr("EndWork");
            if (string.IsNullOrEmpty(addr))
                return "结业地址为空。";

            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr);
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
                var result = HttpHelper.HttpPost<SyncDataResponse>(addr, request, 500);
                return result.IsSuccess ? null : !string.IsNullOrWhiteSpace(result.message) ? result.message : "通知后台同步数据失败。";
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

            return result.Item2.IsSuccess
                ? null
                : string.IsNullOrEmpty(result.Item2.Info) ? "广播消息失败" : result.Item2.Info;
        }

        public Tuple<string, List<BankInfo>> GetAllBankInfos()
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetAllBankInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<BankInfo>>("广播消息地址为空。", null);

            try
            {
                var response = HttpHelper.HttpGet<List<GetBankInfoResponse>>(addr);
                var result = response != null
                    ? response.Select(DataConverter.ToBankInfo).ToList()
                    : new List<BankInfo>();
                return new Tuple<string, List<BankInfo>>(null, result.OrderBy(t => t.SortIndex).ToList());
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, List<BankInfo>>("获取银行信息异常。" + ex.MyMessage(), null);
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
                var result = response != null
                    ? response.Select(DataConverter.ToOnCpyAccInfo).ToList()
                    : new List<OnCompanyAccountInfo>();
                return new Tuple<string, List<OnCompanyAccountInfo>>(null, result);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(exp);
                return new Tuple<string, List<OnCompanyAccountInfo>>("获取挂账单位异常。" + exp.MyMessage(), null);
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
                return new Tuple<string, TradeTime>("获取营业时间异常。" + exp.MyMessage(), null);
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
                return new Tuple<string, bool>(ex.MyMessage(), false);
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
                var result = HttpHelper.HttpGet<GetReportStatisticInfoBase<ReportTipInfoResponse>>(addr, request);
                if (!result.IsSuccess)
                    return new Tuple<string, ReportStatisticInfo>(string.IsNullOrEmpty(result.msg) ? "获取小费统计信息失败。" : result.msg, null);

                return new Tuple<string, ReportStatisticInfo>(null, DataConverter.ToReportStatisticInfo(result));
            }
            catch (Exception ex)
            {
                return new Tuple<string, ReportStatisticInfo>(ex.MyMessage(), null);
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
                    return new Tuple<string, ReportStatisticInfo>(string.IsNullOrEmpty(result.msg) ? "获取品项消费统计信息失败。" : result.msg, null);

                return new Tuple<string, ReportStatisticInfo>(null, DataConverter.ToReportStatisticInfo(result));
            }
            catch (Exception ex)
            {
                return new Tuple<string, ReportStatisticInfo>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, BranchInfo> GetBranchInfo()
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetBranchInfo");
                var result = HttpHelper.HttpGet<GetBranchInfoResponse>(addr);
                if (!result.IsSuccess) //这个接口1是成功，0是失败。
                    return new Tuple<string, BranchInfo>(string.IsNullOrEmpty(result.msg) ? "获取分店信息失败。" : result.msg,
                        null);

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
                return !result.Trim('[', ']').Trim().Equals("\"1\"") ? "打开钱箱失败。" : null; //这里1标示成功。
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
                if (result.Item2 != null && result.Item2.OrderJson != null && result.Item2.OrderJson != null)
                    list = result.Item2.OrderJson.Select(DataConverter.ToQueryOrderInfo).ToList();
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
                var request = new SetCouponFavorRequest
                {
                    preferential = couponId,
                    operationtype = isCommonlyUsed ? "0" : "1"
                };
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

        public Tuple<string, List<PrintStatusInfo>> GetPrinterStatusInfo()
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("GetPrinterList");
                if (string.IsNullOrEmpty(addr))
                    return new Tuple<string, List<PrintStatusInfo>>("打印机状态获取地址为空。", null);

                var response = HttpHelper.HttpGet<GetPrinterStatusResponse>(addr);
                if (!response.isSuccess)
                {
                    var msg = !string.IsNullOrEmpty(response.errorMsg) ? response.errorMsg : "获取打印机状态信息失败。";
                    ErrLog.Instance.E(msg);
                    return new Tuple<string, List<PrintStatusInfo>>(msg, null);
                }

                var list = new List<PrintStatusInfo>();
                if (response.data != null && response.data.Any())
                    list = response.data.Select(DataConverter.ToPrintStatusInfo).ToList();
                return new Tuple<string, List<PrintStatusInfo>>(null, list);
            }
            catch (Exception ex)
            {
                return new Tuple<string, List<PrintStatusInfo>>(ex.MyMessage(), null);
            }
        }

        #region 获取营业明细报表接口

        /// <summary>
        /// 获取营业明细（品类、金额）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Tuple<string, List<MCategory>> GetItemForList(string beginTime, string endTime)
        {
            string msg = "获取营业明细（品类、金额）";

            var addr = ServiceAddrCache.GetServiceAddr("GetItemForList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, List<MCategory>>(msg + "地址为空。", null);
            }
            try
            {
                string parmAddr = string.Format("{0}?beginTime={1}&endTime={2}", addr, beginTime, endTime);
                var response = HttpHelper.HttpGet<List<CategoryResponse>>(parmAddr, null);
                var result = DataConverter.ToCategory(response);
                return new Tuple<string, List<MCategory>>(null, result);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, List<MCategory>>(exp.MyMessage(), null);
            }
        }

        /// <summary>
        /// 获取营业明细(团购券)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Tuple<string, List<MHangingMoney>> GetGrouponForList(string beginTime, string endTime)
        {
            string msg = " 获取营业明细(团购券)";
            var resList = new List<MHangingMoney>();

            var addr = ServiceAddrCache.GetServiceAddr("GetGrouponForList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, List<MHangingMoney>>(msg + "地址为空。", resList);
            }
            try
            {
                string parmAddr = string.Format("{0}?beginTime={1}&endTime={2}&shiftid=-1&bankcardno=-1&settlementWay=5&type=-1", addr, beginTime, endTime);
                var response = HttpHelper.HttpGet<List<GrouponResponse>>(parmAddr, null);
                resList = DataConverter.ToGroupon(response);
                return new Tuple<string, List<MHangingMoney>>(null, resList);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, List<MHangingMoney>>(exp.MyMessage(), resList);
            }
        }

        /// <summary>
        /// 获取营业明细
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Tuple<string, MBusinessDataDetail> GetDayReportList(string beginTime, string endTime, string userName)
        {
            string msg = "获取营业明细(其它)";
            var dataDetail = new MBusinessDataDetail();
            var addr = ServiceAddrCache.GetServiceAddr("GetDayReportList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, MBusinessDataDetail>(msg + "地址为空。", null);
            }
            try
            {
                var request = new Dictionary<string, string>();
                request.Add("beginTime", beginTime);
                request.Add("endTime", endTime);

                var response = HttpHelper.HttpGet<List<DataDetailResponse>>(addr, request);
                if (response.Count > 0)
                {
                    dataDetail.StartTime = DateTime.Parse(beginTime);
                    dataDetail.EndTime = DateTime.Parse(endTime);
                    dataDetail.CurrentTime = DateTime.Now;
                    dataDetail.UserName = userName;
                    dataDetail.kaitaishu = response[0].kaitaishu;

                    dataDetail.bastfree = response[0].bastfree;
                    dataDetail.integralconsum = response[0].integralconsum;
                    dataDetail.meberTicket = response[0].meberTicket;
                    dataDetail.discountmoney = response[0].discountmoney;
                    dataDetail.malingincom = response[0].malingincom;
                    dataDetail.give = response[0].give;
                    dataDetail.handervalue = response[0].handervalue;

                    if (dataDetail.handervalue.Equals("抹零"))
                    {
                        dataDetail.handervalue = "0"; //四舍五入为“0”
                    }
                    else if (string.IsNullOrEmpty(dataDetail.handervalue))
                    {
                        dataDetail.handervalue = "0";
                        dataDetail.malingincom = "0";
                    }
                    else
                    {
                        dataDetail.malingincom = "0";
                    }
                    dataDetail.mebervalueadd = response[0].mebervalueadd;

                    dataDetail.money = response[0].money;
                    dataDetail.card = response[0].card;
                    dataDetail.weixin = response[0].weixin;
                    dataDetail.zhifubao = response[0].zhifubao;
                    dataDetail.icbc = response[0].icbc;
                    dataDetail.otherbank = response[0].otherbank;
                    dataDetail.merbervaluenet = response[0].merbervaluenet;

                    dataDetail.shouldamount = response[0].shouldamount;
                    dataDetail.discountamount = response[0].discountamount;
                    dataDetail.paidinamount = response[0].paidinamount;

                    dataDetail.Categories = GetItemForList(beginTime, endTime).Item2; //获取品项列表

                    dataDetail.HangingMonies = GetGrouponForList(beginTime, endTime).Item2; //获取团购列表

                    dataDetail.HangingMonies.AddRange(GetGzdwForList(beginTime, endTime).Item2); //获取挂账列表

                    dataDetail.xiaofei = GetTipMoney(beginTime, endTime).Item2; //获取消费
                }
                return new Tuple<string, MBusinessDataDetail>(null, dataDetail);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, MBusinessDataDetail>(exp.MyMessage(), null);
            }
        }

        /// <summary>
        /// 获取营业明细（获取挂账单位）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Tuple<string, List<MHangingMoney>> GetGzdwForList(string beginTime, string endTime)
        {
            string msg = " 获取营业明细(获取挂账单位)";
            var resList = new List<MHangingMoney>();

            var addr = ServiceAddrCache.GetServiceAddr("GetGzdwForList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, List<MHangingMoney>>(msg + "地址为空。", resList);
            }
            try
            {
                string parmAddr = string.Format("{0}?beginTime={1}&endTime={2}&billName=0&clearStatus=0", addr, beginTime, endTime);
                var response = HttpHelper.HttpGet<List<GzdwResponse>>(parmAddr, null);
                resList = DataConverter.ToGzdw(response);
                return new Tuple<string, List<MHangingMoney>>(null, resList);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, List<MHangingMoney>>(exp.MyMessage(), resList);
            }
        }

        /// <summary>
        /// 获取小费总额
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Tuple<string, string> GetTipMoney(string beginTime, string endTime)
        {
            string msg = "获取小费总额";

            var addr = ServiceAddrCache.GetServiceAddr("GetTipMoney");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, string>(msg + "地址为空。", "0");
            }
            try
            {
                string parmAddr = string.Format("{0}?beginTime={1}&endTime={2}", addr, beginTime, endTime);
                var response = HttpHelper.HttpGet<TipMoneyResponse>(parmAddr, null);
                return new Tuple<string, string>(null, response.tipMoney);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, string>(exp.MyMessage(), "0");
            }
        }

        #endregion

        public string SetTakeoutOrderOnAccount(string tableNo, string orderId, SetTakeoutOrderOnAccountRequest cmpInfo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SetTakeoutOrderOnAccount");
            if (string.IsNullOrEmpty(addr))
                return "外卖挂单地址为空。";

            var param = new List<string>
            {
                tableNo,
                orderId,
                cmpInfo.CmpCode,
                cmpInfo.CmpName,
                cmpInfo.ContactMobile,
                cmpInfo.ContactName
            };
            var response = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(response.Item1))
                return response.Item1;

            if (!response.Item2.IsSuccess)
                return !string.IsNullOrEmpty(response.Item2.Info) ? response.Item2.Info : "设置外卖挂单失败，请联系管理员或重试。";

            return null;
        }
    }
}