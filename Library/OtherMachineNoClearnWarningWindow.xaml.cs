using System.Linq;
using System.Windows;
using WebServiceReference;

namespace Library
{
    /// <summary>
    /// 其他机器未清机提示信息窗口。
    /// </summary>
    public partial class OtherMachineNoClearnWarningWindow
    {
        public OtherMachineNoClearnWarningWindow()
        {
            InitializeComponent();
        }

        private void ButtonCancel_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnRetry_OnClick(object sender, RoutedEventArgs e)
        {
            //var noClearnMachineList = RestClient.GetNoClearMachineInfos();
            //var localMac = RestClient.GetMacAddr();
            //if (!noClearnMachineList.All(t => t.MachineFlag.Equals(localMac)))
            //{
            //    DialogResult = true;
            //    Close();
            //}
        }
    }
}
