using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Reports;

namespace CanDao.Pos.ReportPrint
{
    /// <summary>
    /// 报表打印辅助类。
    /// </summary>
    public class ReportPrintHelper
    {
        #region Public Methods

        /// <summary>
        /// 打印预结单。
        /// </summary>
        /// <param name="tableFullInfo">订单全信息。</param>
        /// <param name="printUser">当前用户。</param>
        /// <returns></returns>
        public static bool PrintPresettlementReport(TableFullInfo tableFullInfo, string printUser)
        {
            ShowReportPrintingWindow();
            InfoLog.Instance.I("开始获取预结单报表数据...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            var result = service.GetPrintOrderInfo(tableFullInfo.OrderId, printUser, EnumPrintOrderType.PreSettlement);
            InfoLog.Instance.I("结束获取预结单报表数据。");
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("获取预结单报表数据错误：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }

            try
            {
                InfoLog.Instance.I("开始准备预结单报表数据...");
                var mainDb = CreateOrderMainDb();
                AddObject2DataTable(mainDb, result.Item2.OrderInfo);

                var dishesDb = CreateOrderDishDb();
                if (result.Item2.OrderDishInfos != null)
                    result.Item2.OrderDishInfos.ForEach(t => AddObject2DataTable(dishesDb, t));

                var couponDb = CreateCouponsDb();
                if (tableFullInfo.UsedCouponInfos != null && tableFullInfo.UsedCouponInfos.Any())
                {
                    foreach (var usedCouponInfo in tableFullInfo.UsedCouponInfos)
                    {
                        AddObject2DataTable(couponDb, usedCouponInfo);
                    }
                }

                var dic = GetPresettlementDic(tableFullInfo, result.Item2.OrderInfo.FreeAmount);
                var settlementDb = GenerateSettlementDb(dic);

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(dishesDb);
                ds.Tables.Add(settlementDb);
                ds.Tables.Add(couponDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\预结单.frx");
                if (!File.Exists(file))
                {
                    var msg = "预结单报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备预结单报表数据。");
                InfoLog.Instance.I("开始打印预结单报表...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印预结单报表。");

                return true;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("打印预结单时异常。", exp);
                MessageDialog.Warning(string.Format("打印预结单时异常。{0}", exp.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印结账单。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printUser">打印人员。</param>
        /// <returns></returns>
        public static bool PrintSettlementReport(string orderId, string printUser)
        {
            ShowReportPrintingWindow();
            InfoLog.Instance.I("开始获取结账单报表数据...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            var result = service.GetPrintOrderInfo(orderId, printUser, EnumPrintOrderType.Settlement);
            InfoLog.Instance.I("结束获取结账单报表数据。");
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("获取结账单报表数据错误：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }

            try
            {
                InfoLog.Instance.I("开始准备结账单报表数据...");
                var mainDb = CreateOrderMainDb();
                AddObject2DataTable(mainDb, result.Item2.OrderInfo);

                var dishesDb = CreateOrderDishDb();
                if (result.Item2.OrderDishInfos != null)
                    result.Item2.OrderDishInfos.ForEach(t => AddObject2DataTable(dishesDb, t));

                var payDetailDb = CeratePayDetailDb();
                if (result.Item2.PayDetails != null)
                    result.Item2.PayDetails.ForEach(t => AddObject2DataTable(payDetailDb, t));

                var dic = GetSetttlementDic(result.Item2.OrderInfo);
                var settlementDb = GenerateSettlementDb(dic);

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(dishesDb);
                ds.Tables.Add(payDetailDb);
                ds.Tables.Add(settlementDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\结账单.frx");
                if (!File.Exists(file))
                {
                    var msg = "结账单报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备结账单报表数据。");
                InfoLog.Instance.I("开始打印结账单报表...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印结账单报表。");

                return true;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("打印结账单时异常。", exp);
                MessageDialog.Warning(string.Format("打印结账单时错误：{0}", exp.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印客用单。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printUser">当前用户。</param>
        /// <returns></returns>
        public static bool PrintCustomUseBillReport(string orderId, string printUser)
        {
            ShowReportPrintingWindow();
            InfoLog.Instance.I("开始获取客用单报表数据...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            var result = service.GetPrintOrderInfo(orderId, printUser, EnumPrintOrderType.CustomUse);
            InfoLog.Instance.I("结束获取客用单报表数据。");
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("获取客用单报表数据错误：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }

            try
            {
                InfoLog.Instance.I("开始准备客用单报表数据...");
                var mainDb = CreateOrderMainDb();
                AddObject2DataTable(mainDb, result.Item2.OrderInfo);

                var dishesDb = CreateOrderDishDb();
                if (result.Item2.OrderDishInfos != null)
                    result.Item2.OrderDishInfos.ForEach(t => AddObject2DataTable(dishesDb, t));

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(dishesDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\客用单.frx");
                if (!File.Exists(file))
                {
                    var msg = "客用单报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备客用单报表数据。");
                InfoLog.Instance.I("开始打印客用单报表...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印客用单报表。");

                return true;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("打印客用单时异常。", exp);
                MessageDialog.Warning(string.Format("打印客用单时错误：{0}", exp.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印清机报表。
        /// </summary>
        /// <param name="userId">清机人员id。</param>
        /// <returns></returns>
        public static bool PrintClearPosReport(string userId)
        {
            ShowReportPrintingWindow();
            InfoLog.Instance.I("开始获取清机报表数据...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            var result = service.GetClearPosInfo(userId);
            InfoLog.Instance.I("结束获取清机报表数据。");
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("获取清机报表数据错误：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }

            try
            {
                InfoLog.Instance.I("开始准备清机报表数据...");
                var mainDb = CreateClearPosMainDb();
                AddObject2DataTable(mainDb, result.Item2.OrderJson.Data.First());

                var settDb = CreateClearPosSettlementDb();
                if (result.Item2.JSJson != null && result.Item2.JSJson.Data != null)
                    result.Item2.JSJson.Data.ForEach(t => AddObject2DataTable(settDb, t));

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(settDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\清机单.frx");
                if (!File.Exists(file))
                {
                    var msg = "清机单报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备清机报表数据。");
                InfoLog.Instance.I("开始打印清机单...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印清机单。");

                return true;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("打印清机单异常。", exp);
                MessageDialog.Warning(string.Format("打印清机单时错误：{0}", exp.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印发票单。
        /// </summary>
        /// <param name="invoiceInfo">发票单信息。</param>
        /// <returns></returns>
        public static bool PrintInvoiceReport(PrintInvoiceInfo invoiceInfo)
        {
            ShowReportPrintingWindow();
            try
            {
                InfoLog.Instance.I("开始准备打印发票单数据...");
                var mainDb = CreateInvoiceMainDb();
                AddObject2DataTable(mainDb, invoiceInfo);

                var ds = new DataSet();
                ds.Tables.Add(mainDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\发票单.frx");
                if (!File.Exists(file))
                {
                    var msg = "发票单报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备打印发票单数据。");
                InfoLog.Instance.I("开始打印发票单...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印发票单...");

                return true;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("打印开发票单异常。", ex);
                MessageDialog.Warning(string.Format("打印开发票单异常：{0}", ex.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印品项销售统计报表。
        /// </summary>
        /// <param name="statisticInfo">品项销售统计信息。</param>
        /// <returns></returns>
        public static bool PrintDishInfoStatisticReport(ReportStatisticInfo statisticInfo)
        {
            ShowReportPrintingWindow();
            try
            {
                InfoLog.Instance.I("开始准备打印品项销售统计数据...");
                var mainDb = CreateStatisticReportMainDb();
                AddObject2DataTable(mainDb, statisticInfo);

                var dataDb = CreateStatisticReportDataDb();
                if (statisticInfo.DataSource != null)
                    statisticInfo.DataSource.ForEach(t => AddObject2DataTable(dataDb, t));

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(dataDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\品项统计.frx");
                if (!File.Exists(file))
                {
                    var msg = "品项统计报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备打印品项销售统计数据。");
                InfoLog.Instance.I("开始打印品项销售统计单...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印品项销售统计单。");

                return true;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("打印品项销售明细统计报表异常。", ex);
                MessageDialog.Warning(string.Format("打印品项销售明细统计报表异常：{0}", ex.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印小费统计报表。
        /// </summary>
        /// <param name="statisticInfo">小费统计信息。</param>
        /// <returns></returns>
        public static bool PrintTipInfoStatisticReport(ReportStatisticInfo statisticInfo)
        {
            ShowReportPrintingWindow();
            try
            {
                InfoLog.Instance.I("开始准备打印小费统计数据...");
                var mainDb = CreateStatisticReportMainDb();
                AddObject2DataTable(mainDb, statisticInfo);

                var dataDb = CreateStatisticReportDataDb();
                if (statisticInfo.DataSource != null)
                    statisticInfo.DataSource.ForEach(t => AddObject2DataTable(dataDb, t));

                var ds = new DataSet();
                ds.Tables.Add(mainDb);
                ds.Tables.Add(dataDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\小费统计.frx");
                if (!File.Exists(file))
                {
                    var msg = "小费统计报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备打印小费统计数据。");
                InfoLog.Instance.I("开始打印小费统计单...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印小费统计单。");

                return true;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("打印小费统计报表异常。", ex);
                MessageDialog.Warning(string.Format("打印小费统计报表异常：{0}", ex.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印会员交易凭条。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="printerUserId">打印用户id。</param>
        /// <returns></returns>
        public static bool PrintMemberPayBillReport(string orderId, string printerUserId)
        {
            ShowReportPrintingWindow();
            try
            {
                InfoLog.Instance.I("开始获取打印会员交易凭条数据...");
                var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
                var result = service.GetMemberPrintPayInfo(orderId, printerUserId);
                InfoLog.Instance.I("结束获取打印会员交易凭条数据。");
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    var msg = string.Format("获取打印会员交易凭条数据错误：{0}", result.Item1);
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                result.Item2.ReportTitle = "商户联";
                var mainDb = CreateMemberPayBillMainDb();
                AddObject2DataTable(mainDb, result.Item2);

                var ds = new DataSet();
                ds.Tables.Add(mainDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\交易凭条_商.frx");
                if (!File.Exists(file))
                {
                    var msg = "交易凭条商户联报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("开始打印交易凭条商户联...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印交易凭条商户联。");

                result.Item2.ReportTitle = "客户联";
                var mainDbCus = CreateMemberPayBillMainDb();
                AddObject2DataTable(mainDbCus, result.Item2);

                var dsCus = new DataSet();
                dsCus.Tables.Add(mainDbCus);

                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\交易凭条_客.frx");
                if (!File.Exists(file))
                {
                    var msg = "交易凭条客户联报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("开始打印交易凭条客户联...");
                FastReportHelper.Print(file, dsCus);
                InfoLog.Instance.I("结束打印交易凭条客户联。");

                return true;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("打印交易凭条异常。", ex);
                MessageDialog.Warning(string.Format("打印交易凭条异常：{0}", ex.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        /// <summary>
        /// 打印会员储值凭条。
        /// </summary>
        /// <param name="info">会员储值信息。</param>
        /// <returns></returns>
        public static bool PrintMemberStoredReport(PrintMemberStoredInfo info)
        {
            ShowReportPrintingWindow();
            try
            {
                InfoLog.Instance.I("开始准备储值凭条商户联打印数据...");
                info.ReportTitle = "商户联";
                var mainDb = CreateMemberStoredMainDb();
                AddObject2DataTable(mainDb, info);

                var ds = new DataSet();
                ds.Tables.Add(mainDb);

                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\储值凭条_商.frx");
                if (!File.Exists(file))
                {
                    var msg = "储值凭条商户联报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                InfoLog.Instance.I("结束准备储值凭条商户联打印数据。");
                InfoLog.Instance.I("开始打印储值凭条商户联...");
                FastReportHelper.Print(file, ds);
                InfoLog.Instance.I("结束打印储值凭条商户联。");

                InfoLog.Instance.I("开始准备储值凭条客户联打印数据...");
                info.ReportTitle = "客户联";
                var mainDbCus = CreateMemberStoredMainDb();
                AddObject2DataTable(mainDbCus, info);

                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\储值凭条_客.frx");
                if (!File.Exists(file))
                {
                    var msg = "储值凭条商户联报表模板文件缺失。";
                    ErrLog.Instance.E(msg);
                    MessageDialog.Warning(msg);
                    return false;
                }

                var dsCus = new DataSet();
                dsCus.Tables.Add(mainDbCus);

                InfoLog.Instance.I("结束准备储值凭条客户联打印数据。");
                InfoLog.Instance.I("开始打印储值凭条客户联...");
                FastReportHelper.Print(file, dsCus);
                InfoLog.Instance.I("结束打印储值凭条客户联。");
                return true;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("打印储值凭条异常。", ex);
                MessageDialog.Warning(string.Format("打印储值凭条异常：{0}", ex.Message));
                return false;
            }
            finally
            {
                ReportPrintingWindow.Instance.Hide();
            }
        }

        #region 营业明细表打印

        /// <summary>
        /// 打印营业报表明细。
        /// </summary>
        /// <param name="fullInfo"></param>
        public static bool PrintBusinessDataDetail(MBusinessDataDetail businessDataDetail)
        {
            string msg = "打印营业报表明细";
            DataTable mainDb;
            DataTable detailDb;
            DataTable jsDb;
            DataTable yhDb;
            ToBusinessDataDetailTabel(businessDataDetail, out mainDb, out detailDb, out jsDb, out yhDb);

            DataSet ds = new DataSet();
            ds.Tables.Add(mainDb);
            ds.Tables.Add(detailDb);
            ds.Tables.Add(jsDb);
            ds.Tables.Add(yhDb);

            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reports\BusinessDataDetail.frx");
            if (!File.Exists(file))
            {
                ErrLog.Instance.E(msg + "文件缺失。");
                MessageDialog.Warning(msg + "文件缺失。");
                return false;
            }

            InfoLog.Instance.I("开始"+msg);
            FastReportHelper.Print(file, ds);
            InfoLog.Instance.I("结束"+msg);
            return true;

       
        }

        /// <summary>
        /// 创建营业明细Table
        /// </summary>
        /// <param name="businessDataDetail"></param>
        /// <param name="oMainTable"></param>
        /// <param name="oCategoryTable"></param>
        private static void ToBusinessDataDetailTabel(MBusinessDataDetail businessDataDetail, out DataTable oMainTable,
            out DataTable oCategoryTable, out DataTable oJsTable, out DataTable oYhTable)
        {
            oMainTable = new DataTable();
            oCategoryTable = new DataTable();

            var mainTable = new DataTable("tb_main");
            DataColumn dc = CreateDataColumn(typeof(DateTime), "起始时间", "StartTime", DateTime.MinValue);
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "结束时间", "EndTime", DateTime.MinValue);
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "当前时间", "CurrentTime", DateTime.MinValue);
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "操作员", "UserName", "");
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "开台数", "kaitaishu", "");
            mainTable.Columns.Add(dc);

            dc = CreateDataColumn(typeof(string), "应收合计", "shouldamount", "");
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "优惠合计", "discountamount", "");
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "实收合计", "paidinamount", "");
            mainTable.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "小费", "xiaofei", "");
            mainTable.Columns.Add(dc);

            AddObject2DataTable(mainTable, businessDataDetail);
            oMainTable = mainTable;

            var categoryTable = new DataTable("tb_data");

            DataColumn tempColumn = CreateDataColumn(typeof(string), "品项", "DishName", "");
            categoryTable.Columns.Add(tempColumn);
            tempColumn = CreateDataColumn(typeof(decimal), "金额", "money", 0m);
            categoryTable.Columns.Add(tempColumn);

            if (businessDataDetail.Categories != null)
            {
                businessDataDetail.Categories.ForEach(t => AddObject2DataTable(categoryTable, t));
                oCategoryTable = categoryTable;
            }

            var jsTable = new DataTable("tb_js");
            DataColumn jsColumn = CreateDataColumn(typeof(string), "结算方式", "jsName", "");
            jsTable.Columns.Add(jsColumn);
            jsColumn = CreateDataColumn(typeof(decimal), "结算金额", "money", 0m);
            jsTable.Columns.Add(jsColumn);

            jsTable.Rows.Add("现金", businessDataDetail.money);
            foreach (var hang in businessDataDetail.HangingMonies)
            {
                jsTable.Rows.Add(hang.HangingName, hang.HangingMoney);
            }
            jsTable.Rows.Add("微信", businessDataDetail.weixin);
            jsTable.Rows.Add("支付宝", businessDataDetail.zhifubao);
            jsTable.Rows.Add("刷工行卡", businessDataDetail.icbc);
            jsTable.Rows.Add("刷他行卡", businessDataDetail.otherbank);
            jsTable.Rows.Add("会员储值消费净值", businessDataDetail.merbervaluenet);



            oJsTable = jsTable;

            var yhTable = new DataTable("tb_yh");
            DataColumn yhColumn = CreateDataColumn(typeof(string), "优惠方式", "yhName", "");
            yhTable.Columns.Add(yhColumn);
            yhColumn = CreateDataColumn(typeof(decimal), "优惠金额", "money", 0m);
            yhTable.Columns.Add(yhColumn);

            yhTable.Rows.Add("优免", businessDataDetail.bastfree);
            yhTable.Rows.Add("会员积分消费", businessDataDetail.integralconsum);
            yhTable.Rows.Add("会员券消费", businessDataDetail.meberTicket);
            yhTable.Rows.Add("折扣优惠", businessDataDetail.discountmoney);
            yhTable.Rows.Add("抹零", businessDataDetail.malingincom);
            yhTable.Rows.Add("赠送金额", businessDataDetail.give);
            yhTable.Rows.Add("四舍五入", businessDataDetail.handervalue);
            yhTable.Rows.Add("会员储值消费虚增", businessDataDetail.mebervalueadd);

            oYhTable = yhTable;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 显示报表正在打印窗口。
        /// </summary>
        private static void ShowReportPrintingWindow()
        {
            ReportPrintingWindow.Instance.Show();
            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// 创建订单主要信息DataTable。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateOrderMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(string), "门店名称", "BranchName", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "订单号", "OrderId", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "区域", "AreaName", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "餐桌", "TableName", ""));
            db.Columns.Add(CreateDataColumn(typeof(int), "人数", "CustomerNumber", 0));
            db.Columns.Add(CreateDataColumn(typeof(string), "服务员", "WaiterName", ""));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "开始时间", "BeginTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "结束时间", "EndTime", DateTime.Now));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "优惠", "DiscountPrice", 0m));
            db.Columns.Add(CreateDataColumn(typeof(string), "打印人", "Printer", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "品项费", "TotalDishAmount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "赠送金额", "FreeAmount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "应收金额", "TotalAmount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "预结打次", "PrintPresettTimes", 0));
            db.Columns.Add(CreateDataColumn(typeof(int), "结账打次", "PrintSettlementTimes", 0));
            db.Columns.Add(CreateDataColumn(typeof(string), "电话", "BranchTelephone", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "地址", "BranchAddress", ""));
            return db;
        }

        /// <summary>
        /// 创建订单菜品信息DataTable。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateOrderDishDb()
        {
            var dt = new DataTable("tb_dishes");
            dt.Columns.Add(CreateDataColumn(typeof(decimal), "金额", "DishAmount", 0));
            dt.Columns.Add(CreateDataColumn(typeof(string), "数量", "DishNumUnit", ""));
            dt.Columns.Add(CreateDataColumn(typeof(decimal), "单价", "DishPrice", 0));
            dt.Columns.Add(CreateDataColumn(typeof(string), "品项", "DishName", ""));
            return dt;
        }

        /// <summary>
        /// 创建结算方式明细DataTable。
        /// </summary>
        /// <returns></returns>
        private static DataTable CeratePayDetailDb()
        {
            var db = new DataTable("tb_payDetail");
            db.Columns.Add(CreateDataColumn(typeof(string), "结算方式", "PayWay", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "金额", "Payamount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(string), "备注", "Remark", ""));
            return db;
        }

        /// <summary>
        /// 创建优惠券明细DataTable。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateCouponsDb()
        {
            var db = new DataTable("tb_coupon");
            db.Columns.Add(CreateDataColumn(typeof(string), "优惠名称", "Name", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "数量", "Count", 0m));
            db.Columns.Add(CreateDataColumn(typeof(string), "金额", "BillAmount", ""));
            return db;
        }

        /// <summary>
        /// 创建结算备注Db。
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static DataTable GenerateSettlementDb(Dictionary<string, decimal> dic)
        {
            var db = new DataTable("tb_pay");
            db.Columns.Add(CreateDataColumn(typeof(string), "结算名称", "name", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "金额", "value", 0m));

            if (dic == null)
                return db;

            foreach (var item in dic)
            {
                var dr = db.NewRow();
                dr["name"] = item.Key;
                dr["value"] = item.Value;
                db.Rows.Add(dr);
            }

            return db;
        }

        /// <summary>
        /// 创建清机单主表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateClearPosMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(string), "清机单号", "classNo", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "POS编号", "posID", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "操作员编号", "operatorID", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "操作员姓名", "operatorName", ""));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "签到时间", "SignInTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "签退时间", "SignOutTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "备用金", "prettyCash", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "前班未结台数", "lastNonTable", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "前班未结押金", "lastNonDeposit", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "本班开单人数", "tBeginPeople", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "本班开台总数", "tBeginTableTotal", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "本班未结台数", "tNonClosingTable", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "本班未结金额", "tNonClosingMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "本班未退押金", "tNonClosingDeposit", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "本班已结台数", "tClosingTable", 0m));
            db.Columns.Add(CreateDataColumn(typeof(int), "本班已结人数", "tClosingPeople", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "本班赠单金额", "tPresentedMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "本班退菜金额", "tRFoodMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "品项消费", "itemMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "服务费", "serviceMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "包房费", "roomMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "最低消费补齐", "lowConsComp", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "优惠金额", "preferenceMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "应收小计", "accountsReceivableSubtotal", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "抹零金额", "removeMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "定额优惠金额", "ratedPreferenceMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "应收合计", "accountsReceivableTotal", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "计入收入合计", "includedMoneyTotal", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "不计入收入合计", "noIncludedMoneyTotal", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "合计", "TotalMoney", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "餐具", "tableware", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "酒水", "drinks", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "酒水烟汤面", "drinksSmokeNoodle", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "本日营业总额", "todayTurnover", 0m));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "打印时间", "PrintTime", DateTime.Now));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "营业时间", "WorkDateTime", DateTime.MinValue));
            return db;
        }

        /// <summary>
        /// 创建清机单结算备注DataTable。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateClearPosSettlementDb()
        {
            var db = new DataTable("tb_sett");
            db.Columns.Add(CreateDataColumn(typeof(string), "结算类别描述", "itemDesc", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "金额", "payamount", 0m));
            return db;
        }

        /// <summary>
        /// 创建发票单主表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateInvoiceMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(string), "桌号", "TableName", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "订单号", "OrderId", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "发票金额", "InvoiceAmount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(string), "发票抬头", "InvoiceTitle", ""));
            return db;
        }

        /// <summary>
        /// 创建统计报表打印主表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateStatisticReportMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "起始时间", "StartTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "结束时间", "EndTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "当前时间", "CurrentTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(string), "门店编号", "BranchId", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "合计金额", "TotalAmount", 0m));
            return db;
        }

        /// <summary>
        /// 创建统计报表打印数据表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateStatisticReportDataDb()
        {
            var db = new DataTable("tb_data");
            db.Columns.Add(CreateDataColumn(typeof(int), "序号", "Index", 0));
            db.Columns.Add(CreateDataColumn(typeof(string), "名称", "Name", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "数量", "Count", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "金额", "Amount", 0m));
            return db;
        }

        /// <summary>
        /// 创建会员消费凭条主表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateMemberPayBillMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(string), "交易标题", "ReportTitle", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "卡号", "CardNo", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "商户号", "BranchId", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "终端号", "Terminal", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "收银员", "UserId", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "主机流水", "BatchNo", ""));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "交易时间", "TradeTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(string), "商户名称", "BranchName", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "流水号", "OrderId", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "积分增减", "Score", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "储值消费", "StoredPay", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "券消费", "Coupons", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "积分余额", "ScoreBalance", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "储值余额", "StoredBalance", 0m));
            return db;
        }

        /// <summary>
        /// 创建会员储值凭条主表。
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateMemberStoredMainDb()
        {
            var db = new DataTable("tb_main");
            db.Columns.Add(CreateDataColumn(typeof(string), "交易标题", "ReportTitle", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "卡号", "CardNo", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "商户号", "BranchId", ""));
            db.Columns.Add(CreateDataColumn(typeof(DateTime), "交易时间", "TradeTime", DateTime.MinValue));
            db.Columns.Add(CreateDataColumn(typeof(string), "商户名称", "BranchName", ""));
            db.Columns.Add(CreateDataColumn(typeof(string), "交易凭证号", "TraceCode", ""));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "储值金额", "StoredAmount", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "积分余额", "ScoreBalance", 0m));
            db.Columns.Add(CreateDataColumn(typeof(decimal), "储值余额", "StoredBalance", 0m));
            return db;
        }

        /// <summary>
        /// 生成预结单结算备注。
        /// </summary>
        /// <param name="tableFullInfo"></param>
        /// <param name="freeAmount"></param>
        /// <returns></returns>
        private static Dictionary<string, decimal> GetPresettlementDic(TableFullInfo tableFullInfo, decimal freeAmount = 0)
        {
            var dic = new Dictionary<string, decimal> { { "合计：", tableFullInfo.TotalAmount } };
            if (Globals.OddModel == EnumOddModel.Rounding)
            {
                if (tableFullInfo.AdjustmentAmount > 0)
                    dic.Add("四舍五入：", tableFullInfo.AdjustmentAmount);
            }
            else if (Globals.OddModel == EnumOddModel.Wipe)
            {
                if (tableFullInfo.AdjustmentAmount > 0)
                    dic.Add("抹零：", tableFullInfo.AdjustmentAmount);
            }

            if (freeAmount > 0)
                dic.Add("赠送金额：", freeAmount);

            if (tableFullInfo.TotalFreeAmount > 0)
                dic.Add("总优惠：", tableFullInfo.TotalFreeAmount);

            dic.Add("应收：", tableFullInfo.PaymentAmount);
            return dic;
        }

        /// <summary>
        /// 生成结账单结算备注。
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private static Dictionary<string, decimal> GetSetttlementDic(PrintOrderInfo orderInfo)
        {
            var dic = new Dictionary<string, decimal> { { "合计：", orderInfo.TotalAmount } };
            if (Globals.OddModel == EnumOddModel.Rounding)
            {
                if (orderInfo.RoundingAmount > 0)
                    dic.Add("四舍五入：", orderInfo.RoundingAmount);
            }
            else if (Globals.OddModel == EnumOddModel.Wipe)
            {
                if (orderInfo.RemoveOddAmount > 0)
                    dic.Add("抹零：", orderInfo.RemoveOddAmount);
            }

            if (orderInfo.FreeAmount > 0)
                dic.Add("赠送金额：", orderInfo.FreeAmount);

            var favorableAmount = orderInfo.TotalAmount - orderInfo.PaidAmount;//总优惠是合计-实收。
            if (favorableAmount > 0)
                dic.Add("总优惠：", favorableAmount);

            dic.Add("实收：", orderInfo.PaidAmount);
            return dic;
        }

        /// <summary>
        /// 创建DataColumn。
        /// </summary>
        /// <param name="type">列类型。</param>
        /// <param name="caption">列标题。</param>
        /// <param name="columnName">列名。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        private static DataColumn CreateDataColumn(Type type, string caption, string columnName, object defaultValue)
        {
            return new DataColumn(columnName, type)
            {
                Caption = caption,
                DefaultValue = defaultValue
            };
        }

        /// <summary>
        /// 添加对象到DataTable。（反射对象的所有属性到对应的DataTable列中）
        /// </summary>
        /// <param name="db"></param>
        /// <param name="data"></param>
        private static void AddObject2DataTable(DataTable db, object data)
        {
            if (db == null || data == null)
                return;

            try
            {
                DataRow dr = db.NewRow();
                var ppies = data.GetType().GetProperties();
                foreach (var ppy in ppies)
                {
                    try
                    {
                        var obj = dr[ppy.Name];
                    }
                    catch
                    {
                        continue;
                    }

                    if (!IsValueType(ppy))
                        continue;

                    dr[ppy.Name] = ppy.GetValue(data, null);
                }

                db.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("添加对象到DataTable时异常。", ex);
            }
        }

        /// <summary>
        /// 是否是值类型。
        /// </summary>
        /// <param name="ppy"></param>
        /// <returns></returns>
        private static bool IsValueType(PropertyInfo ppy)
        {
            return (ppy.PropertyType == typeof(int) || ppy.PropertyType == typeof(decimal) ||
                    ppy.PropertyType == typeof(string) || ppy.PropertyType == typeof(double) || ppy.PropertyType == typeof(DateTime));
        }

        #endregion
    }
}