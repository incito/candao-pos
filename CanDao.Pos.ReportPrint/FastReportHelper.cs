using System;
using System.Data;
using System.IO;
using CanDao.Pos.Common;
using FastReport;

namespace CanDao.Pos.ReportPrint
{
    /// <summary>
    /// 打印辅助类。
    /// </summary>
    public class FastReportHelper
    {
        /// <summary>
        /// 打印。
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="dataSet"></param>
        /// <param name="printCount"></param>
        /// <returns></returns>
        public static bool Print(string templateFile, DataSet dataSet, int printCount = 1)
        {
            if (string.IsNullOrEmpty(templateFile))
                throw new ArgumentNullException("templateFile");

            if (!File.Exists(templateFile))
            {
                ErrLog.Instance.E("模板文件\"{0}\"不存在。", templateFile);
                return false;
            }

            var report = InitFastReport(dataSet);
            report.Load(templateFile);

            do
            {
                report.Prepare();
                report.Print();
            } while (printCount-- > 1);

            return true;
        }

        /// <summary>
        /// 预览。
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool Preview(string templateFile, DataSet dataSet)
        {
            if (string.IsNullOrEmpty(templateFile))
                throw new ArgumentNullException("templateFile");

            if (!File.Exists(templateFile))
            {
                ErrLog.Instance.E("模板文件\"{0}\"不存在。", templateFile);
                return false;
            }

            var report = InitFastReport(dataSet);
            report.Load(templateFile);

            report.Prepare();
            report.Show();
            return true;
        }

        /// <summary>
        /// 设计。
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool Design(string templateFile, DataSet dataSet)
        {
            var report = InitFastReport(dataSet);
            if (!string.IsNullOrEmpty(templateFile) && File.Exists(templateFile))
                report.Load(templateFile);

            report.Prepare();
            report.Design();
            return true;
        }

        /// <summary>
        /// 初始化打印控件。
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private static Report InitFastReport(DataSet dataSet)
        {
            var report = new Report();
            report.PrintSettings.ShowDialog = false;
            var es = new EnvironmentSettings { ReportSettings = { ShowProgress = false } };

            foreach (DataTable dataTable in dataSet.Tables)
            {
                report.RegisterData(dataTable, dataTable.TableName);
                report.GetDataSource(dataTable.TableName).Enabled = true;
                report.GetDataSource(dataTable.TableName).Init();
            }

            return report;
        }
    }
}