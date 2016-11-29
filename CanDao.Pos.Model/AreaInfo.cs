using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 分区信息。
    /// </summary>
    public class AreaInfo
    {
        public AreaInfo()
        {
            TableInfos = new ObservableCollection<TableInfo>();
        }

        /// <summary>
        /// 排序序号。
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 分区名称。
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 餐台集合。
        /// </summary>
        public ObservableCollection<TableInfo> TableInfos { get; set; }

        public void CloneData(AreaInfo item)
        {
            SortIndex = item.SortIndex;
            AreaName = item.AreaName;
        }

        /// <summary>
        /// 简单信息克隆。
        /// </summary>
        /// <returns></returns>
        public AreaInfo SimpleClone()
        {
            return new AreaInfo
            {
                AreaName = AreaName,
                SortIndex = SortIndex,
            };
        }

        /// <summary>
        /// 全部信息克隆。
        /// </summary>
        /// <returns></returns>
        public AreaInfo Clone()
        {
            var item = new AreaInfo
            {
                SortIndex = SortIndex,
                AreaName = AreaName
            };
            foreach (var tableInfo in TableInfos)
            {
                item.TableInfos.Add(tableInfo.Clone());
            }
            return item;
        }
    }
}