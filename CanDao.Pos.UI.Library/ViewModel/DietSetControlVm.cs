using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.View;
using Common;
using Models;
using Globals = Common.Globals;

namespace CanDao.Pos.UI.Library.ViewModel
{
    public class DietSetControlVm : BaseViewModel
    {
        public DietSetControlVm()
        {
            InitDietInfos();
            SetOtherDietCmd = CreateDelegateCommand(SetOtherDiet);
        }

        /// <summary>
        /// 忌口信息集合。
        /// </summary>
        public List<DietInfo> DietInfos { get; set; }

        /// <summary>
        /// 其他忌口。
        /// </summary>
        private string _otherDiet;

        /// <summary>
        /// 其他忌口。
        /// </summary>
        public string OtherDiet
        {
            get { return _otherDiet; }
            set
            {
                _otherDiet = value;
                RaisePropertyChanged("OtherDiet");
            }
        }

        /// <summary>
        /// 设置其他忌口命令。
        /// </summary>
        public ICommand SetOtherDietCmd { get; private set; }

        /// <summary>
        /// 设置其他忌口信息。
        /// </summary>
        /// <param name="arg"></param>
        private void SetOtherDiet(object arg)
        {var wnd = new InputMoreInfoWindow("忌口", OtherDiet);
            SoftKeyboardHelper.ShowSoftKeyboard();//系统软键盘。
            if (wnd.ShowDialog() == true)
                OtherDiet = wnd.InputInfo;
            SoftKeyboardHelper.CloseSoftKeyboard();
        }

        private void InitDietInfos()
        {
            if (Globals.DietSetting != null)
                DietInfos = Globals.DietSetting.Select(t => new DietInfo { DietName = t }).ToList();
        }

    }
}