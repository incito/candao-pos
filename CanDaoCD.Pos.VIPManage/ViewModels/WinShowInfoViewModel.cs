using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using Models;
using WebServiceReference;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    public class WinShowInfoViewModel : ViewModelBase
    {
        private WinShowInfoView _window;
        private string _insideId=string.Empty;
        #region 属性
        /// <summary>
        /// 内部虚拟Id
        /// </summary>
        public string InsideId
        {
            set { _insideId = value; }
            get { return _insideId; } }
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


        /// <summary>
        /// 确定绑定
        /// </summary>
        private void OkHandel()
        {
            if (!string.IsNullOrEmpty(_insideId))
            {
                TCandaoRetBase ret2 = CanDaoMemberClient.VipChangeCard(Globals.branch_id, Model.CardNum, _insideId);
                if (!ret2.Ret)
                {
                    Model.ShowInfo = string.Format("会员卡绑定失败：{0}", ret2.Retinfo);
                    return;
                }
            }
           
            if (OkReturn != null)
            {
                OkReturn(Model.CardNum);
            }
            _window.Close();
        }

        /// <summary>
        /// 取消绑定
        /// </summary>
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
                TCandaoRetBase ret = CanDaoMemberClient.VipCheckCard(Globals.branch_id, Model.CardNum);
                if (ret.Ret)//会员卡判断
                {
                    Model.ShowInfo = string.Format("会员卡序列号：{0}，确认绑定该实体会员卡吗？", Model.CardNum);
                    Model.IsEnableBtn = true;
                }
                else
                {
                    Model.ShowInfo = string.Format("该会员卡[{0}]{1}，请重新刷卡!", Model.CardNum,ret.Retinfo);
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
