using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Reports;
using CanDao.Pos.Model.Response;
using CanDao.Pos.Model;

namespace CanDao.Pos.ServiceImpl
{
    /// <summary>
    /// 打印服务类
    /// </summary>
    public class PrintServiceImpl : IPrintService
    {
        /// <summary>
        /// 打印预结，结算，客用
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderid"></param>
        /// <param name="printPayType"></param>
        /// <returns></returns>
        public string PrintPay(string Userid, string orderid, EnumPrintPayType printPayType)
        {
            string msg = string.Empty;
            switch (printPayType)
            {
                case EnumPrintPayType.BeforehandPay:
                {
                    msg = "打印预结单";
                    break;
                }
                case EnumPrintPayType.Pay:
                {
                    msg = "打印结算单";
                    break;
                }
                case EnumPrintPayType.CustomerUse:
                {
                    msg = "打印客用单";
                    break;
                }
            }

            var addr = ServiceAddrCache.GetServiceAddr("PrintPay");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                string parmAddr = string.Format("{0}/{1}/{2}/{3}", addr, Userid, orderid, (int)printPayType);
                var response = HttpHelper.HttpPost<JavaResponse>(parmAddr, null);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }
          
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}",msg,exp);
            }
        }
        /// <summary>
        /// 打印清机单
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="jsorder"></param>
        /// <param name="posid"></param>
        /// <returns></returns>
        public string PrintClearMachine(string Userid, string jsorder, string posid)
        {
            string msg = "打印清机单";
            var addr = ServiceAddrCache.GetServiceAddr("PrintClearMachine");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                string parmAddr = string.Format("{0}/{1}/{2}/{3}", addr, Userid, jsorder, posid);
                var response = HttpHelper.HttpPost<JavaResponse>(parmAddr, null);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印会员消费
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public string PrintMemberSale(string Userid, string orderid)
        {
            string msg = "打印会员消费";
            var addr = ServiceAddrCache.GetServiceAddr("PrintMemberSale");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                string parmAddr = string.Format("{0}/{1}/{2}", addr, Userid, orderid);
                var response = HttpHelper.HttpPost<JavaResponse>(parmAddr, null);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印会员充值
        /// </summary>
        /// <param name="storeInfo"></param>
        /// <returns></returns>
        public string PrintMemberStore(PrintMemberStoredInfo storeInfo)
        {
            string msg = "打印会员充值";
            var addr = ServiceAddrCache.GetServiceAddr("PrintMemberStore");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("cardno", storeInfo.CardNo);
                param.Add("memberTitle", storeInfo.ReportTitle);
                param.Add("pzh", storeInfo.TraceCode);
                param.Add("date", storeInfo.TradeTime.ToString("yyyy-MM-dd"));
                param.Add("time", storeInfo.TradeTime.ToString("HH:mm:ss"));
                param.Add("storeName", storeInfo.BranchName);
                param.Add("storedAmount", storeInfo.StoredAmount.ToString());
                param.Add("storedBalance", storeInfo.ScoreBalance.ToString());
                param.Add("storedPoint", storeInfo.ScoreBalance.ToString());

                var response = HttpHelper.HttpPost<JavaResponse>(addr, param);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印品项销售统计报表
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string PrintItemSell(string flag)
        {
            string msg = "打印品项销售统计报表";
            var addr = ServiceAddrCache.GetServiceAddr("PrintItemSell");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("flag", flag);
           
                var response = HttpHelper.HttpGet<JavaResponse>(addr, param);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印小费统计报表
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string PrintTip(string flag)
        {
            string msg = "打印小费统计报表";
            var addr = ServiceAddrCache.GetServiceAddr("PrintTip");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("flag", flag);

                var response = HttpHelper.HttpGet<JavaResponse>(addr, param);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印营业报表明细
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string PrintBusinessDetail(string beginTime, string endTime)
        {
            string msg = "打印营业报表明细";
            var addr = ServiceAddrCache.GetServiceAddr("PrintBusinessDetail");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("beginTime", beginTime);
                param.Add("endTime", endTime);

                var response = HttpHelper.HttpPost<JavaResponse>(addr, param);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
        /// <summary>
        /// 打印发票小票
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string PrintInvoice(string orderid, string amount)
        {
            string msg = "打印发票小票";
            var addr = ServiceAddrCache.GetServiceAddr("PrintInvoice");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("orderid", orderid);
                param.Add("amount", amount);

                var response = HttpHelper.HttpPost<JavaResponse>(addr, param);
                if (response.IsSuccess)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("{0}错误：{1}", msg, response.msg);
                }

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return string.Format("{0}异常：{1}", msg, exp);
            }
        }
    }
}
