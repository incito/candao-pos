using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using CanDaoCD.Pos.Common.Controls.CSystem;

namespace CanDaoCD.Pos.Common.Operates
{
    /// <summary>
    /// 异步加载服务类
    /// </summary>
    public class AsyncLoadServer
    {
        #region 字段

        private BackgroundWorker _backWorker;

        private AsynWindow _asynWindow;

        private Action _processMethod;

        #endregion

        #region 属性

        public string ErrorMessage { set; get; }

        #endregion

        #region 事件

        /// <summary>
        /// 工作状态-0：正常；1：取消;2：异常
        /// </summary>
        public Action<int> ActionWorkerState;

        #endregion

        #region 公共方法

        /// <summary>
        ///  初始化信息
        /// </summary>
        public void Init()
        {
            this._backWorker = new System.ComponentModel.BackgroundWorker();
            this._backWorker.WorkerReportsProgress = true;
            this._backWorker.WorkerSupportsCancellation = true;
            this._backWorker.DoWork += new DoWorkEventHandler(backWorker_DoWork);
            this._backWorker.ProgressChanged += new ProgressChangedEventHandler(backWorker_ProgressChanged);
            this._backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backWorker_RunWorkerCompleted);
        }

        public void Start(Action processMethod)
        {
          
                _processMethod = processMethod;
                _asynWindow = new AsynWindow();
                _asynWindow.Show();
                _asynWindow.ActionCancel = Stop;
                _backWorker.RunWorkerAsync();
         
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="showText">显示内容</param>
        public void SetMessage(string showText)
        {
            _asynWindow.SetShowText(string.Format("{0}", showText));
        }

        public void Stop()
        {
            _backWorker.CancelAsync();
            _asynWindow.Close();
            _backWorker.Dispose();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (_processMethod != null)
                {
                    _processMethod();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// 进度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ProgressWorker(2);
            }

            if (e.Error != null)
            {
                ProgressWorker(2);
            }
            else if (e.Cancelled)
            {
                ProgressWorker(1);
            }
            else
            {
                ProgressWorker(0);
            }
        }

        /// <summary>
        /// 处理工作状态
        /// </summary>
        /// <param name="state"></param>
        private void ProgressWorker(int state)
        {
            if (ActionWorkerState != null)
            {
                ActionWorkerState(state);
            }
            _asynWindow.Close();
        }

        #endregion
    }
}
