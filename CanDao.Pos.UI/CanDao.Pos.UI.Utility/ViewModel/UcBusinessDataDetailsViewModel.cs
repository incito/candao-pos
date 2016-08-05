
using System;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Controls;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.IService;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.Utility.Model;


namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 营业明细表
    /// </summary>
    public class UcBusinessDataDetailsViewModel : ViewModelBase
    {

        #region 字段
        //当前已选择时间Id
        private int _checkId=0;
        #endregion

        #region 属性
        public UcBusinessDataDetailsModel Model { set; get; }

        #endregion

        #region 事件

        public RelayCommand TodayCommand { set; get; }
        public RelayCommand YesterdayCommand { set; get; }
        public RelayCommand MonthCommand { set; get; }
        public RelayCommand LastCommand { set; get; }

        public RelayCommand PrintCommand { set; get; }

        public RelayCommand OpenStarTimeCommand { set; get; }

        public RelayCommand OpenEndTimeCommand { set; get; }
        #endregion

        #region 构造函数

        public UcBusinessDataDetailsViewModel()
        {
            Model = new UcBusinessDataDetailsModel();
            Model.Today = DateTime.Now.ToString("yyyy.MM.dd");
            Model.Yesterday = DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd");
            Model.Month = DateTime.Now.ToString("yyyy.MM");
            Model.LastMonth = DateTime.Now.AddMonths(-1).ToString("yyyy.MM");
            TodayCommand = new RelayCommand(TodayHandel);
            YesterdayCommand=new RelayCommand(YesterdayHandel);
            MonthCommand=new RelayCommand(MonthHandel);
            LastCommand=new RelayCommand(LastHandel);
            OpenStarTimeCommand = new RelayCommand(OpenStarTimeHandel);
            OpenEndTimeCommand=new RelayCommand(OpenEndTimeHandel);

            PrintCommand = new RelayCommand(PrintHandel);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 当天点击时间
        /// </summary>
        private void TodayHandel()
        {
            if (_checkId == 1)
            {
                Model.IsToday = false;
                _checkId = 0;
                Model.StarTime = "开始时间";
                Model.EndTime = "结束时间";
            }
            else
            {
                _checkId = 1;
                Model.StarTime = string.Format("{0} 00:00:00",Model.Today);
                Model.EndTime = string.Format("{0} 23:59:59", Model.Today);
            } 
        }
        /// <summary>
        /// 昨天点击事件
        /// </summary>
        private void YesterdayHandel()
        {
            if (_checkId == 2)
            {
                Model.IsYesterday = false;
                _checkId = 0;
                Model.StarTime = "开始时间";
                Model.EndTime = "结束时间";
            }
            else
            {
                _checkId = 2;
                Model.StarTime = string.Format("{0} 00:00:00", Model.Yesterday);
                Model.EndTime = string.Format("{0} 23:59:59", Model.Yesterday);
            } 
        }
        /// <summary>
        /// 当月点击事件
        /// </summary>
        private void MonthHandel()
        {
            if (_checkId == 3)
            {
                Model.IsMonth = false;
                _checkId = 0;
                Model.StarTime = "开始时间";
                Model.EndTime = "结束时间";
            }
            else
            {
                _checkId = 3;
                Model.StarTime = string.Format("{0}.01 00:00:00", Model.Month);
                Model.EndTime = string.Format("{0}.{1} 23:59:59", Model.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            } 
        }
        /// <summary>
        /// 上月点击事件
        /// </summary>
        private void LastHandel()
        {
            if (_checkId == 4)
            {
                Model.IsLastMonth = false;
                _checkId = 0;
                Model.StarTime = "开始时间";
                Model.EndTime = "结束时间";
            }
            else
            {
                _checkId = 4;
                Model.StarTime = string.Format("{0}.01 00:00:00", Model.LastMonth);
                var time = DateTime.Now.AddMonths(-1);
                Model.EndTime = string.Format("{0}.{1} 23:59:59", Model.LastMonth, DateTime.DaysInMonth(time.Year, time.Month));
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenStarTimeHandel()
        {
            var wTimeSelect=new WTimeSelect();
            DateTime outtemp;
            if (DateTime.TryParse(Model.StarTime, out outtemp))
            {
                wTimeSelect.Init(outtemp);
            }
            wTimeSelect.SelectTimeAction = new Action<DateTime>(StarTimeHandel);
            wTimeSelect.ShowDialog();
        }

        private void StarTimeHandel(DateTime dateTime)
        {
            Model.StarTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenEndTimeHandel()
        {
            var wTimeSelect = new WTimeSelect();
            DateTime outtemp;
            if (DateTime.TryParse(Model.EndTime, out outtemp))
            {
                wTimeSelect.Init(outtemp);
            }
            wTimeSelect.SelectTimeAction = new Action<DateTime>(EndTimeHandel);
            wTimeSelect.ShowDialog();
        }

        private void EndTimeHandel(DateTime dateTime)
        {
            Model.EndTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 打印事件
        /// </summary>
        private void PrintHandel()
        {
            try
            {
                if (Model.StarTime.Equals("开始时间") || Model.EndTime.Equals("结束时间"))
                {
                    OWindowManage.ShowMessageWindow("开始和结束时间不能为空，请检查！", false);
                    return;
                }
                DateTime outsTime;
                DateTime outeTime;
                if (!DateTime.TryParse(Model.StarTime, out outsTime) | !DateTime.TryParse(Model.EndTime, out outeTime))
                {
                    OWindowManage.ShowMessageWindow("打印自定义时间不是时间格式，请检查！", false);
                    return;
                }
                if (outsTime > outeTime)
                {
                    OWindowManage.ShowMessageWindow("自定义时间选择错误：开始时间不能大于结束时间，请检查！", false);
                    return;
                }

                if (!OWindowManage.ShowMessageWindow(string.Format("确定打印{0}至{1}时间段的营业数据明细？", Model.StarTime, Model.EndTime), true))
                {
                    return;
                }
                //var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                //var res2 = service.GetDayReportList(Model.StarTime, Model.EndTime, Globals.UserInfo.UserName);
                if (ReportPrintHelper.PrintBusinessDataDetail(Model.StarTime, Model.EndTime, Globals.UserInfo.UserName))
                {
                    OWindowManage.ShowMessageWindow("打印成功！", false);
                }
                //if (string.IsNullOrEmpty(res2.Item1))
                //{
                //    if (ReportPrintHelper.PrintBusinessDataDetail(res2.Item2))
                //    {
                //        OWindowManage.ShowMessageWindow("打印成功！", false);
                //    }
                      
                //}
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow("打印失败："+ex.Message, false);
            }

        }
        #endregion
    }
}
