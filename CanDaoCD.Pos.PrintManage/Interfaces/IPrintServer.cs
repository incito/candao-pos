using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.PrintManage
{
    /// <summary>
    /// 打印服务接口
    /// </summary>
    interface IPrintServer
    {
        #region 属性
        string ErrorString { set; get; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="port">串口号</param>
        /// <param name="popupMode">弹出模式</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        bool Init(int port, int popupMode=0, int timeOut = 30);

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        bool Release();

        /// <summary>
        /// 进卡
        /// </summary>
        /// <returns></returns>
        bool CardInOver();

        bool Print(List<string> templateList);

        /// <summary>
        /// 擦除弹出
        /// </summary>
        /// <returns></returns>
        bool EraseDischarge();

        bool CardState();

        /// <summary>
        /// 设置打印模板
        /// </summary>
        /// <param name="templateList"></param>
        /// <returns></returns>

        bool SetPrintTemplate(List<string> templateList);

        #endregion
    }
}
