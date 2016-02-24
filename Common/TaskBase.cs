using System;
using System.ComponentModel;

namespace Common
{
    /// <summary>
    /// 后台线程基类。
    /// </summary>
    public class TaskBase
    {
        /// <summary>
        /// 处理前执行委托。
        /// </summary>
        public Action BeforeProcessAction { get; set; }

        /// <summary>
        /// 处理后执行委托。
        /// </summary>
        public Action AfterProcessAction { get; set; }

        /// <summary>
        /// 启动任务。
        /// </summary>
        /// <param name="param">参数。</param>
        /// <param name="processHandler">执行方法。</param>
        /// <param name="complateHandler">执行完成后。</param>
        /// <returns></returns>
        public void Start(object param, Func<object, object> processHandler, Action<object> complateHandler = null)
        {
            if (processHandler == null)
                throw new ArgumentNullException("processHandler");

            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += (sender, args) =>
                {
                    args.Result = processHandler(args.Argument);
                };
                bw.RunWorkerCompleted += (sender, args) =>
                {
                    if (AfterProcessAction != null)
                        AfterProcessAction();

                    if (complateHandler != null)
                        complateHandler(args.Result);
                };
                bw.RunWorkerAsync(param);
            }

            if (BeforeProcessAction != null)
                BeforeProcessAction();
        }
    }
}