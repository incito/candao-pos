using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public partial class UpdateForm : BaseForm
    {
        #region Constants and Fields

        private static readonly string FinishText = Common.GetLocalText("Completed");
        private static readonly string RetryText = Common.GetLocalText("Retry");
        private readonly Manifest _localRelease;
        private readonly Manifest _remoteRelease;
        private readonly string _tempDir;
        private readonly long _totalSize;
        private bool _downloaded;
        private string _totalSizeDesc;
        private readonly IUpdatesChecker _updateProxy;
        private readonly Downloader _worker;
        private readonly WebClient _wc;
        #endregion

        #region Constructors and Destructors

        public UpdateForm(Manifest localRelease, Manifest remoteRelease)
        {
            InitializeComponent();

            btnUpgrade.Focus();

            _tempDir = Path.Combine(Path.GetTempPath(), localRelease.AppName);
            if(!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
            _localRelease = localRelease;
            _remoteRelease = remoteRelease;
            _updateProxy = Common.GetUpdateProxy(_remoteRelease);
            //progressBar.Style = remoteRelease.Packaged ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;


            
            if (remoteRelease.Packaged)
            {
                var diff = _localRelease.GetDifferences(_remoteRelease, out _totalSize);
                 _wc = new WebClient();
                _wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                _wc.DownloadFileCompleted += wc_DownloadFileCompleted;
               
            }
            else
            {
                _worker = new Downloader(_localRelease,_remoteRelease,_tempDir) {WorkerReportsProgress = true};
                    // !remoteRelease.Packaged };
                //if(!remoteRelease.Packaged)
                _worker.ProgressChanged += WorkProgressChanged;
                _worker.RunWorkerCompleted += RunWorkerCompleted;
                _totalSize = _worker.TotalBytes;
            }
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Finish();
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var percentage = (int)((Math.Round(Convert.ToDecimal(e.BytesReceived) / Convert.ToDecimal(e.TotalBytesToReceive) * 1.0M, 2)) * 100);
            progressBar.Value = percentage;
        }

        void WorkProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as Downloader;
            progressBar.Value = e.ProgressPercentage;
            lblSize.Text = string.Format("{0}/{1}", FileUtil.FormatFileSizeDescription(worker.ReceivedBytes), _totalSizeDesc);
        }


        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                FileUtil.DeleteFolder(_tempDir);
                MessageBox.Show(e.Result + Common.GetLocalText("DownFailed") + Environment.NewLine + e.Error.Message);
                OptionalUpdate = true;
                btnUpgrade.Text = RetryText;
                return;
            }

          
        
            Finish();
        }

        private void Finish()
        {
            var installer = new Installer();
            installer.Install(_tempDir, Application.StartupPath, _remoteRelease, Common.IgnoreFiles);

            _remoteRelease.SaveManifest(Path.Combine(Application.StartupPath, Manifest.ManifestFileName));
            btnUpgrade.Enabled = true;
            btnUpgrade.Text = FinishText;
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
            if (_wc != null)
            {
                _wc.DownloadFileAsync(new Uri(_remoteRelease.ZipUrl), Path.Combine(_tempDir, _remoteRelease.Files[0].FileName));
                return;
            }
            _downloaded = false;
            progressBar.Value = 0;
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
            OptionalUpdate = _localRelease.CompareTo(_remoteRelease.MinVersion) >= 0;
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
                        boxDes.Text = _remoteRelease.ReleaseNote;
                    }
                }
                catch 
                {
                    boxDes.Text = _remoteRelease.ReleaseNote;
                }
            
            _totalSizeDesc = FileUtil.FormatFileSizeDescription(_totalSize);
            if (_totalSize == 0)
            {
                btnUpgrade.Text = FinishText;
                Application.Exit();
                Common.Start(_localRelease);
                return;
            }

            progressBar.Maximum = 100;
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