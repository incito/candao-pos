using System.Collections.ObjectModel;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SelectBankWndVm : NormalWindowViewModel<SelectBankWindow>
    {
        public SelectBankWndVm(BankInfo bankInfo)
        {
            BankInfos = new ObservableCollection<BankInfo>();

            if (Globals.BankInfos != null)
                Globals.BankInfos.ForEach(BankInfos.Add);
            SelectedBank = bankInfo;
        }

        /// <summary>
        /// 可选银行集合。
        /// </summary>
        public ObservableCollection<BankInfo> BankInfos { get; private set; }

        /// <summary>
        /// 选择的银行。
        /// </summary>
        public BankInfo SelectedBank { get; set; }

    }
}