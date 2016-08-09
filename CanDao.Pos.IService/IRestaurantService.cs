using System;
using System.Collections.Generic;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Reports;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.IService
{
    /// <summary>
    /// 店铺相关服务接口。
    /// </summary>
    public interface IRestaurantService
    {
        /// <summary>
        /// 检测餐厅是否开业。
        /// </summary>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为是否开业。</returns>
        Tuple<string, bool> CheckRestaurantOpened();

        /// <summary>
        /// 开业。
        /// </summary>
        /// <param name="userName">用户。</param>
        /// <param name="password">密码。</param>
        /// <returns>开业成功返回null，否则返回错误信息。</returns>
        string RestaurantOpening(string userName, string password);

        /// <summary>
        /// 检测零找金是否已经输入。
        /// </summary>
        /// <param name="userName">登录用户名。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为是否已经输入。</returns>
        Tuple<string, bool> CheckPettyCashInput(string userName);

        /// <summary>
        /// 输入零找金。
        /// </summary>
        /// <param name="userName">用户账户。</param>
        /// <param name="amount">零找金金额。</param>
        /// <returns>输入成功返回null，否则返回错误信息。</returns>
        string InputPettyCash(string userName, decimal amount);

        /// <summary>
        /// 获取系统设置。
        /// </summary>
        /// <param name="type">系统设置类型。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为系统设置值。</returns>
        Tuple<string, List<SystemSetData>> GetSystemSetData(EnumSystemDataType type);

        /// <summary>
        /// 获取餐具信息。
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Tuple<string, OrderDishInfo> GetDinnerWareInfo(string userName);

        /// <summary>
        /// 获取所有餐桌信息。
        /// </summary>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为餐桌信息集合。</returns>
        Tuple<string, List<TableInfo>> GetAllTableInfoes();

        /// <summary>
        /// 获取指定类型的餐桌信息。
        /// </summary>
        /// <param name="tableTypes">餐台类型集合。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为餐桌信息集合。</returns>
        Tuple<string, List<TableInfo>> GetTableInfoByType(List<EnumTableType> tableTypes);

        /// <summary>
        /// 开台。
        /// </summary>
        /// <param name="request">开台请求类。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为开台后单号。</returns>
        Tuple<string, string> OpenTable(OpenTableRequest request);

        /// <summary>
        /// 清机。
        /// </summary>
        /// <param name="request">清机请求参数。</param>
        /// <returns>清机成功返回null，否则返回错误信息。</returns>
        string Clearner(ClearnerRequest request);

        /// <summary>
        /// 获取所有未清机的POS信息。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<UnclearMachineInfo>> GetUnclearnPosInfo();

        /// <summary>
        /// 结业。
        /// </summary>
        /// <returns>结业成功返回null，否则返回错误信息。</returns>
        string EndWork();

        /// <summary>
        /// 结业后通知后台同步数据。
        /// </summary>
        /// <returns>调用成功返回null，否则返回错误信息。</returns>
        string EndWorkSyncData();

        /// <summary>
        /// 广播消息。
        /// </summary>
        /// <param name="msgId">消息编号。</param>
        /// <param name="msg">广播的消息内容。</param>
        /// <returns></returns>
        string BroadcastMessage(int msgId, string msg);

        /// <summary>
        /// 获取所有银行数据。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<BankInfo>> GetAllBankInfos();

        /// <summary>
        /// 获取所有可挂账单位。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<OnCompanyAccountInfo>> GetAllOnAccountCompany();

        /// <summary>
        /// 获取营业时间。
        /// </summary>
        /// <returns></returns>
        Tuple<string, TradeTime> GetTradeTime();

        /// <summary>
        /// 检测上一次是否结业。
        /// </summary>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为是否结业，已结业返回true，否则返回false。</returns>
        Tuple<string, bool> CheckWhetherTheLastEndWork();

        /// <summary>
        /// 获取清机信息。
        /// </summary>
        /// <param name="userId">清机用户ID。</param>
        /// <returns></returns>
        Tuple<string, GetClearPosInfoResponse> GetClearPosInfo(string userId);

        /// <summary>
        /// 获取小费统计信息。
        /// </summary>
        /// <param name="periodsType">统计周期类型。</param>
        /// <returns></returns>
        Tuple<string, ReportStatisticInfo> GetReportTipInfo(EnumStatisticsPeriodsType periodsType);

        /// <summary>
        /// 获取品项销售统计信息。
        /// </summary>
        /// <param name="periodsType">统计周期类型。</param>
        /// <returns></returns>
        Tuple<string, ReportStatisticInfo> GetReportDishInfo(EnumStatisticsPeriodsType periodsType);

        /// <summary>
        /// 获取分店信息。
        /// </summary>
        /// <returns></returns>
        Tuple<string, BranchInfo> GetBranchInfo();

        /// <summary>
        /// 开钱箱。
        /// </summary>
        /// <param name="cashIpAddr">钱箱IP地址。</param>
        /// <returns>打开成功返回null，否则返回错误信息。</returns>
        string OpenCash(string cashIpAddr);

        /// <summary>
        /// 账单查询。
        /// </summary>
        /// <param name="userId">查询用户Id。</param>
        /// <returns>Items正常则为null，否则为错误信息，Item2为返回的账单集合。</returns>
        Tuple<string, List<QueryOrderInfo>> QueryOrderInfos(string userId);

        /// <summary>
        /// 设置优惠券偏爱。
        /// </summary>
        /// <param name="couponId">优惠券Id。</param>
        /// <param name="isCommonlyUsed">是否常用</param>
        /// <returns></returns>
        string SetCouponFavor(string couponId, bool isCommonlyUsed);

        /// <summary>
        /// 获取打印机状态集合。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<PrintStatusInfo>> GetPrinterStatusInfo();

        /// <summary>
        /// 获取营业明细（品类、金额）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Tuple<string, List<MCategory>> GetItemForList(string beginTime, string endTime);

        /// <summary>
        /// 设置外卖订单挂账。
        /// </summary>
        /// <param name="tableNo">外卖桌号。</param>
        /// <param name="orderId">订单号。</param>
        /// <param name="cmpInfo">外卖挂账单单位信息。</param>
        /// <returns></returns>
        string SetTakeoutOrderOnAccount(string tableNo, string orderId, SetTakeoutOrderOnAccountRequest cmpInfo);
        /// <summary>
        /// 获取营业明细(团购券)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Tuple<string, List<MHangingMoney>> GetGrouponForList(string beginTime, string endTime);

        /// <summary>
        /// 获取营业明细
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Tuple<string, MBusinessDataDetail> GetDayReportList(string beginTime, string endTime, string userName);

        /// <summary>
        /// 获取营业明细（获取挂账单位）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Tuple<string, List<MHangingMoney>> GetGzdwForList(string beginTime, string endTime);

        /// <summary>
        /// 获取小费总额
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Tuple<string, string> GetTipMoney(string beginTime, string endTime);


    }
}