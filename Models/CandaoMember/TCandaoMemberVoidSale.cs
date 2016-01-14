using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    public class TCandaoMemberVoidSale
    {
        string branch_id;
        string securitycode;
        string cardno;
        string password;
        string serial;
        string tracecode;
        string superpwd;

        public string Tracecode
        {
            get { return tracecode; }
            set { tracecode = value; }
        }

        public string Superpwd
        {
            get { return superpwd; }
            set { superpwd = value; }
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
