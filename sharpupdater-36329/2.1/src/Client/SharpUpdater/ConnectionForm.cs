using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public partial class ConnectionForm : BaseForm
    {
        #region Constants and Fields

        private ReleaseList _localRelease;
        private ReleaseList _remoteRelease;
        private readonly BackgroundWorker _worker;

        #endregion

        #region Constructors and Destructors

        public ConnectionForm()
        {
            InitializeComponent();

            _worker = new BackgroundWorker();
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (runWorkerCompletedEventArgs.Error != null)
            {
                Application.Exit();
                Common.Start(_localRelease, true);
                return;
            }
            _remoteRelease = runWorkerCompletedEventArgs.Result as ReleaseList;

            if (_localRelease.Compare(_remoteRelease) == 0)
            {
                Application.Exit();
                Common.Start(_localRelease, false);
                return;
            }
            //Hide();

            if (CheckProcessing() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }
            var form = new UpdateForm(_localRelease, _remoteRelease);
            form.ShowDialog();
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
           e.Result = FileUtil.ReadReleaseList(e.Argument.ToString());
        }

        #endregion

        #region Methods

        private DialogResult CheckProcessing()
        {
            string exeName = _localRelease.ApplicationStart.Substring(0, _localRelease.ApplicationStart.Length - 4);
            if (Process.GetProcessesByName(exeName).Length > 0)
            {
                DialogResult rs = MessageBox.Show(
                    string.Format(Common.GetLocalText("CloseRunning"), exeName),
                    Common.GetLocalText("Warning"),
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
                if (rs == DialogResult.Retry)
                {
                    return CheckProcessing();
                }
                return rs;
            }
            return DialogResult.OK;
        }

        private void ConnectionFormLoad(object sender, EventArgs e)
        {
            string localXmlPath = Path.Combine(Application.StartupPath, Constants.ReleaseConfigFileName);
            _localRelease = FileUtil.ReadReleaseList(localXmlPath);

            _worker.RunWorkerAsync(_localRelease.ReleaseUrl + "/" + Constants.ReleaseConfigFileName);
        }

        #endregion

        private void ConnectionForm_Shown(object sender, EventArgs e)
        {
           

        }
    }
}