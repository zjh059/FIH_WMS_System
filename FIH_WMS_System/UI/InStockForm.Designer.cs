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
            label4 = new Label();
            cmbStrategy = new ComboBox();
            btnRecommend = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 46);
            label1.Name = "label1";
            label1.Size = new Size(117, 28);
            label1.TabIndex = 0;
            label1.Text = "物料编码：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(429, 46);
            label2.Name = "label2";
            label2.Size = new Size(117, 28);
            label2.TabIndex = 1;
            label2.Text = "入库数量：";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.BackColor = SystemColors.Info;
            txtGoodsCode.Location = new Point(169, 43);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(220, 34);
            txtGoodsCode.TabIndex = 2;
            // 
            // txtQty
            // 
            txtQty.Location = new Point(552, 43);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(220, 34);
            txtQty.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(46, 143);
            label3.Name = "label3";
            label3.Size = new Size(117, 28);
            label3.TabIndex = 4;
            label3.Text = "所在库位：";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(552, 376);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(220, 40);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "✅ 确认入库";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // txtLocCode
            // 
            txtLocCode.Location = new Point(169, 140);
            txtLocCode.Name = "txtLocCode";
            txtLocCode.Size = new Size(220, 34);
            txtLocCode.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(46, 254);
            label4.Name = "label4";
            label4.Size = new Size(117, 28);
            label4.TabIndex = 7;
            label4.Text = "入库策略：";
            // 
            // cmbStrategy
            // 
            cmbStrategy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStrategy.FormattingEnabled = true;
            cmbStrategy.Location = new Point(169, 251);
            cmbStrategy.Name = "cmbStrategy";
            cmbStrategy.Size = new Size(603, 36);
            cmbStrategy.TabIndex = 8;
            // 
            // btnRecommend
            // 
            btnRecommend.Location = new Point(46, 376);
            btnRecommend.Name = "btnRecommend";
            btnRecommend.Size = new Size(144, 40);
            btnRecommend.TabIndex = 9;
            btnRecommend.Text = "智能推荐";
            btnRecommend.UseVisualStyleBackColor = true;
            btnRecommend.Click += btnRecommend_Click;
            // 
            // InStockForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(815, 474);
            Controls.Add(btnRecommend);
            Controls.Add(cmbStrategy);
            Controls.Add(label4);
            Controls.Add(txtLocCode);
            Controls.Add(btnConfirm);
            Controls.Add(label3);
            Controls.Add(txtQty);
            Controls.Add(txtGoodsCode);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "InStockForm";
            Text = "InStockForm";
            Load += InStockForm_Load;
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
        private ComboBox cmbStrategy;
        private Button btnRecommend;
    }
}