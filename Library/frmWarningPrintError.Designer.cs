namespace Library
{
    partial class FrmWarningPrintError
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
            this.RtbMsg = new System.Windows.Forms.RichTextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.CbNoWarning = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // RtbMsg
            // 
            this.RtbMsg.BackColor = System.Drawing.Color.White;
            this.RtbMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RtbMsg.Cursor = System.Windows.Forms.Cursors.Default;
            this.RtbMsg.Enabled = false;
            this.RtbMsg.Font = new System.Drawing.Font("Tahoma", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RtbMsg.ForeColor = System.Drawing.Color.Red;
            this.RtbMsg.Location = new System.Drawing.Point(2, 69);
            this.RtbMsg.Name = "RtbMsg";
            this.RtbMsg.Size = new System.Drawing.Size(535, 106);
            this.RtbMsg.TabIndex = 303;
            this.RtbMsg.Text = "确认消息";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Library.ResImage.ok;
            this.pictureBox4.Location = new System.Drawing.Point(435, 181);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(66, 66);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 303;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 67);
            this.panel1.TabIndex = 302;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(957, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox2.Image = global::Library.ResImage._0111__31_cd;
            this.pictureBox2.Location = new System.Drawing.Point(2, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(211, 65);
            this.pictureBox2.TabIndex = 26;
            this.pictureBox2.TabStop = false;
            // 
            // CbNoWarning
            // 
            this.CbNoWarning.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbNoWarning.Location = new System.Drawing.Point(13, 202);
            this.CbNoWarning.Name = "CbNoWarning";
            this.CbNoWarning.Size = new System.Drawing.Size(250, 33);
            this.CbNoWarning.TabIndex = 304;
            this.CbNoWarning.Text = "10分钟以内不再提醒";
            this.CbNoWarning.UseVisualStyleBackColor = true;
            // 
            // FrmWarningPrintError
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 247);
            this.Controls.Add(this.CbNoWarning);
            this.Controls.Add(this.RtbMsg);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWarningPrintError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmWarning_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmWarning_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.RichTextBox RtbMsg;
        private System.Windows.Forms.CheckBox CbNoWarning;
    }
}