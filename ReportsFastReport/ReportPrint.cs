using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using FastReport;
using FastReport.Data;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceReference;
using Library;
using Models;
using System.Threading;

namespace ReportsFastReport
{
    /// <summary>
    /// 不需要显示界面的打印报表都放在这里
    /// </summary>
    public struct ReportAmount
    {
        public string orderid;//帐单号
        public decimal amount;//合计金额
        public decimal ysAmount;//应收金额
        public decimal mlAmount;//抹零金额
        public decimal amountgz;//挂帐金额
        public decimal amountym;//优免金额
        public decimal amountround;//
    }
    public class ReportPrint
    {
        private static ReportAmount ramount;
        private static Report rptReport = new Report();
        private static frmPrintProgress frmProgress;// = new frmPrintProgress();
        private static decimal tipAmount;
        private static decimal tipPaid;

        public static void Init()
        {
            frmProgress = new frmPrintProgress();
        }

        public static void PrintDishSaleDetail(DishSaleFullInfo fullInfo)
        {
            DataTable mainDb = ToMainDb(fullInfo);
            DataTable detailDb = ToDetailDb(fullInfo.DishSaleInfos);
            DataSet ds = new DataSet();
            ds.Tables.Add(mainDb);
            ds.Tables.Add(detailDb);

            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\DishSaleDetail.frx";
            rptReport.Load(file);//加载报表模板文件
            InitializeReport(ds, ref rptReport, detailDb.TableName);
            PrintRpt(rptReport, 1);
        }

        private static DataTable ToMainDb(DishSaleFullInfo fullInfo)
        {
            var tb = new DataTable("tb_main");
            DataColumn dc = CreateDataColumn(typeof(DateTime), "起始时间", "StartTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "结束时间", "EndTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "当前时间", "CurrentTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "门店编号", "BranchId", "");
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "合计金额", "TotalAmount", 0m);
            tb.Columns.Add(dc);

            AddObject2DataTable(tb, fullInfo);
            return tb;
        }

        private static DataTable ToDetailDb(List<DishSaleInfo> list)
        {
            var tb = new DataTable("tb_data");
            DataColumn dc = CreateDataColumn(typeof(int), "序号", "Index", 0);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "品项", "Name", "");
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "数量", "SalesCount", 0m);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "金额", "SalesAmount", 0m);
            tb.Columns.Add(dc);

            if (list != null)
                list.ForEach(t => AddObject2DataTable(tb, t));

            return tb;
        }

        private static DataColumn CreateDataColumn(Type type, string caption, string columnName, object defaultValue)
        {
            return new DataColumn(columnName, type)
            {
                Caption = caption,
                DefaultValue = defaultValue
            };
        }

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
                    object obj = null;
                    try
                    {
                        obj = dr[ppy.Name];
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
                AllLog.Instance.E("添加对象到DataTable时异常。", ex);
            }
        }

        private static bool IsValueType(PropertyInfo ppy)
        {
            return (ppy.PropertyType == typeof (int) || ppy.PropertyType == typeof (decimal) ||
                    ppy.PropertyType == typeof (string) || ppy.PropertyType == typeof (double) || ppy.PropertyType == typeof(DateTime));
        }

        /// <summary>
        /// 转成主表。
        /// </summary>
        /// <param name="fullInfo"></param>
        /// <returns></returns>
        private static DataTable ToMainDb(StatisticInfoBase fullInfo)
        {
            var tb = new DataTable("tb_main");
            DataColumn dc = CreateDataColumn(typeof(DateTime), "起始时间", "StartTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "结束时间", "EndTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(DateTime), "当前时间", "CurrentTime", DateTime.MinValue);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "门店编号", "BranchId", "");
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "合计金额", "TotalAmount", 0m);
            tb.Columns.Add(dc);

            AddObject2DataTable(tb, fullInfo);
            return tb;
        }

        /// <summary>
        /// 打印小费明细。
        /// </summary>
        /// <param name="fullInfo"></param>
        public static void PrintTipDetail(TipFullInfo fullInfo)
        {
            DataTable mainDb = ToMainDb(fullInfo);
            DataTable detailDb = ToDetailDb(fullInfo.TipInfos);
            DataSet ds = new DataSet();
            ds.Tables.Add(mainDb);
            ds.Tables.Add(detailDb);

            rptReport.Clear();
            rptReport.Load(Application.StartupPath + @"\Reports\TipDetail.frx");//加载报表模板文件
            InitializeReport(ds, ref rptReport, detailDb.TableName);
            PrintRpt(rptReport, 1);
        }

        /// <summary>
        /// 转成明细表。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static DataTable ToDetailDb(List<TipInfo> list)
        {
            var tb = new DataTable("tb_data");
            DataColumn dc = CreateDataColumn(typeof(int), "序号", "Index", 0);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(string), "服务员", "WaiterName", "");
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "小费次数", "TipCount", 0m);
            tb.Columns.Add(dc);
            dc = CreateDataColumn(typeof(decimal), "小费金额", "TipAmount", 0m);
            tb.Columns.Add(dc);

            if (list != null)
                list.ForEach(t => AddObject2DataTable(tb, t));

            return tb;
        }

        /// <summary>
        /// 打印预结单2
        /// </summary>
        public static void PrintPayBill(String billno, String printuser, DataTable yhList, ReportAmount ra)
        {
            //
            ramount = ra;
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrJS = null;
            try
            {
                if (!RestClient.getOrderInfo(Globals.UserInfo.UserName, Globals.CurrOrderInfo.orderid, 1, out jrOrder, out jrList, out jrJS))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder = null;
            DataTable dtList = null;
            DataTable dtJs = null;
            DataTable yh = new DataTable();
            yh = yhList.Copy();
            dtOrder = Bill_Order.getOrder(jrOrder);
            tipAmount = Convert.ToDecimal(((JObject)jrOrder[0])["tipAmount"].ToString());
            tipPaid = Convert.ToDecimal(((JObject)jrOrder[0])["tipPaid"].ToString());
            //dtList = Bill_Order.getOrder_List(jrList);
            dtList = PrintDataHelper.GetOrderListDb(jrList);
            dtJs = Bill_Order.getOrder_Js(jrJS);
            DataTable dtSettlementDetail = Bill_Order.GetSettlementDetailTable(GetPresettlementDetailList((JObject)jrOrder[0], ra));
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptBill.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtList);
            ds.Tables.Add(dtJs);
            ds.Tables.Add(yh);
            ds.Tables.Add(dtSettlementDetail);
            InitializeReport(ds, ref rptReport, dtList.TableName);
            PrintRpt(rptReport, 1);
        }

        private static Dictionary<string, string> GetPresettlementDetailList(JObject jObj, ReportAmount ra)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("合计：", ra.amount.ToString("f2"));

            if (Globals.roundinfo.Itemid.Equals("1")) //四舍五入
            {
                if (ra.amountround > 0)
                    dic.Add("四舍五入：", ra.amountround.ToString("f2"));
            }
            else if (Globals.roundinfo.Itemid.Equals("2")) // 抹零
            {
                if (ra.mlAmount > 0)
                    dic.Add("抹零：", ra.mlAmount.ToString("f2"));
            }

            var valueStr = jObj["zdAmount"].ToString();
            if (!string.IsNullOrEmpty(valueStr))
            {
                var amount = Convert.ToDecimal(valueStr);
                if (amount > 0)
                {
                    dic.Add("赠送金额：", amount.ToString("f2"));
                }
            }

            if (ra.amount - ra.ysAmount > 0)
                dic.Add("总优惠：", Math.Round(ra.amount - ra.ysAmount, 2).ToString("f2"));

            dic.Add("应收：", ra.ysAmount.ToString("f2"));

            return dic;
        }

        /// <summary>
        /// 打印结帐单
        /// </summary>
        public static void PrintPayBill2(String billno, String printuser)
        {
            //
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrJS = null;
            try
            {
                if (!RestClient.getOrderInfo(printuser, billno, 2, out jrOrder, out jrList, out jrJS))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder = null;
            DataTable dtList = null;
            DataTable dtJs = null;

            dtOrder = Bill_Order.getOrder(jrOrder);
            //dtList = Bill_Order.getOrder_List(jrList);
            dtList = PrintDataHelper.GetOrderListDb(jrList);
            dtJs = Bill_Order.getOrder_Js(jrJS);
            var tipAmountJK = ((JObject)jrOrder[0])["tipAmount"];
            tipAmount = tipAmountJK != null ? Convert.ToDecimal(tipAmountJK) : 0m;
            var tipPaidJK = ((JObject)jrOrder[0])["tipPaid"];
            tipPaid = tipPaidJK != null ? Convert.ToDecimal(tipPaidJK) : 0m;
            DataTable dtSettlementDetail = Bill_Order.GetSettlementDetailTable(GetSettlementDetailList((JObject)jrOrder[0]));
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptBill2.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtList);
            ds.Tables.Add(dtJs);
            ds.Tables.Add(dtSettlementDetail);
            InitializeReport(ds, ref rptReport, dtList.TableName);
            PrintRpt(rptReport, 1);
        }

        /// <summary>
        /// 获取结算明细的键值对集合。
        /// </summary>
        /// <param name="jObj"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetSettlementDetailList(JObject jObj)
        {
            var dic = new Dictionary<string, string>();
            var valueStr = jObj["dueamount"].ToString();
            var sumAmount = Convert.ToDecimal(valueStr);//合计金额。
            dic.Add("合计：", sumAmount.ToString("f2"));

            if (Globals.roundinfo.Itemid.Equals("1")) //四舍五入
            {
                valueStr = jObj["payamount2"].ToString();
                if (!string.IsNullOrEmpty(valueStr))
                {
                    var value = Convert.ToDecimal(valueStr);
                    if (value > 0)
                        dic.Add("四舍五入：", value.ToString("f2"));
                }
            }
            else if (Globals.roundinfo.Itemid.Equals("2"))// 抹零
            {
                valueStr = jObj["payamount"].ToString();
                if (!string.IsNullOrEmpty(valueStr))
                {
                    var value = Convert.ToDecimal(valueStr);
                    if (value > 0)
                        dic.Add("抹零：", value.ToString("f2"));
                }
            }

            valueStr = jObj["zdAmount"].ToString();
            if (!string.IsNullOrEmpty(valueStr))
            {
                var amount = Convert.ToDecimal(valueStr);
                if (amount > 0)
                {
                    dic.Add("赠送金额：", amount.ToString("f2"));
                }
            }

            valueStr = jObj["ssamount"].ToString();
            var actualAmount = Convert.ToDecimal(valueStr);//实收金额。
            var favorableAmount = sumAmount - actualAmount;
            if (favorableAmount > 0)
                dic.Add("总优惠：", Math.Round(favorableAmount, 2).ToString("f2"));
            dic.Add("实收：", actualAmount.ToString("f2"));

            return dic;
        }

        /// <summary>
        /// 按是四舍五入还是抹零还决定报表上显示的字段内容
        /// </summary>
        private static void setRtpISRound(ref Report rtp, ref DataTable dt)
        {
            try
            {
                DataRow dr;
                dr = dt.Rows[0];
                float mlAmount = 0;
                float roundAmount = 0;
                try
                {
                    mlAmount = float.Parse(dr["payamount"].ToString());
                }
                catch { }
                try
                {
                    roundAmount = float.Parse(dr["payamount2"].ToString());
                }
                catch { }

                if (Globals.roundinfo.Itemid.Equals("1"))//四舍五入
                {
                    AddedtValue(ref rtp, "edtRound", "四舍五入：");
                    AddedtValue(ref rtp, "edtRoundValue", "[tb_Order.四舍五入]");
                }
                else
                    if (Globals.roundinfo.Itemid.Equals("2"))
                    {
                        AddedtValue(ref rtp, "edtRound", "抹零：");
                        AddedtValue(ref rtp, "edtRoundValue", "[tb_Order.结帐时抹零金额]");
                    }

            }
            catch { }
        }
        /// <summary>
        /// 打印客用单
        /// </summary>
        public static void PrintPayBill3(String billno, String printuser)
        {
            //
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrJS = null;
            try
            {
                if (!RestClient.getOrderInfo(printuser, billno, 3, out jrOrder, out jrList, out jrJS))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder = null;
            DataTable dtList = null;
            DataTable dtJs = null;

            dtOrder = Models.Bill_Order.getOrder(jrOrder);
            //dtList = Models.Bill_Order.getOrder_List(jrList);
            dtList = PrintDataHelper.GetOrderListDb(jrList);
            dtJs = Models.Bill_Order.getOrder_Js(jrJS);
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptBill3.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtList);
            ds.Tables.Add(dtJs);
            InitializeReport(ds, ref rptReport, dtList.TableName);
            PrintRpt(rptReport, 1);
        }
        /// <summary>
        /// 会员交易凭条
        /// </summary>
        /// <param name="billno"></param>
        /// <param name="printuser"></param>
        public static void PrintMemberPay1(String billno, String printuser)
        {
            //
            JArray jrOrder = null;
            DataTable dtOrder = null;
            try
            {
                if (!RestClient.getMemberSaleInfo(printuser, billno, out jrOrder))
                {
                    return;
                }
                dtOrder = Models.Bill_Order.getMemberSaleInfo_List(jrOrder);

                rptReport.Clear();
                if (dtOrder.Rows.Count <= 0)
                {
                    Library.frmWarning.ShowWarning("该单没有会员消费记录!");
                    return;
                }
                string file = Application.StartupPath + @"\Reports\rptyz1.frx";
                rptReport.Load(file);//加载报表模板文件
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOrder);
                InitializeReport(ds, ref rptReport, dtOrder.TableName);
                AddedtValue(ref rptReport, "edtreport_membertitle", WebServiceReference.WebServiceReference.Report_membertitle);
                AddedtValue(ref rptReport, "Text2", "------商户联------");
                visiableObj(ref rptReport, "Rich18", true);
                visiableObj(ref rptReport, "Line1", true);
                PrintRpt(rptReport, 1);
                Application.DoEvents();
                AddedtValue(ref rptReport, "Text2", "------客户联------");
                visiableObj(ref rptReport, "Rich18", false);
                visiableObj(ref rptReport, "Line1", false);
                PrintRpt(rptReport, 1);
            }
            catch { }
        }
        /// <summary>
        /// 打印开发票小票
        /// </summary>
        /// <param name="billno"></param>
        /// <param name="invoice_title"></param>
        /// <param name="tableno"></param>
        public static void PrintInvoice(String billno, String invoice_title, String tableno, decimal amount)
        {
            try
            {

                rptReport.Clear();

                string file = Application.StartupPath + @"\Reports\rptInvoice.frx";
                rptReport.Load(file);//加载报表模板文件
                AddedtValue(ref rptReport, "lbltableno", String.Format("桌  号：{0}", tableno));
                AddedtValue(ref rptReport, "lblorderno", String.Format("帐单号：{0}", billno));
                AddedtValue(ref rptReport, "lblamount", String.Format("发票金额：{0}", amount.ToString()));
                AddedtValue(ref rptReport, "lblinvoicetitle", String.Format("发票抬头：{0}", invoice_title));
                PrintRpt(rptReport, 1);
            }
            catch { }
        }

        /// <summary>
        /// //打印清机报表
        /// </summary>
        /// <param name="userid"></param>
        public static void PrintClearMachine()
        {
            JArray jrOrder = null;
            JArray jrJS = null;
            string printuser = Globals.UserInfo.UserID;
            string jsorder = Globals.jsOrder;
            if (jsorder.Trim().ToString().Length <= 0)
            {
                jsorder = " ";
            }
            try
            {
                if (!RestClient.getClearMachineData(printuser, jsorder, out jrOrder, out jrJS))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder = null;
            DataTable dtJs = null;
            dtOrder = Models.Bill_Order.getClearMachineData(jrOrder);
            dtJs = Models.Bill_Order.getClearMachine_Js(jrJS);
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptClea.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtJs);
            InitializeReport(ds, ref rptReport, dtJs.TableName);
            PrintRpt(rptReport, 1);
        }
        public static void printProgress(object sender, ProgressEventArgs e)
        {
            try
            {
                frmProgress.Progress(e.Message);
            }
            catch { }
        }
        public static void StartProgress(object sender, EventArgs e)
        {

        }
        public static void AddedtValue(ref Report rtp, String edtName, String value)
        {
            try
            {
                if (rtp.FindObject(edtName) != null)
                    (rtp.FindObject(edtName) as TextObject).Text = value;
            }
            catch { }
        }
        public static void visiableObj(ref Report rtp, String edtName, Boolean value)
        {
            try
            {
                if (rtp.FindObject(edtName) != null)
                    (rtp.FindObject(edtName) as ReportComponentBase).Visible = value;
            }
            catch { }
        }
        public static void AddedtValue2(ref Report rtp, String edtName, String value)
        {
            try
            {
                if (rtp.FindObject(edtName) != null)
                    (rtp.FindObject(edtName) as RichObject).Text = value;
            }
            catch { }
        }
        private static void setReportSetText(ref Report rtp)
        {
            AddedtValue(ref rtp, "edtreport_title", Globals.BranchInfo.BranchName);
            AddedtValue(ref rtp, "edtreport_tele", WebServiceReference.WebServiceReference.Report_tele);
            AddedtValue(ref rtp, "edtreport_address", Globals.BranchInfo.BranchAddress);
            AddedtValue(ref rtp, "edtreport_web", WebServiceReference.WebServiceReference.Report_web);
        }
        public static void PrintRpt(Report rtp, int printcount)
        {
            try
            {
                Thread.CurrentThread.ApartmentState = ApartmentState.STA;

                //显示打印对话框
                ShowReportFrm();
                //Application.DoEvents();
                //Globals.Delay(10000);
                rtp.PrintSettings.ShowDialog = false;
                FastReport.EnvironmentSettings es = new EnvironmentSettings();
                es.ReportSettings.ShowProgress = false;
                //es.StartProgress += new EventHandler(StartProgress);
                //.Progress += new FastReport.ProgressEventHandler(printProgress);
                try
                {
                    (rtp.FindObject("edtPrint") as RichObject).Text = "打印人：" + Globals.UserInfo.UserName;
                }
                catch
                {
                }
                AddedtValue(ref rtp, "edtorderid", ramount.orderid);
                AddedtValue(ref rtp, "edtAmountv", ramount.amount.ToString("f2"));
                AddedtValue(ref rtp, "edtysAmount", ramount.ysAmount.ToString("f2"));
                AddedtValue(ref rtp, "edtmlAmount", ramount.mlAmount.ToString("f2"));
                AddedtValue(ref rtp, "edtamountgz", ramount.amountgz.ToString("f2"));
                AddedtValue(ref rtp, "edtamountym", ramount.amountym.ToString("f2"));
                AddedtValue(ref rtp, "edtamountf", (0 - (ramount.amountym + ramount.amountgz)).ToString("f2"));
                setReportSetText(ref rtp);
                if (Globals.roundinfo.Itemid.Equals("1")) //四舍五入
                {
                    AddedtValue(ref rtp, "edtRound2", "四舍五入：");
                    AddedtValue(ref rtp, "edtmlAmount", ramount.amountround.ToString("f2"));
                }
                else if (Globals.roundinfo.Itemid.Equals("2"))
                {
                    AddedtValue(ref rtp, "edtRound2", "抹零：");
                    AddedtValue(ref rtp, "edtmlAmount", ramount.mlAmount.ToString("f2"));
                }

                if (tipAmount == 0)
                    visiableObj(ref rtp, "dfTipAmount", false);

                if (tipPaid == 0)
                    visiableObj(ref rtp, "dfTipPaid", false);

                if (RestClient.ShowReport)
                {
                    HideReportFrm();
                    try
                    {
                        rtp.Show();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                else if (RestClient.ShowAndDesign)
                {
                    HideReportFrm();
                    try
                    {
                        rtp.Prepare(); //准备工作
                        rtp.Design();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
                else
                    try
                    {
                        rtp.PrintSettings.Copies = printcount;
                        rtp.Prepare(); //准备工作

                        for (int i = 0; i <= printcount - 1; i++)
                        {
                            rtp.PrintPrepared();
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(ex);
            }
            finally
            {
                HideReportFrm();
            }
        }

        private static void ShowReportFrm()
        {
            if (frmProgress == null || frmProgress.IsDisposed)
                return;

            if (frmProgress.IsHandleCreated && frmProgress.InvokeRequired)
            {
                frmProgress.BeginInvoke((Action)delegate
                {
                    frmProgress.showFrm();
                    frmProgress.Update();
                });
                return;
            }

            frmProgress.showFrm();
        }

        private static void HideReportFrm()
        {
            if (frmProgress == null || frmProgress.IsDisposed)
                return;

            if (frmProgress.IsHandleCreated)
            {
                if (frmProgress.InvokeRequired)
                {
                    frmProgress.BeginInvoke((Action)delegate { frmProgress.hideFrm(); });
                    return;
                }
            }

            frmProgress.hideFrm();
        }

        private static void InitializeReport(DataSet ds, ref Report rpt, String tableName)
        {
            rpt.PrintSettings.ShowDialog = false;
            //打印单表数据
            //给DataBand(明细数据)绑定数据源
            foreach (DataTable dt in ds.Tables)
            {
                rpt.RegisterData(dt, dt.TableName); //注册数据源,单表
                rpt.GetDataSource(dt.TableName).Enabled = true;
                rpt.GetDataSource(dt.TableName).Init();
            }
            try
            {
                DataBand band = rpt.FindObject("Data1") as DataBand;
                DataSourceBase dataSource = rpt.GetDataSource(tableName);
                dataSource.Init();
                band.DataSource = dataSource;
                //自定义处理
                band.BeforePrint += new EventHandler(band_BeforePrint);
            }
            catch { }
        }
        private static void band_BeforePrint(object sender, EventArgs e)
        {
            //取出当前打印的记录。
            DataRow row = (sender as DataBand).DataSource.CurrentRow as DataRow;
            //做其它特殊处理：
            int i = (sender as DataBand).DataSource.CurrentRowNo;
            //(rptReport.FindObject("Text8") as TextObject).Text = "DataRow:" + i.ToString();
        }

        /// <summary>
        /// 打印会员储值交易凭条
        /// </summary>
        /// <param name="storeinfo"></param>
        public static void PrintMemberStore(TMemberStoredInfo storeinfo)
        {
            try
            {
                string file = Application.StartupPath + @"\Reports\rptMember_CZ.frx";
                rptReport.Load(file);//加载报表模板文件
                AddedtValue(ref rptReport, "edtbilltype", "------商户联------");
                AddedtValue(ref rptReport, "edtcardno", storeinfo.Cardno);
                AddedtValue(ref rptReport, "edtreport_membertitle", storeinfo.Treport_membertitle);
                AddedtValue(ref rptReport, "edtpzh", storeinfo.Pzh);
                AddedtValue(ref rptReport, "edtdate", storeinfo.Date);
                AddedtValue(ref rptReport, "edttime", storeinfo.Time);
                AddedtValue(ref rptReport, "edtstore", storeinfo.Store);
                AddedtValue(ref rptReport, "edtpoint", storeinfo.Point);
                AddedtValue(ref rptReport, "edtamount", storeinfo.Amount);

                PrintRpt(rptReport, 1);
                Application.DoEvents();
                AddedtValue(ref rptReport, "edtbilltype", "------客户联------");
                PrintRpt(rptReport, 1);
            }
            catch { }
        }
        /// <summary>
        /// 开激活凭条
        /// </summary>
        /// <param name="storeinfo"></param>
        public static void PrintMemberNewCard(TMemberStoredInfo storeinfo)
        {
            try
            {
                string file = Application.StartupPath + @"\Reports\rptMember_NewCard.frx";
                rptReport.Load(file);//加载报表模板文件
                AddedtValue(ref rptReport, "edtbilltype", "------商户联------");
                AddedtValue(ref rptReport, "edtcardno", storeinfo.Cardno);
                AddedtValue(ref rptReport, "edtreport_membertitle", storeinfo.Treport_membertitle);
                AddedtValue(ref rptReport, "edtpzh", storeinfo.Pzh);
                AddedtValue(ref rptReport, "edtdate", storeinfo.Date);
                AddedtValue(ref rptReport, "edttime", storeinfo.Time);
                AddedtValue(ref rptReport, "edtstore", "0.00");
                AddedtValue(ref rptReport, "edtpoint", "0.00");

                PrintRpt(rptReport, 1);
                Application.DoEvents();
                AddedtValue(ref rptReport, "edtbilltype", "------客户联------");
                PrintRpt(rptReport, 1);
            }
            catch { }
        }

        public static void PrintPayAmount(decimal amount)
        {
            try
            {
                string file = Application.StartupPath + @"\Reports\rptPayAmount.frx";
                rptReport.Load(file);//加载报表模板文件
                AddedtValue(ref rptReport, "lblamount", amount.ToString());
                PrintRpt(rptReport, 1);
            }
            catch { }
        }
    }
}
