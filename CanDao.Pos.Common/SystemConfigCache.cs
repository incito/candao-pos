using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CanDao.Pos.Model.Enum;
using JunLan.Common.Base;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 系统配置缓存。
    /// </summary>
    public static class SystemConfigCache
    {
        /// <summary>
        /// 配置文件相对路径。
        /// </summary>
        private const string CfgFile = @"Config\SystemCfg.xml";

        /// <summary>
        /// java后台服务地址。
        /// </summary>
        public static string JavaServer { get; private set; }

        /// <summary>
        /// 大数据服务地址。
        /// </summary>
        public static string BigData { get; private set; }

        /// <summary>
        /// DataServer服务地址。
        /// </summary>
        public static string DataServer { get; private set; }

        /// <summary>
        /// 会员等云服务地址。
        /// </summary>
        public static string CloudServer { get; private set; }

        /// <summary>
        /// 会员系统。0：雅座，1：餐道。
        /// </summary>
        public static int MemberSystem { get; set; }

        /// <summary>
        /// 外卖台餐台名称。
        /// </summary>
        //public static string TakeoutTableName { get; set; }

        /// <summary>
        /// Pos编号。
        /// </summary>
        public static string PosId { get; set; }

        /// <summary>
        /// 钱箱IP地址。
        /// </summary>
        public static string OpenCashIp { get; set; }

        /// <summary>
        /// 是否保存优惠券信息。
        /// </summary>
        public static bool SaveCoupon { get; set; }

        /// <summary>
        /// 是否打印开发票小票。
        /// </summary>
        public static bool PrintInvoice { get; set; }

        /// <summary>
        /// 反结算原因集合。
        /// </summary>
        public static List<string> ResettlementReasonList { get; set; }

        /// <summary>
        /// 赠菜可选原因集合。
        /// </summary>
        public static List<string> DishGiftReasonList { get; set; }

        /// <summary>
        /// 退菜可选原因集合。
        /// </summary>
        public static List<string> BackDishReasonList { get; set; }

        public static void LoadCfgFile()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CfgFile);
            if (File.Exists(file))
            {
                try
                {
                    var xDoc = XDocument.Load(file);
                    var root = xDoc.Root;
                    if (root == null)
                        return;

                    JavaServer = GetElementValue(root.Element("JavaServer"));
                    BigData = GetElementValue(root.Element("BigData"));
                    DataServer = GetElementValue(root.Element("DataServer"));
                    CloudServer = GetElementValue(root.Element("CloudServer"));
                    MemberSystem = GetElementInt(root.Element("MemberSystem"), 1);
                    //TakeoutTableName = GetElementValue(root.Element("TakeoutTableName"));
                    Globals.BranchTelephone = GetElementValue(root.Element("BranchTelephone"));
                    PosId = GetElementValue(root.Element("PosId"));
                    OpenCashIp = GetElementValue(root.Element("OpenCashIp"));
                    Globals.MemberSystem = (EnumMemberSystem)MemberSystem;

                    SaveCoupon = GetElementBool(root.Element("SaveCoupon"), false);
                    PrintInvoice = GetElementBool(root.Element("PrintInvoice"), false);

                    var resettlementReason = GetElementValue(root.Element("ResettlementReason"));
                    ResettlementReasonList = !string.IsNullOrEmpty(resettlementReason) ? resettlementReason.Split(';').ToList() : null;

                    var giftDishReason = GetElementValue(root.Element("DishGiftReasons"));
                    DishGiftReasonList = !string.IsNullOrEmpty(giftDishReason) ? giftDishReason.Split(';').ToList() : null;

                    var backDishReason = GetElementValue(root.Element("BackDishReasons"));
                    BackDishReasonList = !string.IsNullOrEmpty(backDishReason) ? backDishReason.Split(';').ToList() : null;
                }
                catch (Exception ex)
                {
                    ErrLog.Instance.E(ex);
                }
            }
        }

        private static string GetElementValue(XElement element)
        {
            return element == null ? null : XmlHelper.GetAttrValue(element, "value");
        }

        private static int GetElementInt(XElement element, int defaultValue)
        {
            var str = GetElementValue(element);
            return !string.IsNullOrEmpty(str) ? Convert.ToInt32(str) : defaultValue;
        }

        private static bool GetElementBool(XElement element, bool defaultValue)
        {
            var str = GetElementValue(element);
            return !string.IsNullOrEmpty(str) ? Convert.ToBoolean(str) : defaultValue;
        }
    }
}