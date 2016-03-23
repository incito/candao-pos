using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceReference;

namespace Business
{
    /// <summary>
    /// 退菜json数据生成类
    /// </summary>
    public class BackDish
    {
        public static DataTable backdt = null;

        public static bool BackAllDish(string orderNo, string tableNo, string userId, string reason = "")
        {
            
        }

        public static bool backDish(string orderNo, string tableno, string discardUserId, string userid, DataTable dt, double backnum, string discardReason)
        {
            double tmpbacknum = backnum;
            double dishnum = 0;
            double num = 0;
            bool backret = false;
            bool backret2 = false;
            foreach (DataRow dr in dt.Rows)
            {
                dishnum = double.Parse(dr["dishnum"].ToString());
                if(dishnum>=tmpbacknum)
                {
                    num = tmpbacknum;
                    tmpbacknum = 0;
                }else
                {
                    num = dishnum;
                    tmpbacknum = tmpbacknum - dishnum;
                }
                string dishtype = dr["dishtype"].ToString();
                StringWriter sw=null;
                string ispot = dr["ispot"].ToString();
                string dishstatus = dr["dishstatus"].ToString();
                string dishtype2 = dr["dishtype"].ToString();
                string ismaster = dr["ismaster"].ToString();
                string childdishtype = dr["childdishtype"].ToString();
                int backtype = 0;
                if (ispot.Equals("1"))
                    backtype = 1; //是鱼锅，退整个锅
                if ((ismaster.Equals("1") && (dishtype.Equals("1"))))
                {
                    backtype = 1; //是鱼锅套餐dish,退整个锅
                }
                if ((ispot.Equals("0")) && (ismaster.Equals("0") && (dishtype.Equals("1"))))
                {
                    backtype = 4; //是鱼锅中的鱼
                }
                //如果是套餐
                if ((childdishtype.Equals("2") && (dishtype.Equals("2"))))
                {
                    backtype = 5; 
                }
                switch (backtype)
                {
                    case 0: //退普通菜
                        sw=getBackDish_1(orderNo,tableno,discardUserId,userid,dr,num,"");
                        backret=RestClient.discarddish(sw);
                        if (backret)
                            backret2 = backret;
                        break;
                    case 1: //退整个鱼锅
                        sw = getBackDish_allyg(orderNo, tableno, discardUserId, userid, dr, num, discardReason);
                        backret=RestClient.discarddish(sw);
                        if (backret)
                            backret2 = backret;
                        break;
                    case 4: //退整个鱼锅
                        sw = getBackDish_y(orderNo, tableno, discardUserId, userid, dr, num, discardReason);
                        backret = RestClient.discarddish(sw);
                        if (backret)
                            backret2 = backret;
                        break;
                    case 5: //套餐 
                        sw = getBackDish_dc(orderNo, tableno, discardUserId, userid, dr, num, discardReason);
                        backret=RestClient.discarddish(sw);
                        if (backret)
                            backret2 = backret;
                        break;
                }
                if(tmpbacknum<=0)
                { break; }
            }
            return backret2;
        }
        public static double getBackNum(DataTable dt)
        {
            double backnum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                backnum = backnum + double.Parse(dr["dishnum"].ToString());
            }
            return backnum;
        }
        /// <summary>
        /// 获取指定的菜所有的退菜列表
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="tableno"></param>
        /// <param name="dishunit"></param>
        /// <returns></returns>
        public static bool getbackdish(string orderNo, string dishid, string dishunit,string tableno, out string msg, out JArray ja)
        {
            DataRow orderdishidinfo=null;
            double dishnum = 0;
            JArray jrorder = null;
            if (!RestClient.getBackDishInfo(orderNo, dishid, dishunit, tableno, out jrorder))
            {
                msg = "获取退菜菜品数量失败!";
                ja = null;
                return false;
            }
            //鱼锅和套餐只能退 1，鱼和普通菜能退查询的数量
            ja = jrorder;
            msg = "";
            return true;
        }
        public static void writeObject(ref JsonWriter writer,string PropertyName,string value)
        {
            if (value == null)
                value = "";
            writer.WritePropertyName(PropertyName);
            writer.WriteValue(value);
        }
        public static void writeObject(ref JsonWriter writer, string PropertyName, double value)
        {
            if (value == null)
                value = 0;
            writer.WritePropertyName(PropertyName);
            writer.WriteValue(value);
        }
        public static void writeObject(ref JsonWriter writer, string PropertyName, int value)
        {
            if (value == null)
                value = 0;
            writer.WritePropertyName(PropertyName);
            writer.WriteValue(value);
        }
        /// <summary>
        /// 退单个菜 根据dishid从服务器中获取orderdishidinfo  torderdishidinfo
        /// </summary>
        /// <param name="dishid"></param>
        /// <returns></returns>
        public static StringWriter getBackDish_1(string orderNo, string tableno, string discardUserId, string userid, DataRow orderdishidinfo, double dishnum, string discardReason)
        {
            string primarykey = orderdishidinfo["primarykey"].ToString();
            string dishNo = orderdishidinfo["dishid"].ToString();
            double dishNum = dishnum;
            string dishunit = orderdishidinfo["dishunit"].ToString();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writeObject(ref writer, "currenttableid", tableno);
            writeObject(ref writer, "orderNo", orderNo);
            writeObject(ref writer, "discardReason", discardReason);
            writeObject(ref writer, "operationType", 2);
            writeObject(ref writer, "primarykey", primarykey);
            writeObject(ref writer, "sequence", 999999);
            writeObject(ref writer, "discardUserId", discardUserId);
            writeObject(ref writer, "userName", userid);
            writeObject(ref writer, "dishunit", dishunit);
            writeObject(ref writer, "dishNum", getdishNum(dishNum));// 
            writeObject(ref writer,"actionType","0");//0退单个菜，1：整单。
            writeObject(ref writer, "dishtype", "0");
            writeObject(ref writer, "dishNo", dishNo);
            writer.WriteEndObject();
            writer.Flush();
            return sw;
        }
        public static string getdishNum(double dishnum)
        {
            return dishnum.ToString();
            //return String.Format("{0:F}", dishNum);
        }
        /// <summary>
        /// 退整个鱼锅
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="tableno"></param>
        /// <param name="discardUserId"></param>
        /// <param name="userid"></param>
        /// <param name="orderdishidinfo"></param>
        /// <param name="dishnum"></param>
        /// <returns></returns>
        public static StringWriter getBackDish_allyg(string orderNo, string tableno, string discardUserId, string userid, DataRow orderdishidinfo, double dishnum, string discardReason)
        {
            string primarykey = orderdishidinfo["primarykey"].ToString();
            string dishNo = orderdishidinfo["dishid"].ToString();
            double dishNum = dishnum;
            string dishunit = orderdishidinfo["dishunit"].ToString();
            string potdishid = orderdishidinfo["parentkey"].ToString();
            string relatedishid = orderdishidinfo["relatedishid"].ToString();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writeObject(ref writer, "actionType", "0");
            writeObject(ref writer, "userName", userid);
            writeObject(ref writer, "orderNo", orderNo);
            writeObject(ref writer, "dishNo", dishNo);
            writeObject(ref writer, "primarykey", primarykey);
            writeObject(ref writer, "dishNum", getdishNum(dishNum));
            writeObject(ref writer, "dishunit", dishunit);
            writeObject(ref writer, "currenttableid", tableno);
            writeObject(ref writer, "dishtype", "1");
            writeObject(ref writer, "potdishid", relatedishid);
            writeObject(ref writer, "hotflag", "1");// 锅底
            writeObject(ref writer, "operationType", 2);
            writeObject(ref writer, "sequence", 999999);
            writeObject(ref writer, "discardUserId", discardUserId);
            writeObject(ref writer, "discardReason", discardReason);
            writer.WriteEndObject();
            writer.Flush();
            return sw;
        }
        /// <summary>
        /// 退锅底
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="tableno"></param>
        /// <param name="discardUserId"></param>
        /// <param name="userid"></param>
        /// <param name="orderdishidinfo"></param>
        /// <param name="dishnum"></param>
        /// <returns></returns>
        public static StringWriter getBackDish_gd(string orderNo, string tableno, string discardUserId, string userid, DataRow orderdishidinfo, double dishnum, string discardReason)
        {
            //+"-0" 如果 -0就是锅底
            string primarykey = orderdishidinfo["primarykey"].ToString();
            string dishNo = orderdishidinfo["dishid"].ToString();
            double dishNum = dishnum;
            string dishunit = orderdishidinfo["dishunit"].ToString();
            string potdishid = orderdishidinfo["parentkey"].ToString();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writeObject(ref writer, "actionType", "0");
            writeObject(ref writer, "userName", userid);
            writeObject(ref writer, "orderNo", orderNo);
            writeObject(ref writer, "dishNo", dishNo);
            writeObject(ref writer, "primarykey", primarykey);
            writeObject(ref writer, "dishNum", getdishNum(dishNum));
            writeObject(ref writer, "dishunit", dishunit);
            writeObject(ref writer, "currenttableid", tableno);
            writeObject(ref writer, "dishtype", "1");
            writeObject(ref writer, "potdishid", potdishid);
            writeObject(ref writer, "hotflag", "1");// 锅底
            writeObject(ref writer, "operationType", 2);
            writeObject(ref writer, "sequence", 999999);
            writeObject(ref writer, "discardUserId", discardUserId);
            writeObject(ref writer, "discardReason", discardReason);
            writer.WriteEndObject();
            writer.Flush();
            return sw;
        }
        /// <summary>
        /// 退鱼锅中的鱼
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="tableno"></param>
        /// <param name="discardUserId"></param>
        /// <param name="userid"></param>
        /// <param name="orderdishidinfo"></param>
        /// <param name="dishnum"></param>
        /// <returns></returns>
        public static StringWriter getBackDish_y(string orderNo, string tableno, string discardUserId, string userid, DataRow orderdishidinfo, double dishnum, string discardReason)
        {
            //+ "-" + fishPosition
            string primarykey = orderdishidinfo["primarykey"].ToString();
            string dishNo = orderdishidinfo["dishid"].ToString();
            double dishNum = dishnum;
            string dishunit = orderdishidinfo["dishunit"].ToString();
            string potdishid = orderdishidinfo["parentkey"].ToString();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writeObject(ref writer, "actionType", "0");
            writeObject(ref writer, "userName", userid);
            writeObject(ref writer, "orderNo", orderNo);
            writeObject(ref writer, "dishNo", dishNo);
            writeObject(ref writer, "primarykey", primarykey);
            writeObject(ref writer, "dishNum", getdishNum(dishNum));
            writeObject(ref writer, "dishunit", dishunit);
            writeObject(ref writer, "currenttableid", tableno);
            writeObject(ref writer, "dishtype", "1");
            writeObject(ref writer, "potdishid", potdishid);
            writeObject(ref writer, "hotflag", "0");// 退鱼
            writeObject(ref writer, "operationType", 2);
            writeObject(ref writer, "sequence", 999999);
            writeObject(ref writer, "discardUserId", discardUserId);
            writeObject(ref writer, "discardReason", discardReason);
            writer.WriteEndObject();
            writer.Flush();
            return sw;
        }
        /// <summary>
        /// 退套餐
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="tableno"></param>
        /// <param name="discardUserId"></param>
        /// <param name="userid"></param>
        /// <param name="orderdishidinfo"></param>
        /// <param name="dishnum"></param>
        /// <returns></returns>
        public static StringWriter getBackDish_dc(string orderNo, string tableno, string discardUserId, string userid, DataRow orderdishidinfo, double dishnum, string discardReason)
        {
            //+ "-" + fishPosition
            string primarykey = orderdishidinfo["primarykey"].ToString();
            string dishNo = orderdishidinfo["dishid"].ToString();
            double dishNum = dishnum;
            string dishunit = orderdishidinfo["dishunit"].ToString();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writeObject(ref writer, "actionType", "0");
            writeObject(ref writer, "userName", userid);
            writeObject(ref writer, "orderNo", orderNo);
            writeObject(ref writer, "dishNo", dishNo);
            writeObject(ref writer, "primarykey", primarykey);
            writeObject(ref writer, "dishNum", getdishNum(dishNum));
            writeObject(ref writer, "dishunit", dishunit);
            writeObject(ref writer, "currenttableid", tableno);
            writeObject(ref writer, "dishtype", "2");
            writeObject(ref writer, "operationType", 2);
            writeObject(ref writer, "sequence", 999999);
            writeObject(ref writer, "discardUserId", discardUserId);
            writeObject(ref writer, "discardReason", discardReason);
            writer.WriteEndObject();
            writer.Flush();
            return sw;
        }

    }
}
