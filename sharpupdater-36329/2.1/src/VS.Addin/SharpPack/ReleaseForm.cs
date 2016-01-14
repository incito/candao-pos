using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CnSharp.IO;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater.SharpPack
{
    public partial class ReleaseForm : Form
    {
        private readonly string _buildDir;
        private readonly List<FileListItem> _folderAndFiles;
        private readonly ReleaseList _releaseList;
        private int _exeCounter;
        private bool _filesVersionGot;
        private int _icoCounter;
        private ReleaseList _remoteList;
        private int _rootLength;
        private string _versionFolder;
        private string _zipName;
        private string _zipPath;

        public ReleaseForm(string buildDir, List<FileListItem> folderAndFiles, ReleaseList releaseList)
        {
            InitializeComponent();
            _buildDir = buildDir;
            _folderAndFiles = folderAndFiles;
            _releaseList = releaseList;
        }


        private void BindReleaseInfo()
        {
            txtAppName.Text = _releaseList.AppName;
            txtCompany.Text = _releaseList.Company;
            txtWebsite.Text = _releaseList.WebSite;
            txtReleaseVer.Text = _releaseList.ReleaseVersion;
            txtMinVer.Text = _releaseList.MinVersion;
            var url = _releaseList.ReleaseUrl;
            if (url != null && !string.IsNullOrEmpty(url) && url.Trim().Length > 0 &&
                url.ToLower().EndsWith(Common.ZipExt))
            {
                url = url.Substring(0, url.LastIndexOf("/", StringComparison.Ordinal));
                chkZip.Checked = true;
            }
            txtRelaseUrl.Text = url;
            var desc = _releaseList.UpdateDescription;
            if (!string.IsNullOrEmpty(desc))
            {
                desc = desc.Replace("\n", Environment.NewLine);
            }
            txtUplog.Text = desc;
            chkZip.Checked = _releaseList.Packaged;
        }


        private void GetFilesVersion()
        {
            if (chkZip.Checked)
                return;
            if (PackSettings.Default.QueryFilesVersionFromReleaseServer)
            {
                var url = txtRelaseUrl.Text.Trim();
                _remoteList = null;
                if (url.Length > 0)
                {
                    if (!url.EndsWith("/"))
                        url += "/";
                    url += Constants.ReleaseConfigFileName;
                    try
                    {
                        _remoteList = FileUtil.ReadReleaseList(url);
                    }
                    catch
                    {
                        _remoteList = null;
                    }
                }
            }
            foreach (DataGridViewRow gr in gridFileList.Rows)
            {
                var fileName = gr.Cells[ColFileName.Name].Value.ToString().ToLower();
                if (fileName.EndsWith(".exe") || fileName.EndsWith(".dll") || fileName.StartsWith("["))
                    continue;
                var got = false;
                if (_remoteList != null)
                {
                    foreach (var file in _remoteList.Files)
                    {
                        if (string.Compare(file.FileName, fileName, true) == 0 && !string.IsNullOrEmpty(file.Version))
                        {
                            gr.Cells[ColFileVersion.Name].Value = file.Version;
                            got = true;
                            break;
                        }
                    }
                    if (got)
                        continue;
                }

                if (_releaseList != null)
                {
                    foreach (var file in _releaseList.Files)
                    {
                        if (string.Compare(file.FileName, fileName, true) == 0 && !string.IsNullOrEmpty(file.Version))
                        {
                            gr.Cells[ColFileVersion.Name].Value = file.Version;
                            got = true;
                            break;
                        }
                    }
                    if (!got)
                    {
                        gr.Cells[ColFileVersion.Name].Value = _releaseList.ReleaseVersion;
                    }
                }
            }
            _filesVersionGot = true;
        }


        private void BindGrid()
        {
            _rootLength = _buildDir.Length;

            _exeCounter = _icoCounter = 0;

            _folderAndFiles.ForEach(item =>
            {
                if (Directory.Exists(item.Dir))
                {
                    var folderName = "[" + item.Dir.Substring(_rootLength) + "]";
                    gridFileList.Rows.Add(item.Selected, false, false, folderName, "-", "-");
                    var folderRow = gridFileList.Rows[gridFileList.Rows.Count - 1];
                    folderRow.Cells[ColExe.Name].ReadOnly = true;
                    folderRow.Cells[ColFileVersion.Name].ReadOnly = true;
                    folderRow.Tag = item;
                }
                else
                {
                    var shortName = item.Dir.Substring(_rootLength);

                    var fi = new FileInfo(item.Dir);
                    var ext = fi.Extension.ToLower();
                    var version = string.Empty;
                    if (ext == ".exe" || ext == ".dll")
                    {
                        version = FileVersionInfo.GetVersionInfo(item.Dir).FileVersion;
                    }
                    if (ext == ".exe")
                    {
                        _exeCounter++;
                    }
                    if (ext == ".ico")
                    {
                        _icoCounter++;
                    }
                    gridFileList.Rows.Add(item.Selected,
                                          !string.IsNullOrEmpty(_releaseList.ApplicationStart)
                                              ? (_releaseList.ApplicationStart.ToLower() == shortName.ToLower())
                                              : _exeCounter == 1,
                                          !string.IsNullOrEmpty(_releaseList.ShortcutIcon)
                                              ? (_releaseList.ShortcutIcon.ToLower() == shortName.ToLower())
                                              : _icoCounter == 1,
                                          shortName, fi.Length, version);

                    if (_exeCounter == 1 && string.IsNullOrEmpty(_releaseList.ApplicationStart))
                        _releaseList.ApplicationStart = shortName;
                    if (_icoCounter == 1 && string.IsNullOrEmpty(_releaseList.ShortcutIcon))
                        _releaseList.ShortcutIcon = shortName;

                    var lastRow = gridFileList.Rows[gridFileList.Rows.Count - 1];
                    lastRow.Tag = item;

                    if (!string.IsNullOrEmpty(version))
                    {
                        lastRow.Cells[ColFileVersion.Name].ReadOnly = true;
                        lastRow.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    lastRow.Cells[ColExe.Name].ReadOnly = (ext != ".exe");
                    lastRow.Cells[ColIco.Name].ReadOnly = (ext != ".ico");
                }
            });

            GetFilesVersion();
        }


        private void txtRelaseUrl_Leave(object sender, EventArgs e)
        {
            pbLoading.Show();
            txtRelaseUrl.Enabled = false;
            btnGen.Enabled = false;
            chkZip.Enabled = false;
            gridFileList.ReadOnly = true;

            GetFilesVersion();

            gridFileList.ReadOnly = false;
            btnGen.Enabled = true;
            chkZip.Enabled = true;
            txtRelaseUrl.Enabled = true;
            pbLoading.Hide();
        }

        private void chkZip_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkZip.Checked && !_filesVersionGot)
                GetFilesVersion();
        }

        private void gridFileList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridFileList.CurrentCell != null && gridFileList.CurrentCell.ReadOnly)
                return;
            switch (e.ColumnIndex)
            {
                case 0:
                    {
                        var dir = GetDir(gridFileList.CurrentRow.Index);
                        var selected = !Convert.ToBoolean(gridFileList.CurrentCell.Value);
                        if (!Directory.Exists(dir)) //find folder
                        {
                            var dirName = Path.GetDirectoryName(dir);
                            var folderIndex = -1;
                            var i = e.RowIndex;
                            var otherSelected = false;
                            while (i > 0)
                            {
                                i--;
                                var folder = GetDir(i);
                                ;
                                if (!Directory.Exists(folder))
                                {
                                    if (Path.GetDirectoryName(folder) != dirName)
                                        return;
                                    if (!otherSelected &&
                                        Convert.ToBoolean(gridFileList.Rows[i].Cells[ColSelect.Name].Value))
                                        otherSelected = true;
                                    continue;
                                }
                                if (folder == dirName)
                                {
                                    folderIndex = i;
                                    break;
                                }
                            }
                            if (folderIndex < 0)
                                return;
                            if (selected || otherSelected)
                            {
                                gridFileList.Rows[folderIndex].Cells[ColSelect.Name].Value = true;
                                return;
                            }
                            i = e.RowIndex;
                            while (i < gridFileList.Rows.Count - 1)
                            {
                                i++;
                                var subling = GetDir(i);
                                ;
                                if (Directory.Exists(subling) || Path.GetDirectoryName(subling) != dirName)
                                {
                                    break;
                                }
                                if (Convert.ToBoolean(gridFileList.Rows[i].Cells[ColSelect.Name].Value))
                                {
                                    gridFileList.Rows[folderIndex].Cells[ColSelect.Name].Value = true;
                                    return;
                                }
                            }
                            gridFileList.Rows[folderIndex].Cells[ColSelect.Name].Value = false;
                        }
                        else //find files
                        {
                            for (var i = e.RowIndex + 1; i < gridFileList.Rows.Count; i++)
                            {
                                var fileDir = GetDir(i);
                                ;
                                if (!fileDir.StartsWith(dir))
                                    break;
                                gridFileList.Rows[i].Cells[0].Value = selected;
                            }
                        }
                    }
                    break;
                case 1:
                case 2:
                    {
                        var selected = !Convert.ToBoolean(gridFileList.CurrentCell.Value);
                        if (!selected)
                            return;
                        foreach (DataGridViewRow gr in gridFileList.Rows)
                        {
                            if (gr.Index != e.RowIndex)
                            {
                                gr.Cells[e.ColumnIndex].Value = false; //unselected
                            }
                        }
                        gridFileList.CurrentRow.Cells[ColSelect.Name].Value = true;
                    }
                    break;
            }
        }

        private string GetDir(int i)
        {
            return (gridFileList.Rows[i].Tag as FileListItem).Dir;
        }

        private void ReleaseForm_Load(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = (Height - gbSummary.Height - splitContainer2.Panel2.Height);
            btnGen.Left = Width - btnGen.Width - 20;

            BindReleaseInfo();
            BindGrid();
        }

        public IEnumerable<string> GetExcludedFiles()
        {
            foreach (DataGridViewRow gr in gridFileList.Rows)
            {
                var fileName = gr.Cells[ColFileName.Name].Value.ToString();
                if (fileName.StartsWith("["))
                    continue;
                if (!Convert.ToBoolean(gr.Cells[ColSelect.Name].Value))
                    yield return fileName;
            }
        }

        public IEnumerable<string> GetExcludedFolders()
        {
            foreach (DataGridViewRow gr in gridFileList.Rows)
            {
                var fileName = gr.Cells[ColFileName.Name].Value.ToString();
                if (!fileName.StartsWith("["))
                    continue;
                if (!Convert.ToBoolean(gr.Cells[ColSelect.Name].Value))
                    yield return fileName.TrimStart('[').TrimEnd(']');
            }
        }

        private string GetSelectedFileName(string colName)
        {
            foreach (DataGridViewRow gr in gridFileList.Rows)
            {
                if (Convert.ToBoolean(gr.Cells[colName].Value))
                    return gr.Cells[ColFileName.Name].Value.ToString();
            }
            return string.Empty;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;
            _releaseList.WebSite = txtWebsite.Text.Trim();
            _releaseList.ReleaseDate = DateTime.Now.ToString();
            _releaseList.Packaged = chkZip.Checked;

            if (!chkZip.Checked)
            {
                var files = new List<ReleaseFile>();
                foreach (DataGridViewRow gr in gridFileList.Rows)
                {
                    gr.DefaultCellStyle.BackColor = SystemColors.Window;
                    var selected = Convert.ToBoolean(gr.Cells[ColSelect.Name].Value);
                    if (!selected)
                        continue;
                    var fileName = gr.Cells[ColFileName.Name].Value.ToString();
                    if (fileName.StartsWith("["))
                        continue;

                    if ((gr.Cells[ColFileVersion.Name].Value == null ||
                         gr.Cells[ColFileVersion.Name].Value.ToString().Trim().Length == 0))
                    {
                        Common.ShowError(gr.Cells[ColFileName.Name].Value + " no version ?");
                        gridFileList.FirstDisplayedScrollingRowIndex = gr.Index;
                        gr.DefaultCellStyle.BackColor = Color.Yellow;
                        return;
                    }
                    files.Add(
                        new ReleaseFile
                        {
                            FileName = fileName,
                            FileSize = long.Parse(gr.Cells[ColSize.Name].Value.ToString()),
                            Version = gr.Cells[ColFileVersion.Name].Value.ToString()
                        });
                }
                _releaseList.Files = files;
            }

            var di = new DirectoryInfo(_buildDir);
            CopyFiles();

            if (chkZip.Checked)
            {
                _zipName = string.Format("{0}_{1}{2}", _releaseList.AppName, _releaseList.ReleaseVersion, Common.ZipExt);
                _zipPath = string.Format("{0}\\{1}", di.Parent.FullName, _zipName);
                var d = new ZipDelegate(ZipUtil.ZipFolder);
                d.BeginInvoke(_versionFolder, _zipPath, ZipCompleted, null);
            }
            else
            {
                var releaseListConfigFile = Path.Combine(_buildDir, Constants.ReleaseConfigFileName);
                FileUtil.SaveReleaseList(_releaseList, releaseListConfigFile);
                File.Copy(releaseListConfigFile, Path.Combine(di.Parent.FullName, Constants.ReleaseConfigFileName), true);
                File.Copy(releaseListConfigFile, Path.Combine(_versionFolder, Constants.ReleaseConfigFileName), true);
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInput()
        {
            if (!chkZip.Checked && gridFileList.Rows.Count == 0)
                return false;

            gridFileList.EndEdit();

            _releaseList.ApplicationStart = GetSelectedFileName(ColExe.Name);
            if (string.IsNullOrEmpty(_releaseList.ApplicationStart))
            {
                Common.ShowError("choose a main .exe file");
                gridFileList.Focus();
                return false;
            }
            _releaseList.Company = txtCompany.Text.Trim();
            if (string.IsNullOrEmpty(_releaseList.Company))
            {
                Common.ShowError("company name is required");
                txtCompany.Focus();
                return false;
            }

            _releaseList.AppName = txtAppName.Text.Trim();
            if (string.IsNullOrEmpty(_releaseList.AppName))
            {
                Common.ShowError("app name is required");
                txtAppName.Focus();
                return false;
            }
            _releaseList.ReleaseUrl = txtRelaseUrl.Text.Trim();
            if (string.IsNullOrEmpty(_releaseList.ReleaseUrl))
            {
                Common.ShowError("release web root is required");
                txtRelaseUrl.Focus();
                return false;
            }
            _releaseList.ShortcutIcon = GetSelectedFileName(ColIco.Name);
            //if (string.IsNullOrEmpty(this.reList.ShortcutIcon))
            //{
            //    Common.ShowError("shortcut icon is required");
            //    this.txtIcon.Focus();
            //    return false;
            //}
            _releaseList.MinVersion = txtMinVer.Text.Trim();
            if (string.IsNullOrEmpty(_releaseList.MinVersion))
            {
                Common.ShowError("min version number is required");
                txtMinVer.Focus();
                return false;
            }
            //_versionChanged = _releaseList.ReleaseVersion != txtReleaseVer.Text.Trim();
            _releaseList.ReleaseVersion = txtReleaseVer.Text.Trim();
            if (string.IsNullOrEmpty(_releaseList.ReleaseVersion))
            {
                Common.ShowError("release version number is required");
                txtReleaseVer.Focus();
                return false;
            }
            if (_releaseList.Compare(_releaseList.MinVersion) < 0)
            {
                Common.ShowError("Minimum Version > Release Version ?");
                txtMinVer.Clear();
                txtMinVer.Focus();
                return false;
            }
            _releaseList.UpdateDescription = txtUplog.Text;
            if (string.IsNullOrEmpty(_releaseList.UpdateDescription))
            {
                Common.ShowError("Update log is required");
                txtUplog.Focus();
                return false;
            }
            return true;
        }


        private void CopyFiles()
        {
            var di = new DirectoryInfo(_buildDir);
            _versionFolder = Path.Combine(di.Parent.FullName, _releaseList.ReleaseVersion);
            if (Directory.Exists(_versionFolder))
            {
                FileUtil.DeleteFolder(_versionFolder);
            }
            Directory.CreateDirectory(_versionFolder);
            foreach (DataGridViewRow gr in gridFileList.Rows)
            {
                var selected = Convert.ToBoolean(gr.Cells[ColSelect.Name].Value);
                if (!selected)
                    continue;
                var fileName = gr.Cells[ColFileName.Name].Value.ToString();
                if (fileName.StartsWith("["))
                    continue;
                var sourceFile = GetDir(gr.Index);
                var targetFile = string.Concat(_versionFolder, "\\" + fileName);
                var targetDir = Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                File.Copy(sourceFile, targetFile);
            }
        }


        private void ZipCompleted(IAsyncResult asyncResult)
        {
            if (asyncResult == null) return;

            var file = new FileInfo(_zipPath);
            _releaseList.Files.Clear();
            _releaseList.Files.Add(new ReleaseFile
            {
                FileName = _zipName,
                FileSize = file.Length,
                Version = _releaseList.ReleaseVersion
            });

            var di = new DirectoryInfo(_buildDir);
            FileUtil.SaveReleaseList(_releaseList, Path.Combine(di.Parent.FullName, Constants.ReleaseConfigFileName));

            DialogResult = DialogResult.OK;
        }

        private delegate void ZipDelegate(string folder, string zipPath);
    }
}