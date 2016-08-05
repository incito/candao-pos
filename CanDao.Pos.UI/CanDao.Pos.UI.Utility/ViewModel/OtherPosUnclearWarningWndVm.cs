using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class OtherPosUnclearWarningWndVm : NormalWindowViewModel
    {
        public ICommand RetryCmd { get; private set; }

        public ICommand AppShutDownCmd { get; private set; }

        private void Retry(object arg)
        {
            TaskService.Start(null, GetUnclearPosInfoProcess, GetUnclearPosInfoComplete, "获取所有未清机信息...");
        }

        private void ApplicationShutDown(object arg)
        {
            if (OwnerWindow.DialogResult != true)
                Application.Current.Shutdown();
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            RetryCmd = CreateDelegateCommand(Retry);
            AppShutDownCmd = CreateDelegateCommand(ApplicationShutDown);
        }

        private object GetUnclearPosInfoProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            return service == null
                ? new Tuple<string, List<UnclearMachineInfo>>("创建IRestaurantService服务失败。", null)
                : service.GetUnclearnPosInfo();
        }

        private void GetUnclearPosInfoComplete(object arg)
        {
            var result = (Tuple<string, List<UnclearMachineInfo>>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1);
                return;
            }

            if (result.Item2.Any()) //这里只需要判断有未清机的就不关闭窗口。
                return;

            TaskService.Start(null, CheckIsOpenProcess, CheckIsOpenComplete, "检测是否已结业...");
        }

        /// <summary>
        /// 检测今日是否开业的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CheckIsOpenProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IRestaurantService服务接口失败！");
                return new Tuple<string, bool>("创建IRestaurantOpeningService服务接口失败！", false);
            }

            return service.CheckRestaurantOpened();
        }

        /// <summary>
        /// 检测今日是否开业完成时执行。
        /// </summary>
        /// <param name="arg"></param>
        private void CheckIsOpenComplete(object arg)
        {
            var result = (Tuple<string, bool>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            if (!result.Item2) //没有开业则说明结业成功。
            {
                MessageDialog.Warning("其他机器已经完成结业，点击\"确定\"关闭程序。");
                Application.Current.Shutdown();
            }

            CloseWindow(true);
        }
    }
}