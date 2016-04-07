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
        /// 获取小费明细。
        /// </summary>
        /// <param name="periodsType">统计周期。</param>
        /// <returns></returns>
        Tuple<string, TipFullInfo> GetTipInfos(EnumStatisticsPeriodsType periodsType);
    }
}