using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.VIPManage.Models
{
    public class UcVipRegModel : ViewModelBase
    {

        #region 字段

        private string _telNum;
        private string _code;
        private string _userName;
        private string _psw;
        private string _pswConfirm;
        private string _cardNum;
        private DateTime _birthday=new DateTime(1990,1,1);

        private bool _isShowCardNum=false;
        private bool _isShowCardBut = true;
        #endregion

        #region 属性
        public bool IsShowCardNum
        {
            get { return _isShowCardNum; }
            set
            {
                _isShowCardNum = value;
                RaisePropertyChanged(() => IsShowCardNum);
            }
        }
        public bool IsShowCardBut
        {
            get { return _isShowCardBut; }
            set
            {
                _isShowCardBut = value;
                RaisePropertyChanged(() => IsShowCardBut);
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
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged(() => Code);
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

        public string PswConfirm
        {
            get { return _pswConfirm; }
            set
            {
                _pswConfirm = value;
                RaisePropertyChanged(() => PswConfirm);
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

        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }
        #endregion

        #region 构造函数

        #endregion

    }
}
