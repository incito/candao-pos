namespace Main
{
    partial class frmStart
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pbxMain = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(13, 341);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 14);
            this.lblMsg.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(147, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = " ";
            // 
            // pbxMain
            // 
            this.pbxMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxMain.Image = global::KYPOS.Properties.Resources.posstart1;
            this.pbxMain.Location = new System.Drawing.Point(0, 0);
            this.pbxMain.Name = "pbxMain";
            this.pbxMain.Size = new System.Drawing.Size(561, 321);
            this.pbxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxMain.TabIndex = 16;
            this.pbxMain.TabStop = false;
            this.pbxMain.Click += new System.EventHandler(this.pbxMain_Click);
            // 
            // frmStart
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 321);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pbxMain);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbxMain;
        private System.Windows.Forms.Label lblMsg;
    }
}