using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Operates;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using Models.CandaoMember;
using WebServiceReference;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    public class UcVipSelectViewModel : ViewModelBase
    {
        #region 字段

        private UcVipSelectView _userControl;

        private WinShowInfoViewModel _winShowInfo;

        #endregion

        #region 属性

        public UcVipSelectModel Model { set; get; }

        #endregion

        #region 事件

        /// <summary>
        /// 修改密码
        /// </summary>
        public RelayCommand ModifyPswCommand { set; get; }

        /// <summary>
        /// 会员储值
        /// </summary>
        public RelayCommand StoredValueCommand { set; get; }

        /// <summary>
        /// 会员注销
        /// </summary>
        public RelayCommand LogOffCommand { set; get; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public UcVipSelectViewModel()
        {
            Model = new UcVipSelectModel();
            Model.TextEnterAction = new Action<TextBox>(SelectHandel);
       
            StoredValueCommand=new RelayCommand(StoredValueHandel);
            LogOffCommand=new RelayCommand(LogOffHandel);
            ModifyPswCommand=new RelayCommand(ModifyPswHandel);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询事件
        /// </summary>
        private void SelectHandel(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "TexTelNum": //电话号码
                {
                    SelectModel(textBox.Text, Model.Psw);
                    break;
                }
                case "TexCardNum": //实体卡号
                {
                    SelectModel(textBox.Text, Model.Psw);
                    break;
                }
            }
        }
        private void ModifyPswHandel()
        {
            var veModel = new UcVipModifyPswViewModel(Model.TelNum);
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }
        private void StoredValueHandel()
        {
            var veModel = new UcVipRechargeViewModel(Model);
   
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }

        /// <summary>
        /// 会员注销
        /// </summary>
        private void LogOffHandel()
        {
            try
            {
                if (OWindowManage.ShowMessageWindow(
               string.Format("是否要注销[{0}]?", Model.UserName), true))
                {
                    var resInfo = CanDaoMemberClient.CardCancellation(Globals.branch_id, "", Model.CardNum, "", "");
                    if (resInfo.Ret)
                    {
                        OWindowManage.ShowMessageWindow(
                            string.Format("[{0}]注销成功！", Model.CardNum), false);
                        Model = new UcVipSelectModel();
                    }
                    else
                    {
                        OWindowManage.ShowMessageWindow(
                           string.Format("注销失败：[{0}]", resInfo.Retinfo), false);
                    }
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
             string.Format("会员注销失败[{0}-{1}]：{2}", Globals.branch_id, Model.CardNum, ex.Message), false);
            }
           
        }

        /// <summary>
        /// 根据查询返回值创建信息
        /// </summary>
        /// <param name="info"></param>
        private void SelectModel(string selectNum,string psw)
        {
            try
            {
                var info = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", selectNum, psw);
                if (info.Retcode.Equals("0"))
                {
                    Model.UserName = info.Name;
                    if (info.Gender == 0)
                    {
                        Model.Sex = "男";
                    }
                    else
                    {
                        Model.Sex = "女";
                    }
                    Model.TelNum = info.Mobile;
                    Model.Birthday = info.Birthday;
                    Model.Integral = info.Integraloverall.ToString();
                    Model.Balance = info.Storecardbalance.ToString();
                    Model.CardNum = info.Mcard;

                    Model.IsOper = true;//启用操作区域
                }
                else
                {
                    OWindowManage.ShowMessageWindow(
                     string.Format("会员查询错误：{0}", info.Retinfo), true);
                    Model.IsOper = false;//禁用操作区域
                }

               
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
               string.Format("会员查询失败[{0}-{1}]：{2}",Globals.branch_id,selectNum, ex.Message), false);
            }
           
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取用户控件
        /// </summary>
        /// <returns></returns>
        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipSelectView();
            _userControl.DataContext = this;
            return _userControl;
        }

        #endregion
    }
}
