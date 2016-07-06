using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CanDaoCD.Pos.SystemConfig.Models;
using CanDaoCD.Pos.SystemConfig.Views;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Operates.FileOperate;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.PublicValues;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.PrintManage;

namespace CanDaoCD.Pos.SystemConfig.ViewModels
{
    /// <summary>
    /// 复写打印
    /// </summary>
    public class UcCopyPrintViewModel : ViewModelBase
    {
        #region 字段

        public UcCopyPrintView _userControl;
        #endregion

        #region 属性

        public UcCopyPrintModel Model { set; get; }

        #endregion


        #region 构造函数
        
        public UcCopyPrintViewModel()
        {
           
        }

        public void InitContor(UcCopyPrintView view)
        {
            _userControl = view;
            Init();
            _userControl.EnterAction = new Action(SaveConfig);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取用户控件
        /// </summary>
        /// <returns></returns>
        public UserControlBase GetUserCtl()
        {
            _userControl = new UcCopyPrintView();
            _userControl.DataContext = this;
            _userControl.EnterAction = new Action(SaveConfig);
            return _userControl;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 关闭检查配置保存
        /// </summary>
        public void CloseThis()
        {
            if (PvSystemConfig.VSystemConfig.IsEnabledPrint != Model.IsEnabledPrint)
            {
               
                    PvSystemConfig.VSystemConfig.IsEnabledPrint = Model.IsEnabledPrint;
                    PvSystemConfig.VSystemConfig.SerialNum = Model.SerialNum;
                    OXmlOperate.SerializerFile<MSystemConfig>(PvSystemConfig.VSystemConfig, PvSystemConfig.VSystemConfigFile);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Model = new UcCopyPrintModel();
          
            if (PvSystemConfig.VSystemConfig != null)
            {
                Model.IsEnabledPrint = PvSystemConfig.VSystemConfig.IsEnabledPrint;
                if (!Model.IsEnabledPrint)
                {
                    Model.SerialNum = 1;
                }
                else
                {
                    Model.SerialNum = PvSystemConfig.VSystemConfig.SerialNum;
                }
               
            }
            else
            {
                PvSystemConfig.VSystemConfig = new MSystemConfig();
              
            }
            Model.SaveAction = new Action(SaveConfig);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                PvSystemConfig.VSystemConfig.IsEnabledPrint = Model.IsEnabledPrint;
                PvSystemConfig.VSystemConfig.SerialNum = Model.SerialNum;
                OXmlOperate.SerializerFile<MSystemConfig>(PvSystemConfig.VSystemConfig, PvSystemConfig.VSystemConfigFile);
                OWindowManage.ShowMessageWindow("配置成功！", false);

                if (PrintService.CheckPrintSate())
                {
                    Model.PrintState = "热敏打印机连接成功";
                }
                else
                {
                    Model.PrintState = "热敏打印机连接失败";
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow("配置异常："+ex.Message, false);
            }
           

        }

        #endregion
    }
}
