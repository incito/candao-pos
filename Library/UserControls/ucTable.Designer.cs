namespace Library.UserControls
{
    partial class ucTable
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNo = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNo
            // 
            this.lblNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNo.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNo.Location = new System.Drawing.Point(0, 0);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(89, 24);
            this.lblNo.TabIndex = 0;
            this.lblNo.Text = "001";
            this.lblNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblNo.Click += new System.EventHandler(this.lblNo_Click);
            // 
            // lbl2
            // 
            this.lbl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl2.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl2.ForeColor = System.Drawing.Color.Gray;
            this.lbl2.Location = new System.Drawing.Point(0, 24);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(89, 24);
            this.lbl2.TabIndex = 1;
            this.lbl2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ucTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lblNo);
            this.Name = "ucTable";
            this.Size = new System.Drawing.Size(89, 59);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblNo;
        public System.Windows.Forms.Label lbl2;
    }
}
