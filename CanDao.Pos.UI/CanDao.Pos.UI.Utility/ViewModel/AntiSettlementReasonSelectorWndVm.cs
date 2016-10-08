using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 反结算窗口Vm。
    /// </summary>
    public class AntiSettlementReasonSelectorWndVm : NormalWindowViewModel<AntiSettlementReasonSelectorWindow>
    {
        private string _selectedReason;

        public string SelectedReason
        {
            get { return _selectedReason; }
            set
            {
                _selectedReason = value;
                RaisePropertiesChanged("SelectedReason");
            }
        }

        public ICommand ReasonListCheckedCmd { get; private set; }

        private void ReasonListChecked(object arg)
        {
            SelectedReason = arg as string;
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            ReasonListCheckedCmd = CreateDelegateCommand(ReasonListChecked);
        }

        protected override void Confirm(object param)
        {
            if (string.IsNullOrEmpty(SelectedReason))
            {
                MessageDialog.Warning("请选择一个反结算原因。", OwnerWindow);
                return;
            }

            base.Confirm(param);
        }
    }
}