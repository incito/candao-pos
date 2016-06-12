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

        private void Init()
        {
            Model = new UcCopyPrintModel();
            //PvSystemConfig.VSystemConfig = OXmlOperate.DeserializeFile<MSystemConfig>(_fileName);
            Model.IsEnabledPrint = PvSystemConfig.VSystemConfig.IsEnabledPrint;
            Model.SerialNum = PvSystemConfig.VSystemConfig.SerialNum;
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
                MessageBox.Show("配置成功！", "成功提示");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "保存异常");
            }
           

        }

        #endregion
    }
}
