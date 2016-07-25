using CanDao.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.SystemConfig.Models
{
    public class UCCashboxModel : ViewModelBase
    {
        private bool _isEnabledCheck;

        /// <summary>
        /// 是否启用验证
        /// </summary>
        public bool IsEnabledCheck
        {
            get { return _isEnabledCheck; }
            set { _isEnabledCheck = value;
            RaisePropertyChanged(() => IsEnabledCheck);
            }
        }

    }
}
