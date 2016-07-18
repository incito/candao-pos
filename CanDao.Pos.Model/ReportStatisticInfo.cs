using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 报表统计信息类。
    /// </summary>
    public class ReportStatisticInfo
    {
        public ReportStatisticInfo()
        {
            DataSource = new List<ReportDataBase>();
        }

        /// <summary>
        /// 统计起始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 统计结束时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 分店ID。
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 总金额。
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 当前时间，打印时间。
        /// </summary>
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// 统计数据集合。
        /// </summary>
        public List<ReportDataBase> DataSource { get; set; }

        public void CloneData(ReportStatisticInfo srcData)
        {
            if (srcData == null)
                return;

            StartTime = srcData.StartTime;
            EndTime = srcData.EndTime;
            BranchId = srcData.BranchId;
            TotalAmount = srcData.TotalAmount;
            DataSource.Clear();
            DataSource = srcData.DataSource;
        }
    }
}