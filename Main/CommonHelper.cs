using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common;
using Library;
using Main;
using Models.Enum;
using Newtonsoft.Json.Linq;
using ReportsFastReport;
using WebServiceReference;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace KYPOS
{
    /// <summary>
    /// 公共辅助类。
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 清理所有POS机。都清理成功返回true，否则返回false。
        /// </summary>
        /// <param name="isInForcedEndWorkModel">是否是强制结业清机。</param>
        /// <returns></returns>
        public static bool ClearAllMachine(bool isInForcedEndWorkModel)
        {
            IRestaurantService service = new RestaurantServiceImpl();
            var result = service.GetUnclearnPosInfo();
            if (!string.IsNullOrEmpty(result.Item1))
            {
                frmBase.Warning(result.Item1);
                return false;
            }

            var noClearnMachineList = result.Item2;
            if (noClearnMachineList.Any())
            {
                var localMac = RestClient.GetMacAddr();
                var thisMachineNoClearList = noClearnMachineList.Where(t => t.MachineFlag.Equals(localMac)).ToList();
                var otherMachineNoClear = noClearnMachineList.Any(t => !t.MachineFlag.Equals(localMac));//是否有其他机器未清机。

                if (isInForcedEndWorkModel && (thisMachineNoClearList.Any() || otherMachineNoClear))
                {
                    frmBase.Warning("昨天未就业且还有未清机，请先清机后再结业。");
                }

                if (thisMachineNoClearList.Any())
                {
                    if (thisMachineNoClearList.Any(t => !ClearMachine(t.UserName)))//任何一个本机收银全清机失败就返回。
                        return false;
                }

                if (otherMachineNoClear)
                {
                    OtherMachineNoClearnWarningWindow warningWnd = new OtherMachineNoClearnWarningWindow();
                    if (warningWnd.ShowDialog() != true)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 清机业务组合。
        /// </summary>
        /// <param name="userName">收银员账号。</param>
        /// <returns>清机成功返回true，否则返回false。</returns>
        public static bool ClearMachine(string userName = null)
        {
            if (!frmPermission2.ShowPermission2("收银员清机", EnumRightType.ClearMachine, userName))
                return false;

            ReportPrint.PrintClearMachine(); //打印清机报表
            ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
            frmBase.Warning("清机成功!");
            return true;
        }

        /// <summary>
        /// 结业处理。
        /// </summary>
        public static bool EndWork()
        {
            if (!frmPermission2.ShowPermission2("结业经理授权", EnumRightType.EndWork))
                return false;

            try
            {
                JObject ja = RestClient.endWork(Globals.UserInfo.UserID);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    do
                    {
                        try
                        {
                            if (RestClient.jdesyndata())//调用上传回调
                                break;
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        if (!frmBase.AskQuestion("发生异常，上传失败，是否重新上传？"))
                            break;
                    } while (true);

                    frmBase.Warning("结业成功!");
                    Application.Exit();
                    return true;
                }

                var msg = ja["Info"].ToString();
                if (string.IsNullOrEmpty(msg))
                    msg = "结业失败！";
                frmBase.Warning(msg);
                return false;
            }
            catch (Exception ex)
            {
                frmBase.Warning("结业失败。" + ex.Message);
                return false;
            }
        }
    }
}