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
    public class UcVipModifyVipInfoModel : ViewModelBase
    {
      

        private string _cardNum;
        private string _telNum;
        private string _oUserName;

        private string _userName;
        private bool _sexNan = true;
        private bool _sexNv = false;
        private DateTime _birthday = new DateTime(1990, 1, 1);

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNum
        {
            get { return _cardNum; }
            set
            {
                _cardNum = value;
                RaisePropertyChanged(() => CardNum);
            }
        }

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
        /// 旧用户名
        /// </summary>
        public string OUserName
        {
            get { return _oUserName; }
            set
            {
                _oUserName = value;
                RaisePropertyChanged(() => OUserName);
            }
        }


        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        /// <summary>
        /// 男
        /// </summary>
        public bool SexNan
        {
            get { return _sexNan; }
            set
            {
                _sexNan = value;
                RaisePropertyChanged(() => SexNan);
            }
        }

        /// <summary>
        /// 女
        /// </summary>
        public bool SexNv
        {
            get { return _sexNv; }
            set
            {
                _sexNv = value;
                RaisePropertyChanged(() => SexNv);
            }
        }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }
    }
}
