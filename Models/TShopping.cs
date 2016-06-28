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
        private string _dishunit;
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
        private string _dishUnitSrc;

        private string _selectedTaste = string.Empty;//口味
        private string _freeuser = string.Empty;//赠菜人
        private string _freeauthorize = string.Empty;//赠菜授权人
        private string _freereason = string.Empty;//赠菜原因

        /// <summary>
        /// 设定的口味
        /// </summary>
        public string SelectedTaste
        {
            get { return _selectedTaste; }
            set
            {
                _selectedTaste = value;
                Title = GenerateDishName(DishName, value, Avoid);
            }
        }

        /// <summary>
        /// 赠菜人
        /// </summary>
        public string Freeuser
        {
            get { return _freeuser; }
            set { _freeuser = value; }
        }
        /// <summary>
        /// 赠菜授权人
        /// </summary>
        public string Freeauthorize
        {
            get { return _freeauthorize; }
            set { _freeauthorize = value; }
        }
        /// <summary>
        /// 赠菜原因
        /// </summary>
        public string Freereason
        {
            get { return _freereason; }
            set { _freereason = value; }
        }

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

        /// <summary>
        /// 单位。
        /// </summary>
        public string Dishunit { get; private set; }

        /// <summary>
        /// 原始单位（中英文国际化后单位只显示中文）
        /// </summary>
        public string DishUnitSrc
        {
            get { return _dishUnitSrc; }
            set
            {
                _dishUnitSrc = value;
                Dishunit = InternationaHelper.GetBeforeSeparatorFlagData(value);
            }
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
            set
            {
                _avoid = value;
                Title = GenerateDishName(DishName, SelectedTaste, value);
            }
        }

        public string PrimaryKey { get; set; }

        /// <summary>
        /// 菜品+口味+忌口。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 口味。
        /// </summary>
        public string Taste { get; set; }

        #endregion Model

        /// <summary>
        /// 生成菜名。
        /// </summary>
        /// <returns></returns>
        private static string GenerateDishName(string name, string taste, string diet)
        {
            if (!string.IsNullOrEmpty(taste))
                name += string.Format("({0})", taste);
            if (!string.IsNullOrEmpty(diet))
                name += string.Format("({0})", diet);

            return name;
        }

        public static void createShoppTable(ref DataTable shopptable)
        {
            DataTable tbyh = new DataTable();
            tbyh.Columns.Clear();
            var column = DataTableHelper.CreateDataColumn(typeof(string), "账单号", "orderid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "标示符", "primarykey", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "下单员工", "userid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(DateTime), "下单时间", "ordertime", DateTime.MinValue);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(int), "状态", "orderstatus", 0);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(double), "数量", "dishnum", 0d);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "桌号", "tableid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "菜品编号", "dishid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "忌口", "avoid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(int), "编号", "dishidleft", 1);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "菜品名称", "title", "");
            tbyh.Columns.Add(column);

            tbyh.Columns.Add(DataTableHelper.CreateDataColumn(typeof(string), "菜品原名", "dishName", ""));

            column = DataTableHelper.CreateDataColumn(typeof(string), "单位", "dishunit", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "原始单位", "dishunitSrc", "");//中英文国际化的原始单位。
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(double), "会员价", "memberprice", 0d);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(double), "单价", "price", 0d);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(double), "单价2", "price2", 0d);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(double), "金额", "amount", 0d);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "source", "source", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "parentdishid", "parentdishid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "groupid", "groupid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "ispot", "ispot", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(int), "ordertype", "ordertype", 1);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "Groupid2", "Groupid2", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(int), "weigh", "weigh", 1);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(int), "primarydishtype", "primarydishtype", 1);
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "口味", "taste", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "赠菜人", "freeuser", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "赠菜授权人", "freeauthorize", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "赠菜原因", "freereason", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "parentkey", "parentkey", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "relatedishid", "relatedishid", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "ismaster", "ismaster", "");
            tbyh.Columns.Add(column);

            column = DataTableHelper.CreateDataColumn(typeof(string), "childdishtype", "childdishtype", "");
            tbyh.Columns.Add(column);

            shopptable = tbyh;
        }

        /// <summary>
        /// 往购物车内增加一行数据
        /// </summary>
        /// <param name="dishrow"></param>
        public static void add(ref DataTable shopptable, t_shopping dishrow, bool isdish)
        {
            //如果dishid和单位相同就相加
            int i = 0;
            if (!isdish)
            {
                //如果是鱼锅不能加一起 套餐
                foreach (DataRow dr2 in shopptable.Rows)
                {
                    string dishid = dr2["dishid"].ToString();
                    string dishunit = dr2["dishunit"].ToString();
                    string primarydishtype = dr2["primarydishtype"].ToString();
                    string dishName = dr2["title"].ToString();

                    if ((dishid.Equals(dishrow.Dishid)) && (dishunit.Equals(dishrow.Dishunit)) &&
                        (primarydishtype.Equals(dishrow.Primarydishtype.ToString())))
                    {
                        if (dishrow.Title.Contains("临时菜"))//临时菜
                        {
                            if (dishName.Equals(dishrow.Title))//临时菜名称一样
                            {
                                float dishnum = float.Parse(dr2["dishnum"].ToString());
                                var countNum = dishnum + dishrow.Dishnum;
                                decimal oldAmount = decimal.Parse(dr2["amount"].ToString());

                                dr2["price"] = dishrow.Price;
                                dr2["dishnum"] = countNum;
                                dr2["amount"] = oldAmount + dishrow.Amount;//本次+上次金额
                                return;
                            }
                            else
                            {
                                //名称不一样时，当成新的菜新加入一列
                            }
                        }
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
            dr["dishnum"] = (decimal) Math.Round(dishrow.Dishnum, 2);
            dr["tableid"] = dishrow.Tableid;
            dr["dishid"] = dishrow.Dishid;
            dr["avoid"] = dishrow.Avoid;
            dr["dishidleft"] = dishrow.Dishidleft;
            dr["title"] = dishrow.Title;
            dr["dishName"] = dishrow.DishName;
            dr["dishunit"] = dishrow.Dishunit;
            dr["dishunitSrc"] = dishrow.DishUnitSrc;
            dr["memberprice"] = dishrow.Memberprice;
            dr["price"] = dishrow.Price;
            dr["price2"] = dishrow.Price2;
            dr["ordertype"] = dishrow.Ordertype;
            dr["amount"] = dishrow.Price*(decimal) dishrow.Dishnum;
            dr["source"] = dishrow.Source;
            dr["weigh"] = dishrow.Weigh; //dishstatus 如果是称重下单为1
            dr["primarydishtype"] = dishrow.Primarydishtype;

            dr["ispot"] = dishrow.IsPot.ToString();
            dr["Parentdishid"] = dishrow.Parentdishid ?? "";
            dr["groupid"] = dishrow.Groupid ?? "";
            dr["Groupid2"] = dishrow.Groupid2 ?? "";
            dr["taste"] = dishrow.SelectedTaste ?? "";
            dr["avoid"] = dishrow.Avoid ?? "";
            dr["freeuser"] = dishrow.Freeuser ?? "";
            dr["freeauthorize"] = dishrow.Freeauthorize ?? "";
            dr["freereason"] = dishrow.Freereason ?? "";

            dr["taste"] = dishrow.Taste;
            dr["freeuser"] = dishrow.Freeuser;
            dr["freeauthorize"] = dishrow.Freeauthorize;
            dr["freereason"] = dishrow.Freereason;


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
            if (dishnum <= 0)
            {
                shopptable.Rows.Remove(dr);
                return;
            }
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

        public static void EditDishDietAndNum(DataRow dr, decimal dishNum, string dishDiet)
        {
            decimal price = decimal.Parse(dr["price"].ToString());
            dr["dishnum"] = dishNum;
            dr["amount"] = dishNum * price;
            dr["avoid"] = dishDiet;
            var name = dr["dishName"].ToString();
            var taste = dr["taste"].ToString();
            dr["title"] = GenerateDishName(name, taste, dishDiet);
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
                if (dishnum <= 0)
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
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
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
        public static void addCJ(JArray jrcj, t_table CurrTableInfo, t_order CurrOrderInfo, string userid, TSetting cjSetting, ref DataTable ShoppTable, int cjnum)
        {
            if (jrcj == null)
                return;
            JObject ja = (JObject)jrcj[0];
            t_shopping dishinfo = new t_shopping();
            dishinfo.Orderid = CurrOrderInfo.orderid;
            dishinfo.PrimaryKey = Guid.NewGuid().ToString();
            dishinfo.Userid = userid;
            dishinfo.Ordertime = DateTime.Now;
            dishinfo.Orderstatus = 0;
            dishinfo.Dishnum = cjnum;// int.Parse(CurrOrderInfo.custnum.ToString());
            dishinfo.Tableid = CurrTableInfo.tableNo;
            dishinfo.Dishid = ja["dishid"].ToString();
            dishinfo.Avoid = "";
            dishinfo.Dishidleft = 1;
            dishinfo.Title = ja["dishname"].ToString();
            dishinfo.DishName = ja["dishname"].ToString();
            dishinfo.DishType = ja["dishtype"].ToString();
            dishinfo.DishUnitSrc = ja["unit"].ToString();
            dishinfo.Ordertype = 0;
            dishinfo.Memberprice = 0;
            if (CurrOrderInfo.memberno == null)
                CurrOrderInfo.memberno = "";
            bool ismember = CurrOrderInfo.memberno.Length > 0;
            decimal price = 0;
            string pricestr = "";
            pricestr = ja["price"].ToString();
            string vipprice = ja["vipprice"].ToString();
            dishinfo.Memberprice = 0;
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
            t_shopping.add(ref ShoppTable, dishinfo, false);
        }
        public string Level
        {
            get { return _level; }
            set { _level = value; }
        }
    }

}

