namespace Library.UserControls
{
    sealed partial class UcTable
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
            this.lbTableName = new System.Windows.Forms.Label();
            this.lbCusNum = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbAmount = new System.Windows.Forms.Label();
            this.lbSep = new System.Windows.Forms.Label();
            this.plInfo = new System.Windows.Forms.Panel();
            this.plInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbTableName
            // 
            this.lbTableName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTableName.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTableName.Location = new System.Drawing.Point(0, 0);
            this.lbTableName.Name = "lbTableName";
            this.lbTableName.Size = new System.Drawing.Size(98, 22);
            this.lbTableName.TabIndex = 0;
            this.lbTableName.Text = "餐台名";
            this.lbTableName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbTableName.Click += new System.EventHandler(this.lable_Click);
            // 
            // lbCusNum
            // 
            this.lbCusNum.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbCusNum.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCusNum.Location = new System.Drawing.Point(0, 22);
            this.lbCusNum.Name = "lbCusNum";
            this.lbCusNum.Size = new System.Drawing.Size(98, 25);
            this.lbCusNum.TabIndex = 1;
            this.lbCusNum.Text = "5人桌";
            this.lbCusNum.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbCusNum.Click += new System.EventHandler(this.lable_Click);
            // 
            // lbTime
            // 
            this.lbTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTime.Location = new System.Drawing.Point(0, 0);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(44, 17);
            this.lbTime.TabIndex = 2;
            this.lbTime.Text = "00:00";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTime.Click += new System.EventHandler(this.lable_Click);
            // 
            // lbAmount
            // 
            this.lbAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbAmount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAmount.Location = new System.Drawing.Point(54, 0);
            this.lbAmount.Name = "lbAmount";
            this.lbAmount.Size = new System.Drawing.Size(44, 17);
            this.lbAmount.TabIndex = 3;
            this.lbAmount.Text = "1234";
            this.lbAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbAmount.Click += new System.EventHandler(this.lable_Click);
            // 
            // lbSep
            // 
            this.lbSep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSep.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSep.Location = new System.Drawing.Point(44, 0);
            this.lbSep.Name = "lbSep";
            this.lbSep.Size = new System.Drawing.Size(10, 17);
            this.lbSep.TabIndex = 4;
            this.lbSep.Text = "|";
            this.lbSep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbSep.Click += new System.EventHandler(this.lable_Click);
            // 
            // plInfo
            // 
            this.plInfo.Controls.Add(this.lbSep);
            this.plInfo.Controls.Add(this.lbTime);
            this.plInfo.Controls.Add(this.lbAmount);
            this.plInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plInfo.Location = new System.Drawing.Point(0, 47);
            this.plInfo.Name = "plInfo";
            this.plInfo.Size = new System.Drawing.Size(98, 17);
            this.plInfo.TabIndex = 5;
            // 
            // UcTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plInfo);
            this.Controls.Add(this.lbCusNum);
            this.Controls.Add(this.lbTableName);
            this.Name = "UcTable";
            this.Size = new System.Drawing.Size(98, 64);
            this.plInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTableName;
        private System.Windows.Forms.Label lbCusNum;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbAmount;
        private System.Windows.Forms.Label lbSep;
        private System.Windows.Forms.Panel plInfo;


    }
}
