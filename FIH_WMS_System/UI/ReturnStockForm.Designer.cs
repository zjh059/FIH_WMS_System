namespace FIH_WMS_System.UI
{
    partial class ReturnStockForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            label1 = new Label();
            txtGoodsCode = new TextBox();
            label2 = new Label();
            numQty = new NumericUpDown();
            btnConfirm = new Button();
            labelTitle = new Label();
            lblOrder = new Label();
            txtOrder = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 251);
            label1.Name = "label1";
            label1.Size = new Size(164, 31);
            label1.TabIndex = 1;
            label1.Text = "退回物料编码:";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(283, 248);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(737, 38);
            txtGoodsCode.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 340);
            label2.Name = "label2";
            label2.Size = new Size(212, 31);
            label2.TabIndex = 3;
            label2.Text = "点料机核实剩余量:";
            // 
            // numQty
            // 
            numQty.Location = new Point(283, 338);
            numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(737, 38);
            numQty.TabIndex = 4;
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = Color.MediumSeaGreen;
            btnConfirm.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(283, 466);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(388, 117);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "🚀 呼叫 AGV 搬回仓库";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            labelTitle.Location = new Point(283, 26);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(449, 44);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "🔄 产线余料退回 (AGV接驳)";
            // 
            // lblOrder
            // 
            lblOrder.AutoSize = true;
            lblOrder.Location = new Point(38, 150);
            lblOrder.Name = "lblOrder";
            lblOrder.Size = new Size(204, 31);
            lblOrder.TabIndex = 6;
            lblOrder.Text = "关联生产单(选填):";
            // 
            // txtOrder
            // 
            txtOrder.Location = new Point(283, 147);
            txtOrder.Name = "txtOrder";
            txtOrder.PlaceholderText = "扫码输入WO工单号";
            txtOrder.Size = new Size(737, 38);
            txtOrder.TabIndex = 7;
            // 
            // ReturnStockForm
            // 
            ClientSize = new Size(1073, 636);
            Controls.Add(labelTitle);
            Controls.Add(label1);
            Controls.Add(txtGoodsCode);
            Controls.Add(label2);
            Controls.Add(numQty);
            Controls.Add(btnConfirm);
            Controls.Add(lblOrder);
            Controls.Add(txtOrder);
            Font = new Font("微软雅黑", 10F);
            Name = "ReturnStockForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "产线返料点";
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label labelTitle;

        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.TextBox txtOrder;
    }
}