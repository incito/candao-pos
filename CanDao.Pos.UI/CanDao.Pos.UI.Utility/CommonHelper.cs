using System;
using System.Linq;
using System.Threading;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.Utility.View;
using JunLan.Common.Base;

namespace CanDao.Pos.UI.Utility
{
    /// <summary>
    /// 公共辅助类。
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 是否是强制结业清机。
        /// </summary>
        private static bool _isInForcedEndWorkModel;

        private static bool _clearPosSuccess = false;

        private static AutoResetEvent _arEvent = new AutoResetEvent(false);

        /// <summary>
        /// 广播消息。
        /// </summary>
        /// <param name="type">广播消息类型。</param>
        /// <param name="msg">广播消息。</param>
        /// <returns></returns>
        public static string BroadcastMessage(EnumBroadcastMsgType type, string msg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            return service == null ? "创建IRestaurantService服务失败。" : service.BroadcastMessage((int)type, msg);
        }

        /// <summary>
        /// 清理所有POS机。都清理成功返回true，否则返回false。
        /// </summary>
        /// <param name="isInForcedEndWorkModel">是否是强制结业清机。</param>
        /// <returns></returns>
        public static bool ClearAllPos(bool isInForcedEndWorkModel)
        {
            _isInForcedEndWorkModel = isInForcedEndWorkModel;
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                MessageDialog.Warning("创建IRestaurantService服务失败。");
                return false;
            }

            var result = service.GetUnclearnPosInfo();
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1);
                return false;
            }

            if (result.Item2.Any())
            {
                var localMac = MachineManage.GetMachineId();
                var thisMachineNoClearList = result.Item2.Where(t => t.MachineFlag.Equals(localMac)).ToList();
                var otherMachineNoClear = result.Item2.Any(t => !t.MachineFlag.Equals(localMac));//是否有其他机器未清机。

                if (_isInForcedEndWorkModel)
                    MessageDialog.Warning("昨天未清机且未结业，请先清机后再结业。");

                if (thisMachineNoClearList.Any())
                {
                    if (thisMachineNoClearList.Any(t => !ClearPos(t.UserName)))
                        return false;
                }

                if (otherMachineNoClear)
                {
                    var wnd = new OtherPosUnclearWarningWindow();
                    if (!WindowHelper.ShowDialog(wnd))
                        return false;
                }
            }

            return true;
        }

        public static bool ClearPos(string userName = null)
        {
            var wnd = new AuthorizationWindow(EnumRightType.Clearner, userName);
            if (!WindowHelper.ShowDialog(wnd))
                return false;

            var request = new ClearnerRequest { UserId = Globals.UserInfo.UserName, UserName = Globals.UserInfo.FullName, Mac = MachineManage.GetMachineId(), Authorizer = Globals.Authorizer.FullName };
            _clearPosSuccess = false;
            var workFlowClear = new WorkFlowInfo(ClearnerProcess, ClearnerComplete, "清机中...");
            WorkFlowService.Start(request, workFlowClear);
            _arEvent.WaitOne(20 * 1000);
            return _clearPosSuccess;
        }

        /// <summary>
        /// 清机执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static object ClearnerProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            var request = (ClearnerRequest)param;
            var result = service.Clearner(request);
            return new Tuple<string, string>(result, request.UserId);
        }

        /// <summary>
        /// 清机执行完成。
        /// </summary>
        /// <param name="param"></param>
        private static Tuple<bool, object> ClearnerComplete(object param)
        {
            var result = (Tuple<string, string>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1);
                return null;
            }

            ReportPrintHelper.PrintClearPosReport(result.Item2);
            _clearPosSuccess = true;
            _arEvent.Set();
            MessageDialog.Warning("清机成功。");
            return null;
        }

        /// <summary>
        /// 强制登录。
        /// </summary>
        public static void ForcedLogin()
        {
            Application.Current.MainWindow.Hide();

            var loginWnd = new AuthorizationWindow(EnumRightType.Login);
            if (WindowHelper.ShowDialog(loginWnd))
                Application.Current.MainWindow.ShowDialog();
            else
                Application.Current.MainWindow.Close();
        }

    }
}