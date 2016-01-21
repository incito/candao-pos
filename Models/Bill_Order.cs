using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models
{
    /// <summary>
    /// 把jsonarray转换成为数据表 
    /// 
    /// </summary>
    public class Bill_Order
    {
        public Bill_Order()
        { }
        public static int strtoint(string str)
        {
            int re = 0;
            try
            {
                re = int.Parse(str);
            }
            catch { re = 0; }
            return re;
        }
        public static float strtofloat(string str)
        {
            float re = 0;
            try
            {
                re = (float)Math.Round(float.Parse(str), 2);
            }
            catch { re = 0; }
            return re;
        }
        public static decimal strtodecimal(string str)
        {
            decimal re = 0;
            try
            {
                re = decimal.Parse(str);
            }
            catch { re = 0; }
            return re;
        }

        public static DateTime strtodatetime(string str)
        {
            DateTime retValue = DateTime.Now;
            if (string.IsNullOrEmpty(str))
                return retValue;

            try
            {
                retValue = DateTime.ParseExact(str, "yyyyMMdd HH:mm:ss", null);
                if (retValue < DateTime.Parse("1980-1-1"))
                    retValue = DateTime.Now;
            }
            catch
            {
                // ignored
            }
            return retValue;
        }
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
        public static DateTime ConvertIntDatetime(double utc)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            startTime = startTime.AddSeconds(utc);
            startTime = startTime.AddHours(8);//转化为北京时间(北京时间=UTC时间+8小时 )
            return startTime;
        }
        public static DataColumn newDataColumn(string DataType, String Caption, String ColumnName, String DefaultValue)
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(DataType);//"System.String"
            column.AllowDBNull = false;
            column.Caption = Caption;//"优惠"
            column.ColumnName = ColumnName;//"yhname"
            column.DefaultValue = DefaultValue;
            return column;
        }
        public static DataColumn newDataColumn(string DataType, String Caption, String ColumnName, int DefaultValue)
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(DataType);//"System.String"
            column.AllowDBNull = false;
            column.Caption = Caption;//"优惠"
            column.ColumnName = ColumnName;//"yhname"
            column.DefaultValue = DefaultValue;
            return column;
        }
        public static DataColumn newDataColumn(string DataType, String Caption, String ColumnName, Double DefaultValue)
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(DataType);//"System.String"
            column.AllowDBNull = false;
            column.Caption = Caption;//"优惠"
            column.ColumnName = ColumnName;//"yhname"
            column.DefaultValue = DefaultValue;
            return column;
        }
        public static DataColumn newDataColumn(string DataType, String Caption, String ColumnName, DateTime DefaultValue)
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType(DataType);//"System.String"
            column.AllowDBNull = false;
            column.Caption = Caption;//"优惠"
            column.ColumnName = ColumnName;//"yhname"
            column.DefaultValue = DefaultValue;
            return column;
        }
        public static void jarray2DataTable(JArray jr, ref DataTable dt)
        {
            string fieldname = "";
            DataRow dr = null;
            foreach (JObject ja in jr)
            {
                dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    try
                    {
                        fieldname = dc.ColumnName;
                        var type = dr[fieldname].GetType();
                        var valueStr = ja[fieldname] != null ? ja[fieldname].ToString() : "";
                        if (type == typeof(double) || type == typeof(decimal))
                        {
                            if (!string.IsNullOrEmpty(valueStr))
                                dr[fieldname] = Math.Round(Convert.ToDecimal(valueStr), 2);
                        }
                        else if (type == typeof(DateTime))
                            dr[fieldname] = strtodatetime(valueStr);
                        else if (type == typeof(int))
                            dr[fieldname] = strtoint(valueStr);
                        else
                            dr[fieldname] = valueStr;
                    }
                    catch
                    {
                        // ignored
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        /// <summary>
        /// JArray转为单头表
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getOrder(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_Order";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "单号", "orderid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "服务员", "userid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "开台时间", "begintime", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "结帐时间", "endtime", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "状态", "orderstatus", "0");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "人数", "custnum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "女士", "womanNum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "男士", "mannum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "桌号", "currenttableid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "全单折扣", "fulldiscountrate", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "应收金额", "dueamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "折扣金额", "discountamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "免单金额", "freeamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "抹零金额", "wipeamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "结算", "payway", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "合作单位", "partnername", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "优惠名称", "couponname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "折扣人", "disuserid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "打印", "printcount", 1);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "打印次数", "befprintcount", 1);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "桌名称", "tableName", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "区域NO", "areaNo", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "区域", "areaname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "结帐时抹零金额", "payamount", 0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "优惠信息", "couponname3", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "服务员姓名", "fullname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "PAD会员登录", "memberno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "结帐实收", "ssamount", 0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "结帐挂帐", "gzamount", 0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "结帐优免", "ymamount", 0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "ordertype", "ordertype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "gzcode", "gzcode", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "gzname", "gzname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "gztele", "gztele", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "gzuser", "gzuser", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "四舍五入", "payamount2", 0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "赠送金额", "zdAmount", (Double)0.00);
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        public static DataTable gett_order_rule(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "t_order_rule";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "orderid", "orderid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "yhname", "yhname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "partnername", "partnername", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "couponrate", "couponrate", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "freeamount", "freeamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "debitamount", "debitamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "memo", "memo", "0");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "couponsno", "couponsno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "num", "num", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "ftype", "ftype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "banktype", "banktype", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "ruleid", "ruleid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "couponid", "couponid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "amount", "amount", (Double)0.00);
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        ///  JArray转为单体表
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getOrder_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            //增加单头表字段
            DataColumn column = newDataColumn("System.Decimal", "金额", "amount", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Decimal", "数量", "dishnum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "菜品ID", "dishid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "下单时间", "begintime", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "下单员工", "userName", "0");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "单价", "orderprice", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "折扣率", "discountrate", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "折扣金额", "discountamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "下单类型", "dishtype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "单位", "dishunit", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "折前金额", "predisamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "应付金额", "payamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "菜品名称", "title", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "图片", "image", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "类别编号", "itemid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "类别名称", "itemDesc", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishstatus", "dishstatus", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "pricetype", "pricetype", "");
            dt.Columns.Add(column);

            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        ///  JArray转为结算表
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getOrder_Js(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_js";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "单号", "orderid", "");
            dt.Columns.Add(column);
            //column = newDataColumn("System.Int32", "id", "sdetailid", 0);
            //dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "数量", "couponNum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "类型", "incometype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "会员号", "membercardno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "银行卡号/优惠名称", "bankcardno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "结算类别", "payway", 0);
            dt.Columns.Add(column);
            //column = newDataColumn("System.Int32", "isclear", "isclear", 0);
            //dt.Columns.Add(column);            
            column = newDataColumn("System.Double", "金额", "payamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "结算类别", "itemDesc", "");
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }

        /// <summary>
        /// 获取结算各项明细的Table表。
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable GetSettlementDetailTable(Dictionary<string, string> dic)
        {
            DataTable dt = new DataTable { TableName = "tb_JSMX" };
            dt.Columns.Add(newDataColumn("System.String", "明细名称", "name", ""));
            dt.Columns.Add(newDataColumn("System.Double", "明细金额", "value", 0.00d));

            foreach (var item in dic)
            {
                var dr = dt.NewRow();
                dr["name"] = item.Key;
                dr["value"] = item.Value;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 获取挂帐单位列表
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getGz_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "code", "code", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "type", "type", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "挂帐单位", "name", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "parternerid", "parternerid", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "telephone", "telephone", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "relaperson", "relaperson", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "address", "address", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "拼音", "py", "");
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        /// 所有外卖FOOD接口
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getFood_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            //增加单头表字段
            //select dishid,dishno,columnid,userid,title,introduction,source,image,imagetitle,content,vipprice,price,unit,dishtype,ordernum,py from t_dish
            DataColumn column = newDataColumn("System.String", "dishid", "dishid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishno", "dishno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "columnid", "columnid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "userid", "userid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "title", "title", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "introduction", "introduction", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "source", "source", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "image", "image", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "imagetitle", "imagetitle", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "content", "content", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "vipprice", "vipprice", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "price", "price", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "unit", "unit", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishtype", "dishtype", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "py", "py", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "ordernum", "ordernum", 0);
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        /// 用户凭条表转换
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getMemberSaleInfo_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            DataColumn column = newDataColumn("System.String", "帐单号", "orderid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "收银员", "userid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "交易时间", "ordertime", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "商户号", "business", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "终端号", "terminal", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "serial", "serial", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "batchno", "batchno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "businessname", "businessname", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "交易类型", "operatetype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "积分增减", "score", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "券增减", "coupons", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "储值增减", "stored", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "积分余额", "scorebalance", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "券余额", "couponsbalance", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "储值余额", "storedbalance", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "卡号", "cardno", "");
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }

        /// <summary>
        /// 获取清机报表数据
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getClearMachineData(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            DataColumn column = newDataColumn("System.String", "清机单号", "classNo", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "POS机ID", "posID", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "操作员ID", "operatorID", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "操作员", "operatorName", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "签到时间", "vIn", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "签退时间", "vOut", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "备用金", "prettyCash", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "前班未结台数", "lastNonTable", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "前班未结押金", "lastNonDeposit", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "本班开单人数", "tBeginPeople", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "本班开台总数", "tBeginTableTotal", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "本班未结台数", "tNonClosingTable", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "本班未结金额", "tNonClosingMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "本班未退押金", "tNonClosingDeposit", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "本班已结台数", "tClosingTable", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "本班已结人数", "tClosingPeople", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "本班赠单金额", "tPresentedMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "本班退菜金额", "tRFoodMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "品项消费", "itemMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "服务费", "serviceMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "包房费", "roomMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "最低消费补齐", "lowConsComp", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "优惠金额", "preferenceMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "应收小计", "accountsReceivableSubtotal", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "抹零金额", "removeMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "定额优惠金额", "ratedPreferenceMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "应收合计", "accountsReceivableTotal", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "计入收入合计", "includedMoneyTotal", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "不计入收入合计", "noIncludedMoneyTotal", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "合计", "TotalMoney", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "餐具", "tableware", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "酒水", "drinks", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "酒水烟汤面", "drinksSmokeNoodle", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "本日营业总额", "todayTurnover", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "打印时间", "priterTime", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "IP", "ipaddress", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.DateTime", "营业时间", "workdate", DateTime.Now);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "班别", "shiftid", 0);
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        /// 清单结算方式明细
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getClearMachine_Js(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_js";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "单号", "orderid", "");
            dt.Columns.Add(column);
            //column = newDataColumn("System.Int32", "id", "sdetailid", 0);
            //dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "数量", "couponNum", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "类型", "incometype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "会员号", "membercardno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "银行卡号/优惠名称", "bankcardno", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "结算类别", "payway", 0);
            dt.Columns.Add(column);
            //column = newDataColumn("System.Int32", "isclear", "isclear", 0);
            //dt.Columns.Add(column);            
            column = newDataColumn("System.Double", "金额", "payamount", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "结算类别", "itemDesc", "");
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }
        /// <summary>
        /// 前台员工权限表
        /// </summary>
        /// <param name="jr"></param>
        /// <returns></returns>
        public static DataTable getRight_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "resourcesPath", "resourcesPath", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "resourcesDesc", "resourcesDesc", "");
            dt.Columns.Add(column);
            jarray2DataTable(jr, ref dt);
            return dt;
        }

        public static DataTable getBackDish_List(JArray jr)
        {
            DataTable dt = new DataTable();
            dt.TableName = "tb_data";
            //增加单头表字段
            DataColumn column = newDataColumn("System.String", "orderid", "orderid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishid", "dishid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishstatus", "dishstatus", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "dishnum", "dishnum", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "userName", "userName", "0");
            dt.Columns.Add(column);
            column = newDataColumn("System.Double", "orderprice", "orderprice", (Double)0.00);
            dt.Columns.Add(column);
            column = newDataColumn("System.Int32", "dishtype", "dishtype", 0);
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "dishunit", "dishunit", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "orderdetailid", "orderdetailid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "relatedishid", "relatedishid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "ordertype", "ordertype", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "parentkey", "parentkey", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "superkey", "superkey", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "primarykey", "primarykey", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "isadddish", "isadddish", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "childdishtype", "childdishtype", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "ispot", "ispot", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "relateorderid", "relateorderid", "");
            dt.Columns.Add(column);
            column = newDataColumn("System.String", "ismaster", "ismaster", "");
            dt.Columns.Add(column);

            jarray2DataTable(jr, ref dt);
            return dt;
        }
    }

}
