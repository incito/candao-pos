using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CanDao.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.VIPManage.Models
{
    /// <summary>
    /// 会员查询模型
    /// </summary>
    public class UcVipSelectModel : ViewModelBase
    {

        #region 字段

        private string _selectNum;
        private int _cardType;

        private string _cardNum;
        private string _telNum;
        private string _userName;

        private string _cardLevel;
        private string _birthday;
        private string _sex;

        private decimal _integral=0;
        private string _balance="0";
        private string _cardState;

        #endregion

        #region 属性
        /// <summary>
        /// 查询卡号
        /// </summary>
        public string SelectNum
        {
            get { return _selectNum; }
            set
            {
                _selectNum = value;
                RaisePropertyChanged(() => SelectNum);
            }
        }

        /// <summary>
        /// 会员卡
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
        /// 电话号码
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
        /// 会员名称
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
        /// 会员等级
        /// </summary>
        public string CardLevel
        {
            get { return _cardLevel; }
            set
            {
                _cardLevel = value;
                RaisePropertyChanged(() => CardLevel);
            }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }
        /// <summary>
        /// 性别(汉字)
        /// </summary>
        public string Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                RaisePropertyChanged(() => Sex);
            }
        }

        /// <summary>
        /// 余额
        /// </summary>
        public string Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                RaisePropertyChanged(() => Balance);
            }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Integral
        {
            get { return _integral; }
            set
            {
                _integral = value;
                RaisePropertyChanged(() => Integral);
            }
        }

        /// <summary>
        /// 卡状态
        /// </summary>
        public string CardState
        {
            get { return _cardState; }
            set
            {
                _cardState = value;
                RaisePropertyChanged(() => CardState);
            }
        }

        /// <summary>
        /// 卡类型{0:虚拟卡，1：实体卡，2：微会员}
        /// </summary>
        public int CardType
        {
            get { return _cardType; }
            set
            {
                _cardType = value;
                RaisePropertyChanged(() => CardType);
            }
        }
        #endregion
    }
}
