using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using FastReport;
using FastReport.Data;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceReference;
using Library;
using Models;

namespace ReportsFastReport
{
    /// <summary>
    /// 不需要显示界面的打印报表都放在这里
    /// </summary>
    public struct ReportAmount
    {
        public string orderid;//帐单号
        public double amount;//合计金额
        public double ysAmount;//应收金额
        public double mlAmount;//抹零金额
        public double amountgz;//挂帐金额
        public double amountym;//优免金额
        public double amountround;//
    }
    public class ReportPrint
    {
        private static ReportAmount ramount;
        private static Report rptReport=new Report();
        private static frmPrintProgress frmProgress =new frmPrintProgress();
        /// <summary>
        /// 打印预结单2
        /// </summary>
        public static void PrintPayBill(String billno, String printuser, DataTable yhList,ReportAmount ra)
        {
            //
            ramount =ra;
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrJS = null;
            try
            {
                if(!RestClient.getOrderInfo(Globals.UserInfo.UserName, Globals.CurrOrderInfo.orderid,1, out jrOrder, out jrList, out jrJS))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder=null;
            DataTable dtList = null;
            DataTable dtJs = null;
            DataTable yh = new DataTable();
            yh = yhList.Copy();
            dtOrder = Models.Bill_Order.getOrder(jrOrder);
            dtList = Models.Bill_Order.getOrder_List(jrList);
            dtJs = Models.Bill_Order.getOrder_Js(jrJS); 
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptBill.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtList);
            ds.Tables.Add(dtJs);
            ds.Tables.Add(yh);
            InitializeReport(ds, ref rptReport, dtList.TableName);
            PrintRpt(rptReport,1);
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

            dtOrder = Models.Bill_Order.getOrder(jrOrder);
            dtList = Models.Bill_Order.getOrder_List(jrList);
            dtJs = Models.Bill_Order.getOrder_Js(jrJS);
            rptReport.Clear();
            string file = Application.StartupPath + @"\Reports\rptBill2.frx";
            rptReport.Load(file);//加载报表模板文件
            DataSet ds = new DataSet();
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtList);
            ds.Tables.Add(dtJs);
            //如果是四舍五入，如果是抹零
            setRtpISRound(ref rptReport, ref dtOrder);
            InitializeReport(ds, ref rptReport, dtList.TableName);
            PrintRpt(rptReport, 1);
        }
        /// <summary>
        /// 按是四舍五入还是抹零还决定报表上显示的字段内容
        /// </summary>
        private static void setRtpISRound(ref Report rtp,ref DataTable dt)
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
            dtList = Models.Bill_Order.getOrder_List(jrList);
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
            if(dtOrder.Rows.Count<=0)
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
            PrintRpt(rptReport,1);
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
        public static void PrintInvoice(String billno, String invoice_title,String tableno,decimal amount)
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
            PrintRpt(rptReport,1);
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
            if (jsorder.Trim().ToString().Length<=0)
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
        public static void AddedtValue(ref Report rtp,String edtName,String value)
        {
                 try
                {
                    if (rtp.FindObject(edtName)!=null)
                    (rtp.FindObject(edtName) as TextObject).Text =value;
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
            AddedtValue(ref rtp, "edtreport_title", WebServiceReference.WebServiceReference.Report_title);
            AddedtValue(ref rtp, "edtreport_tele", WebServiceReference.WebServiceReference.Report_tele);
            AddedtValue(ref rtp, "edtreport_address", WebServiceReference.WebServiceReference.Report_address);
            AddedtValue(ref rtp, "edtreport_web", WebServiceReference.WebServiceReference.Report_web);
        }
        public static void PrintRpt(Report rtp,int printcount)
        {
            try
            {
                //显示打印对话框
                frmProgress.showFrm();
                Application.DoEvents();
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
                catch { }
                AddedtValue(ref rtp,"edtorderid",ramount.orderid);
                AddedtValue(ref rtp, "edtAmountv", ramount.amount.ToString("f2"));
                AddedtValue(ref rtp, "edtysAmount", ramount.ysAmount.ToString("f2"));
                AddedtValue(ref rtp, "edtmlAmount", ramount.mlAmount.ToString("f2"));
                AddedtValue(ref rtp, "edtamountgz", ramount.amountgz.ToString("f2"));
                AddedtValue(ref rtp, "edtamountym", ramount.amountym.ToString("f2"));
                AddedtValue(ref rtp, "edtamountf", (0 - (ramount.amountym + ramount.amountgz)).ToString("f2"));
                setReportSetText(ref rtp);
                if (Globals.roundinfo.Itemid.Equals("1"))//四舍五入
                {
                    AddedtValue(ref rtp, "edtRound2", "四舍五入：");
                    AddedtValue(ref rtp, "edtmlAmount", ramount.amountround.ToString("f2"));
                }
                else
                    if (Globals.roundinfo.Itemid.Equals("2"))
                    {
                        AddedtValue(ref rtp, "edtRound2", "抹零：");
                        AddedtValue(ref rtp, "edtmlAmount", ramount.mlAmount.ToString("f2"));
                    }

                if (RestClient.getShowReport())
                {
                    frmProgress.hideFrm();
                    try
                    {
                        rtp.Show();
                    }
                    catch (Exception e) { MessageBox.Show(e.Message); }
                }
                else
                    if (RestClient.getPrintDesign())
                    {
                        frmProgress.hideFrm();
                        try
                        {
                            rtp.Prepare();//准备工作
                            rtp.Design();
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }

                    }
                    else
                    {
                        try
                        {
                            rtp.PrintSettings.Copies = printcount;
                            rtp.Prepare();//准备工作

                            for (int i = 0; i <= printcount - 1; i++)
                            {
                                rtp.PrintPrepared();
                            }

                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }
            }finally
            {
                //关闭打印对话框
                frmProgress.hideFrm();
            }
        }
        private static void InitializeReport(DataSet ds,ref Report  rpt,String tableName)
        {
            rpt.PrintSettings.ShowDialog = false;
            //打印单表数据
            //给DataBand(明细数据)绑定数据源
            foreach(DataTable dt in ds.Tables)
            {
                rpt.RegisterData(dt,dt.TableName); //注册数据源,单表
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
