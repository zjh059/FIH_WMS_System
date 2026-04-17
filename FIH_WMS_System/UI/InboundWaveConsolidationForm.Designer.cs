namespace FIH_WMS_System.UI
{
    partial class InboundWaveConsolidationForm
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
            clbOrders = new CheckedListBox();
            btnConfirm = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("微软雅黑", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(457, 34);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "请勾选需要合并为同一波次的采购单：";
            // 
            // clbOrders
            // 
            clbOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            clbOrders.CheckOnClick = true;
            clbOrders.Font = new Font("微软雅黑", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 134);
            clbOrders.FormattingEnabled = true;
            clbOrders.Location = new Point(25, 60);
            clbOrders.Name = "clbOrders";
            clbOrders.Size = new Size(910, 688);
            clbOrders.TabIndex = 1;
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnConfirm.BackColor = Color.MediumSeaGreen;
            btnConfirm.Font = new Font("微软雅黑", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(25, 789);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(910, 50);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "🌊 确认合并波次";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // InboundWaveConsolidationForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(962, 862);
            Controls.Add(btnConfirm);
            Controls.Add(clbOrders);
            Controls.Add(lblTitle);
            Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Name = "InboundWaveConsolidationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🌊 入库波次合并中心";
            Load += InboundWaveConsolidationForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckedListBox clbOrders;
        private System.Windows.Forms.Button btnConfirm;
    }
}