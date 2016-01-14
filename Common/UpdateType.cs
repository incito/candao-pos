///*************************************************************************/
///*
///* 文件名    ：UpdateType.cs                                      
///* 程序说明  : 数据窗体的操作类型
///* 原创作者  ： 
///* 
///* Copyright 2010-2011 
///**************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 数据窗体的操作类型
    /// </summary>
    public enum UpdateType
    {
        None,

        /// <summary>
        /// 新增状态
        /// </summary>
        Add,

        /// <summary>
        /// 修改状态
        /// </summary>
        Modify
    }
}
