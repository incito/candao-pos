using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    public class TCandaoMemberInfo:TCandaoRetBase
    {
        string _mcard;
        decimal _storecardbalance;
        decimal _integraloverall;
        decimal _couponsoverall;
        string _ticketinfo;
        string _tracecode;
        int _cardtype;
        string _cardlevel;
        string _mobile;
        string _name;
        int _gender;
        string _birthday;
        string _memberaddress;
        string _regdate;
        string _cardlist;
        string _member_avatar;

        public string Regdate
        {
            get { return _regdate; }
            set { _regdate = value; }
        }


        public string Cardlist
        {
            get { return _cardlist; }
            set { _cardlist = value; }
        }


        public string Member_avatar
        {
            get { return _member_avatar; }
            set { _member_avatar = value; }
        }
        public string Memberaddress
        {
            get { return _memberaddress; }
            set { _memberaddress = value; }
        }
        public string Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        public string Cardlevel
        {
            get { return _cardlevel; }
            set { _cardlevel = value; }
        }
        public int Cardtype
        {
            get { return _cardtype; }
            set { _cardtype = value; }
        }
        public string Tracecode
        {
            get { return _tracecode; }
            set { _tracecode = value; }
        }
        public string Ticketinfo
        {
            get { return _ticketinfo; }
            set { _ticketinfo = value; }
        }

        public decimal Couponsoverall
        {
            get { return _couponsoverall; }
            set { _couponsoverall = value; }
        }
        public decimal Storecardbalance
        {
            get { return _storecardbalance; }
            set { _storecardbalance = value; }
        }
        public string Mcard
        {
            get { return _mcard; }
            set { _mcard = value; }
        }
        public decimal Integraloverall
        {
            get { return _integraloverall; }
            set { _integraloverall = value; }
        }

    }
}
