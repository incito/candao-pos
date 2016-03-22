using System;
using System.Data;

namespace Models
{
    /// <summary>
    /// DataTable辅助类。
    /// </summary>
    public class DataTableHelper
    {
        /// <summary>
        /// 创建一列。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caption"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DataColumn CreateDataColumn(Type type, string caption, string columnName, object defaultValue)
        {
            return new DataColumn(columnName, type)
            {
                AllowDBNull = false,
                Caption = caption,
                DefaultValue = defaultValue
            };
        }

        /// <summary>
        /// 添加一个对象到DataTable表。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="data"></param>
        public static void AddObject2DataTable(DataTable db, object data)
        {
            if (db == null || data == null)
                return;

            try
            {
                DataRow dr = db.NewRow();
                var ppies = data.GetType().GetProperties();
                foreach (var ppy in ppies)
                {
                    if (dr[ppy.Name] == null)
                        continue;

                    dr[ppy.Name] = ppy.GetValue(data, null);
                }

                db.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E("添加对象到DataTable时异常。", ex);
            }
        }
 
    }
}