using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 挂账结算方式信息。
    /// </summary>
    public class PayWayOnCmpAccount : PayWayInfo
    {

        /// <summary>
        /// 选择的挂账单位。
        /// </summary>
        private OnCompanyAccountInfo _selectedOnCmpAccInfo;
        /// <summary>
        /// 获取或设置选择的挂账单位。
        /// </summary>
        public OnCompanyAccountInfo SelectedOnCmpAccInfo
        {
            get { return _selectedOnCmpAccInfo; }
            set
            {
                _selectedOnCmpAccInfo = value;
                RaisePropertyChanged("SelectedOnCmpAccInfo");
            }
        }

        public override List<BillPayInfo> GenerateBillPayInfo()
        {
            var list = new List<BillPayInfo>();
            var onAcc = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Name : "";
            var cmpId = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Id : "";
            list.Add(new BillPayInfo(Amount, ItemId, onAcc) { CouponDetailId = cmpId });
            return list;
        }

        public override string CheckTheBillAllowPay()
        {
            if (Amount > 0 && SelectedOnCmpAccInfo == null)
                return "使用挂账金额请先选择挂账单位。";
            return null;
        }
    }
}