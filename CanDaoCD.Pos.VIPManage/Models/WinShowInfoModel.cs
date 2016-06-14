using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.VIPManage.Models
{
    public class WinShowInfoModel : ViewModelBase
    {
        private string _title = "请刷卡";

        private string _loadInfo = "正在读取会员卡信息... ...";

        private string _showInfo;

        private bool _isEnable = false;

        private string _cardNum;


        public string CardNum 
        {
            set
            {
                _cardNum = value;

                RaisePropertyChanged(() => CardNum);
            }
            get { return _cardNum; }
        }


        public string Title
        {
            set
            {
                _title = value;

                RaisePropertyChanged(() => Title);
            }
            get { return _title; }
        }

        public string LoadInfo
        {
            set
            {
                _loadInfo = value;

                RaisePropertyChanged(() => LoadInfo);
            }
            get { return _loadInfo; }
        }

        public string ShowInfo
        {
            set
            {
                _showInfo = value;

                RaisePropertyChanged(() => ShowInfo);
            }
            get { return _showInfo; }
        }

        public bool IsEnableBtn
        {
            set
            {
                _isEnable = value;

                RaisePropertyChanged(() => IsEnableBtn);
            }
            get { return _isEnable; }
        }
    }
}
