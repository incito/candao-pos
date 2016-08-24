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
        private readonly Window _ownerWnd;

        public ReportPrintHelper2(Window ownerWindow)
        {
            _ownerWnd = ownerWindow;
        }

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
            NotifyDialog.Notify("交易凭条打印完成。", _ownerWnd);
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
        /// 处理打印预结单、结账单、客用单的执行方法。
        /// </summary>
        /// <param name="printType">打印类型。</param>
        /// <param name="result">结果。</param>
        private void PrintPayBillComplete(EnumPrintOrderType printType, string result)
        {
            ReportPrintingWindow.Instance.Hide();
            var typeString = "";
            switch (printType)
            {
                case EnumPrintOrderType.PreSettlement:
                    typeString = "预结单";
                    break;
                case EnumPrintOrderType.Settlement:
                    typeString = "结账单";
                    break;
                case EnumPrintOrderType.CustomUse:
                    typeString = "客用单";
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
            NotifyDialog.Notify(msg2, _ownerWnd);
        }

        /// <summary>
        /// 打印预结单完成时执行。
        /// </summary>
        /// <param name="param"></param>
        private void PrintPresettlementBillComplete(object param)
        {
            PrintPayBillComplete(EnumPrintOrderType.PreSettlement, (string)param);
        }

        /// <summary>
        /// 打印结账单执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintSettlementBillComplete(object param)
        {
            PrintPayBillComplete(EnumPrintOrderType.Settlement, (string)param);
        }

        /// <summary>
        /// 打印客用单完成时执行。
        /// </summary>
        /// <param name="param"></param>
        private void PrintCustomUseBillComplete(object param)
        {
            PrintPayBillComplete(EnumPrintOrderType.CustomUse, (string)param);
        }

        #endregion
    }

    /// <summary>
    /// 打印参数类。
    /// </summary>
    public class PrintParam
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

    public class PrintParamWithType : PrintParam
    {
        public PrintParamWithType(string orderId, string printer, EnumPrintPayType printType)
            : base(orderId, printer)
        {
            PrintType = printType;
        }

        public EnumPrintPayType PrintType { get; set; }
    }
}