using System;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.ReportPrint;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 品项销售信息报表ViewModel。
    /// </summary>
    public class ReportDishInfoControlViewModel : ReportControlViewModelBase
    {
        protected override object GetReportStatisticInfoProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, ReportStatisticInfo>("创建IRestaurantService服务失败。", null);

            return service.GetReportDishInfo((EnumStatisticsPeriodsType)arg);
        }

        protected override void PrintReport()
        {
            ReportPrintHelper.PrintDishInfoStatisticReport((int)StatisticsPeriodsType);
        }
    }
}