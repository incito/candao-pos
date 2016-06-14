using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CanDao.Pos.SystemConfig.Models;
using CanDao.Pos.SystemConfig.Views;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Operates.FileOperate;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.PublicValues;
using CanDaoCD.Pos.Common.Operates;

namespace CanDao.Pos.SystemConfig.ViewModels
{
    /// <summary>
    /// 复写打印
    /// </summary>
    public class UcCopyPrintViewModel : ViewModelBase
    {
        #region 字段

        private UserControlBase _userControl;
        #endregion

        #region 属性

        public UcCopyPrintModel Model { set; get; }

        #endregion


        #region 构造函数
        
        public UcCopyPrintViewModel()
        {
            Init();
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
                if (OWindowManage.ShowMessageWindow("配置已修改，是否进行保存？", true))
                {
                    PvSystemConfig.VSystemConfig.IsEnabledPrint = Model.IsEnabledPrint;
                    PvSystemConfig.VSystemConfig.SerialNum = Model.SerialNum;
                    OXmlOperate.SerializerFile<MSystemConfig>(PvSystemConfig.VSystemConfig, PvSystemConfig.VSystemConfigFile);
                }
               
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Model = new UcCopyPrintModel();
          
            //PvSystemConfig.VSystemConfig = OXmlOperate.DeserializeFile<MSystemConfig>(_fileName);
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
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow("配置异常："+ex.Message, false);
            }
           

        }

        #endregion
    }
}
