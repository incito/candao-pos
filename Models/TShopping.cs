using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
namespace Models
{
    /// <summary>
    /// tshopping:购物车实体类(属性说明自动提取数据库字段的描述信息) //tdishinfo 和菜品类共用 
    /// </summary>
    [Serializable]
    public partial class t_shopping
    {
        public t_shopping()
        { }
        #region Model
        private string _orderid;//帐单号
        private string _userid;//员工
        private DateTime _ordertime;//下单时间
        private int _orderstatus;//状态
        private float _dishnum;//数量
        private string _tableid;//桌号
        private string _dishid;//菜品编号
        private decimal _price;//单价
        private decimal _price2;//单价
        private string _avoid;//忌口
        private decimal _memberprice;//会员价
        private int _dishidleft;//编号
        private string _title;//菜品名称
        private string _dishunit;//单位
        private decimal _amount;//金额
        private string _source;//分类
        private string _dishtype;//类别
        private int _ispot;//鱼锅
        private string _parentdishid;
        private string _groupid;//是不是一组
        private string _menuid;
        private string _contactdishid;
        private int _ordertype;
        private string _groupid2;//是不是一组
        private int _weigh;//是不是称重
        private int _primarydishtype;
        private string _level;

        public int Primarydishtype
        {
            get { return _primarydishtype; }
            set { _primarydishtype = value; }
        }

        public int Weigh
        {
            get { return _weigh; }
            set { _weigh = value; }
        }

        public string Groupid2
        {
            get { return _groupid2; }
            set { _groupid2 = value; }
        }

        public int Ordertype
        {
            get { return _ordertype; }
            set { _ordertype = value; }
        }

        public string Contactdishid
        {
            get { return _contactdishid; }
            set { _contactdishid = value; }
        }

        public string Menuid
        {
            get { return _menuid; }
            set { _menuid = value; }
        }
        public string Groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }

        public string Parentdishid
        {
            get { return _parentdishid; }
            set { _parentdishid = value; }
        }
        public int IsPot
        {
            get { return _ispot; }
            set { _ispot = value; }
        }
        public string DishType
        {
            get { return _dishtype; }
            set { _dishtype = value; }
        }
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public decimal Price2
        {
            get { return _price2; }
            set { _price2 = value; }
        }
        public string Dishunit
        {
            get { return _dishunit; }
            set { _dishunit = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public int Dishidleft
        {
            get { return _dishidleft; }
            set { _dishidleft = value; }
        }
        public string Orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

        public string Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }


        public DateTime Ordertime
        {
            get { return _ordertime; }
            set { _ordertime = value; }
        }


        public int Orderstatus
        {
            get { return _orderstatus; }
            set { _orderstatus = value; }
        }


        public float Dishnum
        {
            get { return _dishnum; }
            set { _dishnum = value; }
        }


        public string Tableid
        {
            get { return _tableid; }
            set { _tableid = value; }
        }


        public string Dishid
        {
            get { return _dishid; }
            set { _dishid = value; }
        }
 

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }


        public decimal Memberprice
        {
            get { return _memberprice; }
            set { _memberprice = value; }
        }


        public string Avoid
        {
            get { return _avoid; }
            set { _avoid = value; }
        }

        public string PrimaryKey { get; set; }

        #endregion Model
        public static  void createShoppTable(ref DataTable shopptable)
        {
            DataTable tbyh=new DataTable();
            tbyh.Columns.Clear();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "帐单号";
            column.ColumnName = "orderid";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "下单员工";
            column.ColumnName = "userid";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "标示符";
            column.ColumnName = "primarykey";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.AllowDBNull = false;
            column.Caption = "下单时间";
            column.ColumnName = "ordertime";
            //column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "状态";
            column.ColumnName = "orderstatus";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "数量";
            column.ColumnName = "dishnum";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "桌号";
            column.ColumnName = "tableid";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "菜品编号";
            column.ColumnName = "dishid";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "忌口";
            column.ColumnName = "avoid";
            //column.DefaultValue = 1;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "编号";
            column.ColumnName = "dishidleft";
            column.DefaultValue = 1;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "菜品名称";
            column.ColumnName = "title"; 
            //column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "单位";
            column.ColumnName = "dishunit"; 
            //column.DefaultValue = "0";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "会员价";
            column.ColumnName = "memberprice"; 
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "单价";
            column.ColumnName = "price";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "单价2";
            column.ColumnName = "price2";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "金额";
            column.ColumnName = "amount";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "source";
            column.ColumnName = "source";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "parentdishid";
            column.ColumnName = "parentdishid";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "groupid";
            column.ColumnName = "groupid";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "ispot";
            column.ColumnName = "ispot";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "ordertype";
            column.ColumnName = "ordertype";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "Groupid2";
            column.ColumnName = "Groupid2";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "weigh";
            column.ColumnName = "weigh";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "primarydishtype";
            column.ColumnName = "primarydishtype";
            tbyh.Columns.Add(column);
            
            shopptable = tbyh;
        }
        /// <summary>
        /// 往购物车内增加一行数据
        /// </summary>
        /// <param name="dishrow"></param>
        public static void add(ref DataTable shopptable, t_shopping dishrow,bool isdish)
        {
            //如果dishid和单位相同就相加
            int i = 0;
            if (!isdish)
            {                    //如果是鱼锅不能加一起 套餐
                foreach (DataRow dr2 in shopptable.Rows)
                {
                    string dishid = dr2["dishid"].ToString();
                    string dishunit = dr2["dishunit"].ToString();
                    string primarydishtype = dr2["primarydishtype"].ToString();

                    if ((dishid.Equals(dishrow.Dishid)) && (dishunit.Equals(dishrow.Dishunit)) && (primarydishtype.Equals(dishrow.Primarydishtype.ToString())))
                    {
                        //如果已经有一条相同的
                        adddish(ref shopptable, i);
                        return;
                    }
                    i++;
                }
            }
            DataRow dr = shopptable.NewRow();
            dr["orderid"] = dishrow.Orderid;
            dr["primarykey"] = dishrow.PrimaryKey;
            dr["userid"] = dishrow.Userid;
            dr["ordertime"] = dishrow.Ordertime;
            dr["orderstatus"] = dishrow.Orderstatus;
            dr["dishnum"] = (decimal)Math.Round(dishrow.Dishnum, 2);
            dr["tableid"] = dishrow.Tableid;
            dr["dishid"] = dishrow.Dishid;
            dr["avoid"] = dishrow.Avoid;
            dr["dishidleft"] = dishrow.Dishidleft;
            dr["title"] = dishrow.Title;
            dr["dishunit"] = dishrow.Dishunit;
            dr["memberprice"] = dishrow.Memberprice;
            dr["price"] = dishrow.Price ;
            dr["price2"] = dishrow.Price2;
            dr["ordertype"] = dishrow.Ordertype;
            dr["amount"] = dishrow.Price * (decimal)dishrow.Dishnum;
            dr["source"] = dishrow.Source;
            dr["weigh"] = dishrow.Weigh;//dishstatus 如果是称重下单为1
            dr["primarydishtype"] = dishrow.Primarydishtype;
            if (dishrow.IsPot == null)
                dr["ispot"] = "";
            else
                dr["ispot"] = dishrow.IsPot.ToString();
            if (dishrow.Parentdishid == null)
                dr["Parentdishid"] = "";
            else
                dr["Parentdishid"] = dishrow.Parentdishid;

            if (dishrow.Groupid == null)
                dr["groupid"] = "";
            else
                dr["groupid"] = dishrow.Groupid;

            if (dishrow.Groupid2 == null)
                dr["Groupid2"] = "";
            else
                dr["Groupid2"] = dishrow.Groupid2;

            shopptable.Rows.Add(dr);

        }
        public static void del(ref DataTable shopptable, int index)
        {
            shopptable.Rows.RemoveAt(index);
        }
        public static void decdish(ref DataTable shopptable, int index)
        {
            DataRow dr = shopptable.Rows[index];
            decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
            dishnum--;
            //计算金额
            if(dishnum<=0)
            {
                shopptable.Rows.Remove(dr);
                return;
            }
            decimal price = decimal.Parse(dr["price"].ToString());
            decimal amount = dishnum * price;
            dr["dishnum"] = dishnum;
            dr["amount"] = amount;
        }
        public static void adddish(ref DataTable shopptable, int index)
        {
            DataRow dr = shopptable.Rows[index];
            decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
            dishnum++;
            //计算金额
            decimal price = decimal.Parse(dr["price"].ToString());
            decimal amount = dishnum * price;
            dr["dishnum"] = dishnum;
            dr["amount"] = amount;
        }
        public static void adddish(ref DataTable shopptable, DataRow dr)
        {
            //DataRow dr = shopptable.Rows[index];
            decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
            //如果是锅和鱼锅不能加，只能加鱼
            string Groupid = dr["Groupid"].ToString();
            string Orderstatus = dr["Orderstatus"].ToString();
            string primarydishtype = dr["primarydishtype"].ToString();
            if (!Groupid.Equals(""))
            {
                if (primarydishtype.Equals("2"))
                {
                    return;
                }
                if (!Orderstatus.Equals("3"))
                {
                    return;
                }
            }
            dishnum++;
            //计算金额
            decimal price = decimal.Parse(dr["price"].ToString());
            decimal amount = dishnum * price;
            dr["dishnum"] = dishnum;
            dr["amount"] = amount;
        }
        public static void decdish(ref DataTable shopptable, DataRow dr)
        {
            //DataRow dr = shopptable.Rows[index];
            //如果是锅都减掉，如果是鱼，数量为0了就把整个group减掉
            string Groupid = dr["Groupid"].ToString();
            string Orderstatus = dr["Orderstatus"].ToString();
            decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
            dishnum--;
            string primarydishtype = dr["primarydishtype"].ToString();
            if (!Groupid.Equals(""))
            {
                if (primarydishtype.Equals("2"))
                {
                    //把整份都减掉
                    delAllGroup(ref shopptable, Groupid);
                    return;
                }
            }
            if (!Groupid.Equals("") && (!Orderstatus.Equals("3")))
            {
                //如果是鱼，数量减到0，或是锅或是整个鱼锅，把整个group删掉
                if ((dishnum <= 0) || Orderstatus.Equals("0") || Orderstatus.Equals("1"))
                {
                    delAllGroup(ref shopptable, Groupid);
                    return;
                }
                if(dishnum<=0)
                {
                    dishnum = 0;
                }
            }
            else
            if (dishnum <= 0)
            {
                if (!Orderstatus.Equals("3"))
                  shopptable.Rows.Remove(dr);
                if (dishnum <= 0)
                {
                    dishnum = 0;
                }
            }
            ////计算金额
            decimal price = decimal.Parse(dr["price"].ToString());
            decimal amount = dishnum * price;
            dr["dishnum"] = dishnum;
            dr["amount"] = amount;
        }
        private static void delAllGroup(ref DataTable dt, string groupid)
        {
            DataRow dr;
            for (int i = dt.Rows.Count - 1; i >= 0;i-- )
            {
                dr = dt.Rows[i];
                if (dr["Groupid"].ToString().Equals(groupid))
                {
                    dt.Rows.Remove(dr);
                }
            }
        }
        public static decimal getAmount(ref DataTable shopptable)
        {
            decimal amount = 0;
            foreach (DataRow dr in shopptable.Rows)
            {
                decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
                decimal price = decimal.Parse(dr["price"].ToString());
                amount = (decimal)(amount + (dishnum * price));

            }
            return amount;
        }
        public static void setMemberPrice(ref DataTable shopptable)
        {
            foreach (DataRow dr in shopptable.Rows)
            {
                dr["price"] = dr["memberprice"].ToString();
                decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
                decimal price = decimal.Parse(dr["price"].ToString());
                decimal amount = dishnum * price;
                dr["amount"] = amount;
            }
  
        }
        public static void setPrice2(ref DataTable shopptable)
        {
            foreach (DataRow dr in shopptable.Rows)
            {
                dr["price"] = dr["price2"].ToString();
                decimal dishnum = decimal.Parse(dr["dishnum"].ToString());
                decimal price = decimal.Parse(dr["price"].ToString());
                decimal amount = dishnum * price;
                dr["amount"] = amount;
            }
        }
        public static void addCJ(JArray jrcj, t_table CurrTableInfo, t_order CurrOrderInfo, string userid, TSetting cjSetting, ref DataTable ShoppTable,int cjnum)
        {
            if (jrcj == null)
                return;
            JObject ja =(JObject)jrcj[0];
            t_shopping dishinfo = new t_shopping();
            dishinfo.Orderid = CurrOrderInfo.orderid;
            dishinfo.Userid = userid;
            dishinfo.Ordertime = DateTime.Now;
            dishinfo.Orderstatus = 0;
            dishinfo.Dishnum = cjnum;// int.Parse(CurrOrderInfo.custnum.ToString());
            dishinfo.Tableid = CurrTableInfo.tableNo;
            dishinfo.Dishid = ja["dishid"].ToString();
            dishinfo.Avoid = "";
            dishinfo.Dishidleft = 1;
            dishinfo.Title = ja["dishname"].ToString();
            dishinfo.DishType = ja["dishtype"].ToString();
            dishinfo.Dishunit = ja["unit"].ToString();
            dishinfo.Ordertype = 0;
            dishinfo.Memberprice = 0;
            if (CurrOrderInfo.memberno == null)
                CurrOrderInfo.memberno = "";
            bool ismember = CurrOrderInfo.memberno.Length > 0;
            decimal price = 0;
            string pricestr = "";
            pricestr = ja["price"].ToString();
            string vipprice = ja["vipprice"].ToString();
            dishinfo.Memberprice =0 ;
            try
            {
                dishinfo.Memberprice = (decimal)float.Parse(vipprice);
            }
            catch { }
            dishinfo.Price2 = (decimal)float.Parse(pricestr);
            if (dishinfo.Memberprice <= 0)
            {
                dishinfo.Memberprice = dishinfo.Price2;
            }
            if (ismember)
            {
                price = dishinfo.Memberprice;
            }
            else
            {
                price = dishinfo.Price2;
            }
            dishinfo.Price = price;

            dishinfo.Amount = 0;
            dishinfo.Source = ja["source"].ToString();
            t_shopping.add(ref ShoppTable, dishinfo,false);
        }
        public string Level
        {
            get { return _level; }
            set { _level = value; }
        }
    }

}

