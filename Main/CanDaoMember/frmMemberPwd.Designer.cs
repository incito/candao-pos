namespace Main
{
    partial class frmMemberPwd
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
            this.tmrGetIdentCode = new System.Windows.Forms.Timer(this.components);
            this.btnGetIdentCode = new System.Windows.Forms.Button();
            this.edtIdentCode = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlNum = new System.Windows.Forms.Panel();
            this.button28 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.edtPwd2 = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.edtPwd = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.edtMobile = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.button27 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.edtIdentCode.Properties)).BeginInit();
            this.pnlNum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMobile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrGetIdentCode
            // 
            this.tmrGetIdentCode.Interval = 1000;
            this.tmrGetIdentCode.Tick += new System.EventHandler(this.tmrGetIdentCode_Tick);
            // 
            // btnGetIdentCode
            // 
            this.btnGetIdentCode.Location = new System.Drawing.Point(375, 131);
            this.btnGetIdentCode.Name = "btnGetIdentCode";
            this.btnGetIdentCode.Size = new System.Drawing.Size(82, 38);
            this.btnGetIdentCode.TabIndex = 311;
            this.btnGetIdentCode.Tag = "0";
            this.btnGetIdentCode.Text = "发送";
            this.btnGetIdentCode.UseVisualStyleBackColor = true;
            this.btnGetIdentCode.Click += new System.EventHandler(this.btnGetIdentCode_Click);
            // 
            // edtIdentCode
            // 
            this.edtIdentCode.EditValue = "";
            this.edtIdentCode.Location = new System.Drawing.Point(177, 131);
            this.edtIdentCode.Name = "edtIdentCode";
            this.edtIdentCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.edtIdentCode.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtIdentCode.Properties.Appearance.Options.UseFont = true;
            this.edtIdentCode.Properties.Appearance.Options.UseForeColor = true;
            this.edtIdentCode.Size = new System.Drawing.Size(192, 40);
            this.edtIdentCode.TabIndex = 310;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(43, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 34);
            this.label6.TabIndex = 309;
            this.label6.Text = "验证码：";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(318, 346);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 52);
            this.button1.TabIndex = 308;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlNum
            // 
            this.pnlNum.BackColor = System.Drawing.Color.White;
            this.pnlNum.Controls.Add(this.button28);
            this.pnlNum.Controls.Add(this.button2);
            this.pnlNum.Controls.Add(this.button15);
            this.pnlNum.Controls.Add(this.button16);
            this.pnlNum.Controls.Add(this.button17);
            this.pnlNum.Controls.Add(this.button18);
            this.pnlNum.Controls.Add(this.button19);
            this.pnlNum.Controls.Add(this.button20);
            this.pnlNum.Controls.Add(this.button21);
            this.pnlNum.Controls.Add(this.button22);
            this.pnlNum.Controls.Add(this.button23);
            this.pnlNum.Controls.Add(this.button24);
            this.pnlNum.Controls.Add(this.button25);
            this.pnlNum.Controls.Add(this.button26);
            this.pnlNum.Location = new System.Drawing.Point(469, 82);
            this.pnlNum.Name = "pnlNum";
            this.pnlNum.Size = new System.Drawing.Size(293, 236);
            this.pnlNum.TabIndex = 307;
            // 
            // button28
            // 
            this.button28.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button28.ForeColor = System.Drawing.Color.Blue;
            this.button28.Location = new System.Drawing.Point(220, 5);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(65, 56);
            this.button28.TabIndex = 288;
            this.button28.Text = "<-";
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button2.ForeColor = System.Drawing.Color.Blue;
            this.button2.Location = new System.Drawing.Point(220, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 170);
            this.button2.TabIndex = 287;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button15.ForeColor = System.Drawing.Color.Blue;
            this.button15.Location = new System.Drawing.Point(149, 176);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(69, 56);
            this.button15.TabIndex = 286;
            this.button15.Text = ".";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button26_Click);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button16.ForeColor = System.Drawing.Color.Blue;
            this.button16.Location = new System.Drawing.Point(78, 176);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(69, 56);
            this.button16.TabIndex = 285;
            this.button16.Text = "0";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button26_Click);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button17.ForeColor = System.Drawing.Color.Blue;
            this.button17.Location = new System.Drawing.Point(7, 176);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(69, 56);
            this.button17.TabIndex = 284;
            this.button17.Text = "00";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button18.ForeColor = System.Drawing.Color.Blue;
            this.button18.Location = new System.Drawing.Point(149, 119);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(69, 56);
            this.button18.TabIndex = 283;
            this.button18.Text = "9";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button26_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button19.ForeColor = System.Drawing.Color.Blue;
            this.button19.Location = new System.Drawing.Point(78, 119);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(69, 56);
            this.button19.TabIndex = 282;
            this.button19.Text = "8";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button26_Click);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button20.ForeColor = System.Drawing.Color.Blue;
            this.button20.Location = new System.Drawing.Point(7, 119);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(69, 56);
            this.button20.TabIndex = 281;
            this.button20.Text = "7";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button26_Click);
            // 
            // button21
            // 
            this.button21.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button21.ForeColor = System.Drawing.Color.Blue;
            this.button21.Location = new System.Drawing.Point(149, 62);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(69, 56);
            this.button21.TabIndex = 280;
            this.button21.Text = "6";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button26_Click);
            // 
            // button22
            // 
            this.button22.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button22.ForeColor = System.Drawing.Color.Blue;
            this.button22.Location = new System.Drawing.Point(78, 62);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(69, 56);
            this.button22.TabIndex = 279;
            this.button22.Text = "5";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button26_Click);
            // 
            // button23
            // 
            this.button23.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button23.ForeColor = System.Drawing.Color.Blue;
            this.button23.Location = new System.Drawing.Point(7, 62);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(69, 56);
            this.button23.TabIndex = 278;
            this.button23.Text = "4";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button26_Click);
            // 
            // button24
            // 
            this.button24.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button24.ForeColor = System.Drawing.Color.Blue;
            this.button24.Location = new System.Drawing.Point(149, 5);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(69, 56);
            this.button24.TabIndex = 277;
            this.button24.Text = "3";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button26_Click);
            // 
            // button25
            // 
            this.button25.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button25.ForeColor = System.Drawing.Color.Blue;
            this.button25.Location = new System.Drawing.Point(78, 5);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(69, 56);
            this.button25.TabIndex = 276;
            this.button25.Text = "2";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button26_Click);
            // 
            // button26
            // 
            this.button26.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button26.ForeColor = System.Drawing.Color.Blue;
            this.button26.Location = new System.Drawing.Point(7, 5);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(69, 56);
            this.button26.TabIndex = 275;
            this.button26.Text = "1";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // edtPwd2
            // 
            this.edtPwd2.EditValue = "";
            this.edtPwd2.Location = new System.Drawing.Point(177, 225);
            this.edtPwd2.Name = "edtPwd2";
            this.edtPwd2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.edtPwd2.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtPwd2.Properties.Appearance.Options.UseFont = true;
            this.edtPwd2.Properties.Appearance.Options.UseForeColor = true;
            this.edtPwd2.Properties.PasswordChar = '*';
            this.edtPwd2.Size = new System.Drawing.Size(280, 40);
            this.edtPwd2.TabIndex = 306;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 34);
            this.label5.TabIndex = 305;
            this.label5.Text = "确认密码：";
            // 
            // edtPwd
            // 
            this.edtPwd.EditValue = "";
            this.edtPwd.Location = new System.Drawing.Point(177, 177);
            this.edtPwd.Name = "edtPwd";
            this.edtPwd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.edtPwd.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtPwd.Properties.Appearance.Options.UseFont = true;
            this.edtPwd.Properties.Appearance.Options.UseForeColor = true;
            this.edtPwd.Properties.PasswordChar = '*';
            this.edtPwd.Size = new System.Drawing.Size(280, 40);
            this.edtPwd.TabIndex = 304;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(43, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 34);
            this.label4.TabIndex = 303;
            this.label4.Text = "新密码：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 67);
            this.panel1.TabIndex = 292;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::KYPOS.Properties.Resources._return;
            this.pictureBox4.Location = new System.Drawing.Point(695, 1);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(66, 66);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 30;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::KYPOS.Properties.Resources._return;
            this.pictureBox3.Location = new System.Drawing.Point(820, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(66, 66);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 28;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::KYPOS.Properties.Resources.exit;
            this.pictureBox2.Location = new System.Drawing.Point(912, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(94, 41);
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.ErrorImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Image = global::KYPOS.Properties.Resources._0111__31_cd;
            this.pictureBox1.InitialImage = global::KYPOS.Properties.Resources.logintop;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 65);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // edtMobile
            // 
            this.edtMobile.EditValue = "";
            this.edtMobile.Enabled = false;
            this.edtMobile.Location = new System.Drawing.Point(177, 85);
            this.edtMobile.Name = "edtMobile";
            this.edtMobile.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.edtMobile.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtMobile.Properties.Appearance.Options.UseFont = true;
            this.edtMobile.Properties.Appearance.Options.UseForeColor = true;
            this.edtMobile.Size = new System.Drawing.Size(280, 40);
            this.edtMobile.TabIndex = 291;
            this.edtMobile.Enter += new System.EventHandler(this.edtRoom_Enter);
            this.edtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtRoom_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(43, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 34);
            this.label10.TabIndex = 289;
            this.label10.Text = "手机号：";
            // 
            // button27
            // 
            this.button27.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button27.ForeColor = System.Drawing.Color.Blue;
            this.button27.Location = new System.Drawing.Point(177, 346);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(135, 52);
            this.button27.TabIndex = 288;
            this.button27.Text = "确定修改";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // frmMemberPwd
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 424);
            this.Controls.Add(this.btnGetIdentCode);
            this.Controls.Add(this.edtIdentCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnlNum);
            this.Controls.Add(this.edtPwd2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edtPwd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.edtMobile);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button27);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMemberPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.Load += new System.EventHandler(this.frmMemberStoredValue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtIdentCode.Properties)).EndInit();
            this.pnlNum.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMobile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit edtMobile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private DevExpress.XtraEditors.TextEdit edtPwd;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit edtPwd2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlNum;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.TextEdit edtIdentCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGetIdentCode;
        private System.Windows.Forms.Timer tmrGetIdentCode;

    }
}