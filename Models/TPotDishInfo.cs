using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models
{
    /// <summary>
    /// 鱼锅类 一个锅，两个鱼
    /// </summary>
    public partial class TPotDishInfo
    {
        private t_shopping potDish;//整个锅
        private t_shopping potInfo;//锅
        private t_shopping fishDishInfo1;//鱼1
        private t_shopping fishDishInfo2;//鱼2

        public t_shopping PotDish
        {
            get { return potDish; }
            set { potDish = value; }
        }
        public t_shopping PotInfo
        {
            get { return potInfo; }
            set { potInfo = value; }
        }
        public t_shopping FishDishInfo1
        {
            get { return fishDishInfo1; }
            set { fishDishInfo1 = value; }
        }

        public t_shopping FishDishInfo2
        {
            get { return fishDishInfo2; }
            set { fishDishInfo2 = value; }
        }
        private string memberno;

        public static TPotDishInfo getPotDishInfo(string memberno,string dishid, JArray groupData)
        {
            TPotDishInfo potDishInfo = new TPotDishInfo();
            JObject ja = (JObject)groupData[0];
            string ispot=ja["ispot"].ToString();
            potDishInfo.potInfo = ParseDishInfo(memberno,ja);
            potDishInfo.potInfo.IsPot = 1;
            potDishInfo.potInfo = ParseDishInfo(memberno, ja);
            potDishInfo.memberno = memberno;
            ja = (JObject)groupData[1];
            potDishInfo.fishDishInfo1 = ParseDishInfo(memberno, ja);
            if (groupData.Count > 2)
            {
                ja = (JObject)groupData[2];
                potDishInfo.fishDishInfo2 = ParseDishInfo(memberno, ja);
            }
            return potDishInfo;
        }
        private static t_shopping ParseDishInfo(string memberno,JObject ja)
        {
            t_shopping dishinfo = new t_shopping();
            dishinfo.Dishid = ja["dishid"].ToString();
            dishinfo.Avoid = "";
            dishinfo.Dishidleft = 1;
            dishinfo.Title = ja["title"].ToString();
            dishinfo.DishType = ja["dishtype"].ToString();
            dishinfo.DishUnitSrc = ja["unit"].ToString();
            dishinfo.Memberprice = 0;
            if (memberno == null)
                memberno = "";
            bool ismember =memberno.Length > 0;
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
            dishinfo.Weigh = int.Parse(ja["weigh"].ToString());
            if (dishinfo.Weigh==1)
            {
                dishinfo.Title = dishinfo.Title + "(称重)";
            }
            return dishinfo;
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
