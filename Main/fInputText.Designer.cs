namespace Main
{
    partial class frmInputText
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
            this.rbtDiscount = new System.Windows.Forms.RadioButton();
            this.rbtAmount = new System.Windows.Forms.RadioButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.edtMemberCard = new DevExpress.XtraEditors.TextEdit();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlNum = new System.Windows.Forms.Panel();
            this.button28 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMemberCard.Properties)).BeginInit();
            this.pnlNum.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtDiscount
            // 
            this.rbtDiscount.AutoSize = true;
            this.rbtDiscount.Font = new System.Drawing.Font("Tahoma", 20F);
            this.rbtDiscount.Location = new System.Drawing.Point(16, 61);
            this.rbtDiscount.Name = "rbtDiscount";
            this.rbtDiscount.Size = new System.Drawing.Size(87, 37);
            this.rbtDiscount.TabIndex = 303;
            this.rbtDiscount.Text = "折扣";
            this.rbtDiscount.UseVisualStyleBackColor = true;
            this.rbtDiscount.CheckedChanged += new System.EventHandler(this.rbtDiscount_CheckedChanged);
            // 
            // rbtAmount
            // 
            this.rbtAmount.AutoSize = true;
            this.rbtAmount.Checked = true;
            this.rbtAmount.Font = new System.Drawing.Font("Tahoma", 20F);
            this.rbtAmount.Location = new System.Drawing.Point(16, 12);
            this.rbtAmount.Name = "rbtAmount";
            this.rbtAmount.Size = new System.Drawing.Size(87, 37);
            this.rbtAmount.TabIndex = 302;
            this.rbtAmount.TabStop = true;
            this.rbtAmount.Text = "金额";
            this.rbtAmount.UseVisualStyleBackColor = true;
            this.rbtAmount.CheckedChanged += new System.EventHandler(this.rbtAmount_CheckedChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::KYPOS.Properties.Resources.close;
            this.pictureBox3.Location = new System.Drawing.Point(21, 119);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(66, 66);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 301;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // edtMemberCard
            // 
            this.edtMemberCard.EditValue = "";
            this.edtMemberCard.Location = new System.Drawing.Point(116, 34);
            this.edtMemberCard.Name = "edtMemberCard";
            this.edtMemberCard.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.edtMemberCard.Properties.Appearance.Options.UseFont = true;
            this.edtMemberCard.Size = new System.Drawing.Size(283, 46);
            this.edtMemberCard.TabIndex = 299;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTitle.Location = new System.Drawing.Point(105, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(168, 34);
            this.lblTitle.TabIndex = 300;
            this.lblTitle.Text = "金额/折扣";
            // 
            // pnlNum
            // 
            this.pnlNum.BackColor = System.Drawing.Color.White;
            this.pnlNum.Controls.Add(this.button28);
            this.pnlNum.Controls.Add(this.button27);
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
            this.pnlNum.Location = new System.Drawing.Point(109, 86);
            this.pnlNum.Name = "pnlNum";
            this.pnlNum.Size = new System.Drawing.Size(290, 236);
            this.pnlNum.TabIndex = 263;
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
            // button27
            // 
            this.button27.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button27.ForeColor = System.Drawing.Color.Blue;
            this.button27.Location = new System.Drawing.Point(220, 62);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(65, 170);
            this.button27.TabIndex = 287;
            this.button27.Text = "确定";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
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
            // frmInputText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 328);
            this.Controls.Add(this.rbtDiscount);
            this.Controls.Add(this.rbtAmount);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.edtMemberCard);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlNum);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请输入鱼券编号";
            this.Activated += new System.EventHandler(this.frmInputText_Activated);
            this.Load += new System.EventHandler(this.frmInputText_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMemberCard.Properties)).EndInit();
            this.pnlNum.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlNum;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button27;
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
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.TextEdit edtMemberCard;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.RadioButton rbtAmount;
        private System.Windows.Forms.RadioButton rbtDiscount;
    }
}