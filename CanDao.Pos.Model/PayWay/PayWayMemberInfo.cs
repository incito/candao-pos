using System.Collections.Generic;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 会员卡结算方式信息。
    /// </summary>
    public class PayWayMemberInfo : PayWayInfo
    {

        /// <summary>
        /// 会员积分消费。
        /// </summary>
        private decimal _integralAmount;

        /// <summary>
        /// 会员积分消费。
        /// </summary>
        public decimal IntegralAmount
        {
            get { return _integralAmount; }
            set
            {
                _integralAmount = value;
                RaisePropertyChanged("IntegralAmount");
                OnAmountChanged();
            }
        }

        /// <summary>
        /// 会员信息。
        /// </summary>
        private MemberInfo _memberInfo;

        /// <summary>
        /// 会员信息。
        /// </summary>
        public MemberInfo MemberInfo
        {
            get { return _memberInfo; }
            set
            {
                _memberInfo = value;
                RaisePropertyChanged("MemberInfo");

                IsMemberLogin = value != null;
                RaisePropertyChanged("IsMemberLogin");

                if (value == null)//退出会员时清空会员的相关信息。
                {
                    Remark = null;
                    MemberPassword = null;
                    Amount = 0;
                    IntegralAmount = 0;
                }
            }
        }

        /// <summary>
        /// 会员密码。
        /// </summary>
        private string _memberPassword;
        /// <summary>
        /// 会员密码。
        /// </summary>
        public string MemberPassword
        {
            get { return _memberPassword; }
            set
            {
                _memberPassword = value;
                RaisePropertyChanged("MemberPassword");
            }
        }

        /// <summary>
        /// 是否登录会员。
        /// </summary>
        public bool IsMemberLogin { get; set; }

        public override List<string> GenerateSettlementInfo()
        {
            var info = new List<string>();
            if (Amount > 0)
                info.Add(string.Format("会员消费：{0:f2}", Amount));
            if (IntegralAmount > 0)
                info.Add(string.Format("会员积分：{0:f2}", IntegralAmount));
            return info;
        }

        public override List<BillPayInfo> GenerateBillPayInfo()
        {
            var list = new List<BillPayInfo>();
            if (Amount > 0)
                list.Add(new BillPayInfo(Amount, (int)EnumBillPayType.MemberCard, "", Remark));
            if (IntegralAmount > 0)
                list.Add(new BillPayInfo(IntegralAmount, (int)EnumBillPayType.MemberIntegral, "", Remark));
            return list;
        }

        public override string CheckTheBillAllowPay()
        {
            if (IsMemberLogin)
            {
                if (Amount > MemberInfo.StoredBalance)
                    return "会员储值余额不足。";

                if (IntegralAmount > MemberInfo.Integral)
                    return "积分余额不足。";
            }
            return null;
        }
    }
}