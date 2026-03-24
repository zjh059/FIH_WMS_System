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
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 120);
            label1.Name = "label1";
            label1.Size = new Size(164, 31);
            label1.TabIndex = 1;
            label1.Text = "退回物料编码:";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(287, 117);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(246, 38);
            txtGoodsCode.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 209);
            label2.Name = "label2";
            label2.Size = new Size(212, 31);
            label2.TabIndex = 3;
            label2.Text = "点料机核实剩余量:";
            // 
            // numQty
            // 
            numQty.Location = new Point(287, 207);
            numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(246, 38);
            numQty.TabIndex = 4;
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = Color.MediumSeaGreen;
            btnConfirm.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(131, 329);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(280, 45);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "🚀 呼叫 AGV 搬回仓库";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            labelTitle.Location = new Point(61, 20);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(449, 44);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "🔄 产线余料退回 (AGV接驳)";
            // 
            // ReturnStockForm
            // 
            ClientSize = new Size(571, 420);
            Controls.Add(labelTitle);
            Controls.Add(label1);
            Controls.Add(txtGoodsCode);
            Controls.Add(label2);
            Controls.Add(numQty);
            Controls.Add(btnConfirm);
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
    }
}