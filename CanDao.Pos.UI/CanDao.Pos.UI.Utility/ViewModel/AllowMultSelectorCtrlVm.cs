using System.Collections.ObjectModel;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.Controls;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 允许多选的控件VM。
    /// </summary>
    public class AllowMultSelectorCtrlVm : BaseViewModel
    {

        public AllowMultSelectorCtrlVm()
        {
            ItemSource = new ObservableCollection<AllowSelectInfo>();
            SetOtherInfoCmd = CreateDelegateCommand(SetOtherInfo);
        }

        /// <summary>
        /// 可选信息集合。
        /// </summary>
        public ObservableCollection<AllowSelectInfo> ItemSource { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        private string _title;
        /// <summary>
        /// 标题。
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// 其他信息。
        /// </summary>
        private string _otherInfo;

        /// <summary>
        /// 其他信息。
        /// </summary>
        public string OtherInfo
        {
            get { return _otherInfo; }
            set
            {
                _otherInfo = value;
                RaisePropertyChanged("OtherInfo");
            }
        }

        /// <summary>
        /// 设置其他信息命令。
        /// </summary>
        public ICommand SetOtherInfoCmd { get; private set; }

        /// <summary>
        /// 设置其他信息。
        /// </summary>
        /// <param name="arg"></param>
        private void SetOtherInfo(object arg)
        {
            var wnd = new InputMoreInfoWindow(Title, OtherInfo);
            SoftKeyboardHelper.ShowSoftKeyboard();//系统软键盘。
            if (wnd.ShowDialog() == true)
                OtherInfo = wnd.InputInfo;
            SoftKeyboardHelper.CloseSoftKeyboard();
        }
    }
}