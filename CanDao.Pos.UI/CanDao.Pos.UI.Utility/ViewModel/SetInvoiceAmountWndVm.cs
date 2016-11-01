using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SetInvoiceAmountWndVm : NormalWindowViewModel<SetInvoiceAmountWindow>
    {
        public SetInvoiceAmountWndVm(TableFullInfo tableFullInfo)
        {
            Data = tableFullInfo;
            InvoiceAmount = Data.TotalAmount;
        }

        public TableFullInfo Data { get; set; }

        /// <summary>
        /// 发票金额。
        /// </summary>
        public decimal InvoiceAmount { get; set; }

        protected override void Confirm(object param)
        {
            if (InvoiceAmount == 0)
            {
                MessageDialog.Warning("请先设置发票金额。", OwnerWindow);
                return;
            }

            if (InvoiceAmount > Data.TotalAmount)
            {
                MessageDialog.Warning("发票金额不能大于账单金额。", OwnerWindow);
                return;
            }

            WorkFlowService.Start(null, new WorkFlowInfo(UpdateOrderInvoiceProcess, UpdateOrderInvoiceComplete));
            //if (SystemConfigCache.PrintInvoice)
            //{
            //    var invoiceInfo = new PrintInvoiceInfo
            //    {
            //        OrderId = Data.OrderId,
            //        TableName = Data.TableName,
            //        InvoiceAmount = InvoiceAmount,
            //        InvoiceTitle = Data.OrderInvoiceTitle,
            //    };
            //    ReportPrintHelper.PrintInvoiceReport(invoiceInfo);
            //}
            base.Confirm(param);
        }

        protected override void Cancel(object param)
        {
            if (!MessageDialog.Quest("确定不开发票吗？", OwnerWindow))
                return;

            base.Cancel(param);
        }

        /// <summary>
        /// 更新发票信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object UpdateOrderInvoiceProcess(object arg)
        {
            InfoLog.Instance.I("开始更新发票信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var cardNo = string.IsNullOrEmpty(Data.MemberNo) ? "" : Data.MemberNo;
            return service.UpdateOrderInvoice(Data.OrderId, InvoiceAmount, cardNo);
        }

        /// <summary>
        /// 更新发票信息完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> UpdateOrderInvoiceComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }
            //打印发票
            var invoice = new PrintInvoiceInfo();
            invoice.OrderId = Data.OrderId;
            invoice.InvoiceAmount = InvoiceAmount;
            var print = new ReportPrintHelper2(OwnerWindow.Owner);
            print.PrintInvoiceReport(invoice);
            //ReportPrintHelper.PrintInvoiceReport(invoice);
            InfoLog.Instance.I("结束更新发票信息。");
            return null;
        }
    }
}