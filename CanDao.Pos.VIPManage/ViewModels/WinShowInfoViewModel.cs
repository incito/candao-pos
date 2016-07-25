using CanDao.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.VIPManage.Models;
using CanDao.Pos.VIPManage.Views;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.IService;

namespace CanDao.Pos.VIPManage.ViewModels
{
    public class WinShowInfoViewModel : ViewModelBase
    {
        private WinShowInfoView _window;
        private string _insideId=string.Empty;
        private IMemberService _memberService = null;

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
            _memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
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
                var res = _memberService.VipInsertCard(Globals.BranchInfo.BranchId, Model.CardNum, _insideId);
                if (!string.IsNullOrEmpty(res))
                {
                    Model.ShowInfo = string.Format("新增实体卡失败：{0}", res);
                    return;
                }
                else
                {
                    OWindowManage.ShowMessageWindow("新增实体卡成功！", false);
                }
            }
            else if(!string.IsNullOrEmpty(VipChangeInfo.CardNum))
            {
                var ret = _memberService.VipChangeCardNum(Globals.BranchInfo.BranchId, VipChangeInfo.CardNum, Model.CardNum);
                if (!string.IsNullOrEmpty(ret))
                {
                    OWindowManage.ShowMessageWindow("会员卡修改失败：" + ret, false);
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
                var ret = _memberService.VipCheckCard(Globals.BranchInfo.BranchId, Model.CardNum);
                if (string.IsNullOrEmpty(ret))//会员卡判断
                {
                    if (!string.IsNullOrEmpty(_insideId))
                    {
                        Model.ShowInfo = string.Format("会员卡序列号：{0}，确认新增该实体会员卡吗？", Model.CardNum);
                    }
                    else if(!string.IsNullOrEmpty(VipChangeInfo.CardNum))
                    {
                        Model.ShowInfo = string.Format("确定将会员卡号[{0}]修改为：[{1}]吗？",VipChangeInfo.CardNum, Model.CardNum);
                    }
                    else
                    {
                        Model.ShowInfo = string.Format("会员卡序列号：{0}，确认绑定该实体会员卡吗？", Model.CardNum);
                    }
                    Model.IsEnableBtn = true;
                }
                else
                {
                    Model.ShowInfo = string.Format("该会员卡[{0}]{1}，请重新刷卡!", Model.CardNum,ret);
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
