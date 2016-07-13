using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            var dishName = GetJObjectString(dataJObj, "title");
            dishName = GetDishNameWithUnit(dishName, unit);

            var priceType = Convert.ToInt32(dataJObj["pricetype"]);
            if (priceType == 1) //1是赠菜。
                dishName += "(赠)";

            var item = new OrderDishPrintInfo
            {
                DishName = dishName,
                Amount = GetJObjectDecimal(dataJObj, "amount"),
                DishNumUnit = num.ToString(CultureInfo.InvariantCulture),
                DishPrice = GetJObjectDecimal(dataJObj, "orderprice")
            };
            return item;
        }

        /// <summary>
        /// 组合菜品和单位。
        /// </summary>
        /// <param name="dishName">菜名。</param>
        /// <param name="dishUnit">菜单位。</param>
        /// <returns></returns>
        private static string GetDishNameWithUnit(string dishName, string dishUnit)
        {
            if (!InternationaHelper.HasInternationaFlag(dishName))
                return string.Format("{0}({1})", dishName, dishUnit);

            var names = InternationaHelper.SplitBySeparatorFlag(dishName);
            var units = InternationaHelper.SplitBySeparatorFlag(dishUnit);

            var result = string.Format("{0}({1})", names[0], units[0]);
            if (names.Count > 1)
            {
                var result2 = string.Format("{0}({1})", names[1], units.Count > 1 ? units[1] : units[0]);
                result = string.Format("{0}\n{1}", result, result2);
            }
            return result;
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