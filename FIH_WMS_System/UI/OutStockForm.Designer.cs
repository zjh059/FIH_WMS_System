namespace FIH_WMS_System.UI
{
    partial class OutStockForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            label1 = new Label();
            label2 = new Label();
            txtGoodsCode = new TextBox();
            txtQty = new TextBox();
            label3 = new Label();
            btnConfirm = new Button();
            txtLocCode = new TextBox();
            label4 = new Label();
            txtScanner = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(87, 230);
            label1.Name = "label1";
            label1.Size = new Size(117, 28);
            label1.TabIndex = 0;
            label1.Text = "商品编码：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(87, 315);
            label2.Name = "label2";
            label2.Size = new Size(117, 28);
            label2.TabIndex = 1;
            label2.Text = "出库数量：";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(285, 227);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(175, 34);
            txtGoodsCode.TabIndex = 2;
            // 
            // txtQty
            // 
            txtQty.Location = new Point(285, 312);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(175, 34);
            txtQty.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(87, 404);
            label3.Name = "label3";
            label3.Size = new Size(117, 28);
            label3.TabIndex = 4;
            label3.Text = "所在库位：";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(169, 501);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(152, 40);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "✅ 确认出库";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // txtLocCode
            // 
            txtLocCode.Location = new Point(285, 401);
            txtLocCode.Name = "txtLocCode";
            txtLocCode.Size = new Size(175, 34);
            txtLocCode.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(169, 58);
            label4.Name = "label4";
            label4.Size = new Size(194, 28);
            label4.TabIndex = 7;
            label4.Text = "🔫 扫码枪接收区：";
            // 
            // txtScanner
            // 
            txtScanner.BackColor = SystemColors.Info;
            txtScanner.Location = new Point(54, 127);
            txtScanner.Name = "txtScanner";
            txtScanner.Size = new Size(445, 34);
            txtScanner.TabIndex = 8;
            txtScanner.KeyDown += txtScanner_KeyDown;
            // 
            // OutStockForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(558, 612);
            Controls.Add(txtScanner);
            Controls.Add(label4);
            Controls.Add(txtLocCode);
            Controls.Add(btnConfirm);
            Controls.Add(label3);
            Controls.Add(txtQty);
            Controls.Add(txtGoodsCode);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "OutStockForm";
            Text = "OutStockForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtGoodsCode;
        private TextBox txtQty;
        private Label label3;
        private Button btnConfirm;
        private TextBox txtLocCode;
        private Label label4;
        private TextBox txtScanner;
    }
}