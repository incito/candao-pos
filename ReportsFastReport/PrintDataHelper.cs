using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common;
using Models;
using Newtonsoft.Json.Linq;

namespace ReportsFastReport
{
    /// <summary>
    /// 打印数据辅助类。
    /// </summary>
    public class PrintDataHelper
    {
        /// <summary>
        /// 获取订单菜品数据表。
        /// </summary>
        /// <param name="jArray">Json数据集合。</param>
        /// <returns></returns>
        public static DataTable GetOrderListDb(JArray jArray)
        {
            DataTable dt = new DataTable("tb_data");
            dt.Columns.Add(DataTableHelper.CreateDataColumn(typeof(decimal), "金额", "Amount", 0));
            dt.Columns.Add(DataTableHelper.CreateDataColumn(typeof(string), "数量", "DishNumUnit", ""));
            dt.Columns.Add(DataTableHelper.CreateDataColumn(typeof(decimal), "单价", "DishPrice", 0));
            dt.Columns.Add(DataTableHelper.CreateDataColumn(typeof(string), "品项", "DishName", ""));

            var dataList = ToOrderDishPrintInfoes(jArray);
            if (dataList != null)
            {
                dataList.ForEach(t => DataTableHelper.AddObject2DataTable(dt, t));
            }

            return dt;
        }

        private static List<OrderDishPrintInfo> ToOrderDishPrintInfoes(JArray jArray)
        {
            if (jArray == null)
                return null;

            return (from JObject jObject in jArray select ToOrderDishPrintInfo(jObject)).ToList();
        }

        private static OrderDishPrintInfo ToOrderDishPrintInfo(JObject dataJObj)
        {
            if (dataJObj == null)
                return null;

            var num = GetJObjectDecimal(dataJObj, "dishnum");
            var unit = GetJObjectString(dataJObj, "dishunit");
            unit = InternationaHelper.FilterSeparatorFlag(unit);
            var dishName = GetJObjectString(dataJObj, "title");
            dishName = InternationaHelper.FilterSeparatorFlag(dishName);

            var item = new OrderDishPrintInfo
            {
                DishName = dishName,
                Amount = GetJObjectDecimal(dataJObj, "amount"),
                DishNumUnit = string.Format("{0}{1}", num, unit),
                DishPrice = GetJObjectDecimal(dataJObj, "orderprice")
            };
            return item;
        }

        private static string GetJObjectString(JObject jObj, string key, string defaultValue = "")
        {
            var temp = jObj[key];
            return temp != null ? temp.ToString() : defaultValue;
        }

        private static int GetJObjectInt(JObject jObj, string key, int defaultValue = 0)
        {
            var temp = jObj[key];
            if (temp != null)
            {
                try
                {
                    var value = Convert.ToInt32(temp.ToString());
                    return value;
                }
                catch (Exception ex)
                {
                    AllLog.Instance.E(ex);
                }
            }
            return defaultValue;
        }

        private static Decimal GetJObjectDecimal(JObject jObj, string key, decimal defaultValue = 0)
        {
            var temp = jObj[key];
            if (temp != null)
            {
                try
                {
                    var value = Convert.ToDecimal(temp.ToString());
                    return value;
                }
                catch (Exception ex)
                {
                    AllLog.Instance.E(ex);
                }
            }
            return defaultValue;
        }
    }
}