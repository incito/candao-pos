using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public partial class ConnectionForm : BaseForm
    {
        #region Constants and Fields

        private readonly Monitor _worker;

        #endregion

        #region Constructors and Destructors

        public ConnectionForm()
        {
            InitializeComponent();

            var manifestFileName = Path.Combine(Application.StartupPath, Manifest.ManifestFileName);
            if (!File.Exists(manifestFileName))
            {
                var releaseListFileName = Path.Combine(Application.StartupPath, "ReleaseList.xml");
                if (!File.Exists(releaseListFileName))
                {
                    return;
                }
                GenerateManifestFromReleaseList(manifestFileName, releaseListFileName);
            }
            _worker = new Monitor(manifestFileName,null);

            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (runWorkerCompletedEventArgs.Error != null)
            {
                Application.Exit();
                Common.Start(_worker.LocalManifest, true);
                return;
            }
        
            if (!_worker.HasNewVersion)
            {
                Application.Exit();
                Common.Start(_worker.LocalManifest, false);
                return;
            }
            //Hide();

            if (CheckProcessing() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }
            var form = new UpdateForm(_worker.LocalManifest, _worker.RemoteManifest);
            form.ShowDialog();
        }

        #endregion

        #region Methods

        private DialogResult CheckProcessing()
        {
            string exeName = _worker.LocalManifest.EntryPoint.Substring(0, _worker.LocalManifest.EntryPoint.Length - 4);
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


            _worker.RunWorkerAsync();
        }

        #endregion

        private void ConnectionForm_Shown(object sender, EventArgs e)
        {
           

        }


        private void GenerateManifestFromReleaseList(string fileName,string oldFileName)
        {
            var doc = new XmlDocument();
            doc.Load(oldFileName);
            doc.InnerXml =
                doc.InnerXml.Replace("ReleaseList", "Manifest")
                    .Replace("ApplicationStart", "EntryPoint")
                    .Replace("UpdateDescription", "ReleaseNote");
            doc.Save(fileName);
        }
    }
}