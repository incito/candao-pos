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
        private string _couponname;
        private string _description;
        private DateTime? _begintime;
        private DateTime? _endtime;
        private DateTime? _inserttime;
        private string _ruleid;
        private string _couponid;
        private string _dishid;
        private int? _dishnum;
        private string _freedishid;
        private int? _freedishnum;
        private int? _couponway;
        private int? _comsumeway;
        private decimal? _couponrate;
        private decimal? _couponamount;
        private decimal? _totalamount;
        private decimal? _couponcash;
        private int? _couponnum;
        private decimal? _freeamount;
        private string _banktype;
        private string _partnername;
        private string _groupweb;
        private string _unitid;
        private decimal? _debitamount;
        private int? _type;
        private string _wholesingle;
        private string _sub_type;

        public string Color { get; set; }

        public string sub_type
        {
            set { _sub_type = value; }
            get { return _sub_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string couponname
        {
            set { _couponname = value; }
            get { return _couponname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? begintime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? endtime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? inserttime
        {
            set { _inserttime = value; }
            get { return _inserttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ruleid
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string couponid
        {
            set { _couponid = value; }
            get { return _couponid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dishid
        {
            set { _dishid = value; }
            get { return _dishid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? dishnum
        {
            set { _dishnum = value; }
            get { return _dishnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string freedishid
        {
            set { _freedishid = value; }
            get { return _freedishid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? freedishnum
        {
            set { _freedishnum = value; }
            get { return _freedishnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? couponway
        {
            set { _couponway = value; }
            get { return _couponway; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? comsumeway
        {
            set { _comsumeway = value; }
            get { return _comsumeway; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? couponrate
        {
            set { _couponrate = value; }
            get { return _couponrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? couponamount
        {
            set { _couponamount = value; }
            get { return _couponamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? totalamount
        {
            set { _totalamount = value; }
            get { return _totalamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? couponcash
        {
            set { _couponcash = value; }
            get { return _couponcash; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? couponnum
        {
            set { _couponnum = value; }
            get { return _couponnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? freeamount
        {
            set { _freeamount = value; }
            get { return _freeamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string banktype
        {
            set { _banktype = value; }
            get { return _banktype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string partnername
        {
            set { _partnername = value; }
            get { return _partnername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string groupweb
        {
            set { _groupweb = value; }
            get { return _groupweb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string unitid
        {
            set { _unitid = value; }
            get { return _unitid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string wholesingle
        {
            set { _wholesingle = value; }
            get { return _wholesingle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? debitamount
        {
            set { _debitamount = value; }
            get { return _debitamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model
        public static int  strtoint(string str)
        {
            int re=0;
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
        public static VCouponRule  Parse(JObject ja)
        {
            VCouponRule vcr = new VCouponRule();

            vcr.couponname = ja["couponname"].ToString();
            vcr.description = ja["description"].ToString();
            /*if (ja["begintime"] != null && ja["begintime"].ToString() != "")
            {
                vcr.begintime = DateTime.Parse(ja["begintime"].ToString());
            }
            if (ja["endtime"] != null && ja["endtime"].ToString() != "")
            {
                vcr.endtime = DateTime.Parse(ja["endtime"].ToString());
            }
            if (ja["inserttime"] != null && ja["inserttime"].ToString() != "")
            {
                vcr.inserttime = DateTime.Parse(ja["inserttime"].ToString());
            }*/

            vcr.ruleid = ja["ruleid"].ToString();
            vcr.couponid = ja["couponid"].ToString();
            vcr.dishid = ja["dishid"].ToString();
            vcr.banktype = ja["banktype"].ToString();
            if (vcr.banktype == "100")
            {
                vcr.dishnum = strtoint(ja["dishnum"].ToString());
            }else
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
            catch {
                try
                {
                    vcr.couponname = ja["free_reason"].ToString();
                }
                catch { vcr.couponname = ja["couponname"].ToString(); }
            }
            //vcr.couponname = ja["name"].ToString();//couponname
            if(vcr.couponname.Equals(""))
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
