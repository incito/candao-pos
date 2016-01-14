namespace CnSharp.Windows.Updater
{
	partial class UpdateForm
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
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
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
      SingleAssemblyComponentResourceManager resources = new SingleAssemblyComponentResourceManager(typeof(UpdateForm));
      this.boxDes = new System.Windows.Forms.RichTextBox();
      this.lblSize = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnUpgrade = new System.Windows.Forms.Button();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.lblTitle = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // boxDes
      // 
      resources.ApplyResources(this.boxDes, "boxDes");
      this.boxDes.BackColor = System.Drawing.SystemColors.Window;
      this.boxDes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.boxDes.Name = "boxDes";
      this.boxDes.ReadOnly = true;
      // 
      // lblSize
      // 
      resources.ApplyResources(this.lblSize, "lblSize");
      this.lblSize.Name = "lblSize";
      // 
      // btnCancel
      // 
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnUpgrade
      // 
      resources.ApplyResources(this.btnUpgrade, "btnUpgrade");
      this.btnUpgrade.Name = "btnUpgrade";
      this.btnUpgrade.UseVisualStyleBackColor = true;
      this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
      // 
      // progressBar1
      // 
      resources.ApplyResources(this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      // 
      // lblTitle
      // 
      resources.ApplyResources(this.lblTitle, "lblTitle");
      this.lblTitle.Name = "lblTitle";
      // 
      // panel1
      // 
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Controls.Add(this.boxDes);
      this.panel1.Controls.Add(this.btnCancel);
      this.panel1.Controls.Add(this.btnUpgrade);
      this.panel1.Controls.Add(this.lblSize);
      this.panel1.Controls.Add(this.progressBar1);
      this.panel1.Controls.Add(this.lblTitle);
      this.panel1.Name = "panel1";
      // 
      // UpdateForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UpdateForm";
      this.ShowIcon = false;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdate_FormClosing);
      this.Load += new System.EventHandler(this.FormUpdate_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnUpgrade;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.RichTextBox boxDes;
		private System.Windows.Forms.Panel panel1;
	}
}