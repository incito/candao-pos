namespace Main
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblbranchid = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pcInputArea = new DevExpress.XtraEditors.PanelControl();
            this.chkSaveLoginInfo = new DevExpress.XtraEditors.CheckEdit();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPwd = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.lblLoadingInfo = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnModifyPwd = new System.Windows.Forms.LinkLabel();
            this.txtDataset = new DevExpress.XtraEditors.LookUpEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcInputArea)).BeginInit();
            this.pcInputArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSaveLoginInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataset.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblServer);
            this.panel2.Controls.Add(this.lblTime);
            this.panel2.Controls.Add(this.lblbranchid);
            this.panel2.Controls.Add(this.lblVer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 417);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(634, 21);
            this.panel2.TabIndex = 230;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(448, 2);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(47, 14);
            this.lblServer.TabIndex = 3;
            this.lblServer.Text = "服务器:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(175, 2);
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
            this.lblVer.Location = new System.Drawing.Point(364, 2);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(89, 14);
            this.lblVer.TabIndex = 0;
            this.lblVer.Text = "版本：v1.0.0.0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 67);
            this.panel1.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))));
            this.label3.Location = new System.Drawing.Point(331, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 30);
            this.label3.TabIndex = 27;
            this.label3.Text = "欢迎登录餐道POS收银系统";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.ErrorImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Image = global::KYPOS.Properties.Resources._0111__31_cd;
            this.pictureBox1.InitialImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Location = new System.Drawing.Point(4, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 63);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // pcInputArea
            // 
            this.pcInputArea.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pcInputArea.Controls.Add(this.chkSaveLoginInfo);
            this.pcInputArea.Controls.Add(this.button13);
            this.pcInputArea.Controls.Add(this.button12);
            this.pcInputArea.Controls.Add(this.button11);
            this.pcInputArea.Controls.Add(this.button10);
            this.pcInputArea.Controls.Add(this.button9);
            this.pcInputArea.Controls.Add(this.button8);
            this.pcInputArea.Controls.Add(this.button7);
            this.pcInputArea.Controls.Add(this.button6);
            this.pcInputArea.Controls.Add(this.button5);
            this.pcInputArea.Controls.Add(this.button4);
            this.pcInputArea.Controls.Add(this.button3);
            this.pcInputArea.Controls.Add(this.button2);
            this.pcInputArea.Controls.Add(this.button1);
            this.pcInputArea.Controls.Add(this.txtUser);
            this.pcInputArea.Controls.Add(this.label1);
            this.pcInputArea.Controls.Add(this.txtPwd);
            this.pcInputArea.Controls.Add(this.label2);
            this.pcInputArea.Location = new System.Drawing.Point(16, 95);
            this.pcInputArea.Name = "pcInputArea";
            this.pcInputArea.Size = new System.Drawing.Size(598, 265);
            this.pcInputArea.TabIndex = 24;
            // 
            // chkSaveLoginInfo
            // 
            this.chkSaveLoginInfo.Location = new System.Drawing.Point(123, 55);
            this.chkSaveLoginInfo.Name = "chkSaveLoginInfo";
            this.chkSaveLoginInfo.Properties.Caption = "保存登录信息";
            this.chkSaveLoginInfo.Size = new System.Drawing.Size(104, 19);
            this.chkSaveLoginInfo.TabIndex = 27;
            // 
            // button13
            // 
            this.button13.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button13.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button13.Location = new System.Drawing.Point(359, 75);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(62, 185);
            this.button13.TabIndex = 242;
            this.button13.Text = "确定";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button12.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button12.Location = new System.Drawing.Point(268, 214);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(88, 46);
            this.button12.TabIndex = 241;
            this.button12.Text = "<-";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button11.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button11.Location = new System.Drawing.Point(177, 214);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(88, 46);
            this.button11.TabIndex = 240;
            this.button11.Text = "清除";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button10.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button10.Location = new System.Drawing.Point(87, 214);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(88, 46);
            this.button10.TabIndex = 239;
            this.button10.Text = "0";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button1_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button9.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button9.Location = new System.Drawing.Point(268, 169);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(88, 46);
            this.button9.TabIndex = 238;
            this.button9.Text = "9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button1_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button8.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button8.Location = new System.Drawing.Point(177, 169);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(88, 46);
            this.button8.TabIndex = 237;
            this.button8.Text = "8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button1_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button7.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button7.Location = new System.Drawing.Point(87, 169);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(88, 46);
            this.button7.TabIndex = 236;
            this.button7.Text = "7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button1_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button6.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button6.Location = new System.Drawing.Point(268, 122);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 46);
            this.button6.TabIndex = 235;
            this.button6.Text = "6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button5.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button5.Location = new System.Drawing.Point(177, 122);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 46);
            this.button5.TabIndex = 234;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button4.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button4.Location = new System.Drawing.Point(87, 122);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 46);
            this.button4.TabIndex = 233;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button3.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button3.Location = new System.Drawing.Point(268, 75);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 46);
            this.button3.TabIndex = 232;
            this.button3.Text = "3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button2.Location = new System.Drawing.Point(177, 75);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 46);
            this.button2.TabIndex = 231;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button1.Location = new System.Drawing.Point(87, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 46);
            this.button1.TabIndex = 230;
            this.button1.Text = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtUser
            // 
            this.txtUser.EditValue = "";
            this.txtUser.Location = new System.Drawing.Point(133, 18);
            this.txtUser.Name = "txtUser";
            this.txtUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtUser.Properties.Appearance.Options.UseFont = true;
            this.txtUser.Size = new System.Drawing.Size(106, 32);
            this.txtUser.TabIndex = 21;
            this.txtUser.EditValueChanged += new System.EventHandler(this.txtUser_EditValueChanged);
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            this.txtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label1.Location = new System.Drawing.Point(82, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户：";
            // 
            // txtPwd
            // 
            this.txtPwd.EditValue = "";
            this.txtPwd.Location = new System.Drawing.Point(308, 18);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtPwd.Properties.Appearance.Options.UseFont = true;
            this.txtPwd.Properties.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(112, 32);
            this.txtPwd.TabIndex = 22;
            this.txtPwd.Enter += new System.EventHandler(this.txtPwd_Enter);
            this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label2.Location = new System.Drawing.Point(250, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "密码：";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(344, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 38);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消 (&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(245, 371);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(93, 38);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "登录 (&L)";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblLoadingInfo
            // 
            this.lblLoadingInfo.AutoSize = true;
            this.lblLoadingInfo.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblLoadingInfo.Location = new System.Drawing.Point(40, 339);
            this.lblLoadingInfo.Name = "lblLoadingInfo";
            this.lblLoadingInfo.Size = new System.Drawing.Size(67, 14);
            this.lblLoadingInfo.TabIndex = 12;
            this.lblLoadingInfo.Text = "准备就绪...";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::KYPOS.Properties.Resources._2009011340768021;
            this.pictureBox2.Location = new System.Drawing.Point(15, 71);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(238, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "用户登录(User Login)";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(15, 362);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(598, 2);
            this.label5.TabIndex = 9;
            // 
            // btnModifyPwd
            // 
            this.btnModifyPwd.AutoSize = true;
            this.btnModifyPwd.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnModifyPwd.LinkColor = System.Drawing.Color.RoyalBlue;
            this.btnModifyPwd.Location = new System.Drawing.Point(647, 339);
            this.btnModifyPwd.Name = "btnModifyPwd";
            this.btnModifyPwd.Size = new System.Drawing.Size(43, 14);
            this.btnModifyPwd.TabIndex = 229;
            this.btnModifyPwd.TabStop = true;
            this.btnModifyPwd.Text = "改密码";
            this.btnModifyPwd.Visible = false;
            this.btnModifyPwd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnModifyPwd_LinkClicked);
            // 
            // txtDataset
            // 
            this.txtDataset.Location = new System.Drawing.Point(637, 368);
            this.txtDataset.Name = "txtDataset";
            this.txtDataset.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataset.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DataSetID", 100, "帐套编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DataSetName", 200, "帐套名称")});
            this.txtDataset.Properties.NullText = "";
            this.txtDataset.Properties.PopupWidth = 300;
            this.txtDataset.Size = new System.Drawing.Size(191, 20);
            this.txtDataset.TabIndex = 228;
            this.txtDataset.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(591, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 14);
            this.label8.TabIndex = 20;
            this.label8.Text = "帐套：";
            this.label8.Visible = false;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 438);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pcInputArea);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblLoadingInfo);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnModifyPwd);
            this.Controls.Add(this.txtDataset);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统登录";
            this.Activated += new System.EventHandler(this.frmLogin_Activated);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcInputArea)).EndInit();
            this.pcInputArea.ResumeLayout(false);
            this.pcInputArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSaveLoginInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataset.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblLoadingInfo;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private DevExpress.XtraEditors.TextEdit txtPwd;
        private DevExpress.XtraEditors.PanelControl pcInputArea;
        private DevExpress.XtraEditors.LookUpEdit txtDataset;
        private System.Windows.Forms.LinkLabel btnModifyPwd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private DevExpress.XtraEditors.CheckEdit chkSaveLoginInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblbranchid;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblServer;
    }
}