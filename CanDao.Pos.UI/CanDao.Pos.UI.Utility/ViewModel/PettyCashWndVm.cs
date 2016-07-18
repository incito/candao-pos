using System;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Service;
using CanDao.Pos.IService;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class PettyCashWndVm : NormalWindowViewModel
    {
        private string _amount;

        /// <summary>
        /// 金额。
        /// </summary>
        public string Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertiesChanged("Amount");
            }
        }

        protected override void Confirm(object param)
        {
            if(!MessageDialog.Quest("确定输入零找金：" + Amount))
                return;

            var info = new Tuple<string, decimal>(Globals.UserInfo.UserName, Convert.ToDecimal(Amount));
            TaskService.Start(info, InputPettyCashProcess, InputPettyCashComplete, "输入零找金中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(Amount);
        }

        /// <summary>
        /// 输入零找金执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object InputPettyCashProcess(object param)
        {
            var info = (Tuple<string, decimal>)param;
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            return service.InputPettyCash(info.Item1, info.Item2);
        }

        /// <summary>
        /// 输入零找金执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void InputPettyCashComplete(object param)
        {
            var result = param as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            CloseWindow(true);
        }
    }
}