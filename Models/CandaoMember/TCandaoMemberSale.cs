using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TCandaoMemberSale
    {
        string branch_id;
        string securitycode;
        string cardno;
        string password;
        string serial;
        decimal fcash;
        decimal fintegral;
        decimal fstore;
        string fticketlist;

        public decimal Fstore
        {
            get { return fstore; }
            set { fstore = value; }
        }

        public string Fticketlist
        {
            get { return fticketlist; }
            set { fticketlist = value; }
        }
        public decimal Fintegral
        {
            get { return fintegral; }
            set { fintegral = value; }
        }
        public decimal Fcash
        {
            get { return fcash; }
            set { fcash = value; }
        }
        public string Serial
        {
            get { return serial; }
            set { serial = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Cardno
        {
            get { return cardno; }
            set { cardno = value; }
        }
        public string Securitycode
        {
            get { return securitycode; }
            set { securitycode = value; }
        }
        public string Branch_id
        {
            get { return branch_id; }
            set { branch_id = value; }
        }

    }
}
