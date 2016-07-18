using System.Collections.ObjectModel;
using CanDao.Pos.Common;
using CanDao.Pos.Model;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SelectBankWndVm : NormalWindowViewModel
    {
        public SelectBankWndVm(BankInfo bankInfo)
        {
            BankInfos = new ObservableCollection<BankInfo>();

            if (Globals.BankInfos != null)
                Globals.BankInfos.ForEach(BankInfos.Add);
            SelectedBank = bankInfo;
        }

        public ObservableCollection<BankInfo> BankInfos { get; private set; }

        public BankInfo SelectedBank { get; set; }

    }
}