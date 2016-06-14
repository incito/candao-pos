using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.SystemConfig.Models
{
   
   public class UcCopyPrintModel:ViewModelBase
   {
       private int _serialNum=1;

       private bool _isEnabledPrint;
       [NonSerialized]
       private Action _saveAction;

       /// <summary>
       /// 串口号
       /// </summary>
       public int SerialNum
       {
           set
           {
               _serialNum = value;

               RaisePropertyChanged(() => SerialNum);
           }
           get
           {
               return _serialNum;
               
           }
       }
       /// <summary>
       /// 是否启用打印
       /// </summary>
       public bool IsEnabledPrint
       {
           set
           {
               _isEnabledPrint = value;
               RaisePropertyChanged(() => IsEnabledPrint);
           }
           get
           {
               return _isEnabledPrint;

           }
       }

       /// <summary>
       /// 保存
       /// </summary>

       public Action SaveAction
       {
           set
           {
               _saveAction = value;

               RaisePropertyChanged(() => SaveAction);
           }
           get
           {
               return _saveAction;

           }
       }
   }
}
