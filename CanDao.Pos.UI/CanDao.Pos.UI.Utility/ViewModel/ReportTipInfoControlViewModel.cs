using System;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.ReportPrint;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 小费报表控件ViewModel。
    /// </summary>
    public class ReportTipInfoControlViewModel : ReportControlViewModelBase
    {
        protected override object GetReportStatisticInfoProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, ReportStatisticInfo>("创建IRestaurantService服务失败。", null);

            return service.GetReportTipInfo((EnumStatisticsPeriodsType)arg);
        }

        protected override void PrintReport()
        {
            ReportPrintHelper.PrintTipInfoStatisticReport(Data);
        }
    }
}