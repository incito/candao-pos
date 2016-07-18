using System;
using System.Windows;
using JunLan.Common.Base;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 后台任务线程。
    /// </summary>
    public class TaskService
    {
        /// <summary>
        /// 开始执行后台任务。
        /// </summary>
        /// <param name="param">处理参数。</param>
        /// <param name="processHandler">处理过程委托。</param>
        /// <param name="complateHandler">完成后执行委托。</param>
        /// <param name="infoMsg">提示信息，如果纯后台执行则传null即可。</param>
        /// <param name="ownerWindow">所属窗口。</param>
        public static void Start(object param, Func<object, object> processHandler, Action<object> complateHandler = null, string infoMsg = "处理中...", Window ownerWindow = null)
        {
            TaskBase taskBase = new TaskBase();
            LoadingWindow wnd = null;

            if (!string.IsNullOrEmpty(infoMsg))
            {
                taskBase.BeforeProcessAction = delegate
                {
                    wnd = new LoadingWindow { InfoMsg = infoMsg };
                    if (!Equals(wnd, Application.Current.MainWindow))
                    {
                        wnd.Owner = ownerWindow ?? Application.Current.MainWindow;
                        wnd.MaxWidth = wnd.Owner.Width;
                    }
                    wnd.ShowDialog();
                };

                taskBase.AfterProcessAction = delegate
                {
                    if (wnd != null)
                        wnd.Close();
                };
            }
            taskBase.Start(param, processHandler, complateHandler);
        }
    }
}