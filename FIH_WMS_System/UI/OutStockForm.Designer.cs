namespace FIH_WMS_System.UI
{
    partial class OutStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            gbStandard = new GroupBox();
            labelScanner = new Label();
            txtScanner = new TextBox();
            lblStrategy = new Label();
            cmbStrategy = new ComboBox();
            label1 = new Label();
            txtGoodsCode = new TextBox();
            label2 = new Label();
            txtQty = new TextBox();
            label3 = new Label();
            txtLocCode = new TextBox();
            btnConfirm = new Button();
            gbBOM = new GroupBox();
            btnExecuteBOM = new Button();
            dgvBOM = new DataGridView();
            btnAnalyze = new Button();
            txtProduceQty = new TextBox();
            lblProduceQty = new Label();
            txtProductCode = new TextBox();
            lblProduct = new Label();
            gbStandard.SuspendLayout();
            gbBOM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBOM).BeginInit();
            SuspendLayout();
            // 
            // gbStandard
            // 
            gbStandard.Controls.Add(labelScanner);
            gbStandard.Controls.Add(txtScanner);
            gbStandard.Controls.Add(lblStrategy);
            gbStandard.Controls.Add(cmbStrategy);
            gbStandard.Controls.Add(label1);
            gbStandard.Controls.Add(txtGoodsCode);
            gbStandard.Controls.Add(label2);
            gbStandard.Controls.Add(txtQty);
            gbStandard.Controls.Add(label3);
            gbStandard.Controls.Add(txtLocCode);
            gbStandard.Controls.Add(btnConfirm);
            gbStandard.Font = new Font("微软雅黑", 10F);
            gbStandard.Location = new Point(40, 53);
            gbStandard.Name = "gbStandard";
            gbStandard.Size = new Size(1255, 331);
            gbStandard.TabIndex = 0;
            gbStandard.TabStop = false;
            gbStandard.Text = "按单品出库 (常规模式)";
            // 
            // labelScanner
            // 
            labelScanner.AutoSize = true;
            labelScanner.ForeColor = Color.Blue;
            labelScanner.Location = new Point(39, 71);
            labelScanner.Name = "labelScanner";
            labelScanner.Size = new Size(140, 31);
            labelScanner.TabIndex = 0;
            labelScanner.Text = "扫码枪输入:";
            // 
            // txtScanner
            // 
            txtScanner.BackColor = SystemColors.Info;
            txtScanner.Location = new Point(185, 68);
            txtScanner.Name = "txtScanner";
            txtScanner.Size = new Size(818, 38);
            txtScanner.TabIndex = 1;
            txtScanner.KeyDown += txtScanner_KeyDown;
            // 
            // lblStrategy
            // 
            lblStrategy.AutoSize = true;
            lblStrategy.Location = new Point(556, 231);
            lblStrategy.Name = "lblStrategy";
            lblStrategy.Size = new Size(68, 31);
            lblStrategy.TabIndex = 8;
            lblStrategy.Text = "策略:";
            // 
            // cmbStrategy
            // 
            cmbStrategy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStrategy.FormattingEnabled = true;
            cmbStrategy.Location = new Point(664, 228);
            cmbStrategy.Name = "cmbStrategy";
            cmbStrategy.Size = new Size(339, 38);
            cmbStrategy.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 153);
            label1.Name = "label1";
            label1.Size = new Size(116, 31);
            label1.TabIndex = 2;
            label1.Text = "物料编码:";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(185, 150);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(339, 38);
            txtGoodsCode.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(556, 153);
            label2.Name = "label2";
            label2.Size = new Size(68, 31);
            label2.TabIndex = 4;
            label2.Text = "数量:";
            // 
            // txtQty
            // 
            txtQty.Location = new Point(664, 150);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(339, 38);
            txtQty.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 231);
            label3.Name = "label3";
            label3.Size = new Size(116, 31);
            label3.TabIndex = 6;
            label3.Text = "出库库位:";
            // 
            // txtLocCode
            // 
            txtLocCode.Location = new Point(185, 228);
            txtLocCode.Name = "txtLocCode";
            txtLocCode.Size = new Size(339, 38);
            txtLocCode.TabIndex = 7;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(1067, 68);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(156, 194);
            btnConfirm.TabIndex = 10;
            btnConfirm.Text = "确认出库";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // gbBOM
            // 
            gbBOM.Controls.Add(btnExecuteBOM);
            gbBOM.Controls.Add(dgvBOM);
            gbBOM.Controls.Add(btnAnalyze);
            gbBOM.Controls.Add(txtProduceQty);
            gbBOM.Controls.Add(lblProduceQty);
            gbBOM.Controls.Add(txtProductCode);
            gbBOM.Controls.Add(lblProduct);
            gbBOM.Font = new Font("微软雅黑", 10F);
            gbBOM.Location = new Point(40, 417);
            gbBOM.Name = "gbBOM";
            gbBOM.Size = new Size(1255, 535);
            gbBOM.TabIndex = 1;
            gbBOM.TabStop = false;
            gbBOM.Text = "按 BOM 展开出库 (产线工单模式)";
            // 
            // btnExecuteBOM
            // 
            btnExecuteBOM.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            btnExecuteBOM.ForeColor = Color.DarkGreen;
            btnExecuteBOM.Location = new Point(993, 466);
            btnExecuteBOM.Name = "btnExecuteBOM";
            btnExecuteBOM.Size = new Size(230, 45);
            btnExecuteBOM.TabIndex = 6;
            btnExecuteBOM.Text = "🚀 确认全套出库并呼叫AGV";
            btnExecuteBOM.UseVisualStyleBackColor = true;
            btnExecuteBOM.Click += btnExecuteBOM_Click;
            // 
            // dgvBOM
            // 
            dgvBOM.AllowUserToAddRows = false;
            dgvBOM.AllowUserToDeleteRows = false;
            dgvBOM.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBOM.Location = new Point(39, 133);
            dgvBOM.Name = "dgvBOM";
            dgvBOM.ReadOnly = true;
            dgvBOM.RowHeadersWidth = 51;
            dgvBOM.RowTemplate.Height = 35;
            dgvBOM.Size = new Size(1184, 327);
            dgvBOM.TabIndex = 5;
            // 
            // btnAnalyze
            // 
            btnAnalyze.Location = new Point(39, 467);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(248, 45);
            btnAnalyze.TabIndex = 4;
            btnAnalyze.Text = "📊 分析BOM齐套性";
            btnAnalyze.UseVisualStyleBackColor = true;
            btnAnalyze.Click += btnAnalyze_Click;
            // 
            // txtProduceQty
            // 
            txtProduceQty.Location = new Point(877, 78);
            txtProduceQty.Name = "txtProduceQty";
            txtProduceQty.Size = new Size(346, 38);
            txtProduceQty.TabIndex = 3;
            // 
            // lblProduceQty
            // 
            lblProduceQty.AutoSize = true;
            lblProduceQty.Location = new Point(675, 81);
            lblProduceQty.Name = "lblProduceQty";
            lblProduceQty.Size = new Size(164, 31);
            lblProduceQty.TabIndex = 2;
            lblProduceQty.Text = "计划生产数量:";
            // 
            // txtProductCode
            // 
            txtProductCode.Location = new Point(220, 78);
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Size = new Size(426, 38);
            txtProductCode.TabIndex = 1;
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(39, 81);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(164, 31);
            lblProduct.TabIndex = 0;
            lblProduct.Text = "成品生产编码:";
            // 
            // OutStockForm
            // 
            ClientSize = new Size(1333, 975);
            Controls.Add(gbBOM);
            Controls.Add(gbStandard);
            Name = "OutStockForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "智能出库与生产领料单";
            Load += OutStockForm_Load;
            gbStandard.ResumeLayout(false);
            gbStandard.PerformLayout();
            gbBOM.ResumeLayout(false);
            gbBOM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBOM).EndInit();
            ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox gbStandard;
        private System.Windows.Forms.Label labelScanner;
        private System.Windows.Forms.TextBox txtScanner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLocCode;
        private System.Windows.Forms.Label lblStrategy;
        private System.Windows.Forms.ComboBox cmbStrategy;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox gbBOM;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProduceQty;
        private System.Windows.Forms.TextBox txtProduceQty;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.DataGridView dgvBOM;
        private System.Windows.Forms.Button btnExecuteBOM;
    }
}