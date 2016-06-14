using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    public class WinShowInfoViewModel : ViewModelBase
    {
        private WinShowInfoView _window;

        #region 属性

        public WinShowInfoModel Model
        {
            set; get; }

        #endregion

        #region 事件

        /// <summary>
        /// 确认事件
        /// </summary>
        public RelayCommand OkCommand { set; get; }

        /// <summary>
        /// 取消事件
        /// </summary>
        public RelayCommand CancelCommand { set; get; }
        /// <summary>
        /// 确认返回
        /// </summary>

        public Action<string> OkReturn;


        #endregion

        public WinShowInfoViewModel()
        {
            OkCommand = new RelayCommand(OkHandel);
            CancelCommand = new RelayCommand(CancelHandel);
            Model = new WinShowInfoModel();

        }

       

        private void OkHandel()
        {
            if (OkReturn != null)
            {
                OkReturn(Model.CardNum);
            }
            _window.Close();
        }

        private void CancelHandel()
        {
            _window.Close();
        }

        /// <summary>
        /// 文本回车后
        /// </summary>
        private void TexChange()
        {
            if (!string.IsNullOrEmpty(Model.CardNum))
            {
                if (true)//会员卡判断
                {
                    Model.ShowInfo = string.Format("会员卡序列号：{0}，确认绑定该实体会员卡吗？", Model.CardNum);
                    Model.IsEnableBtn = true;
                }
                else
                {
                    Model.ShowInfo = string.Format("该会员卡[{0}]已绑定，请重新刷卡!", Model.CardNum);
                    StartReadCardNum();
                }
            }
        }

        /// <summary>
        /// 获取对应View
        /// </summary>
        /// <returns></returns>
        public Window GetShoWindow()
        {
            _window = new WinShowInfoView();
            _window.DataContext = this;
            _window.TexCardChange = new Action(TexChange);
            return _window;
        }
        /// <summary>
        /// 
        /// </summary>
        public void StartReadCardNum()
        {
            Model.CardNum = "";
            _window.SetTextFocus();
        }
    }
}
