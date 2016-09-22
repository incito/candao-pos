﻿using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 雅座会员储值窗口的VM。
    /// </summary>
    public class MemberYaZuoStoredWndVm : NormalWindowViewModel<MemberYaZuoStoredWindow>
    {
        #region Properties

        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo { get; set; }

        /// <summary>
        /// 储值信息。
        /// </summary>
        public YaZuoStorageInfo StorageInfo { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        private decimal _storedValue;
        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredValue
        {
            get { return _storedValue; }
            set
            {
                _storedValue = value;
                RaisePropertyChanged("StoredValue");
            }
        }

        /// <summary>
        /// 充值付款类型。
        /// </summary>
        private EnumStoragePayType _storagePayType;
        /// <summary>
        /// 充值付款类型。
        /// </summary>
        public EnumStoragePayType StoragePayType
        {
            get { return _storagePayType; }
            set
            {
                _storagePayType = value;
                RaisePropertyChanged("StoragePayType");
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnWindowLoaded(object param)
        {
            ((MemberYaZuoStoredWindow)OwnerWindow).TbMemberNo.Focus();
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "Cash":
                    StoragePayType = EnumStoragePayType.Cash;
                    break;
                case "Bank":
                    StoragePayType = EnumStoragePayType.Bank;
                    break;
            }
        }

        protected override void Confirm(object param)
        {
            if (MessageDialog.Quest(string.Format("确定给会员号：\"{0}\"储值：\"{1}\"吗？", MemberNo, StoredValue)))
                TaskService.Start(null, StorageProcess, StorageComplete, "会员储值处理中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(MemberNo) && StoredValue > 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 会员储值执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object StorageProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.StorageYaZuo(MemberNo, StoredValue, StoragePayType);
        }

        /// <summary>
        /// 会员储值执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void StorageComplete(object param)
        {
            var result = (Tuple<string, YaZuoStorageInfo>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("会员储值失败：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            StorageInfo = result.Item2;
            NotifyDialog.Notify(string.Format("会员储值成功。{0}交易流水号：{1}", Environment.NewLine, result.Item2.TradeCode), OwnerWindow.Owner);
            TaskService.Start(null, MemberQueryProcess, MemberQueryComplete, "会员积分余额查询中...");
        }

        /// <summary>
        /// 会员查询执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object MemberQueryProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            return service.QueryYaZuo(MemberNo);
        }

        /// <summary>
        /// 会员查询执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void MemberQueryComplete(object param)
        {
            var result = (Tuple<string, YaZuoMemberInfo>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("雅座会员查询失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }

            StorageInfo.IntegralBalance = result.Item2.IntegralBalance;

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
                CardNo = StorageInfo.CardNo,
                TraceCode = StorageInfo.TradeCode,
                TradeTime = DateTime.Now,
                StoredBalance = StorageInfo.StoredBalance,
                ScoreBalance = StorageInfo.IntegralBalance,
                StoredAmount = StoredValue,
            };
        }

        #endregion
    }
}