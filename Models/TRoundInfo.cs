using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TRoundInfo
    {
        /*
         type为“ROUNDING”的是零头处理方式的类型，
         type为“ACCURACY”的为单位
         type为ROUNDING:
              itemid： 0 表示 不处理，1 表示 四舍五入，2表示 抹零
         type为ACCURACY:
              itemid： 0 表示 分，1 表示 角，2表示 元 
         */
        string _id;
        string _status;
        string _itemSort;
        string _typename;
        string _itemDesc;
        string _itemid;//itemid： 0 表示 分，1 表示 角，2表示 元
        string _type;
        string _roundtype;//0 表示 分，1 表示 角，2表示 元 

        public string Roundtype
        {
            get { return _roundtype; }
            set { _roundtype = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string ItemSort
        {
            get { return _itemSort; }
            set { _itemSort = value; }
        }
        public string Typename
        {
            get { return _typename; }
            set { _typename = value; }
        }
        public string ItemDesc
        {
            get { return _itemDesc; }
            set { _itemDesc = value; }
        }
        public string Itemid
        {
            get { return _itemid; }
            set { _itemid = value; }
        }
    }
}
