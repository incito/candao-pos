namespace Main
{
    partial class frmAllTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAllTable));
            this.timer1 = new System.Windows.Forms.Timer();
            this.timer2 = new System.Windows.Forms.Timer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnShapping = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnend = new System.Windows.Forms.Button();
            this.pnlState1 = new System.Windows.Forms.Panel();
            this.lblState1 = new System.Windows.Forms.Label();
            this.pnlState0 = new System.Windows.Forms.Panel();
            this.lblState0 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblbranchid = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.panelPage = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlState1.SuspendLayout();
            this.pnlState0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 78);
            this.panel1.TabIndex = 235;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnShapping);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.btnend);
            this.panel4.Controls.Add(this.pnlState1);
            this.panel4.Controls.Add(this.pnlState0);
            this.panel4.Controls.Add(this.btnReport);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 650);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1008, 58);
            this.panel4.TabIndex = 234;
            // 
            // pnlState1
            // 
            this.pnlState1.BackColor = System.Drawing.Color.LightSalmon;
            this.pnlState1.Controls.Add(this.lblState1);
            this.pnlState1.Location = new System.Drawing.Point(89, 7);
            this.pnlState1.Name = "pnlState1";
            this.pnlState1.Size = new System.Drawing.Size(75, 38);
            this.pnlState1.TabIndex = 237;
            // 
            // lblState1
            // 
            this.lblState1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblState1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblState1.Location = new System.Drawing.Point(0, 0);
            this.lblState1.Name = "lblState1";
            this.lblState1.Size = new System.Drawing.Size(75, 38);
            this.lblState1.TabIndex = 0;
            this.lblState1.Text = "就餐(0)";
            this.lblState1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlState0
            // 
            this.pnlState0.BackColor = System.Drawing.Color.White;
            this.pnlState0.Controls.Add(this.lblState0);
            this.pnlState0.Location = new System.Drawing.Point(10, 7);
            this.pnlState0.Name = "pnlState0";
            this.pnlState0.Size = new System.Drawing.Size(75, 38);
            this.pnlState0.TabIndex = 236;
            // 
            // lblState0
            // 
            this.lblState0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblState0.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblState0.Location = new System.Drawing.Point(0, 0);
            this.lblState0.Name = "lblState0";
            this.lblState0.Size = new System.Drawing.Size(75, 38);
            this.lblState0.TabIndex = 0;
            this.lblState0.Text = "空闲(0)";
            this.lblState0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1008, 708);
            this.pnlMain.TabIndex = 233;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.lblUser);
            this.panel2.Controls.Add(this.lblTime);
            this.panel2.Controls.Add(this.lblbranchid);
            this.panel2.Controls.Add(this.lblVer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 708);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 21);
            this.panel2.TabIndex = 232;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(117, 2);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(67, 14);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "登录员工：";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(313, 2);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(67, 14);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "当前时间：";
            // 
            // lblbranchid
            // 
            this.lblbranchid.AutoSize = true;
            this.lblbranchid.Location = new System.Drawing.Point(10, 2);
            this.lblbranchid.Name = "lblbranchid";
            this.lblbranchid.Size = new System.Drawing.Size(95, 14);
            this.lblbranchid.TabIndex = 1;
            this.lblbranchid.Text = "店铺编号：0013";
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Location = new System.Drawing.Point(632, 2);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(89, 14);
            this.lblVer.TabIndex = 0;
            this.lblVer.Text = "版本：v1.0.0.0";
            // 
            // btnDown
            // 
            this.btnDown.Image = global::KYPOS.Properties.Resources.down21;
            this.btnDown.Location = new System.Drawing.Point(515, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(42, 39);
            this.btnDown.TabIndex = 1;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::KYPOS.Properties.Resources.up21;
            this.btnUp.Location = new System.Drawing.Point(456, 1);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(42, 39);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::KYPOS.Properties.Resources._return;
            this.pictureBox3.Location = new System.Drawing.Point(926, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(77, 77);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 29;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.ErrorImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Image = global::KYPOS.Properties.Resources._0111__31_cd21;
            this.pictureBox1.InitialImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Location = new System.Drawing.Point(2, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(330, 76);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // btnShapping
            // 
            this.btnShapping.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnShapping.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnShapping.Image = global::KYPOS.Properties.Resources.打包;
            this.btnShapping.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShapping.Location = new System.Drawing.Point(390, 2);
            this.btnShapping.Name = "btnShapping";
            this.btnShapping.Size = new System.Drawing.Size(131, 53);
            this.btnShapping.TabIndex = 240;
            this.btnShapping.Text = "      外卖";
            this.btnShapping.UseVisualStyleBackColor = true;
            this.btnShapping.Click += new System.EventHandler(this.btnShapping_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button3.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button3.Image = global::KYPOS.Properties.Resources.receipt;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(525, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(131, 53);
            this.button3.TabIndex = 239;
            this.button3.Text = "         帐单查询";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnend
            // 
            this.btnend.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnend.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnend.Image = global::KYPOS.Properties.Resources.files2;
            this.btnend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnend.Location = new System.Drawing.Point(771, 2);
            this.btnend.Name = "btnend";
            this.btnend.Size = new System.Drawing.Size(115, 53);
            this.btnend.TabIndex = 238;
            this.btnend.Text = "        结业";
            this.btnend.UseVisualStyleBackColor = true;
            this.btnend.Click += new System.EventHandler(this.btnend_Click);
            // 
            // btnReport
            // 
            this.btnReport.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnReport.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnReport.Image = global::KYPOS.Properties.Resources.Report48;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(887, 2);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(115, 53);
            this.btnReport.TabIndex = 235;
            this.btnReport.Tag = "0";
            this.btnReport.Text = "        报表";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button1.Image = global::KYPOS.Properties.Resources.clear2;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(656, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 53);
            this.button1.TabIndex = 233;
            this.button1.Text = "        清机";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);// 
            // panelPage
            // 
            this.panelPage.Controls.Add(this.btnDown);
            this.panelPage.Controls.Add(this.btnUp);
            this.panelPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPage.Location = new System.Drawing.Point(0, 610);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(1008, 40);
            this.panelPage.TabIndex = 236;
            this.panelPage.Visible = false;
            // 
            // frmAllTable
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panelPage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAllTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAllTable_FormClosed);
            this.Load += new System.EventHandler(this.frmAllTable_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnlState1.ResumeLayout(false);
            this.pnlState0.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblbranchid;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlState0;
        private System.Windows.Forms.Label lblState0;
        private System.Windows.Forms.Panel pnlState1;
        private System.Windows.Forms.Label lblState1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btnend;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnShapping;
        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;

    }
}