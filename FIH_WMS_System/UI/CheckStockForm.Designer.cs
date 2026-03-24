namespace FIH_WMS_System.UI
{
    partial class CheckStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblType = new Label();
            cmbCountType = new ComboBox();
            lblKeyword = new Label();
            txtKeyword = new TextBox();
            btnGenerate = new Button();
            dgvCountList = new DataGridView();
            btnSubmit = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCountList).BeginInit();
            SuspendLayout();
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(20, 25);
            lblType.Name = "lblType";
            lblType.Size = new Size(117, 28);
            lblType.TabIndex = 0;
            lblType.Text = "盘点方式：";
            // 
            // cmbCountType
            // 
            cmbCountType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCountType.FormattingEnabled = true;
            cmbCountType.Location = new Point(135, 22);
            cmbCountType.Name = "cmbCountType";
            cmbCountType.Size = new Size(310, 36);
            cmbCountType.TabIndex = 1;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(462, 25);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(168, 28);
            lblKeyword.TabIndex = 2;
            lblKeyword.Text = "库位/物料编码：";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(632, 22);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(251, 34);
            txtKeyword.TabIndex = 3;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(1073, 23);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(237, 32);
            btnGenerate.TabIndex = 4;
            btnGenerate.Text = "🔍 生成盘点清单";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // dgvCountList
            // 
            dgvCountList.AllowUserToAddRows = false;
            dgvCountList.AllowUserToDeleteRows = false;
            dgvCountList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCountList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCountList.Location = new Point(20, 70);
            dgvCountList.Name = "dgvCountList";
            dgvCountList.RowHeadersWidth = 51;
            dgvCountList.RowTemplate.Height = 29;
            dgvCountList.Size = new Size(1290, 758);
            dgvCountList.TabIndex = 5;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmit.Location = new Point(1110, 848);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(200, 45);
            btnSubmit.TabIndex = 6;
            btnSubmit.Text = "✅ 确认提交差异并平账";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // CheckStockForm
            // 
            ClientSize = new Size(1332, 911);
            Controls.Add(btnSubmit);
            Controls.Add(dgvCountList);
            Controls.Add(btnGenerate);
            Controls.Add(txtKeyword);
            Controls.Add(lblKeyword);
            Controls.Add(cmbCountType);
            Controls.Add(lblType);
            Name = "CheckStockForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "智能库存盘点";
            Load += CheckStockForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCountList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbCountType;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView dgvCountList;
        private System.Windows.Forms.Button btnSubmit;
    }
}