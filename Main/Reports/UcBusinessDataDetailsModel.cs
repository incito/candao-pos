using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KYPOS.Reports
{
    public class UcBusinessDataDetailsModel : ViewModelBase
    {
        private string _today;

        private string _yesterday;

        private string _month;

        private string _lastMonth;

        private bool _isToday=false;

        private bool _isYesterday=false;

        private bool _isMonth=false;

        private bool _isLastMonth=false;

        private string _starTime = "开始时间";

        private string _endTime = "结束时间";

        public bool IsToday
        {
            set
            {
                _isToday = value;
                RaisePropertyChanged(() => IsToday);
            }
            get { return _isToday; }
        }
        public bool IsYesterday
        {
            set
            {
                _isYesterday = value;
                RaisePropertyChanged(() => IsYesterday);
            }
            get { return _isYesterday; }
        }
        public bool IsMonth
        {
            set
            {
                _isMonth = value;
                RaisePropertyChanged(() => IsMonth);
            }
            get { return _isMonth; }
        }
        public bool IsLastMonth
        {
            set
            {
                _isLastMonth = value;
                RaisePropertyChanged(() => IsLastMonth);
            }
            get { return _isLastMonth; }
        }
        /// <summary>
        /// 当天日期
        /// </summary>
        public string Today
        {
            set
            {
                _today = value;
                RaisePropertyChanged(() => Today);
            }
            get { return _today; }
        }

        /// <summary>
        /// 昨天
        /// </summary>
        public string Yesterday
        {
            set
            {
                _yesterday = value;
                RaisePropertyChanged(() => Yesterday);
            }
            get { return _yesterday; }
        }

        /// <summary>
        /// 当月
        /// </summary>
        public string Month
        {
            set
            {
                _month = value;
                RaisePropertyChanged(() => Month);
            }
            get { return _month; }
        }

        /// <summary>
        /// 上月
        /// </summary>
        public string LastMonth
        {
            set
            {
                _lastMonth = value;
                RaisePropertyChanged(() => LastMonth);
            }
            get { return _lastMonth; }
        }

        
        public string StarTime
        {
            set
            {
                _starTime = value;
                RaisePropertyChanged(() => StarTime);
            }
            get { return _starTime; }
        }

        public string EndTime
        {
            set
            {
                _endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
            get { return _endTime; }
        }
    }
}
