namespace CnSharp.Windows.Updater.SharpPack
{
	partial class VersionEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionEditForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.projectGrid = new System.Windows.Forms.DataGridView();
            this.ColProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSame = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(509, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Next>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(643, 405);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.projectGrid);
            this.groupBox1.Location = new System.Drawing.Point(24, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(694, 350);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Projects";
            // 
            // projectGrid
            // 
            this.projectGrid.AllowUserToAddRows = false;
            this.projectGrid.AllowUserToDeleteRows = false;
            this.projectGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.projectGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColProductName,
            this.ColVersion});
            this.projectGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectGrid.Location = new System.Drawing.Point(3, 17);
            this.projectGrid.Name = "projectGrid";
            this.projectGrid.RowTemplate.Height = 23;
            this.projectGrid.Size = new System.Drawing.Size(688, 330);
            this.projectGrid.TabIndex = 0;
            // 
            // ColProductName
            // 
            this.ColProductName.FillWeight = 200F;
            this.ColProductName.HeaderText = "Project Name";
            this.ColProductName.Name = "ColProductName";
            this.ColProductName.ReadOnly = true;
            // 
            // ColVersion
            // 
            this.ColVersion.HeaderText = "Version";
            this.ColVersion.Name = "ColVersion";
            // 
            // chkSame
            // 
            this.chkSame.AutoSize = true;
            this.chkSame.Location = new System.Drawing.Point(27, 409);
            this.chkSame.Name = "chkSame";
            this.chkSame.Size = new System.Drawing.Size(222, 16);
            this.chkSame.TabIndex = 7;
            this.chkSame.Text = "Same Version as the First Project";
            this.chkSame.UseVisualStyleBackColor = true;
            this.chkSame.CheckedChanged += new System.EventHandler(this.chkSame_CheckedChanged);
            // 
            // VersionEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 450);
            this.Controls.Add(this.chkSame);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Step 1: Edit Version Number";
            this.Load += new System.EventHandler(this.FormVersionLoad);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.projectGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

  private System.Windows.Forms.Button button1;
  private System.Windows.Forms.Button button2;
  private System.Windows.Forms.GroupBox groupBox1;
  private System.Windows.Forms.DataGridView projectGrid;
  private System.Windows.Forms.DataGridViewTextBoxColumn ColProductName;
  private System.Windows.Forms.DataGridViewTextBoxColumn ColVersion;
  private System.Windows.Forms.CheckBox chkSame;
	}
}