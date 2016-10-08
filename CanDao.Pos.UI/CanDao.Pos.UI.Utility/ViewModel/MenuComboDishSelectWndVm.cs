using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class MenuComboDishSelectWndVm : NormalWindowViewModel<MenuComboDishSelectWindow>
    {
        #region Fileds

        /// <summary>
        /// 窗体里的套餐容器控件。
        /// </summary>
        private ScrollViewer _wndSv;

        #endregion

        #region Constructor

        public MenuComboDishSelectWndVm(MenuComboFullInfo info)
        {
            Data = info;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 套餐数据。
        /// </summary>
        public MenuComboFullInfo Data { get; set; }

        /// <summary>
        /// 是否需要上下翻页。
        /// </summary>
        private bool _needPageUpDown;
        /// <summary>
        /// 是否需要上下翻页。
        /// </summary>
        public bool NeedPageUpDown
        {
            get { return _needPageUpDown; }
            set
            {
                _needPageUpDown = value;
                RaisePropertiesChanged("NeedPageUpDown");
            }
        }

        /// <summary>
        /// 选择的忌口信息。
        /// </summary>
        public string SelectedDiet
        {
            get { return OwnerWnd.DietSetCtrl.SelectedInfo; }
        }

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            var selectDone = Data.ComboDishInfos.All(t => t.IsSelectedDone);
            if (!selectDone)
            {
                MessageDialog.Warning("套餐内还有未选组合。", OwnerWindow);
                return;
            }

            if (!MessageDialog.Quest("确定选好了吗？", OwnerWindow))
                return;

            Data.ComboSelfInfo.SelectedCount = 1;//设定点菜数量为1。
            Data.SingleDishInfos.ForEach(t => { t.SelectedCount = 1; });//将套餐内的单品菜选择数量设为1。

            base.Confirm(param);
        }

        protected override void OnWindowLoaded(object param)
        {
            NeedPageUpDown = ((MenuComboDishSelectWindow)OwnerWindow).SvList.ComputedVerticalScrollBarVisibility == Visibility.Visible;
            _wndSv = ((MenuComboDishSelectWindow)OwnerWindow).SvList;
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "PageUp":
                    if (_wndSv != null)
                        _wndSv.PageUp();
                    break;
                case "PageDown":
                    if (_wndSv != null)
                        _wndSv.PageDown();
                    break;
            }
        }

        protected override bool CanOperMethod(object param)
        {
            switch (param as string)
            {
                case "PageUp":
                    return _wndSv != null && _wndSv.VerticalOffset > 0;
                case "PageDown":
                    return _wndSv != null && (_wndSv.ContentVerticalOffset + _wndSv.ViewportHeight < _wndSv.ExtentHeight);
                default:
                    return true;
            }
        }

        #endregion
    }
}