using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class MenuComboDishSelectWndVm : NormalWindowViewModel
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

        #endregion

        #region Commands

        /// <summary>
        /// 翻页上翻。
        /// </summary>
        public ICommand PageUpCmd { get; private set; }

        /// <summary>
        /// 翻页下翻。
        /// </summary>
        public ICommand PageDownCmd { get; private set; }

        /// <summary>
        /// 窗口加载命令。
        /// </summary>
        public ICommand WndLoadCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 上翻页的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void PageUp(object arg)
        {
            if (_wndSv == null)
                return;

            _wndSv.PageUp();
        }

        /// <summary>
        /// 是否允许上翻页。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanPageUp(object arg)
        {
            if (_wndSv == null)
                return false;

            return _wndSv.VerticalOffset > 0;
        }

        /// <summary>
        /// 下翻执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void PageDown(object arg)
        {
            if (_wndSv == null)
                return;

            _wndSv.PageDown();
        }

        /// <summary>
        /// 是否允许下翻。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanPageDown(object arg)
        {
            if (_wndSv == null)
                return false;

            return _wndSv.ContentVerticalOffset + _wndSv.ViewportHeight < _wndSv.ExtentHeight;
        }

        /// <summary>
        /// 窗口加载命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void WndLoad(object arg)
        {
            NeedPageUpDown = ((MenuComboDishSelectWindow)OwnerWindow).SvList.ComputedVerticalScrollBarVisibility == Visibility.Visible;
            _wndSv = ((MenuComboDishSelectWindow)OwnerWindow).SvList;
        }

        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            PageUpCmd = CreateDelegateCommand(PageUp, CanPageUp);
            PageDownCmd = CreateDelegateCommand(PageDown, CanPageDown);
            WndLoadCmd = CreateDelegateCommand(WndLoad);
        }

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

            //对选定套餐的一些处理
            Data.ComboSelfInfo.SelectedCount = 1;//设定点菜数量为1。
            Data.SingleDishInfos.ForEach(t => { t.SelectedCount = t.DishCount; });//将套餐内的单品菜选择数量设为套餐设定的数量。

            //对套餐内所有菜品设定忌口信息

            base.Confirm(param);
        }

        #endregion
    }
}