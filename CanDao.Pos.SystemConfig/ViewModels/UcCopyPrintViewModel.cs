using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.SystemConfig.Models;
using CanDao.Pos.SystemConfig.Views;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Operates.FileOperate;
using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Models;
using CanDao.Pos.Common.PublicValues;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.ReportPrint;


namespace CanDao.Pos.SystemConfig.ViewModels
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
                MessageDialog.Warning("配置成功！");

                if (CopyReportHelper.CheckPrintSate())
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
                MessageDialog.Warning("配置异常：" + ex.MyMessage());
            }
           

        }

        #endregion
    }
}
