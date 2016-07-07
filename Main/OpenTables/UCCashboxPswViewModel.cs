using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Operates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Controls.CSystem;

namespace KYPOS.OpenTables
{
    /// <summary>
    /// 钱箱密码验证
    /// </summary>
    public class UCCashboxPswViewModel : ViewModelBase
    {
        #region 字段

        private string _psw = "518818";
        private UCCashboxPswView _userControl;
        private Action _enterAction;

        #endregion

        #region 属性

        public UCCashboxPswModel Model { set; get; }

        #endregion

        #region 构造函数

        public UCCashboxPswViewModel()
        {
            Model = new UCCashboxPswModel();
           
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 验证事件
        /// </summary>
        private void CheckPswHandel()
        {
            if (string.IsNullOrEmpty(Model.Password))
            {
                OWindowManage.ShowMessageWindow("密码不能为空。", false);
                return;
            }

            if (!Model.Password.Equals(_psw))
            {
                OWindowManage.ShowMessageWindow("密码验证错误，请输入正确密码。", false);
                return;
            }
            else
            {
                //验证确定关闭
                _userControl.UcCloseaAction(true);
            }

        }

        #endregion

        #region 公共方法
        public UserControlBase GetUserCtl()
        {
            _userControl = new UCCashboxPswView();
            _userControl.DataContext = this;
            _userControl.EnterAction = new Action(CheckPswHandel);
            return _userControl;
        }
        #endregion
    }
}
