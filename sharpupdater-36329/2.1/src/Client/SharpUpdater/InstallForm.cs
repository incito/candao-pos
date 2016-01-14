using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public partial class InstallForm : BaseForm
    {
        #region Constants and Fields

        private readonly string _configFile;
        private readonly string _tempDir;
        private ReleaseList _remoteRelease;
        private BackgroundWorker _worker;

        #endregion

        #region Constructors and Destructors

        public InstallForm(string tempDir, string configFile)
        {
            InitializeComponent();
            this._tempDir = tempDir;
            this._configFile = configFile;
        }

        #endregion

        #region Methods

        private void ConnectionFormLoad(object sender, EventArgs e)
        {
            bool downloaded = false;
            string newVersionXmlPath = "";
            if (!string.IsNullOrEmpty(_tempDir) && _tempDir.Trim().Length > 0)
            {
                if (Directory.Exists(_tempDir))
                {
                    newVersionXmlPath = Path.Combine(_tempDir, Constants.ReleaseConfigFileName);
                    downloaded = true;
                }
                else if (File.Exists(_tempDir) && !string.IsNullOrEmpty(_configFile) && _configFile.Trim().Length > 0)
                {
                    newVersionXmlPath = _configFile;
                    downloaded = true;
                }
            }
            if (downloaded)
            {
                try
                {
                    _remoteRelease = FileUtil.ReadReleaseList(newVersionXmlPath);
                    lblStatus.Text = Common.GetLocalText("UpdateTo") + "V" + _remoteRelease.ReleaseVersion;
                    Install();
                }
                catch
                {
                    DialogResult = DialogResult.No;
                }
            }
            else
            {
                DialogResult = DialogResult.No;
            }
        }

        private void Install()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            _worker.RunWorkerAsync(_tempDir);
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            string tempDir = e.Argument.ToString();
            var installer = new Installer();
            //installer.ZipCompleted += new EventHandler(installer_ZipCompleted);
            installer.Install(tempDir, Application.StartupPath, _remoteRelease);
            if (Directory.Exists(tempDir))
            {
                FileUtil.DeleteFolder(tempDir);
            }
            else if (File.Exists(tempDir))
            {
                File.Delete(tempDir);
                if (_configFile.Length > 0 && File.Exists(_configFile))
                {
                    File.Delete(_configFile);
                }
            }
            CreateShortcut(_remoteRelease);
        }

        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult result = (e.Error != null) ? DialogResult.No : DialogResult.Yes;
            if (result == DialogResult.Yes)
            {
                Common.Start(_remoteRelease);
                Application.Exit();
            }
            else
            {
                var connForm = new ConnectionForm();
                connForm.Show();
                Close();
            }
        }

        #endregion
    }
}