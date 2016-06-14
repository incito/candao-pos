///*************************************************************************/
///*
///* 文件名    ：frmBase.cs                              
///* 程序说明  : 所有窗体基类,继承XtraForm用于设置皮肤
///* 原创作者  ： 
///* 
///* Copyright 2014-205
///*
///**************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common;

namespace Library
{
    /// <summary>
    /// 所有窗体基类,继承XtraForm用于设置皮肤
    /// </summary>
    public partial class frmBase : XtraForm
    {

        public DevExpress.LookAndFeel.DefaultLookAndFeel DefaultLookAndFeel;
        private int maxnum = 0;
        public frmBase()
        {
            InitializeComponent();
            this.DefaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.LoadSkin();
        }

        #region IFormBase 成員

        /// <summary>
        /// 加载皮肤
        /// </summary>
        public void LoadSkin()
        {
            if (SystemConfig.CurrentConfig != null) SetSkin(SystemConfig.CurrentConfig.SkinName);
        }

        /// <summary>
        /// 设置窗体皮肤
        /// </summary>
        /// <param name="skinName">名称</param>
        public void SetSkin(string skinName)
        {
            this.DefaultLookAndFeel.LookAndFeel.SkinName = skinName;
        }

        #endregion

        private void frmBase_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 打开对话框
        /// </summary>
        /// <param name="msg">本次对话内容</param>
        /// <returns></returns>
        public static bool AskQuestion(string msg)
        {
            return frmAskQuestion.ShowAskQuestion(msg);

        }
        public static bool ShowInputNum(string msg, out int inputNum, int maxnum)
        {
            return frmInputNum.ShowInputNum(msg, out inputNum, maxnum);

        }
        public static bool ShowInputNum(string msg, string lbltext, out int inputNum, int maxnum)
        {
            return frmInputNum.ShowInputNum(msg, lbltext, out inputNum, maxnum);

        }
        public static bool ShowInputNum(string msg, string lbltext, out int inputNum, double maxnum)
        {
            return frmInputNum.ShowInputNum(msg, lbltext, out inputNum, maxnum);

        }
        public static bool ShowInputNum(string msg, string lbltext, out double inputNum, double maxnum)
        {
            return frmInputNum.ShowInputNum(msg, lbltext, out inputNum, maxnum);

        }
        public static bool Warning(string msg)
        {
            return frmWarning.ShowWarning(msg);

        }
    }
}