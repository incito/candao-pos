using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using CanDao.Pos.ReportPrint;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class CanDaoMemberStorageWndVm : NormalWindowViewModel
    {
        /// <summary>
        /// 会员卡号。
        /// </summary>
        private readonly string _cardNo;

        public CanDaoMemberStorageWndVm(string mobile, string cardNo)
        {
            PayType = EnumBillPayType.Cash;
            Mobile = mobile;
            _cardNo = cardNo;
        }

        #region Properties

        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 储值金额。
        /// </summary>
        private decimal _storeAmount;
        /// <summary>
        /// 储值金额。
        /// </summary>
        public decimal StoreAmount
        {
            get { return _storeAmount; }
            set
            {
                _storeAmount = value;
                RaisePropertiesChanged("StoreAmount");
            }
        }

        /// <summary>
        /// 储值的付款方式。
        /// </summary>
        private EnumBillPayType _payType;
        /// <summary>
        /// 储值的付款方式。
        /// </summary>
        public EnumBillPayType PayType
        {
            get { return _payType; }
            set
            {
                _payType = value;
                RaisePropertiesChanged("PayType");
            }
        }

        /// <summary>
        /// 会员储值返回数据类。
        /// </summary>
        private CanDaoMemberStorageResponse _data;
        /// <summary>
        /// 会员储值返回数据类。
        /// </summary>
        public CanDaoMemberStorageResponse Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertiesChanged("Data");
            }
        }

        /// <summary>
        /// 是否储值成功。
        /// </summary>
        private bool _isStored;
        /// <summary>
        /// 是否储值成功。
        /// </summary>
        public bool IsStored
        {
            get { return _isStored; }
            set
            {
                _isStored = value;
                RaisePropertiesChanged("IsStored");
            }
        }

        #endregion

        /// <summary>
        /// 异步执行会员储值。
        /// </summary>
        public void StoreAmountAsync()
        {
            if (StoreAmount == 0)
            {
                MessageDialog.Warning("请输入储值金额。", OwnerWindow);
                return;
            }

            var request = new CanDaoMemberStorageRequest
            {
                Amount = StoreAmount,
                branch_id = Globals.BranchInfo.BranchId,
                cardno = _cardNo,
                Serial = Globals.BranchInfo.BranchId,
                ChargeType = ((int)PayType).ToString(),
            };

            TaskService.Start(request, StoreAmountProcess, StoreAmountComplete, "储值操作中...");
        }

        protected override void Confirm(object param)
        {
            StoreAmountAsync();
        }

        /// <summary>
        /// 会员储值的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object StoreAmountProcess(object arg)
        {
            InfoLog.Instance.I("开始调用储值接口...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, CanDaoMemberStorageResponse>("创建IMemberService服务失败。", null);

            return service.StorageCanDao((CanDaoMemberStorageRequest)arg);
        }

        /// <summary>
        /// 储值完成时执行。
        /// </summary>
        /// <param name="arg"></param>
        private void StoreAmountComplete(object arg)
        {
            var result = (Tuple<string, CanDaoMemberStorageResponse>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("会员储值时错误：", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("会员储值成功。");
            Data = result.Item2;
            IsStored = true;

            ReportPrintHelper.PrintMemberStoredReport(GenerateMemberStoredInfo(Data));
            StoreAmount = 0m;//打印后清空。
            MessageDialog.Warning(string.Format("储值成功。交易流水号：{0}", Data.TraceCode), OwnerWindow);
            CloseWindow(true);
        }

        /// <summary>
        /// 生成会员储值打印数据。
        /// </summary>
        /// <param name="data">会员储值的返回数据。</param>
        /// <returns></returns>
        private PrintMemberStoredInfo GenerateMemberStoredInfo(CanDaoMemberStorageResponse data)
        {
            return new PrintMemberStoredInfo
            {
                BranchId = Globals.BranchInfo.BranchId,
                BranchName = Globals.BranchInfo.BranchName,
                CardNo = _cardNo,
                StoredBalance = data.StoreCardBalance,
                ScoreBalance = 0,
                StoredAmount = StoreAmount,
                TradeTime = DateTime.Now,
                TraceCode = data.TraceCode,
            };
        }
    }
}