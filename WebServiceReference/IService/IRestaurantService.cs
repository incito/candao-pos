using System;
using System.Collections.Generic;
using Models;
using Models.Enum;
using Models.Request;

namespace WebServiceReference.IService
{
    public interface IRestaurantService
    {
        /// <summary>
        /// 清机。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="userName">用户姓名。</param>
        /// <returns>清机成功返回null，否则返回错误信息。</returns>
        string Clearner(string userId, string userName);

        /// <summary>
        /// 获取所有未清机的POS信息。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<NoClearMachineInfo>> GetUnclearnPosInfo();

        /// <summary>
        /// 获取所有餐桌信息。
        /// </summary>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为餐桌信息集合。</returns>
        Tuple<string, List<TableInfo>> GetAllTableInfoes();

        /// <summary>
        /// 检测上一次是否结业。
        /// </summary>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为是否结业，已结业返回true，否则返回false。</returns>
        Tuple<string, bool> CheckWhetherTheLastEndWork();

        /// <summary>
        /// 获取店铺营业时间。
        /// </summary>
        /// <returns></returns>
        Tuple<string, RestaurantTradeTime> GetRestaurantTradeTime();

        /// <summary>
        /// 获取品项销售明细。
        /// </summary>
        /// <param name="periodsType">统计周期。</param>
        /// <returns></returns>
        Tuple<string, DishSaleFullInfo> GetDishSaleInfo(EnumStatisticsPeriodsType periodsType);

        /// <summary>
        /// 设置优惠券是常用或不常用。
        /// </summary>
        /// <param name="couponId">优惠券ID。</param>
        /// <param name="isCommonlyUsed">是否是常用，常用的为true，不常用的为false</param>
        /// <returns>设置成功返回null，否则返回错误信息。</returns>
        string SetCouponFavor(string couponId, bool isCommonlyUsed);

        /// <summary>
        /// 结算小费。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tipAmount">小费实付金额。</param>
        /// <returns>结算成功返回null，否则返回错误信息。</returns>
        string BillingTip(string orderId, float tipAmount);

        /// <summary>
        /// 获取小费明细。
        /// </summary>
        /// <param name="periodsType">统计周期。</param>
        /// <returns></returns>
        Tuple<string, TipFullInfo> GetTipInfos(EnumStatisticsPeriodsType periodsType);

        /// <summary>
        /// 获取打印机状态集合。
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<PrintStatusInfo>> GetPrinterStatusInfo();

        /// <summary>
        /// 获取系统设置。
        /// </summary>
        /// <param name="type">系统设置类型。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为系统设置值。</returns>
        Tuple<string, List<SystemSetData>> GetSystemSetData(EnumSystemDataType type);

        /// <summary>
        /// 整单退菜。
        /// </summary>
        /// <param name="tableNo">餐桌号。</param>
        /// <param name="orderId">订单号。</param>
        /// <param name="authorizer">授权人。</param>
        /// <param name="reason">退菜原因。</param>
        /// <returns></returns>
        string BackAllDish(string tableNo, string orderId, string authorizer, string reason);

        /// <summary>
        /// 获取指定类型的餐桌信息。
        /// </summary>
        /// <param name="tableTypes">餐台类型集合。</param>
        /// <returns>Item1全部正常则为null，否则为错误信息，Item2为餐桌信息集合。</returns>
        Tuple<string, List<TableInfo>> GetTableInfoByType(List<EnumTableType> tableTypes);
    }
}