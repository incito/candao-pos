using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models
{
    public class VCouponRule : ICloneable
    {
        public VCouponRule()
        { }
        #region Model

        public string Color { get; set; }

        public string sub_type { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string couponname { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string description { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? begintime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? endtime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? inserttime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string ruleid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string couponid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string dishid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? dishnum { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string freedishid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? freedishnum { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? couponway { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? comsumeway { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? couponrate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? couponamount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? totalamount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? couponcash { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? couponnum { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? freeamount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string banktype { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string partnername { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string groupweb { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string unitid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string wholesingle { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? debitamount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? type { set; get; }

        /// <summary>
        /// 当是手工优免类型时，这里0：赠菜，1：折扣，2：减免。
        /// </summary>
        public string FreeReason { get; set; }

        /// <summary>
        /// 是否是不常用优惠券，不常用为true，默认常用为false。
        /// </summary>
        public bool IsUncommonlyUsed { get; set; }

        #endregion Model
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
                re = float.Parse(str);
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
        public static VCouponRule Parse(JObject ja)
        {
            VCouponRule vcr = new VCouponRule();

            vcr.couponname = ja["couponname"].ToString();
            vcr.description = ja["description"].ToString();
            vcr.ruleid = ja["ruleid"].ToString();
            vcr.couponid = ja["couponid"].ToString();
            vcr.dishid = ja["dishid"].ToString();
            vcr.banktype = ja["banktype"].ToString();
            if (vcr.banktype == "100")
            {
                vcr.dishnum = strtoint(ja["dishnum"].ToString());
            }
            else
            {
                vcr.dishnum = 20;
            }
            vcr.freedishid = ja["freedishid"].ToString();
            vcr.freedishnum = strtoint(ja["freedishnum"].ToString());
            vcr.couponway = strtoint(ja["couponway"].ToString());
            vcr.comsumeway = strtoint(ja["comsumeway"].ToString());
            vcr.couponrate = strtodecimal(ja["couponrate"].ToString());
            vcr.couponamount = strtodecimal(ja["couponamount"].ToString());
            vcr.totalamount = strtodecimal(ja["totalamount"].ToString());
            vcr.couponcash = strtodecimal(ja["couponcash"].ToString());
            vcr.couponnum = strtoint(ja["couponnum"].ToString());
            vcr.freeamount = strtodecimal(ja["freeamount"].ToString());
            vcr.partnername = ja["partnername"].ToString();
            vcr.groupweb = ja["groupweb"].ToString();
            vcr.unitid = ja["unitid"].ToString();
            vcr.wholesingle = ja["wholesingle"].ToString();
            try
            {
                vcr.debitamount = strtodecimal(ja["debitamount"].ToString());
            }
            catch { vcr.debitamount = 0; }
            vcr.type = strtoint(ja["type"].ToString());
            vcr.sub_type = "0";
            return vcr;
        }
        /// <summary>
        /// 新表结构转为对像
        /// </summary>
        /// <param name="ja"></param>
        /// <returns></returns>
        public static VCouponRule Parsev2(JObject ja)
        {
            VCouponRule vcr = new VCouponRule();
            try
            {
                vcr.couponname = ja["name"].ToString();
            }
            catch
            {
                try
                {
                    vcr.couponname = ja["free_reason"].ToString();
                }
                catch { vcr.couponname = ja["couponname"].ToString(); }
            }
            vcr.FreeReason = ja["free_reason"] != null ? ja["free_reason"].ToString() : null;
            //vcr.couponname = ja["name"].ToString();//couponname
            if (vcr.couponname.Equals(""))
            {
                try
                { vcr.couponname = ja["company_name"].ToString(); }
                catch { }
            }
            try
            {
                vcr.description = ja["activity_introduction"].ToString();//description
            }
            catch { vcr.description = ""; }

            vcr.Color = ja["color"] != null ? ja["color"].ToString() : null;
            vcr.ruleid = ja["id"].ToString();//ruleid
            vcr.couponid = ja["preferential"].ToString();//couponid
            try
            { vcr.dishid = ja["dish"].ToString(); } //dishid
            catch { }
            try
            {
                vcr.banktype = ja["type"].ToString(); //banktype
            }
            catch { vcr.banktype = "06"; }

            if (vcr.banktype == "100")
            {
                vcr.dishnum = strtoint(ja["dishnum"].ToString());
            }
            else
            {
                vcr.dishnum = 20;
            }
            vcr.freedishid = vcr.dishid;// ja["dish"].ToString(); //freedishid
            vcr.freedishnum = 0;// strtoint(ja["freedishnum"].ToString());
            vcr.couponway = 0;// strtoint(ja["couponway"].ToString());
            vcr.comsumeway = 0;// strtoint(ja["comsumeway"].ToString());

            try
            {
                vcr.couponrate = decimal.Parse(ja["discount"].ToString()); //banktype
            }
            catch { vcr.couponrate = 0; }
            //vcr.couponrate = 0;// strtodecimal(ja["couponrate"].ToString());
            vcr.couponamount = 0;// strtodecimal(ja["couponamount"].ToString());
            vcr.totalamount = 0;// strtodecimal(ja["totalamount"].ToString());
            vcr.couponcash = 0;// strtodecimal(ja["couponcash"].ToString());
            vcr.couponnum = 0;// strtoint(ja["couponnum"].ToString());
            string bill_amount = "0";
            try
            {
                bill_amount = ja["bill_amount"].ToString();
            }
            catch { }
            string amount = "0";
            try
            {
                amount = ja["amount"].ToString();
            }
            catch { }
            if (bill_amount.Equals(""))
            {
                vcr.freeamount = strtodecimal(amount);
            }
            else
                vcr.freeamount = strtodecimal(bill_amount) - strtodecimal(amount);//freeamount
            try
            {
                vcr.partnername = ja["company_name"].ToString(); //partnername
            }
            catch { }
            if (vcr.partnername == null)
                vcr.partnername = "";
            vcr.groupweb = "0";// ja["groupweb"].ToString();
            try
            {
                vcr.unitid = ja["unit"].ToString();//unitid
            }
            catch { }
            if (vcr.freedishid != null)
                vcr.wholesingle = "1"; //单品折扣类调用新接口获取优惠的金额
            else
                vcr.wholesingle = "0";
            //vcr.wholesingle = ja["wholesingle"].ToString();
            try
            {
                if (bill_amount.Equals(""))
                {
                    vcr.debitamount = 0;
                }
                else
                    vcr.debitamount = strtodecimal(amount);//strtodecimal(ja["debitamount"].ToString());
            }
            catch { vcr.debitamount = 0; }
            try
            {
                vcr.type = strtoint(ja["type"].ToString());
            }
            catch { vcr.type = 6; }
            try
            {
                vcr.sub_type = ja["sub_type"].ToString();
            }
            catch { vcr.sub_type = ""; }

            if (vcr.sub_type == null)
                vcr.sub_type = "";

            var status = ja["status"] != null ? ja["status"].ToString() : "";
            vcr.IsUncommonlyUsed = status == "2";
            return vcr;
        }

        public object Clone()
        {
            //return this as object;          //引用同一个对象
            return this.MemberwiseClone();  //浅复制
            //return new VCouponRule() as object; //深复制
        }
    }

}
