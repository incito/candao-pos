using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Models;

namespace CanDao.Pos.VIPManage.Models
{
   public class  UcVipRechargeModel:ViewModelBase
   {
       private string _telNum;
       private string _rechargeValue;
       private string _giveValue="0";
       private bool _isCash=true;
       private bool _isBankCard;

       private bool _isUp = true;
       private bool _isDown;

       private bool _isEnabledNum = true;
       public bool IsEnabledNum
       {
           get { return _isEnabledNum; }
           set
           {
               _isEnabledNum = value;
               RaisePropertyChanged(() => IsEnabledNum);
           }
       }

       private string _cardBalance;
       public string CardBalance
       {
           get { return _cardBalance; }
           set
           {
               _cardBalance = value;
               RaisePropertyChanged(() => CardBalance);
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

       public string RechargeValue
       {
           get { return _rechargeValue; }
           set
           {
               _rechargeValue = value;
               RaisePropertyChanged(() => RechargeValue);
           }
       }

       public string GiveValue
       {
           get { return _giveValue; }
           set
           {
               _giveValue = value;
               RaisePropertyChanged(() => GiveValue);
           }
       }

       public bool IsCash
       {
           get { return _isCash; }
           set
           {
               _isCash = value;
               RaisePropertyChanged(() => IsCash);
           }
       }

       public bool IsBankCard
       {
           get { return _isBankCard; }
           set
           {
               _isBankCard = value;
               RaisePropertyChanged(() => IsBankCard);
           }
       }

       public bool IsUp
       {
           get { return _isUp; }
           set
           {
               _isUp = value;
               RaisePropertyChanged(() => IsUp);
           }
       }

       public bool IsDown
       {
           get { return _isDown; }
           set
           {
               _isDown = value;
               RaisePropertyChanged(() => IsDown);
           }
       }

   }
}
