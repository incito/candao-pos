using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.VIPManage.Models
{
    /// <summary>
    /// 修改会员信息
    /// </summary>
    public class UcVipModifyTelNumModel : ViewModelBase
    {
        private string _telNum;
        private string _code;
        private string _nTelNum;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelNum
        {
            get { return _telNum; }
            set
            {
                _telNum = value;
                RaisePropertyChanged(() => TelNum);
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged(() => Code);
            }
        }
        /// <summary>
        /// 新手机号码
        /// </summary>

        public string NTelNum
        {
            get { return _nTelNum; }
            set
            {
                _nTelNum = value;
                RaisePropertyChanged(() => NTelNum);
            }
        }
    }
}
