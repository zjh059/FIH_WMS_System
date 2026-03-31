namespace FIH_WMS_System.UI
{
    partial class WarningForm
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
            btnAutoOrder = new Button();
            dgv = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("微软雅黑", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Red;
            lblTitle.Location = new Point(29, 28);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(932, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "⚠️ 以下物料已低于安全库存底线，随时可能导致产线停工，请及时处理！";
            // 
            // btnAutoOrder
            // 
            btnAutoOrder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAutoOrder.BackColor = Color.Orange;
            btnAutoOrder.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnAutoOrder.ForeColor = Color.White;
            btnAutoOrder.Location = new Point(977, 20);
            btnAutoOrder.Margin = new Padding(4, 4, 4, 4);
            btnAutoOrder.Name = "btnAutoOrder";
            btnAutoOrder.Size = new Size(318, 56);
            btnAutoOrder.TabIndex = 1;
            btnAutoOrder.Text = "🚀 一键生成采购补货单";
            btnAutoOrder.UseVisualStyleBackColor = false;
            btnAutoOrder.Click += BtnAutoOrder_Click;
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.WhiteSmoke;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new Point(29, 98);
            dgv.Margin = new Padding(4, 4, 4, 4);
            dgv.Name = "dgv";
            dgv.ReadOnly = true;
            dgv.RowHeadersWidth = 51;
            dgv.RowTemplate.Height = 29;
            dgv.Size = new Size(1266, 555);
            dgv.TabIndex = 2;
            // 
            // WarningForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1325, 700);
            Controls.Add(dgv);
            Controls.Add(btnAutoOrder);
            Controls.Add(lblTitle);
            Margin = new Padding(4, 4, 4, 4);
            Name = "WarningForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🚨 智能库存预警与补货中心";
            Load += WarningForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAutoOrder;
        private System.Windows.Forms.DataGridView dgv;
    }
}