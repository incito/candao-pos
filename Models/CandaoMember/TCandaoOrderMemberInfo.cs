using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TCandaoOrderMemberInfo : TCandaoRetBase
    {
        string orderid;//string	帐单号
        string userid;//string	
        string business;//string	分店号
        string terminal;//string	
        string serial;//string	
        string businessname;//string	商家名称
        decimal score;//float	积分增减 
        decimal coupons;//float	券金额
        decimal stored;//Float	使用储值金额
        decimal scorebalance;//Float	积分余额
        string couponsbalance;//Float	券余额
        decimal storedbalance;//Float	储值余额
        decimal psexpansivity;//string	消费虚增
        decimal netvalue;//Float	净值
        decimal Inflated;//Float	虚增值
        string cardno;

        public string Cardno
        {
            get { return cardno; }
            set { cardno = value; }
        }
        public string Orderid
        {
            get { return orderid; }
            set { orderid = value; }
        }
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }
        public string Business
        {
            get { return business; }
            set { business = value; }
        }
        public string Terminal
        {
            get { return terminal; }
            set { terminal = value; }
        }
        public string Serial
        {
            get { return serial; }
            set { serial = value; }
        }
        public string Businessname
        {
            get { return businessname; }
            set { businessname = value; }
        }
        public decimal Score
        {
            get { return score; }
            set { score = value; }
        }
        public decimal Scorebalance
        {
            get { return scorebalance; }
            set { scorebalance = value; }
        }
        public string Couponsbalance
        {
            get { return couponsbalance; }
            set { couponsbalance = value; }
        }
        public decimal Storedbalance
        {
            get { return storedbalance; }
            set { storedbalance = value; }
        }
        public decimal Psexpansivity
        {
            get { return psexpansivity; }
            set { psexpansivity = value; }
        }
        public decimal Netvalue
        {
            get { return netvalue; }
            set { netvalue = value; }
        }
        public decimal Inflated1
        {
            get { return Inflated; }
            set { Inflated = value; }
        }
        public decimal Coupons
        {
            get { return coupons; }
            set { coupons = value; }
        }
        public decimal Stored
        {
            get { return stored; }
            set { stored = value; }
        }
    }
}
