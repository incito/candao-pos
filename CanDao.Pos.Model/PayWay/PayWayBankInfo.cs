using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 银行卡结算方式。
    /// </summary>
    public class PayWayBankInfo : PayWayInfo
    {
        private BankInfo _selectedBankInfo;

        /// <summary>
        /// 选择的银行卡信息。
        /// </summary>
        public BankInfo SelectedBankInfo
        {
            get { return _selectedBankInfo; }
            set
            {
                _selectedBankInfo = value;
                RaisePropertyChanged("SelectedBankInfo");
            }
        }

        public override List<string> GenerateSettlementInfo()
        {
            var info = new List<string>();
            if (Amount > 0)
                info.Add(string.Format("银行卡：{0:f2}", Amount));
            return info;
        }

        public override List<BillPayInfo> GenerateBillPayInfo()
        {
            var list = new List<BillPayInfo>();
            int bankId = SelectedBankInfo != null ? SelectedBankInfo.Id : 0;
            list.Add(new BillPayInfo(Amount, ItemId, Remark, bankId.ToString()));
            return list;
        }
    }
}