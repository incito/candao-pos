using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Models.VipModels;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using WebServiceReference;
using Models;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    /// <summary>
    /// 会员基本信息修改
    /// </summary>
    public class UcVipModifyVipInfoViewModel : ViewModelBase
    {
        #region 字段

        private UcVipModifyVipInfoView _userControl;
       
        #endregion

        #region 属性

        public UcVipModifyVipInfoModel Model { set; get; }

        private MVipChangeInfo VipChangeInfo { set; get; }
        #endregion

        #region 事件

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

        public UcVipModifyVipInfoViewModel(MVipChangeInfo vipInfo)
        {

            Model = new UcVipModifyVipInfoModel();
            VipChangeInfo = vipInfo;

            Model.OUserName = VipChangeInfo.VipName;
            Model.TelNum = VipChangeInfo.TelNum;
            Model.CardNum = VipChangeInfo.CardNum;
            Model.UserName = VipChangeInfo.VipName;
            Model.Birthday = DateTime.Parse(VipChangeInfo.Birthday);
            if (VipChangeInfo.Sex == 0)
            {
                Model.SexNan = true;
                Model.SexNv = false;
            }
            else
            {
                Model.SexNan = false;
                Model.SexNv = true;
            }

            SureCommand = new RelayCommand(SureHandel);
            CloseCommand = new RelayCommand(CloseHandel);

        }

        #endregion

        #region 公共方法

        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipModifyVipInfoView();
            _userControl.DataContext = this;

            return _userControl;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 确定
        /// </summary>
        private void SureHandel()
        {
            if (IsCheckInput())
            {

                VipChangeInfo.VipName = Model.UserName;
                VipChangeInfo.Birthday = Model.Birthday.ToString("yyyy-MM-dd");
                if (Model.SexNan)
                {
                    VipChangeInfo.Sex = 0;
                }
                else
                {
                    VipChangeInfo.Sex = 1;
                }

                TCandaoRetBase ret = CanDaoMemberClient.VipChangeInfo(Globals.branch_id, VipChangeInfo);
                if (ret.Ret)
                {
                    OWindowManage.ShowMessageWindow("修改成功!", false);
                    CloseStateHandel(true);

                }
                else
                {
                    OWindowManage.ShowMessageWindow("修改失败：" + ret.Retinfo, false);
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
            if (string.IsNullOrEmpty(Model.UserName))
            {
                OWindowManage.ShowMessageWindow("用户名不能为空！", false);
                return false;
            }
            return true;
        }

        #endregion

    }
}
