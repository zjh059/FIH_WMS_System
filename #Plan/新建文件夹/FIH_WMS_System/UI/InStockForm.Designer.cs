namespace FIH_WMS_System.UI
{
    partial class InStockForm
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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 46);
            label1.Name = "label1";
            label1.Size = new Size(117, 28);
            label1.TabIndex = 0;
            label1.Text = "商品编码：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(46, 131);
            label2.Name = "label2";
            label2.Size = new Size(117, 28);
            label2.TabIndex = 1;
            label2.Text = "入库数量：";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(244, 43);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(175, 34);
            txtGoodsCode.TabIndex = 2;
            // 
            // txtQty
            // 
            txtQty.Location = new Point(244, 128);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(175, 34);
            txtQty.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(46, 220);
            label3.Name = "label3";
            label3.Size = new Size(117, 28);
            label3.TabIndex = 4;
            label3.Text = "所在库位：";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(143, 318);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(152, 40);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "✅ 确认入库";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // txtLocCode
            // 
            txtLocCode.Location = new Point(244, 217);
            txtLocCode.Name = "txtLocCode";
            txtLocCode.Size = new Size(175, 34);
            txtLocCode.TabIndex = 6;
            // 
            // InStockForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(465, 414);
            Controls.Add(txtLocCode);
            Controls.Add(btnConfirm);
            Controls.Add(label3);
            Controls.Add(txtQty);
            Controls.Add(txtGoodsCode);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "InStockForm";
            Text = "InStockForm";
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
    }
}