using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.VIPManage.Models
{
   public class UcVipModifyPswModel:ViewModelBase
   {
       private string _telNum;
       private string _code;
       private string _psw;
       private string _pswConfirm;

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

   }
}
