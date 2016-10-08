using System;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 雅座会员查询窗口的VM。
    /// </summary>
    public class MemberYaZuoQueryWndVm : NormalWindowViewModel<MemberYaZuoQueryWindow>
    {
        #region Properties

        /// <summary>
        /// 会员号。
        /// </summary>
        private string _memberNo;
        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo
        {
            get { return _memberNo; }
            set
            {
                _memberNo = value;
                RaisePropertyChanged("MemberNo");
            }
        }

        /// <summary>
        /// 会员信息。
        /// </summary>
        private YaZuoMemberInfo _memberInfo;

        /// <summary>
        /// 会员信息。
        /// </summary>
        public YaZuoMemberInfo MemberInfo
        {
            get { return _memberInfo; }
            set
            {
                _memberInfo = value;
                RaisePropertyChanged("MemberInfo");
            }
        }

        /// <summary>
        /// 会员优惠券集合。
        /// </summary>
        public ObservableCollection<YaZuoCouponInfo> CouponInfos { get; set; }

        #endregion

        #region Constructor

        public MemberYaZuoQueryWndVm()
        {
            CouponInfos = new ObservableCollection<YaZuoCouponInfo>();
        }

        #endregion

        #region Protected Methods

        protected override void OnWindowLoaded(object param)
        {
            ((MemberYaZuoQueryWindow)OwnerWindow).TbMemberNo.Focus();
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(MemberNo);
        }

        protected override void Confirm(object param)
        {
            TaskService.Start(null, MemberQueryProcess, MemberQueryComplete, "会员查询中...");
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "CouponPreGroup":
                    OwnerWnd.GsCoupon.PreviousGroup();
                    break;
                case "CouponNextGroup":
                    OwnerWnd.GsCoupon.NextGroup();
                    break;
            }
        }

        protected override bool CanOperMethod(object param)
        {
            switch (param as string)
            {
                case "CouponPreGroup":
                    return OwnerWnd.GsCoupon.CanPreviousGroup;
                case "CouponNextGroup":
                    return OwnerWnd.GsCoupon.CanNextGruop;
                default:
                    return true;
            }
        }

        #endregion

        #region Private Methods

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
                MemberInfo = null;
                var errMsg = string.Format("雅座会员查询失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }

            if (result.Item2.CardNoList != null && result.Item2.CardNoList.Any())
            {
                var cardSelectVm = new MultMemberCardSelectWndVm(result.Item2.CardNoList, result.Item2.CardNo);
                if (WindowHelper.ShowDialog(cardSelectVm, OwnerWindow))
                {
                    MemberNo = cardSelectVm.SelectedCard;
                    if (!result.Item2.CardNo.Equals(cardSelectVm.SelectedCard)) //当默认的卡号跟选择的卡号不同时，重新查询。
                    {
                        TaskService.Start(null, MemberQueryProcess, MemberQueryComplete, "会员查询中...");
                        return;
                    }
                }
            }

            MemberInfo = result.Item2;
            CouponInfos.Clear();
            if (result.Item2.CouponList != null)
                result.Item2.CouponList.ForEach(CouponInfos.Add);
        }

        #endregion
    }
}