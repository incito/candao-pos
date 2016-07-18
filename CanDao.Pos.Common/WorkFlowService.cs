using System;
using System.Threading.Tasks;
using System.Windows;
using CanDao.Pos.Model;

namespace CanDao.Pos.Common
{
    public class WorkFlowService
    {
        public static void Start(object param, WorkFlowInfo workFlowInfo, Window ownerWindow = null)
        {
            if (workFlowInfo == null)
                throw new ArgumentNullException("workFlowInfo");

            LoadingWindow wnd = null;
            var task = Task.Factory.StartNew(() => workFlowInfo.ProcessHandler != null ? workFlowInfo.ProcessHandler(param) : null);
            task.ContinueWith(obj => Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                if (wnd != null)
                    wnd.Close();

                if (workFlowInfo.ComplateHandler != null)
                {
                    var result = workFlowInfo.ComplateHandler(obj.IsFaulted ? obj.Exception : obj.Result);
                    if (result != null && result.Item1 && workFlowInfo.NextWorkFlowInfo != null)
                        Start(result.Item2, workFlowInfo.NextWorkFlowInfo);
                    if (result != null && !result.Item1 && workFlowInfo.ErrorWorkFlowInfo != null)
                        Start(result.Item2, workFlowInfo.ErrorWorkFlowInfo);
                }
            }));
            if (!string.IsNullOrEmpty(workFlowInfo.InfoMsg))
            {
                wnd = new LoadingWindow { InfoMsg = workFlowInfo.InfoMsg };
                if (!Equals(wnd, Application.Current.MainWindow))
                {
                    wnd.Owner = ownerWindow ?? Application.Current.MainWindow;
                    wnd.MaxWidth = wnd.Owner.Width;
                }
                wnd.ShowDialog();
            }
        }
    }
}