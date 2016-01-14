using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Models
{
    public class TCombo
    {
        int startnum;
        int endnum;
        string status;
        string columnname;
        float ordernum;
        string columnid;
        string id;
        string dishid;
        string itemDesc;
        private ArrayList dishs = new ArrayList();
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Columnname
        {
            get { return columnname; }
            set { columnname = value; }
        }

        public float Ordernum
        {
            get { return ordernum; }
            set { ordernum = value; }
        }

        public string Columnid
        {
            get { return columnid; }
            set { columnid = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Dishid
        {
            get { return dishid; }
            set { dishid = value; }
        }

        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }

        public ArrayList Dishs
        {
            get { return dishs; }
            set { dishs = value; }
        }
        public int Startnum
        {
            get { return startnum; }
            set { startnum = value; }
        }
        public int Endnum
        {
            get { return endnum; }
            set { endnum = value; }
        }

    }
    public class TComboDish
    {
        private t_shopping dishinfo;
        private ArrayList combodishs = new ArrayList();
        private ArrayList onlydishs = new ArrayList();

        public ArrayList Onlydishs
        {
            get { return onlydishs; }
            set { onlydishs = value; }
        }
        public ArrayList Combodishs
        {
            get { return combodishs; }
        }
        public t_shopping Dishinfo
        {
            get { return dishinfo; }
            set { dishinfo = value; }
        }
        public static TComboDish parse(JObject jaData)
        {
            TComboDish combodish = new TComboDish();
            JArray jronly = (JArray)jaData["only"];
            JArray jrcombo = (JArray)jaData["combo"];
            //必选
            for (int i = 0; i <= jronly.Count - 1; i++)
            {
                //如果是单品 dishes
                JObject ja=(JObject)jronly[i];
                string dishes = ja["dishes"].ToString();
                if (dishes == null)
                    dishes = "";
                if(dishes.Length>0)
                {
                    //鱼锅
                    TPotDishInfo potinfo = parseonlyPot(ja);
                    combodish.Onlydishs.Add(potinfo);
                }
                else
                {
                    t_shopping dishinfo = parseonly(ja,false);
                    combodish.Onlydishs.Add(dishinfo);
                }
            }
            //几选几
            for (int i = 0; i <= jrcombo.Count - 1; i++)
            {
                TCombo combo = parsecombo((JObject)jrcombo[i]);
                combodish.Combodishs.Add(combo);
            }
            return combodish;
        }
        public static TCombo parsecombo(JObject ja)
        {
            TCombo combo = new TCombo();
            combo.Startnum = int.Parse(ja["startnum"].ToString());
            combo.Endnum = int.Parse(ja["endnum"].ToString());
            combo.Status = ja["status"].ToString();
            combo.Columnname = ja["columnname"].ToString();
            combo.Ordernum = float.Parse(ja["ordernum"].ToString());
            combo.Columnid = ja["columnid"].ToString();
            combo.Id = ja["id"].ToString();
            combo.Dishid = ja["dishid"].ToString();
            combo.ItemDesc = ja["itemDesc"].ToString();
            JArray jr = (JArray)ja["alldishes"];
            for (int i = 0; i <= jr.Count - 1; i++)
            {
                JObject jacombo = (JObject)jr[i];
                string dishes = jacombo["dishes"].ToString();
                if (dishes == null)
                    dishes = "";
                if (dishes.Length > 0)
                {
                    //鱼锅
                    TPotDishInfo potinfo = parseonlyPot(jacombo);
                    combo.Dishs.Add(potinfo);
                }
                else
                {
                    t_shopping dishinfo = parseonly(jacombo,false);
                    combo.Dishs.Add(dishinfo);
                }
            }
            return combo;
        }
        public static t_shopping parseonly(JObject ja,bool ispot)
        {
            t_shopping dishinfo = new t_shopping();
            dishinfo.Orderid = "";
            dishinfo.Userid = "";// Globals.UserInfo.UserID;
            dishinfo.Ordertime = DateTime.Now;
            dishinfo.Orderstatus = 0;
            dishinfo.Dishnum =float.Parse(ja["dishnum"].ToString());
            dishinfo.Tableid = "";
            if (ispot)
              dishinfo.Dishid = ja["dishid"].ToString();
            else
              dishinfo.Dishid = ja["contactdishid"].ToString();
            dishinfo.Avoid = "";
            dishinfo.Dishidleft = 1;
            dishinfo.Title = ja["contactdishname"].ToString();
            dishinfo.DishType = ja["dishtype"].ToString();
            dishinfo.Dishunit = ja["dishunitid"].ToString();
            dishinfo.Memberprice = 0;
            decimal price = 0;
            string pricestr = "";
            pricestr = ja["vipprice"].ToString(); //可能还会有多单位的问题
            if (pricestr.Equals("") || pricestr.Equals(""))
            {
                pricestr = ja["price"].ToString();
            }
            dishinfo.Memberprice = strtofloat(pricestr);
            pricestr = ja["price"].ToString();
            dishinfo.Price2 = strtofloat(pricestr);
            if (dishinfo.Memberprice <= 0)
            {
                dishinfo.Memberprice = dishinfo.Price2;
            }
            price = dishinfo.Price2;
            dishinfo.Price = price;
            dishinfo.Amount = 0;
            dishinfo.Source = "";
            dishinfo.Groupid = ja["groupid"].ToString();
            dishinfo.Contactdishid = ja["contactdishid"].ToString();
            return dishinfo;
        }
        public static TPotDishInfo parseonlyPot(JObject ja)
        {
            TPotDishInfo potInfo = new TPotDishInfo();
            potInfo.PotDish = parseonly(ja, false);
            JArray jrDishs = (JArray)ja["dishes"];
            for (int i = 0; i <= jrDishs.Count - 1; i++)
            {
                JObject ja1 = (JObject)jrDishs[i];
                if(ja1["ispot"].ToString().Equals("1"))
                {
                    potInfo.PotInfo = parseonly(ja1, false);
                }
                else
                {
                    if(potInfo.FishDishInfo1==null)
                    {
                        potInfo.FishDishInfo1 = parseonly(ja1, false);
                    }
                    else
                    {
                        potInfo.FishDishInfo2 = parseonly(ja1, false);
                        break;
                    }
                }
            }
            return potInfo;
        }
        public static decimal strtofloat(string str)
        {
            decimal re = 0;
            try
            {
                re = decimal.Parse(str);
            }
            catch { re = 0; }
            return re;
        }
    }
}
