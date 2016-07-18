using System;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 工作流信息。
    /// </summary>
    public class WorkFlowInfo
    {
        /// <summary>
        /// 工作流执行方法。
        /// </summary>
        public Func<object, object> ProcessHandler { get; set; }

        /// <summary>
        /// 工作流执行完成。
        /// </summary>
        public Func<object, Tuple<bool, object>> ComplateHandler { get; set; }

        /// <summary>
        /// 工作流执行时提示信息。
        /// </summary>
        public string InfoMsg { get; set; }

        /// <summary>
        /// 正确时下一步工作任务。
        /// </summary>
        public WorkFlowInfo NextWorkFlowInfo { get; set; }

        /// <summary>
        /// 错误时执行的工作任务。
        /// </summary>
        public WorkFlowInfo ErrorWorkFlowInfo { get; set; }

        public WorkFlowInfo(Func<object, object> processHandler, Func<object, Tuple<bool, object>> complateHandler, string infoMsg = null)
        {
            ProcessHandler = processHandler;
            ComplateHandler = complateHandler;
            InfoMsg = infoMsg;
        }
    }
}