using System;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.ReportPrint
{
    /// <summary>
    /// 打印辅助类（Java后台接口打印版）
    /// </summary>
    public class ReportPrintHelper2
    {
        #region Fields

        private readonly Window _ownerWnd;

        #endregion

        #region Constructor

        public ReportPrintHelper2(Window ownerWindow)
        {
            _ownerWnd = ownerWindow;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 打印预结单。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printUser">打印人员。</param>
        public void PrintPresettlementReport(string orderId, string printUser)
        {
            ReportPrintingWindow.Instance.Show();
            var param = new PrintParamWithType(orderId, printUser, EnumPrintPayType.BeforehandPay);
            TaskService.Start(param, PrintPayBillProcess, PrintPresettlementBillComplete, "打印预结单中...");
        }

        /// <summary>
        /// 打印结账单。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printUser">打印人员。</param>
        public void PrintSettlementReport(string orderId, string printUser)
        {
            ReportPrintingWindow.Instance.Show();
            var param = new PrintParamWithType(orderId, printUser, EnumPrintPayType.Pay);
            TaskService.Start(param, PrintPayBillProcess, PrintSettlementBillComplete, "打印结账单中...");
        }

        /// <summary>
        /// 打印客用单。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="printUser"></param>
        public void PrintCustomUseBillReport(string orderId, string printUser)
        {
            ReportPrintingWindow.Instance.Show();
            var param = new PrintParamWithType(orderId, printUser, EnumPrintPayType.CustomerUse);
            TaskService.Start(param, PrintPayBillProcess, PrintCustomUseBillComplete, "打印客用单中...");
        }

        /// <summary>
        /// 打印会员交易凭条。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printer">打印用户id。</param>
        public void PrintMemberPayBillReport(string orderId, string printer)
        {
            ReportPrintingWindow.Instance.Show();
            var param = new PrintParam(orderId, printer);
            TaskService.Start(param, PrintPrintMemberPayBillProcess, PrintPrintMemberPayBillComplete, "打印会员交易凭条中...");
        }

        /// <summary>
        /// 打印清机单。
        /// </summary>
        /// <param name="userId"></param>
        public void PrintClearPosReport(string userId)
        {
            ReportPrintingWindow.Instance.Show();
            TaskService.Start(userId, PrintClearPosProcess, PrintClearPosComplete, "打印清机单中...");
        }

        public bool PrintClearPosReportSync(string userId)
        {
            ReportPrintingWindow.Instance.Show();

            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IPrintServer服务失败。");
                return false;
            }

            InfoLog.Instance.I("开始打印清机单...");
            var result = service.PrintClearMachine(userId, " ", SystemConfigCache.PosId);
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("清机单打印失败：{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, _ownerWnd);
                return false;
            }

            var msg2 = "清机单打印完成。";
            InfoLog.Instance.I(msg2);
            NotifyDialog.Notify(msg2, _ownerWnd.Owner);

            ReportPrintingWindow.Instance.Hide();
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 打印会员交易凭条执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintPrintMemberPayBillProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            var arg = (PrintParam)param;
            InfoLog.Instance.I("开始打印交易凭条...");
            return service.PrintMemberSale(arg.Printer, arg.OrderId);
        }

        /// <summary>
        /// 打印会员交易凭条执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintPrintMemberPayBillComplete(object param)
        {
            ReportPrintingWindow.Instance.Hide();
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("交易凭条打印失败：{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, _ownerWnd);
                return;
            }

            InfoLog.Instance.I("交易凭条打印完成。");
            NotifyDialog.Notify("交易凭条打印完成。", _ownerWnd.Owner);
        }

        /// <summary>
        /// 打印结账相关账单的执行方法。（包括预结单，结账单，客用单）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintPayBillProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            var arg = (PrintParamWithType)param;
            InfoLog.Instance.I("开始打印结账单...");
            return service.PrintPay(arg.Printer, arg.OrderId, arg.PrintType);
        }

        /// <summary>
        /// 打印预结单完成时执行。
        /// </summary>
        /// <param name="param"></param>
        private void PrintPresettlementBillComplete(object param)
        {
            PrintReportComplete(EnumPrintType.PreSettlement, (string)param);
        }

        /// <summary>
        /// 打印结账单执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintSettlementBillComplete(object param)
        {
            PrintReportComplete(EnumPrintType.Settlement, (string)param);
        }

        /// <summary>
        /// 打印客用单完成时执行。
        /// </summary>
        /// <param name="param"></param>
        private void PrintCustomUseBillComplete(object param)
        {
            PrintReportComplete(EnumPrintType.CustomUse, (string)param);
        }

        private object PrintClearPosProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始打印清机单...");
            return service.PrintClearMachine((string)param, " ", SystemConfigCache.PosId);
        }

        private void PrintClearPosComplete(object param)
        {
            PrintReportComplete(EnumPrintType.ClearPos, (string)param);
        }

        /// <summary>
        /// 打印报表结束处理。
        /// </summary>
        /// <param name="printType"></param>
        /// <param name="result"></param>
        private void PrintReportComplete(EnumPrintType printType, string result)
        {
            ReportPrintingWindow.Instance.Hide();
            var typeString = "";
            switch (printType)
            {
                case EnumPrintType.PreSettlement:
                    typeString = "预结单";
                    break;
                case EnumPrintType.Settlement:
                    typeString = "结账单";
                    break;
                case EnumPrintType.CustomUse:
                    typeString = "客用单";
                    break;
                case EnumPrintType.ClearPos:
                    typeString = "清机单";
                    break;
                case EnumPrintType.MemberPay:
                    typeString = "会员凭条";
                    break;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("{0}打印失败：{1}", typeString, result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, _ownerWnd);
                return;
            }

            var msg2 = string.Format("{0}打印完成。", typeString);
            InfoLog.Instance.I(msg2);
            NotifyDialog.Notify(msg2, _ownerWnd.Owner);
        }

        #endregion
    }

    /// <summary>
    /// 打印参数类。
    /// </summary>
    internal class PrintParam
    {
        public PrintParam(string orderId, string printer)
        {
            OrderId = orderId;
            Printer = printer;
        }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 打印人。
        /// </summary>
        public string Printer { get; set; }
    }

    internal class PrintParamWithType : PrintParam
    {
        public PrintParamWithType(string orderId, string printer, EnumPrintPayType printType)
            : base(orderId, printer)
        {
            PrintType = printType;
        }

        public EnumPrintPayType PrintType { get; set; }
    }

    /// <summary>
    /// 打印类型枚举。
    /// </summary>
    internal enum EnumPrintType
    {
        /// <summary>
        /// 预结单。
        /// </summary>
        PreSettlement,
        /// <summary>
        /// 结账单。
        /// </summary>
        Settlement,
        /// <summary>
        /// 客用单。
        /// </summary>
        CustomUse,
        /// <summary>
        /// 会员交易凭条。
        /// </summary>
        MemberPay,
        /// <summary>
        /// 清机单。
        /// </summary>
        ClearPos,
    }
}