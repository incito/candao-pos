using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Operates;
using Common;
using WebServiceReference;
using CanDaoCD.Pos.VIPManage.Models;

namespace CanDaoCD.Pos.VIPManage.Operates
{
   /// <summary>
   /// 会员管理类
   /// </summary>
   public class OVipManage
    {
       /// <summary>
       /// 会员查询
       /// </summary>
       /// <param name="selectNum"></param>
       /// <param name="psw"></param>
       /// <param name="Model"></param>
       /// <returns></returns>
       public static bool SelectVipInfo(string selectNum,string psw,ref UcVipSelectModel Model)
       {
        
           var info = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", selectNum, psw);
           if (info.Retcode.Equals("0"))
           {
               Model.UserName = info.Name;
               if (info.Gender == 0)
               {
                   Model.Sex = "男";
               }
               else
               {
                   Model.Sex = "女";
               }
               Model.TelNum = info.Mobile;
               Model.Birthday = info.Birthday;
               Model.Integral = info.Integraloverall.ToString();
               Model.Balance = info.Storecardbalance.ToString();
               Model.CardNum = info.Mcard;

               return true;
           }
           else
           {
               OWindowManage.ShowMessageWindow(
                string.Format("会员查询错误：{0}", info.Retinfo), true);

               return false;
               Model.IsOper = false;//禁用操作区域
           }
       }


    }
}
