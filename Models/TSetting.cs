using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    //{"rows":[{"id":"DISHES_98","status":"1","itemSort":"","typename":"餐具设置","itemDesc":"餐具设置","itemid":"11","type":"DISHES"}]}
    public class TSetting
    {
        string _id;
        string _status;
        string _itemSort;
        string _typename;
        string _itemDesc;
        string _itemid;
        string _type;

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
