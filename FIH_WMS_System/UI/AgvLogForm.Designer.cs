namespace FIH_WMS_System.UI
{
    partial class AgvLogForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvAgvLogs = new System.Windows.Forms.DataGridView();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtTaskFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgvLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAgvLogs
            // 
            this.dgvAgvLogs.AllowUserToAddRows = false;
            this.dgvAgvLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvAgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgvLogs.Location = new System.Drawing.Point(20, 110);
            this.dgvAgvLogs.Name = "dgvAgvLogs";
            this.dgvAgvLogs.ReadOnly = true;
            this.dgvAgvLogs.RowHeadersVisible = false;
            this.dgvAgvLogs.RowTemplate.Height = 30;
            this.dgvAgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAgvLogs.Size = new System.Drawing.Size(840, 430);
            this.dgvAgvLogs.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblHeader.Location = new System.Drawing.Point(20, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(262, 31);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "🚚 AGV 运行轨迹追溯";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(740, 65);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 35);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出日志";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtTaskFilter
            // 
            this.txtTaskFilter.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtTaskFilter.Location = new System.Drawing.Point(140, 68);
            this.txtTaskFilter.Name = "txtTaskFilter";
            this.txtTaskFilter.Size = new System.Drawing.Size(250, 27);
            this.txtTaskFilter.TabIndex = 3;
            this.txtTaskFilter.TextChanged += new System.EventHandler(this.txtTaskFilter_TextChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(25, 72);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(109, 20);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "输入任务单号：";
            // 
            // AgvLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(255)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.txtTaskFilter);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.dgvAgvLogs);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Name = "AgvLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AGV 运行轨迹日志";
            this.Load += new System.EventHandler(this.AgvLogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAgvLogs;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtTaskFilter;
        private System.Windows.Forms.Label lblFilter;
    }
}