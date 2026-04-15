namespace FIH_WMS_System.UI
{
    partial class SysLogForm
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
            lblTitle = new Label();
            btnRefresh = new Button();
            dgvLogs = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkSlateBlue;
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(405, 44);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🛡️ 系统操作审计日志中心";
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.SteelBlue;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1079, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(220, 40);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "🔄 刷新最新日志";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // dgvLogs
            // 
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.BackgroundColor = Color.WhiteSmoke;
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogs.Location = new Point(20, 75);
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowHeadersWidth = 51;
            dgvLogs.RowTemplate.Height = 35;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.Size = new Size(1279, 686);
            dgvLogs.TabIndex = 2;
            // 
            // SysLogForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1321, 789);
            Controls.Add(dgvLogs);
            Controls.Add(btnRefresh);
            Controls.Add(lblTitle);
            Font = new Font("微软雅黑", 9F);
            Name = "SysLogForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🛡️ 系统操作审计日志";
            Load += SysLogForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvLogs;
    }
}