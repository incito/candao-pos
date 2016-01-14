namespace Main
{
    partial class frmOpenTable
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
            this.tmrFocus = new System.Windows.Forms.Timer(this.components);
            this.edtUserid = new System.Windows.Forms.TextBox();
            this.edtCj = new DevExpress.XtraEditors.TextEdit();
            this.lblcj = new System.Windows.Forms.Label();
            this.bthCheck4 = new DevExpress.XtraEditors.CheckButton();
            this.bthCheck3 = new DevExpress.XtraEditors.CheckButton();
            this.bthCheck2 = new DevExpress.XtraEditors.CheckButton();
            this.bthCheck1 = new DevExpress.XtraEditors.CheckButton();
            this.edtwomanNum = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.edtmanNum = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.edtRoom = new DevExpress.XtraEditors.TextEdit();
            this.lblRmNo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button27 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.edtCj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtwomanNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtmanNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtRoom.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlNum.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrFocus
            // 
            this.tmrFocus.Interval = 1000;
            this.tmrFocus.Tick += new System.EventHandler(this.tmrFocus_Tick);
            // 
            // edtUserid
            // 
            this.edtUserid.Font = new System.Drawing.Font("Tahoma", 18F);
            this.edtUserid.Location = new System.Drawing.Point(41, 111);
            this.edtUserid.Name = "edtUserid";
            this.edtUserid.Size = new System.Drawing.Size(280, 36);
            this.edtUserid.TabIndex = 308;
            this.edtUserid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtRoom_KeyPress);
            // 
            // edtCj
            // 
            this.edtCj.EditValue = "";
            this.edtCj.Location = new System.Drawing.Point(41, 373);
            this.edtCj.Name = "edtCj";
            this.edtCj.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.edtCj.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtCj.Properties.Appearance.Options.UseFont = true;
            this.edtCj.Properties.Appearance.Options.UseForeColor = true;
            this.edtCj.Size = new System.Drawing.Size(280, 36);
            this.edtCj.TabIndex = 307;
            this.edtCj.Enter += new System.EventHandler(this.edtCj_Enter);
            // 
            // lblcj
            // 
            this.lblcj.AutoSize = true;
            this.lblcj.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblcj.Location = new System.Drawing.Point(37, 346);
            this.lblcj.Name = "lblcj";
            this.lblcj.Size = new System.Drawing.Size(58, 24);
            this.lblcj.TabIndex = 306;
            this.lblcj.Text = "餐具";
            // 
            // bthCheck4
            // 
            this.bthCheck4.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.bthCheck4.Appearance.Options.UseFont = true;
            this.bthCheck4.Location = new System.Drawing.Point(321, 413);
            this.bthCheck4.Name = "bthCheck4";
            this.bthCheck4.Size = new System.Drawing.Size(88, 41);
            this.bthCheck4.TabIndex = 303;
            this.bthCheck4.Tag = "4";
            this.bthCheck4.Text = "老年";
            // 
            // bthCheck3
            // 
            this.bthCheck3.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.bthCheck3.Appearance.Options.UseFont = true;
            this.bthCheck3.Location = new System.Drawing.Point(227, 413);
            this.bthCheck3.Name = "bthCheck3";
            this.bthCheck3.Size = new System.Drawing.Size(88, 41);
            this.bthCheck3.TabIndex = 302;
            this.bthCheck3.Tag = "3";
            this.bthCheck3.Text = "中年";
            // 
            // bthCheck2
            // 
            this.bthCheck2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.bthCheck2.Appearance.Options.UseFont = true;
            this.bthCheck2.Location = new System.Drawing.Point(133, 413);
            this.bthCheck2.Name = "bthCheck2";
            this.bthCheck2.Size = new System.Drawing.Size(88, 41);
            this.bthCheck2.TabIndex = 301;
            this.bthCheck2.Tag = "2";
            this.bthCheck2.Text = "青年";
            // 
            // bthCheck1
            // 
            this.bthCheck1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.bthCheck1.Appearance.Options.UseFont = true;
            this.bthCheck1.Location = new System.Drawing.Point(39, 413);
            this.bthCheck1.Name = "bthCheck1";
            this.bthCheck1.Size = new System.Drawing.Size(88, 41);
            this.bthCheck1.TabIndex = 300;
            this.bthCheck1.Tag = "1";
            this.bthCheck1.Text = "儿童";
            // 
            // edtwomanNum
            // 
            this.edtwomanNum.EditValue = "";
            this.edtwomanNum.Location = new System.Drawing.Point(41, 306);
            this.edtwomanNum.Name = "edtwomanNum";
            this.edtwomanNum.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.edtwomanNum.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtwomanNum.Properties.Appearance.Options.UseFont = true;
            this.edtwomanNum.Properties.Appearance.Options.UseForeColor = true;
            this.edtwomanNum.Size = new System.Drawing.Size(280, 36);
            this.edtwomanNum.TabIndex = 299;
            this.edtwomanNum.EditValueChanged += new System.EventHandler(this.edtmanNum_EditValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(37, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 24);
            this.label3.TabIndex = 298;
            this.label3.Text = "就餐人数（女）";
            // 
            // edtmanNum
            // 
            this.edtmanNum.EditValue = "";
            this.edtmanNum.Location = new System.Drawing.Point(41, 243);
            this.edtmanNum.Name = "edtmanNum";
            this.edtmanNum.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.edtmanNum.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtmanNum.Properties.Appearance.Options.UseFont = true;
            this.edtmanNum.Properties.Appearance.Options.UseForeColor = true;
            this.edtmanNum.Size = new System.Drawing.Size(280, 36);
            this.edtmanNum.TabIndex = 297;
            this.edtmanNum.EditValueChanged += new System.EventHandler(this.edtmanNum_EditValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(37, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 24);
            this.label1.TabIndex = 296;
            this.label1.Text = "就餐人数（男）";
            // 
            // edtRoom
            // 
            this.edtRoom.EditValue = "";
            this.edtRoom.Enabled = false;
            this.edtRoom.Location = new System.Drawing.Point(41, 177);
            this.edtRoom.Name = "edtRoom";
            this.edtRoom.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.edtRoom.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.edtRoom.Properties.Appearance.Options.UseFont = true;
            this.edtRoom.Properties.Appearance.Options.UseForeColor = true;
            this.edtRoom.Size = new System.Drawing.Size(280, 36);
            this.edtRoom.TabIndex = 295;
            // 
            // lblRmNo
            // 
            this.lblRmNo.AutoSize = true;
            this.lblRmNo.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRmNo.Location = new System.Drawing.Point(35, 150);
            this.lblRmNo.Name = "lblRmNo";
            this.lblRmNo.Size = new System.Drawing.Size(58, 24);
            this.lblRmNo.TabIndex = 294;
            this.lblRmNo.Text = "桌号";
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
            this.panel1.Size = new System.Drawing.Size(663, 67);
            this.panel1.TabIndex = 293;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::KYPOS.Properties.Resources._return;
            this.pictureBox3.Location = new System.Drawing.Point(588, 1);
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
            this.pictureBox1.InitialImage = global::KYPOS.Properties.Resources.logintop;
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
            this.pnlNum.Location = new System.Drawing.Point(349, 111);
            this.pnlNum.Name = "pnlNum";
            this.pnlNum.Size = new System.Drawing.Size(305, 248);
            this.pnlNum.TabIndex = 292;
            // 
            // button28
            // 
            this.button28.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button28.ForeColor = System.Drawing.Color.Blue;
            this.button28.Location = new System.Drawing.Point(237, 3);
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
            this.button2.Location = new System.Drawing.Point(237, 60);
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
            this.button15.Location = new System.Drawing.Point(159, 174);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(77, 56);
            this.button15.TabIndex = 286;
            this.button15.Text = ".";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button26_Click);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button16.ForeColor = System.Drawing.Color.Blue;
            this.button16.Location = new System.Drawing.Point(81, 174);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(77, 56);
            this.button16.TabIndex = 285;
            this.button16.Text = "0";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button26_Click);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button17.ForeColor = System.Drawing.Color.Blue;
            this.button17.Location = new System.Drawing.Point(3, 173);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(77, 56);
            this.button17.TabIndex = 284;
            this.button17.Text = "00";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button18.ForeColor = System.Drawing.Color.Blue;
            this.button18.Location = new System.Drawing.Point(159, 117);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(77, 56);
            this.button18.TabIndex = 283;
            this.button18.Text = "9";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button26_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button19.ForeColor = System.Drawing.Color.Blue;
            this.button19.Location = new System.Drawing.Point(81, 117);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(77, 56);
            this.button19.TabIndex = 282;
            this.button19.Text = "8";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button26_Click);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button20.ForeColor = System.Drawing.Color.Blue;
            this.button20.Location = new System.Drawing.Point(3, 116);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(77, 56);
            this.button20.TabIndex = 281;
            this.button20.Text = "7";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button26_Click);
            // 
            // button21
            // 
            this.button21.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button21.ForeColor = System.Drawing.Color.Blue;
            this.button21.Location = new System.Drawing.Point(159, 60);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(77, 56);
            this.button21.TabIndex = 280;
            this.button21.Text = "6";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button26_Click);
            // 
            // button22
            // 
            this.button22.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button22.ForeColor = System.Drawing.Color.Blue;
            this.button22.Location = new System.Drawing.Point(81, 60);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(77, 56);
            this.button22.TabIndex = 279;
            this.button22.Text = "5";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button26_Click);
            // 
            // button23
            // 
            this.button23.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button23.ForeColor = System.Drawing.Color.Blue;
            this.button23.Location = new System.Drawing.Point(3, 59);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(77, 56);
            this.button23.TabIndex = 278;
            this.button23.Text = "4";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button26_Click);
            // 
            // button24
            // 
            this.button24.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button24.ForeColor = System.Drawing.Color.Blue;
            this.button24.Location = new System.Drawing.Point(159, 3);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(77, 56);
            this.button24.TabIndex = 277;
            this.button24.Text = "3";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button26_Click);
            // 
            // button25
            // 
            this.button25.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button25.ForeColor = System.Drawing.Color.Blue;
            this.button25.Location = new System.Drawing.Point(81, 3);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(77, 56);
            this.button25.TabIndex = 276;
            this.button25.Text = "2";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button26_Click);
            // 
            // button26
            // 
            this.button26.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button26.ForeColor = System.Drawing.Color.Blue;
            this.button26.Location = new System.Drawing.Point(3, 2);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(77, 56);
            this.button26.TabIndex = 275;
            this.button26.Text = "1";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(208, 460);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 52);
            this.button1.TabIndex = 290;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(35, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 24);
            this.label10.TabIndex = 289;
            this.label10.Text = "服务员编号：";
            // 
            // button27
            // 
            this.button27.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button27.ForeColor = System.Drawing.Color.Blue;
            this.button27.Location = new System.Drawing.Point(39, 460);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(159, 52);
            this.button27.TabIndex = 288;
            this.button27.Text = "确定开台";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // frmOpenTable
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Window;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 591);
            this.Controls.Add(this.edtUserid);
            this.Controls.Add(this.edtCj);
            this.Controls.Add(this.lblcj);
            this.Controls.Add(this.bthCheck4);
            this.Controls.Add(this.bthCheck3);
            this.Controls.Add(this.bthCheck2);
            this.Controls.Add(this.bthCheck1);
            this.Controls.Add(this.edtwomanNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edtmanNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edtRoom);
            this.Controls.Add(this.lblRmNo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlNum);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button27);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpenTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "开台";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmPettyCash_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOpenTable_FormClosed);
            this.Load += new System.EventHandler(this.frmPettyCash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtCj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtwomanNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtmanNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtRoom.Properties)).EndInit();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private DevExpress.XtraEditors.TextEdit edtRoom;
        private System.Windows.Forms.Label lblRmNo;
        private DevExpress.XtraEditors.TextEdit edtmanNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit edtwomanNum;
        private DevExpress.XtraEditors.CheckButton bthCheck1;
        private DevExpress.XtraEditors.CheckButton bthCheck2;
        private DevExpress.XtraEditors.CheckButton bthCheck3;
        private DevExpress.XtraEditors.CheckButton bthCheck4;
        private DevExpress.XtraEditors.TextEdit edtCj;
        private System.Windows.Forms.Label lblcj;
        public System.Windows.Forms.Timer tmrFocus;
        public System.Windows.Forms.TextBox edtUserid;

    }
}