using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TCandaoRegMemberInfo
    {
        string branch_id;
        string securitycode;
        string mobile;
        string cardno;
        string password;
        string name;
        string gender;
        string birthday;
        int tenant_id;
        int regtype;
        string memberavatar;

        public string Member_avatar
        {
            get { return memberavatar; }
            set { memberavatar = value; }
        }
        public string Securitycode
        {
            get { return securitycode; }
            set { securitycode = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        public string Cardno
        {
            get { return cardno; }
            set { cardno = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public string Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        public int Regtype
        {
            get { return regtype; }
            set { regtype = value; }
        }
        public int Tenant_id
        {
            get { return tenant_id; }
            set { tenant_id = value; }
        }
        string member_avatar;
        public string Branch_id
        {
            get { return branch_id; }
            set { branch_id = value; }
        }

    }
}
