﻿using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Operates.FileOperate;
using CanDaoCD.Pos.Common.PublicValues;
using CanDaoCD.Pos.SystemConfig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.SystemConfig.ViewModels
{
    /// <summary>
    /// 钱箱权限验证
    /// </summary>
    public class UCCashboxViewModel : ViewModelBase
    {
        #region 属性
        public UCCashboxModel Model { set; get; }
        #endregion

        #region 构造函数
        public UCCashboxViewModel()
        {
            Model = new UCCashboxModel();
            Model.PropertyChanged += Model_PropertyChanged;
            if (PvSystemConfig.VSystemConfig != null)
            {
                Model.IsEnabledCheck = PvSystemConfig.VSystemConfig.IsEnabledCheck;
            }
            else
            {
                PvSystemConfig.VSystemConfig = new MSystemConfig();
            }
        }
        /// <summary>
        /// 启用变更保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="IsEnabledCheck")
            {
                PvSystemConfig.VSystemConfig.IsEnabledCheck = Model.IsEnabledCheck;
                OXmlOperate.SerializerFile<MSystemConfig>(PvSystemConfig.VSystemConfig, PvSystemConfig.VSystemConfigFile);
            }
        }
        #endregion
    }
}