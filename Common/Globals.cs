///*************************************************************************/
///*
///* 文件名    ：Globals.cs                                      
///* 程序说明  : 公共单元
///* 原创作者  ： 
///* 
///* Copyright 2010-2011 
///**************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Data;
using System.Linq;
using Models;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using Models.Enum;

namespace Common
{
    /// <summary>
    /// 公共单元
    /// </summary>
    public struct sUserInfo
    {
        public String UserName;
        public String PassWord;
        public String UserID;
        public String msg;
    }
    public struct pszTicket
    {
        public String Coupons_Name; //券名称
        public String Coupon_code;  //券ID
        public float Coupon_Amount; //券金额
        public String Coupon_No;  //券张数
        public float Coupon_NoAmount;//单张金额
        public String Copon_Type;//券类型
    }
    public struct sUserRight
    {
        public bool right1;//登录
        public bool right2;//开业
        public bool right3;//反结算
        public bool right4;//清机
        public bool right5;//结业
        public bool right6;//收银
        public bool right7;
        public bool right8;
        public bool right9;
        public void initRight()
        {
            right1 = false;
            right2 = false;
            right3 = false;
            right4 = false;
            right5 = false;
            right6 = false;
            right7 = false;
            right8 = false;
            right9 = false;
        }
        public bool getSyRigth()
        {
            return right6;
        }
        public bool getJyRight()
        {
            return right5;
        }
    }

    public class Globals
    {
        static Globals()
        {
            SystemSetDatas = new List<SystemSetData>();
        }

        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();
        public const string DEF_PROGRAM_NAME = "";
        public const string DEF_DATE_FORMAT = "yyyy-MM-dd";// "dd/MM/yyyy";
        public const string DEF_LONE_DATE_FORMAT = "yyyy-MM-dd hh:mm:ss";//"dd/MM/yyyy hh:mm:ss";
        public const string DEF_YYYYMMDD = "yyyyMMdd";
        public const string DEF_YYYYMMDD_LONG = "yyyy-MM-dd";//"yyyy/MM/dd";
        public const string DEF_DATE_LONG_FORMAT = "yyyy-MM-dd hh:mm:ss";//"yyyy/MM/dd hh:mm:ss";                
        public const string DEF_NULL_DATETIME = "1900-1-1";
        public const string DEF_NULL_VALUE = "NULL";
        public const string DEF_CURRENCY = "RMB";//预设货币
        public const string DEF_DECIMAL_FORMAT = "0.00"; //输出格式        
        public const string DEF_NO_TEXT = "*自动生成*";
        /// <summary>
        /// 系统数据库名。开发框架2.2版支持多帐套管理，帐套表定义在系统数据库。
        /// 打开登录窗体时加载帐套数据给用户选择。        
        /// 
        /// 帐套数据由系统管理员在后台配置，不提供配置窗体。
        /// </summary>
        public const string DEF_SYSTEM_DB = "System";
        public const string DEF_MASTER_DB = "master";

        public const int DEF_DECIMAL_ROUND = 2;//四舍五入小数位

        public static sUserInfo UserInfo;//当前登录用户
        public static t_table CurrTableInfo = new t_table();//当前选择桌台信息
        public static t_order CurrOrderInfo = new t_order();//当前帐单信息
        public static DataTable OrderTable = new DataTable();//帐单转换为C# datatable格式数据
        public static string workdate = "";

        /// <summary>
        /// 授权人信息。
        /// </summary>
        public static sUserInfo AuthorizerInfo;

        /// <summary>
        /// 外卖购物车表
        /// </summary>
        public static DataTable ShoppTable = null;//下单时的临时表

        public static bool canCash = false;//有没有收银权限

        public static DataTable tbUserRigth = new DataTable();
        public static sUserRight userRight;//当前用户权限
        public static string jsOrder = "";
        public static string superpwd = "11111111";
        public static string ProductVersion = "";
        public static TRoundInfo roundinfo;//零头处理方式
        public static TSetting cjSetting;//餐具设置

        /// <summary>
        /// 全单备注。
        /// </summary>
        public static string OrderRemark { get; set; }

        /// <summary>
        /// 赠菜原因。
        /// </summary>
        public static string DishGiftReason { get; set; }

        private static List<string> _dietSetting;
        /// <summary>
        /// 忌口设置。
        /// </summary>
        public static List<string> DietSetting
        {
            get
            {
                if (_dietSetting != null && _dietSetting.Any())
                    return _dietSetting;

                if (!SystemSetDatas.Any())
                    return new List<string>();

                _dietSetting = SystemSetDatas.Where(t => t.Type == EnumSystemDataType.JI_KOU_SPECIAL).Select(t => t.ItemDesc).ToList();
                return _dietSetting;
            }
        }

        /// <summary>
        /// 零头处理方式。
        /// </summary>
        private static EnumOddModel? _oddModel;
        /// <summary>
        /// 获取零头处理方式。
        /// </summary>
        public static EnumOddModel OddModel
        {
            get
            {
                if (_oddModel.HasValue)
                    return _oddModel.Value;

                if (!SystemSetDatas.Any())
                    return EnumOddModel.None;

                var oddSetting = SystemSetDatas.FirstOrDefault(t => t.Type == EnumSystemDataType.ROUNDING);
                _oddModel = oddSetting == null ? EnumOddModel.None : (EnumOddModel)oddSetting.ItemId;
                return _oddModel.Value;
            }
            set { _oddModel = value; }
        }

        /// <summary>
        /// 零头处理精度。
        /// </summary>
        private static EnumOddAccuracy? _oddAccuracy;

        public static EnumOddAccuracy OddAccuracy
        {
            get
            {
                if (_oddAccuracy.HasValue)
                    return _oddAccuracy.Value;

                if (!SystemSetDatas.Any())
                    return EnumOddAccuracy.Jiao;

                var oddAccSetting = SystemSetDatas.FirstOrDefault(t => t.Type == EnumSystemDataType.ACCURACY);
                _oddAccuracy = oddAccSetting == null ? EnumOddAccuracy.Fen : (EnumOddAccuracy)oddAccSetting.ItemId;
                return _oddAccuracy.Value;
            }
        }

        /// <summary>
        /// 系统设置集合。
        /// </summary>
        public static List<SystemSetData> SystemSetDatas { get; private set; }

        public static JArray cjFood;//餐具
        public static String branch_id = "";//分店ID号

        /// <summary>
        /// 店铺营业时间。
        /// </summary>
        public static RestaurantTradeTime TradeTime { get; set; }

        /// <summary>
        /// 所有银行集合。
        /// </summary>
        public static List<BankInfo> BankInfos { get; set; }

        //客用单
        public static DataTable CustPrintTable = new DataTable();
        /// <summary>
        /// 加载Debug\Images目录下的的图片
        /// </summary>
        /// <param name="imgFileName">文件名</param>
        /// <returns></returns>
        public static Image LoadImage(string imgFileName)
        {
            string file = Application.StartupPath + @"\images\" + imgFileName;
            if (File.Exists(file))
                return Image.FromFile(file);
            else
                return null;
        }

        /// <summary>
        /// 加载Debug\Images目录下的的图片
        /// </summary>
        /// <param name="imgFileName">文件名</param>
        /// <returns></returns>
        public static Bitmap LoadBitmap(string imgFileName)
        {
            string file = Application.StartupPath + @"\images\" + imgFileName;

            if (File.Exists(file))
                return new Bitmap(Bitmap.FromFile(file));
            else
                return null;
        }

        /// <summary>
        /// 移除ＳＱＬ注入非法字符
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <param name="returnMaxLength">返回的长度，0长度为不处理．</param>
        /// <returns></returns>
        public static string RemoveInjection(string content, int returnMaxLength)
        {
            string replaced = content.Replace("'", "").Replace("@", "").Replace("0x", "");
            if (returnMaxLength == 0)
                return replaced;
            else
                return replaced.Substring(0, replaced.Length < returnMaxLength ? replaced.Length : returnMaxLength);
        }


        public static void SetButton(Button button)
        {
            MethodInfo methodinfo = button.GetType().GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            methodinfo.Invoke(button, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, new object[] { ControlStyles.Selectable, false }, Application.CurrentCulture);
        }

        public static JArray GetTableJson(DataTable dt)
        {
            string str = "";
            str = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray ja = new JArray(str);
            return ja;
        }

        /// <summary>   
        /// 将 Json 解析成 DateTable。  
        /// Json 数据格式如: 
        ///     {table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]} /// </summary>   
        /// <param name="strJson">要解析的 Json 字符串</param>   
        /// <returns>返回 DateTable</returns>   
        public static DataTable JsonToDataTable(string strJson)
        {
            // 取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;

            // 去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            // 获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                // 创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].Replace("\"", "");
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                // 增加内容   
                DataRow dr = tb.NewRow();
                for (int j = 0; j < strRows.Length; j++)
                {
                    dr[j] = strRows[j].Split(':')[1].Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        public static void Delay(uint ms)
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms)
            {
                Application.DoEvents();
            }
        }

    }
}
