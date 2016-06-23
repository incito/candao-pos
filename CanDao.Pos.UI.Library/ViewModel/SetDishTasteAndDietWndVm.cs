using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.Model;
using CanDao.Pos.UI.Library.View;
using Common;
using Models;
using Globals = Common.Globals;

namespace CanDao.Pos.UI.Library.ViewModel
{
    public class SetDishTasteAndDietWndVm : NormalWindowViewModel
    {
        #region Constructor

        public SetDishTasteAndDietWndVm(List<string> tasteList, DishSimpleInfo dishSimpleInfo)
        {
            if (tasteList != null)
            {
                HasTasteInfos = tasteList.Any();
                DishTasteInfos = tasteList.Select(t => new TasteInfo { TasteTitle = t }).ToList();
            }
            DishInfo = dishSimpleInfo;
            DishNum = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 菜品口味集合。
        /// </summary>
        public List<TasteInfo> DishTasteInfos { get; private set; }

        /// <summary>
        /// 菜品基本信息。
        /// </summary>
        public DishSimpleInfo DishInfo { get; set; }

        /// <summary>
        /// 菜品个数。
        /// </summary>
        private int _dishNum;
        /// <summary>
        /// 菜品个数。
        /// </summary>
        public int DishNum
        {
            get { return _dishNum; }
            set
            {
                _dishNum = value;
                RaisePropertyChanged("DishNum");
            }
        }

        /// <summary>
        /// 选择的口味。
        /// </summary>
        public string SelectedTaste
        {
            get
            {
                var item = DishTasteInfos.FirstOrDefault(t => t.IsSelected);
                return item != null ? item.TasteTitle : null;
            }
        }

        /// <summary>
        /// 是否包含口味。
        /// </summary>
        public bool HasTasteInfos { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 设置菜品数量命令。
        /// </summary>
        public ICommand SetDishNumCmd { get; private set; }

        /// <summary>
        /// 菜品数量减少命令。
        /// </summary>
        public ICommand DishNumSubCmd { get; private set; }

        /// <summary>
        /// 菜品数量增加命令。
        /// </summary>
        public ICommand DishNumAddCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 设置菜品数量命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void SetDishNum(object arg)
        {
            var wnd = new DishInfoEditWindow(DishInfo.DishName, DishInfo.DishPrice);
            if (wnd.ShowDialog() == true)
            {
                DishNum = Convert.ToInt32(wnd.DishNum);
            }
        }

        /// <summary>
        /// 菜品数量减少命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void DishNumSub(object arg)
        {
            DishNum--;
        }

        /// <summary>
        /// 菜品数量减少命令是否可用的判断方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanDishNumSub(object arg)
        {
            return DishNum > 1;
        }

        /// <summary>
        /// 菜品数量增加命令的执行方法。
        /// </summary>
        private void DishNumAdd(object arg)
        {
            DishNum++;
        }

        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            SetDishNumCmd = CreateDelegateCommand(SetDishNum);
            DishNumAddCmd = CreateDelegateCommand(DishNumAdd);
            DishNumSubCmd = CreateDelegateCommand(DishNumSub, CanDishNumSub);
        }

        protected override bool CanConfirm(object param)
        {
            return DishTasteInfos == null || DishTasteInfos.Any(t => t.IsSelected);//当有口味时必须选择一个口味。
        }

        #endregion

        #region Private Methods

        #endregion
    }
}