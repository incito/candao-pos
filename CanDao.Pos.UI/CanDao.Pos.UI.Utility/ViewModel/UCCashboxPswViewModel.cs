
using System;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.UI.Utility.Model;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.Common;


namespace CanDao.Pos.UI.Utility.ViewModel
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
                MessageDialog.Warning("密码不能为空。");
                return;
            }

            if (!Model.Password.Equals(_psw))
            {
                MessageDialog.Warning("密码验证错误，请输入正确密码。");
                return;
            }
            else
            {
                //验证确定关闭
                _userControl.DialogResult = true;
            }

        }

        #endregion

        #region 公共方法
        public UCCashboxPswView GetUserCtl()
        {
            _userControl = new UCCashboxPswView();
            _userControl.DataContext = this;
            _userControl.EnterAction = new Action(CheckPswHandel);
            return _userControl;
        }
        #endregion
    }
}
