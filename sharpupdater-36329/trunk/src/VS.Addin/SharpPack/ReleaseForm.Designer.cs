namespace CnSharp.Delivery.VisualStudio.PackingTool
{
    partial class ReleaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReleaseForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.gridFileList = new System.Windows.Forms.DataGridView();
            this.ColSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColExe = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColIco = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFileVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gbSummary = new System.Windows.Forms.GroupBox();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtRelaseUrl = new System.Windows.Forms.TextBox();
            this.lblReleaseUrl = new System.Windows.Forms.Label();
            this.txtUplog = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMinVer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReleaseVer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkZip = new System.Windows.Forms.CheckBox();
            this.btnGen = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1141, 564);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.TabIndex = 0;
            // 
            // gbFiles
            // 
            this.gbFiles.Controls.Add(this.gridFileList);
            this.gbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFiles.Location = new System.Drawing.Point(0, 0);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(1141, 287);
            this.gbFiles.TabIndex = 0;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            // 
            // gridFileList
            // 
            this.gridFileList.AllowUserToAddRows = false;
            this.gridFileList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFileList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gridFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSelect,
            this.ColExe,
            this.ColIco,
            this.ColFileName,
            this.ColSize,
            this.ColFileVersion});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridFileList.DefaultCellStyle = dataGridViewCellStyle6;
            this.gridFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFileList.Location = new System.Drawing.Point(3, 17);
            this.gridFileList.Name = "gridFileList";
            this.gridFileList.RowTemplate.Height = 23;
            this.gridFileList.Size = new System.Drawing.Size(1135, 267);
            this.gridFileList.TabIndex = 16;
            this.gridFileList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFileList_CellClick);
            // 
            // ColSelect
            // 
            this.ColSelect.HeaderText = "Select";
            this.ColSelect.Name = "ColSelect";
            this.ColSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColSelect.Width = 50;
            // 
            // ColExe
            // 
            this.ColExe.HeaderText = "Main";
            this.ColExe.Name = "ColExe";
            this.ColExe.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColExe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColExe.ToolTipText = "Main exe file";
            this.ColExe.Width = 50;
            // 
            // ColIco
            // 
            this.ColIco.HeaderText = "Icon";
            this.ColIco.Name = "ColIco";
            this.ColIco.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColIco.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColIco.ToolTipText = "Shortcut icon";
            this.ColIco.Width = 50;
            // 
            // ColFileName
            // 
            this.ColFileName.HeaderText = "Name";
            this.ColFileName.Name = "ColFileName";
            this.ColFileName.ReadOnly = true;
            this.ColFileName.Width = 480;
            // 
            // ColSize
            // 
            this.ColSize.HeaderText = "File Size(K)";
            this.ColSize.Name = "ColSize";
            this.ColSize.ReadOnly = true;
            this.ColSize.Width = 150;
            // 
            // ColFileVersion
            // 
            this.ColFileVersion.HeaderText = "Version";
            this.ColFileVersion.Name = "ColFileVersion";
            this.ColFileVersion.Width = 120;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gbSummary);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chkZip);
            this.splitContainer2.Panel2.Controls.Add(this.btnGen);
            this.splitContainer2.Size = new System.Drawing.Size(1141, 273);
            this.splitContainer2.SplitterDistance = 223;
            this.splitContainer2.TabIndex = 0;
            // 
            // gbSummary
            // 
            this.gbSummary.Controls.Add(this.txtWebsite);
            this.gbSummary.Controls.Add(this.label5);
            this.gbSummary.Controls.Add(this.pbLoading);
            this.gbSummary.Controls.Add(this.txtCompany);
            this.gbSummary.Controls.Add(this.txtRelaseUrl);
            this.gbSummary.Controls.Add(this.lblReleaseUrl);
            this.gbSummary.Controls.Add(this.txtUplog);
            this.gbSummary.Controls.Add(this.label7);
            this.gbSummary.Controls.Add(this.txtMinVer);
            this.gbSummary.Controls.Add(this.label4);
            this.gbSummary.Controls.Add(this.txtReleaseVer);
            this.gbSummary.Controls.Add(this.label3);
            this.gbSummary.Controls.Add(this.label2);
            this.gbSummary.Controls.Add(this.txtAppName);
            this.gbSummary.Controls.Add(this.label1);
            this.gbSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSummary.Location = new System.Drawing.Point(0, 0);
            this.gbSummary.Name = "gbSummary";
            this.gbSummary.Size = new System.Drawing.Size(1141, 223);
            this.gbSummary.TabIndex = 1;
            this.gbSummary.TabStop = false;
            this.gbSummary.Text = "Release Summary";
            // 
            // pbLoading
            // 
            this.pbLoading.Image = ((System.Drawing.Image)(resources.GetObject("pbLoading.Image")));
            this.pbLoading.Location = new System.Drawing.Point(663, 81);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(16, 16);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLoading.TabIndex = 15;
            this.pbLoading.TabStop = false;
            this.pbLoading.Visible = false;
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(532, 32);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(125, 21);
            this.txtCompany.TabIndex = 1;
            // 
            // txtRelaseUrl
            // 
            this.txtRelaseUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtRelaseUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.txtRelaseUrl.Location = new System.Drawing.Point(144, 81);
            this.txtRelaseUrl.Name = "txtRelaseUrl";
            this.txtRelaseUrl.Size = new System.Drawing.Size(513, 21);
            this.txtRelaseUrl.TabIndex = 2;
            this.txtRelaseUrl.Leave += new System.EventHandler(this.txtRelaseUrl_Leave);
            // 
            // lblReleaseUrl
            // 
            this.lblReleaseUrl.AutoSize = true;
            this.lblReleaseUrl.Location = new System.Drawing.Point(25, 84);
            this.lblReleaseUrl.Name = "lblReleaseUrl";
            this.lblReleaseUrl.Size = new System.Drawing.Size(89, 12);
            this.lblReleaseUrl.TabIndex = 14;
            this.lblReleaseUrl.Text = "Release URL(*)";
            // 
            // txtUplog
            // 
            this.txtUplog.Location = new System.Drawing.Point(714, 45);
            this.txtUplog.Multiline = true;
            this.txtUplog.Name = "txtUplog";
            this.txtUplog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUplog.Size = new System.Drawing.Size(393, 142);
            this.txtUplog.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(712, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "Update Log(*)";
            // 
            // txtMinVer
            // 
            this.txtMinVer.Location = new System.Drawing.Point(538, 129);
            this.txtMinVer.Name = "txtMinVer";
            this.txtMinVer.Size = new System.Drawing.Size(119, 21);
            this.txtMinVer.TabIndex = 4;
            this.txtMinVer.Text = "1.0.0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(401, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Minimum Version(*)";
            // 
            // txtReleaseVer
            // 
            this.txtReleaseVer.Location = new System.Drawing.Point(144, 129);
            this.txtReleaseVer.Name = "txtReleaseVer";
            this.txtReleaseVer.Size = new System.Drawing.Size(125, 21);
            this.txtReleaseVer.TabIndex = 3;
            this.txtReleaseVer.Text = "1.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Release Version(*)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Company Name(*)";
            // 
            // txtAppName
            // 
            this.txtAppName.Location = new System.Drawing.Point(144, 35);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(125, 21);
            this.txtAppName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product Name(*)";
            // 
            // chkZip
            // 
            this.chkZip.AutoSize = true;
            this.chkZip.Location = new System.Drawing.Point(27, 18);
            this.chkZip.Name = "chkZip";
            this.chkZip.Size = new System.Drawing.Size(138, 16);
            this.chkZip.TabIndex = 14;
            this.chkZip.Text = "Release as Zip Pack";
            this.chkZip.UseVisualStyleBackColor = true;
            this.chkZip.CheckedChanged += new System.EventHandler(this.chkZip_CheckedChanged);
            // 
            // btnGen
            // 
            this.btnGen.Image = ((System.Drawing.Image)(resources.GetObject("btnGen.Image")));
            this.btnGen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGen.Location = new System.Drawing.Point(943, 11);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(177, 23);
            this.btnGen.TabIndex = 0;
            this.btnGen.Text = "Generate Release List";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "Support Website";
            // 
            // txtWebsite
            // 
            this.txtWebsite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtWebsite.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.txtWebsite.Location = new System.Drawing.Point(144, 172);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(513, 21);
            this.txtWebsite.TabIndex = 17;
            // 
            // ReleaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 564);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReleaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stpe 2:Release";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReleaseForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gbFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.gbSummary.ResumeLayout(false);
            this.gbSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.DataGridView gridFileList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox gbSummary;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.TextBox txtRelaseUrl;
        private System.Windows.Forms.Label lblReleaseUrl;
        private System.Windows.Forms.TextBox txtUplog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMinVer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReleaseVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.CheckBox chkZip;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColExe;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIco;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFileVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWebsite;
    }
}