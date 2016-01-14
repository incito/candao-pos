namespace Main
{
    partial class frmPermission2
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
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.button15 = new System.Windows.Forms.Button();
            this.txtPwd = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlNum = new System.Windows.Forms.Panel();
            this.button11 = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button27 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlNum.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.EditValue = "";
            this.txtUser.Location = new System.Drawing.Point(135, 144);
            this.txtUser.Name = "txtUser";
            this.txtUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.txtUser.Properties.Appearance.Options.UseFont = true;
            this.txtUser.Size = new System.Drawing.Size(163, 36);
            this.txtUser.TabIndex = 296;
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            this.txtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F);
            this.label1.Location = new System.Drawing.Point(70, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 294;
            this.label1.Text = "编号：";
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button15.ForeColor = System.Drawing.Color.Blue;
            this.button15.Location = new System.Drawing.Point(602, 291);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(85, 56);
            this.button15.TabIndex = 286;
            this.button15.Text = ".";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Visible = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.EditValue = "";
            this.txtPwd.Location = new System.Drawing.Point(368, 145);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.txtPwd.Properties.Appearance.Options.UseFont = true;
            this.txtPwd.Properties.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(163, 36);
            this.txtPwd.TabIndex = 297;
            this.txtPwd.Enter += new System.EventHandler(this.txtPwd_Enter);
            this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F);
            this.label2.Location = new System.Drawing.Point(302, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 29);
            this.label2.TabIndex = 295;
            this.label2.Text = "密码：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(638, 67);
            this.panel1.TabIndex = 293;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::KYPOS.Properties.Resources._return;
            this.pictureBox3.Location = new System.Drawing.Point(560, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(66, 66);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 29;
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
            this.pictureBox1.InitialImage = global::KYPOS.Properties.Resources._0111__31_cd;
            this.pictureBox1.Location = new System.Drawing.Point(2, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 65);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // pnlNum
            // 
            this.pnlNum.BackColor = System.Drawing.Color.White;
            this.pnlNum.Controls.Add(this.button11);
            this.pnlNum.Controls.Add(this.button28);
            this.pnlNum.Controls.Add(this.button2);
            this.pnlNum.Controls.Add(this.button16);
            this.pnlNum.Controls.Add(this.button18);
            this.pnlNum.Controls.Add(this.button19);
            this.pnlNum.Controls.Add(this.button20);
            this.pnlNum.Controls.Add(this.button21);
            this.pnlNum.Controls.Add(this.button22);
            this.pnlNum.Controls.Add(this.button23);
            this.pnlNum.Controls.Add(this.button24);
            this.pnlNum.Controls.Add(this.button25);
            this.pnlNum.Controls.Add(this.button26);
            this.pnlNum.Location = new System.Drawing.Point(78, 201);
            this.pnlNum.Name = "pnlNum";
            this.pnlNum.Size = new System.Drawing.Size(487, 236);
            this.pnlNum.TabIndex = 292;
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button11.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button11.Location = new System.Drawing.Point(142, 176);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(129, 56);
            this.button11.TabIndex = 289;
            this.button11.Text = "清除";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button28
            // 
            this.button28.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button28.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button28.Location = new System.Drawing.Point(277, 176);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(129, 56);
            this.button28.TabIndex = 288;
            this.button28.Text = "<-";
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button2.Location = new System.Drawing.Point(412, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 227);
            this.button2.TabIndex = 287;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button16.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button16.Location = new System.Drawing.Point(7, 176);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(129, 56);
            this.button16.TabIndex = 285;
            this.button16.Text = "0";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button26_Click);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button18.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button18.Location = new System.Drawing.Point(277, 119);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(129, 56);
            this.button18.TabIndex = 283;
            this.button18.Text = "9";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button26_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button19.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button19.Location = new System.Drawing.Point(142, 119);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(129, 56);
            this.button19.TabIndex = 282;
            this.button19.Text = "8";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button26_Click);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button20.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button20.Location = new System.Drawing.Point(7, 119);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(129, 56);
            this.button20.TabIndex = 281;
            this.button20.Text = "7";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button26_Click);
            // 
            // button21
            // 
            this.button21.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button21.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button21.Location = new System.Drawing.Point(277, 62);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(129, 56);
            this.button21.TabIndex = 280;
            this.button21.Text = "6";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button26_Click);
            // 
            // button22
            // 
            this.button22.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button22.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button22.Location = new System.Drawing.Point(142, 62);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(129, 56);
            this.button22.TabIndex = 279;
            this.button22.Text = "5";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button26_Click);
            // 
            // button23
            // 
            this.button23.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button23.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button23.Location = new System.Drawing.Point(7, 62);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(129, 56);
            this.button23.TabIndex = 278;
            this.button23.Text = "4";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button26_Click);
            // 
            // button24
            // 
            this.button24.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button24.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button24.Location = new System.Drawing.Point(277, 5);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(129, 56);
            this.button24.TabIndex = 277;
            this.button24.Text = "3";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button26_Click);
            // 
            // button25
            // 
            this.button25.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button25.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button25.Location = new System.Drawing.Point(142, 5);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(129, 56);
            this.button25.TabIndex = 276;
            this.button25.Text = "2";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button26_Click);
            // 
            // button26
            // 
            this.button26.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button26.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.button26.Location = new System.Drawing.Point(7, 5);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(129, 56);
            this.button26.TabIndex = 275;
            this.button26.Text = "1";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(602, 411);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 52);
            this.button1.TabIndex = 290;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label10.Location = new System.Drawing.Point(69, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(219, 34);
            this.label10.TabIndex = 289;
            this.label10.Text = "经理开业授权";
            // 
            // button27
            // 
            this.button27.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button27.ForeColor = System.Drawing.Color.Blue;
            this.button27.Location = new System.Drawing.Point(602, 353);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(116, 52);
            this.button27.TabIndex = 288;
            this.button27.Text = "确定";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Visible = false;
            // 
            // frmPermission2
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 467);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlNum);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button27);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPermission2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Activated += new System.EventHandler(this.frmPermission2_Activated);
            this.Load += new System.EventHandler(this.frmPermission2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlNum.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnlNum;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtPwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.PictureBox pictureBox3;

    }
}