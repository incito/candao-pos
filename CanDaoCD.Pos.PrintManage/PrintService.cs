using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.Common.PublicValues;
using CanDaoCD.Pos.PrintManage.Operates;

namespace CanDaoCD.Pos.PrintManage
{
   public class PrintService
   {
       private static OPrintManage _printManage;

       /// <summary>
       /// 打印数据
       /// </summary>
       private static List<string> model;
       static PrintService()
       {
           _printManage=new OPrintManage();
           if (PvSystemConfig.VSystemConfig.IsEnabledPrint)
           {
               _printManage.Init(PvSystemConfig.VSystemConfig.SerialNum);
           }
       }

       public static bool CheckPrintSate()
       {
           return _printManage.TestConnect(PvSystemConfig.VSystemConfig.SerialNum);
       }
       /// <summary>
       /// 卡状态检查
       /// </summary>
       /// <param name="printList"></param>
       public static bool CardCheck()
       {
           if (PvSystemConfig.VSystemConfig.IsEnabledPrint)
           {
               if (!_printManage.CardState())
               {
                   if (OWindowManage.ShowMessageWindow("请先将复写卡插入到复写卡打印机，再尝试。", true))
                   {
                       CardCheck();
                   }
                   else
                   {
                       return false;
                   }
               }
               return true;
           }
           else
           {
               return true;
           }
         
       }

       /// <summary>
       /// 注册打印
       /// </summary>
       public static void RegPrint(string vipName, string vipNum, Action<int> workAction)
       {
           if (PvSystemConfig.VSystemConfig.IsEnabledPrint)
           {
               model = new List<string>();
               model.Add(vipNum);
               model.Add(vipName);

               model.Add("0.00");
               model.Add("0.00元");

               model.Add("0.00");
               model.Add("0.00元");

               model.Add("0.00");
               model.Add("0.00元");

               AsyncLoadServer asyncLoadServer = new AsyncLoadServer();
               asyncLoadServer.Init();
               asyncLoadServer.ActionWorkerState = new Action<int>(workAction);
               asyncLoadServer.Start(PatPrintR);
               asyncLoadServer.SetMessage("正在打印复写卡，请稍等... ...");
            
           }
           else
           {
               workAction(0);
           }
       }

      
       /// <summary>
       /// 消费打印
       /// </summary>
       public static void PayPrint(string vipName, string vipNum, string lastRecharge, string recharge, string nowRecharge, string oldIntegral, string integral, string nowIntegral)
       {


           if (PvSystemConfig.VSystemConfig.IsEnabledPrint)
           {
               model = new List<string>();
               model.Add(vipNum);
               model.Add(vipName);

               model.Add(string.Format("{0}", oldIntegral));
               model.Add(string.Format("{0}元", lastRecharge));

               model.Add(string.Format("{0}", integral));
               model.Add(string.Format("{0}元", recharge));

               model.Add(string.Format("{0}", nowIntegral));
               model.Add(string.Format("{0}元", nowRecharge));


               AsyncLoadServer asyncLoadServer = new AsyncLoadServer();
               asyncLoadServer.Init();
               asyncLoadServer.Start(PatPrintR);
               asyncLoadServer.SetMessage("正在打印复写卡，请稍等... ...");
            
           }
       }

       /// <summary>
       /// 打印
       /// </summary>
       public static void PatPrintR()
       {
           if (!_printManage.Print(CreadModel(model)))
           {
               //OWindowManage.ShowMessageWindow("打印失败:" + _printManage.ErrorString, false);
           }
       }

       /// <summary>
       /// 充值打印
       /// </summary>
       public static void RechargePrint(string vipName, string vipNum, string lastRecharge, string recharge, string nowRecharge, string oldIntegral,Action<int> workAction)
       {
           if (PvSystemConfig.VSystemConfig.IsEnabledPrint)
           {
               model = new List<string>();
               model.Add(vipNum);
               model.Add(vipName);

               model.Add(string.Format("{0}", oldIntegral));
               model.Add(string.Format("{0}元", lastRecharge));

               model.Add("0");
               model.Add(string.Format("{0}元", recharge));

               model.Add(string.Format("{0}", oldIntegral));
               model.Add(string.Format("{0}元", nowRecharge));

               AsyncLoadServer asyncLoadServer = new AsyncLoadServer();
               asyncLoadServer.Init();
               asyncLoadServer.ActionWorkerState = workAction;
               asyncLoadServer.Start(PatPrintR);
               asyncLoadServer.SetMessage("正在打印复写卡，请稍等... ...");
              
           }
           else
           {
               workAction(0);
           }
       }
       /// <summary>
       /// 创建通用打印模板
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       private static List<string> CreadModel(List<string> model)
       {

           var cModel = new List<string>();
           cModel.Add(string.Format("开卡门店：{0}", WebServiceReference.WebServiceReference.Report_title));
           cModel.Add(string.Format("卡片类型：{0}", "储值卡"));
           cModel.Add(string.Format("手机号码：{0}", model[0]));
           cModel.Add(string.Format("会员姓名：{0}", model[1]));
           if (true)
           {
               cModel.Add(string.Format("上次积分余额：{0}", model[2]));
           }
           cModel.Add(string.Format("上次储值余额：{0}", model[3]));
           if (true)
           {
               cModel.Add(string.Format("本次积分增减：{0}", model[4]));
           }
           cModel.Add(string.Format("本次储值增减：{0}", model[5]));
           if (true)
           {
               cModel.Add(string.Format("当前积分余额：{0}", model[6]));
           }
           cModel.Add(string.Format("当前储值余额：{0}", model[7]));

           cModel.Add(string.Format("当前时间：{0}", DateTime.Today.ToString("yyyy-MM-dd")));
           cModel.Add(string.Format(""));
           cModel.Add(string.Format("温馨提示"));
           cModel.Add(string.Format("消费时必须出示本卡"));
           return cModel;
       }
   }
}
