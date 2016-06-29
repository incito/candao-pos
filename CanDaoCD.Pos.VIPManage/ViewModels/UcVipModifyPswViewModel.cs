using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using WebServiceReference;
using Models;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    public class UcVipModifyPswViewModel : ViewModelBase
    {
        #region 字段

        private UcVipModifyPswView _userControl;

        //手机验证码
        private string _receiveCode;
        private bool _isStartTime = false;
        #endregion

        #region 属性

        public UcVipSelectModel VipInfo { set; get; }

        public UcVipModifyPswModel Model { set; get; }

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

        public UcVipModifyPswViewModel(string telNum="")
        {
            
            Model = new UcVipModifyPswModel();
            Model.TelNum = telNum;
            SureCommand = new RelayCommand(SureHandel);
            CloseCommand=new RelayCommand(CloseHandel);
            SendCodeCommand=new RelayCommand(SendCodeHandel);
        }

        #endregion

        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipModifyPswView();
            _userControl.DataContext = this;

            return _userControl;
        }

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
                CanDaoMemberClient.SendAccountByMobile(Globals.branch_id, "", Model.TelNum, out _receiveCode);
                if (_receiveCode.Equals(""))
                {
                    OWindowManage.ShowMessageWindow("发送失败，请重试！", false);
                }
            }
            else
            {
                OWindowManage.ShowMessageWindow("手机号码不正确，请检查！", false);
            }
        }

        private void SureHandel()
        {
            if (IsCheckInput())
            {

                TCandaoRetBase ret = CanDaoMemberClient.VipChangePsw(Globals.branch_id, Model.TelNum, Model.Psw);
                if (ret.Ret)
                {
                    OWindowManage.ShowMessageWindow("修改成功!", false);
                    CloseHandel();

                }
                else
                {
                    OWindowManage.ShowMessageWindow("修改失败："+ret.Retinfo, false);
                }

            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void CloseHandel()
        {
            try
            {
                if (_userControl.UcClose != null)
                {
                    _userControl.UcClose();
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
            if (!OCheckFormat.IsMobilePhone(Model.TelNum))
            {
                OWindowManage.ShowMessageWindow("手机号码为空或格式不正确，请检查！", false);
                return false;
            }
            if (string.IsNullOrEmpty(Model.Code))
            {
                OWindowManage.ShowMessageWindow("验证码不能为空，请检查！", false);
                return false;
            }
            if (!Model.Code.Equals(_receiveCode) )
            {
                OWindowManage.ShowMessageWindow("验证码错误，请检查！", false);
                return false;
            }
            
            if (string.IsNullOrEmpty(Model.Psw))
            {
                OWindowManage.ShowMessageWindow("密码不能为空，请检查！", false);
                return false;
            }
            if (Model.Psw.Length < 6)
            {
                OWindowManage.ShowMessageWindow("密码长度不能少于6位，请检查！", false);
                return false;
            }
            if (Model.Psw != Model.PswConfirm)
            {
                OWindowManage.ShowMessageWindow("2个密码不一致，请检查！", false);
                return false;
            }
            return true;
        }

        #endregion

    }
}
