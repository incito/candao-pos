using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;
using CnSharp.Windows.Updater.Util.UpdateProxy;

namespace CnSharp.Windows.Updater
{
    public partial class UpdateForm : BaseForm
    {
        #region Constants and Fields

        private static readonly string _finishText = Common.GetLocalText("Completed");
        private static readonly string _retryText = Common.GetLocalText("Retry");
        private readonly ReleaseList _localRelease;
        private readonly ReleaseList _remoteRelease;
        private readonly string _tempDir;
        private ReleaseFile[] _diff;
        private bool _downloaded;
        private long _totalSize;
        private string _totalSizeDesc;
        private long _downloadedSize;
        private readonly IUpdateProxy _updateProxy;
        private readonly BackgroundWorker _worker;
        #endregion

        #region Constructors and Destructors

        public UpdateForm(ReleaseList localRelease, ReleaseList remoteRelease)
        {
            InitializeComponent();

            btnUpgrade.Focus();

            _tempDir = Path.Combine(Path.GetTempPath(), localRelease.AppName);
            if(!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
            _localRelease = localRelease;
            _remoteRelease = remoteRelease;
            _updateProxy = Common.GetUpdateProxy(_remoteRelease);
            _worker = new BackgroundWorker {WorkerReportsProgress = true};
            _worker.ProgressChanged += WorkProgressChanged;
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += RunWorkerCompleted;
        }


        void DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (ReleaseFile file in _diff)
            {
                try
                {
                    DownloadHelper.DownloadFile(
                        _tempDir,
                        _remoteRelease.ReleaseUrl + "/" + _remoteRelease.ReleaseVersion,
                        file.FileName,
                        _worker,
                        _totalSize,
                       ref  _downloadedSize);
                }
                catch 
                {
                    e.Result = file.FileName;
                    throw;
                }
            }
            
        }

        void WorkProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            //_downloadedSize += (long)e.UserState;
            lblSize.Text = string.Format("{0}/{1}", DownloadUtil.FormatFileSizeDescription(_downloadedSize), _totalSizeDesc);
        }


        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                FileUtil.DeleteFolder(_tempDir);
                MessageBox.Show(e.Result + Common.GetLocalText("DownFailed") + Environment.NewLine + e.Error.Message);
                //+ex.Message+ex.StackTrace);
                OptionalUpdate = true;
                btnUpgrade.Text = _retryText;
                return;
            }
            //try
            //{
                var installer = new Installer();
                //installer.ZipCompleted += installer_ZipCompleted;
                installer.Install(_tempDir, Application.StartupPath, _remoteRelease);
            //}
            //catch (Exception ex)
            //{
            //    //FileUtil.DeleteFolder(_tempDir);
            //    MessageBox.Show(ex.Message, Common.GetLocalText("Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    OptionalUpdate = true;
            //    btnUpgrade.Text = _retryText;
            //    return;
            //}

            //if(_remoteRelease.Packaged)
            //    return;
        
          Finish();
        }

        private void Finish()
        {
               FileUtil.SaveReleaseList(_remoteRelease,Path.Combine(Application.StartupPath, Constants.ReleaseConfigFileName));
            btnUpgrade.Enabled = true;
            btnUpgrade.Text = _finishText;
            _downloaded = true;

            try
            {
                FileUtil.DeleteFolder(_tempDir);
                CreateShortcut(_remoteRelease);
            }
            finally
            {
                Application.Exit();
                Common.Start(_localRelease);
            }
        }

        #endregion

        #region Properties

        private bool OptionalUpdate
        {
            set { isEnableCloseButton = btnUpgrade.Enabled = btnCancel.Enabled = value; }
        }

        #endregion

        #region Methods

        private void DoUpgrade()
        {
            _downloaded = false;
            progressBar1.Value = 0;
            _downloadedSize = 0;
            if(_worker.IsBusy)
                return;
            _worker.RunWorkerAsync();
        }

        private void FormUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            lblSize.Text = string.Empty;
            Init();
        }

        private void Init()
        {
            OptionalUpdate = _localRelease.Compare(_remoteRelease.MinVersion) >= 0;
            lblTitle.Text = string.Format(
                lblTitle.Text,
                _localRelease.ReleaseVersion,
                _remoteRelease.ReleaseVersion,
                _remoteRelease.ReleaseDate);
                try
                {
                    if (_updateProxy != null)
                        boxDes.Text = _updateProxy.GetUpdateLogBetweenVersion(_localRelease.ReleaseVersion,
                                                                              _remoteRelease.ReleaseVersion);
                    else
                    {
                        boxDes.Text = _remoteRelease.UpdateDescription;
                    }
                }
                catch 
                {
                    boxDes.Text = _remoteRelease.UpdateDescription;
                }
            _diff = _localRelease.GetDifferences(_remoteRelease, out _totalSize);
            _totalSizeDesc = DownloadUtil.FormatFileSizeDescription(_totalSize);
            if (_diff == null || _totalSize == 0)
            {
                btnUpgrade.Text = _finishText;
                return;
            }

            progressBar1.Maximum = 100;
            if (!btnUpgrade.Enabled)
            {
                DoUpgrade();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Directory.Delete(tempPath, true);
            Application.Exit();
            Common.Start(_localRelease);
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            btnUpgrade.Enabled = false;
            btnCancel.Enabled = false;
            if (!_downloaded)
            {
                DoUpgrade();
            }
            else
            {
                Application.Exit();
                Common.Start(_localRelease);
            }
        }

        #endregion
    }
}