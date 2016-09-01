using System.Globalization;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
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
            var param = new PrintParam(orderId, printer);
            TaskService.Start(param, PrintPrintMemberPayBillProcess, PrintPrintMemberPayBillComplete, "打印会员交易凭条中...");
        }

        /// <summary>
        /// 打印清机单。
        /// </summary>
        /// <param name="userId"></param>
        public void PrintClearPosReport(string userId)
        {
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

        /// <summary>
        /// 打印发票单。
        /// </summary>
        /// <param name="invoiceInfo">发票单信息。</param>
        public void PrintInvoiceReport(PrintInvoiceInfo invoiceInfo)
        {
            TaskService.Start(invoiceInfo, PrintInvoiceProcess, PrintInvoiceComplete, "打印发票单中...");
        }

        /// <summary>
        /// 打印品项销售明细。
        /// </summary>
        /// <param name="type">统计周期。</param>
        public void PrintDishInfoStatisticReport(EnumStatisticsPeriodsType type)
        {
            TaskService.Start(type, PrintDishInfoStatisticReportProcess, PrintDishInfoStatisticReportComplete, "打印品项销售明细中...");
        }

        /// <summary>
        /// 打印小费统计信息。
        /// </summary>
        /// <param name="type"></param>
        public void PrintTipInfoStatisticReport(EnumStatisticsPeriodsType type)
        {
            TaskService.Start(type, PrintTipInfoStatisticReportProcess, PrintTipInfoStatisticReportComplete, "打印小费统计明细中...");
        }

        /// <summary>
        /// 打印会员储值凭条。
        /// </summary>
        /// <param name="info">会员储值信息。</param>
        public void PrintMemberStoredReport(PrintMemberStoredInfo info)
        {
            TaskService.Start(info, PrintMemberStoredReportProcess, PrintMemberStoredReportComplete, "打印会员储值凭条中...");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 打印报表结束处理。
        /// </summary>
        /// <param name="printType"></param>
        /// <param name="result"></param>
        private void PrintReportComplete(EnumPrintType printType, string result)
        {
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
                case EnumPrintType.Invoice:
                    typeString = "发票单";
                    break;
                case EnumPrintType.DishInfoStatistic:
                    typeString = "品项销售明细";
                    break;
                case EnumPrintType.TipInfoStatistic:
                    typeString = "小费统计明细";
                    break;
                case EnumPrintType.MemberStored:
                    typeString = "会员储值";
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
            NotifyDialog.Notify(msg2, _ownerWnd != null ? _ownerWnd.Owner : null);
        }

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

        /// <summary>
        /// 打印清机单的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintClearPosProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始打印清机单...");
            return service.PrintClearMachine((string)param, " ", SystemConfigCache.PosId);
        }

        /// <summary>
        /// 打印清机单执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintClearPosComplete(object param)
        {
            PrintReportComplete(EnumPrintType.ClearPos, (string)param);
        }

        /// <summary>
        /// 打印发票的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintInvoiceProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始打印发票单...");
            var invoiceInfo = (PrintInvoiceInfo)param;
            return service.PrintInvoice(invoiceInfo.OrderId, invoiceInfo.InvoiceAmount.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 打印发票执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintInvoiceComplete(object param)
        {
            PrintReportComplete(EnumPrintType.Invoice, (string)param);
        }

        /// <summary>
        /// 品项销售明细打印执行。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintDishInfoStatisticReportProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始准备打印品项销售统计数据...");
            var type = (EnumStatisticsPeriodsType)param;
            return service.PrintItemSell(((int)type).ToString());
        }

        /// <summary>
        /// 品项销售明细打印完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintDishInfoStatisticReportComplete(object param)
        {
            PrintReportComplete(EnumPrintType.DishInfoStatistic, (string)param);
        }

        /// <summary>
        /// 打印小费统计执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintTipInfoStatisticReportProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始准备打印小费统计数据...");
            var type = (EnumStatisticsPeriodsType)param;
            return service.PrintTip(((int)type).ToString());
        }

        /// <summary>
        /// 打印小费统计执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintTipInfoStatisticReportComplete(object param)
        {
            PrintReportComplete(EnumPrintType.TipInfoStatistic, (string)param);
        }

        /// <summary>
        /// 打印会员储值凭条执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PrintMemberStoredReportProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IPrintService>();
            if (service == null)
                return "创建IPrintServer服务失败。";

            InfoLog.Instance.I("开始准备打印小费统计数据...");
            var info = (PrintMemberStoredInfo)param;
            return service.PrintMemberStore(info);
        }

        /// <summary>
        /// 打印会员储值凭条执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void PrintMemberStoredReportComplete(object param)
        {
            PrintReportComplete(EnumPrintType.MemberStored, (string)param);
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
        /// <summary>
        /// 发票。
        /// </summary>
        Invoice,
        /// <summary>
        /// 品项销售明细。
        /// </summary>
        DishInfoStatistic,
        /// <summary>
        /// 小费明细。
        /// </summary>
        TipInfoStatistic,
        /// <summary>
        /// 会员储值凭条。
        /// </summary>
        MemberStored,
    }
}