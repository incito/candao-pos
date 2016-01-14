using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Models
{
    public class TUserGroup
    {
        public static string __TableName = "tb_MyUserGroup";

        public static string __KeyName = "GroupCode";

        public static string isid = "isid";
        public static string GroupCode = "GroupCode";

        public static string GroupName = "GroupName";

    }

    public class BusinessDataSetIndex
    {
        public const int Groups = 0;
        public const int GroupUsers = 1;
        public const int GroupAuthorities = 2;
        public const int GroupAvailableUser = 3;
    }
}
