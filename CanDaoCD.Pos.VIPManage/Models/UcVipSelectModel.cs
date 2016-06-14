using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CanDaoCD.Pos.Common.Classes.Mvvms;

namespace CanDaoCD.Pos.VIPManage.Models
{
    public class UcVipSelectModel : ViewModelBase
    {
        #region 字段

        private string _telNum;
        private string _userName;
        private string _psw;
        private string _sex;
        private string _integral;
        private string _cardNum;
        private string _birthday;
        private string _balance;

        private bool _isEnabledPsw = false;
        private bool _isOper = false;

        private Action _sureAction;

        private Action<TextBox> _textEnterAction;
        #endregion

        #region 属性
        public Action<TextBox> TextEnterAction
        {
            get { return _textEnterAction; }
            set
            {
                _textEnterAction = value;
                RaisePropertyChanged(() => TextEnterAction);
            }
        }

        public Action SureAction
        {
            get { return _sureAction; }
            set
            {
                _sureAction = value;
                RaisePropertyChanged(() => SureAction);
            }
        }

        public string Integral
        {
            get { return _integral; }
            set
            {
                _integral = value;
                RaisePropertyChanged(() => Integral);
            }
        }

        public string Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                RaisePropertyChanged(() => Balance);
            }
        }
        public string Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                RaisePropertyChanged(() => Sex);
            }
        }

        public bool IsEnabledPsw
        {
            get { return _isEnabledPsw; }
            set
            {
                _isEnabledPsw = value;
                RaisePropertyChanged(() => IsEnabledPsw);
            }
        }
      
        public string TelNum
        {
            get { return _telNum; }
            set
            {
                _telNum = value;
                RaisePropertyChanged(() => TelNum);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public string Psw
        {
            get { return _psw; }
            set
            {
                _psw = value;
                RaisePropertyChanged(() => Psw);
            }
        }


        public string CardNum
        {
            get { return _cardNum; }
            set
            {
                _cardNum = value;
                RaisePropertyChanged(() => CardNum);
            }
        }

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }

        public bool IsOper
        {
            get { return _isOper; }
            set
            {
                _isOper = value;
                RaisePropertyChanged(() => IsOper);
            }
        }
      
        #endregion
    }
}
