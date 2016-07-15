using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.Common.Models.VipModels;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using Models;
using WebServiceReference;
using CanDaoCD.Pos.Common.Operates;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    public class WinShowInfoViewModel : ViewModelBase
    {
        private WinShowInfoView _window;
        private string _insideId=string.Empty;

        #region 属性
        /// <summary>
        /// 绑定卡手机号
        /// </summary>
        public string InsideId
        {
            set { _insideId = value; }
            get { return _insideId; } }

        public WinShowInfoModel Model
        {
            set; get; }

        public MVipChangeInfo VipChangeInfo { set; get; }
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

        #endregion

        public WinShowInfoViewModel()
        {
            OkCommand = new RelayCommand(OkHandel);
            CancelCommand = new RelayCommand(CancelHandel);
            Model = new WinShowInfoModel();
            VipChangeInfo=new MVipChangeInfo();
        }


        /// <summary>
        /// 确定绑定
        /// </summary>
        private void OkHandel()
        {
            if (!string.IsNullOrEmpty(_insideId))
            {
                TCandaoRetBase ret2 = CanDaoMemberClient.VipInsertCard(Globals.branch_id, Model.CardNum, _insideId);
                if (!ret2.Ret)
                {
                    Model.ShowInfo = string.Format("新增实体卡失败：{0}", ret2.Retinfo);
                    return;
                }
                else
                {
                    OWindowManage.ShowMessageWindow("新增实体卡成功！", false);
                }
            }
            else if(!string.IsNullOrEmpty(VipChangeInfo.CardNum))
            {
                TCandaoRetBase ret = CanDaoMemberClient.VipChangeCardNum(Globals.branch_id, VipChangeInfo.CardNum, Model.CardNum);
                if (!ret.Ret)
                {
                    OWindowManage.ShowMessageWindow("会员卡修改失败：" + ret.Retinfo, false);
                    return;
                }
                else
                {
                    OWindowManage.ShowMessageWindow("会员卡修改成功！", false);
                }
            }

            _window.DialogResult = true;
        }

        /// <summary>
        /// 取消绑定
        /// </summary>
        private void CancelHandel()
        {
            _window.DialogResult = false;
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
                    if (!string.IsNullOrEmpty(_insideId))
                    {
                        Model.ShowInfo = string.Format("会员卡序列号：{0}，确认新增该实体会员卡吗？", Model.CardNum);
                    }
                    else
                    {
                        Model.ShowInfo = string.Format("确定将会员卡号[{0}]修改为：[{1}]吗？",VipChangeInfo.CardNum, Model.CardNum);
                    }
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
