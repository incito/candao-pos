using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.IService
{
    /// <summary>
    /// 打印服务接口
    /// </summary>
    public interface IPrintService
    {
        /// <summary>
        /// 打印预结，结算，客用
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderid"></param>
        /// <param name="printPayType">预结单=1；结账单=2；客用单=3</param>
        /// <returns></returns>
        string PrintPay(string Userid, string orderid, EnumPrintPayType printPayType);
        /// <summary>
        /// 打印清机单
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="jsorder">结算ID</param>
        /// <param name="posid"></param>
        /// <returns></returns>
        string PrintClearMachine(string Userid, string jsorder, string posid);
        /// <summary>
        /// 打印会员消费
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        string PrintMemberSale(string Userid, string orderid);
        /// <summary>
        /// 打印会员储值
        /// </summary>
        /// <param name="storeInfo"></param>
        /// <returns></returns>
        string PrintMemberStore(PrintMemberStoredInfo storeInfo);

        /// <summary>
        /// 打印品项销售统计
        /// </summary>
        /// <param name="flag">1：今天；2：本周；3：本月；4：上月</param>
        /// <returns></returns>
        string PrintItemSell(string flag);
        /// <summary>
        /// 打印小费统计
        /// </summary>
        /// <param name="flag">1：今天；2：本周；3：本月；4：上月</param>
        /// <returns></returns>
        string PrintTip(string flag);

        /// <summary>
        /// 打印营业报表明细
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        string PrintBusinessDetail(string beginTime, string endTime, string operationname);
        /// <summary>
        /// 打印发票
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        string PrintInvoice(string orderid, string amount);
    }
}
