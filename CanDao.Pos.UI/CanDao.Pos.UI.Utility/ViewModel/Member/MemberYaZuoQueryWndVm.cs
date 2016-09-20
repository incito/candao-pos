using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class MemberYaZuoQueryWndVm : NormalWindowViewModel
    {
        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo { get; set; }

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
        /// 积分余额。
        /// </summary>
        private decimal _integralValue;
        /// <summary>
        /// 积分余额。
        /// </summary>
        public decimal IntegralValue
        {
            get { return _integralValue; }
            set
            {
                _integralValue = value;
                RaisePropertyChanged("IntegralValue");
            }
        }

        #region Protected Methods

        protected override void OnWindowLoaded(object param)
        {
            var wnd = OwnerWindow as MemberYaZuoQueryWindow;
            if (wnd != null)
                wnd.TbMemberNo.Focus();
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(MemberNo);
        }

        protected override void Confirm(object param)
        {
            TaskService.Start(null, MemberQueryProcess, MemberQueryComplete, "会员查询中...");
        }

        #endregion

        #region Private Methods

        private object MemberQueryProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            return service.QueryYaZuo(MemberNo);
        }

        private void MemberQueryComplete(object param)
        {
            var result = (Tuple<string, MemberInfo>) param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                StoredValue = 0;
                IntegralValue = 0;
                var errMsg = string.Format("雅座会员查询失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
                return;
            }

            StoredValue = result.Item2.StoredBalance;
            IntegralValue = result.Item2.Integral;
        }

        #endregion
    }
}