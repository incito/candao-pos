using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 雅座会员激活窗口的VM。
    /// </summary>
    public class MemberYaZuoCardActiveWndVm : NormalWindowViewModel<MemberYaZuoCardActiveWindow>
    {
        #region Properties

        /// <summary>
        /// 会员卡号。
        /// </summary>
        public string MemberCardNo { get; set; }

        /// <summary>
        /// 会员卡号密码。
        /// </summary>
        public string MemberPassword { get; set; }

        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnWindowLoaded(object param)
        {
            ((MemberYaZuoCardActiveWindow)OwnerWindow).TbMemberNo.Focus();
        }

        protected override void Confirm(object param)
        {
            if (MessageDialog.Quest(string.Format("确定激活会员卡号：\"{0}\"给手机号：\"{1}\"吗？", MemberCardNo, Mobile)))
                TaskService.Start(null, CardActiveProcess, CardActiveComplete, "会员卡激活中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(MemberCardNo) && !string.IsNullOrEmpty(MemberPassword) && !string.IsNullOrEmpty(Mobile);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 会员卡激活执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object CardActiveProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, YaZuoCardActiveInfo>("创建IMemberService服务失败。", null);

            return service.CardActiveYaZuo(MemberCardNo, MemberPassword, Mobile);
        }

        /// <summary>
        ///  会员卡激活执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void CardActiveComplete(object param)
        {
            var result = (Tuple<string, YaZuoCardActiveInfo>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("会员卡激活失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }

            NotifyDialog.Notify("会员卡激活成功。", OwnerWindow);

            var print = new ReportPrintHelper2(null);
            print.PrintMemberStoredReport(GeneratePrintInfo());

            CloseWindow(true);
        }

        /// <summary>
        /// 生成打印的会员储值信息。
        /// </summary>
        /// <returns></returns>
        private PrintMemberStoredInfo GeneratePrintInfo()
        {
            return new PrintMemberStoredInfo
            {
                ReportTitle = Globals.BranchInfo.BranchName,
                CardNo = MemberCardNo,
                TraceCode = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                TradeTime = DateTime.Now,
                StoredBalance = 0,
                ScoreBalance = 0,
                StoredAmount = 0,
            };
        }

        #endregion
    }
}