using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Models;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Request;
using CanDao.Pos.VIPManage.Models;
using CanDao.Pos.VIPManage.Views;

namespace CanDao.Pos.VIPManage.ViewModels
{
    /// <summary>
    /// 修改手机号码
    /// </summary>
    public class UcVipModifyTelNumViewModel : ViewModelBase
    {
        #region 字段
        private IMemberService _memberService = null;

        private UcVipModifyTelNumView _userControl;

        //手机验证码
        private string _receiveCode;
        private bool _isStartTime = false;

        private MVipChangeInfo _vipChangeInfo;

        #endregion

        #region 属性

        public MVipChangeInfo VipChangeInfo { set; get; }

        public UcVipModifyTelNumModel Model { set; get; }

        /// <summary>
        /// 验证码发送按钮样式启动
        /// </summary>
        public bool IsStartTime
        {
            set
            {
                _isStartTime = value;
                RaisePropertyChanged(() => IsStartTime);
            }
            get { return _isStartTime; }

        }

        #endregion

        #region 事件

        /// <summary>
        /// 确定
        /// </summary>
        public RelayCommand SendCodeCommand { set; get; }

        /// <summary>
        /// 确定
        /// </summary>
        public RelayCommand SureCommand { set; get; }

        /// <summary>
        /// 取消
        /// </summary>
        public RelayCommand CloseCommand { set; get; }

        #endregion

        #region 构造函数

        public UcVipModifyTelNumViewModel(MVipChangeInfo vipChangeInfo)
        {
            _memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            Model = new UcVipModifyTelNumModel();

            VipChangeInfo = vipChangeInfo;
            Model.TelNum = VipChangeInfo.TelNum;

            SureCommand = new RelayCommand(SureHandel);
            CloseCommand = new RelayCommand(CloseHandel);
            SendCodeCommand = new RelayCommand(SendCodeHandel);
        }

        #endregion

        #region 公共方法

        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipModifyTelNumView();
            _userControl.DataContext = this;

            return _userControl;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 发送验证码请求
        /// </summary>
        private void SendCodeHandel()
        {

            if (OCheckFormat.IsMobilePhone(Model.TelNum))
            {
                IsStartTime = true;

                //调用会员接口
                _receiveCode = "";

                var response = new SendVerifyCodeRequest();
                response.branch_id = Globals.BranchInfo.BranchId;
                response.mobile = Model.TelNum;


                var res = _memberService.SendVerifyCode(response);

                if (!string.IsNullOrEmpty(res.Item1))
                {
                    OWindowManage.ShowMessageWindow("发送失败，请重试！", false);
                }
                else
                {
                    _receiveCode = res.Item2.valicode;
                }
            }
            else
            {
                OWindowManage.ShowMessageWindow("手机号码不正确，请检查！", false);
            }
        }

        /// <summary>
        /// 修改确定
        /// </summary>
        private void SureHandel()
        {
            if (IsCheckInput())
            {
                var request = new CanDaoMemberCheckMobileRepeatRequest();
                request.branch_id = Globals.BranchInfo.BranchId;
                request.mobile = Model.TelNum;
                //验证手机验证码
                var res = _memberService.CheckMobileRepeat(request);
                if (!string.IsNullOrEmpty(res))
                {
                    OWindowManage.ShowMessageWindow(string.Format("手机号码变更失败：{0}", res), false);

                    return;
                }
               

                var ret = _memberService.VipChangeInfo(Globals.BranchInfo.BranchId, VipChangeInfo, Model.NTelNum);
                if (string.IsNullOrEmpty(ret))
                {
                    OWindowManage.ShowMessageWindow("手机号码变更成功!", false);
                    CloseStateHandel(true);

                }
                else
                {
                    OWindowManage.ShowMessageWindow("手机号码变更失败：" + ret, false);
                }

            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void CloseHandel()
        {
            CloseStateHandel(false);
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="state">状态</param>
        private void CloseStateHandel(bool state)
        {
            try
            {
                if (_userControl.UcCloseaAction != null)
                {
                    _userControl.UcCloseaAction(state);
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 检查输入项
        /// </summary>
        /// <returns></returns>
        private bool IsCheckInput()
        {
            if (!OCheckFormat.IsMobilePhone(Model.NTelNum))
            {
                OWindowManage.ShowMessageWindow("手机号码为空或格式不正确，请检查！", false);
                return false;
            }
            if (string.IsNullOrEmpty(Model.Code))
            {
                OWindowManage.ShowMessageWindow("验证码不能为空，请检查！", false);
                return false;
            }
            if (!Model.Code.Equals(_receiveCode))
            {
                OWindowManage.ShowMessageWindow("验证码错误，请检查！", false);
                return false;
            }

            return true;
        }

        #endregion

    }
}
