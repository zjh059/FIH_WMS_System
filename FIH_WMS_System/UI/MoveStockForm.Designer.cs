namespace FIH_WMS_System.UI
{
    partial class MoveStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            gbManual = new GroupBox();
            btnConfirm = new Button();
            txtQty = new TextBox();
            txtToLoc = new TextBox();
            label4 = new Label();
            label3 = new Label();
            txtFromLoc = new TextBox();
            label2 = new Label();
            txtGoodsCode = new TextBox();
            label1 = new Label();
            gbSmart = new GroupBox();
            btnExecuteSmart = new Button();
            dgvAdvice = new DataGridView();
            btnScan = new Button();
            gbManual.SuspendLayout();
            gbSmart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAdvice).BeginInit();
            SuspendLayout();
            // 
            // gbManual
            // 
            gbManual.Controls.Add(btnConfirm);
            gbManual.Controls.Add(txtQty);
            gbManual.Controls.Add(txtToLoc);
            gbManual.Controls.Add(label4);
            gbManual.Controls.Add(label3);
            gbManual.Controls.Add(txtFromLoc);
            gbManual.Controls.Add(label2);
            gbManual.Controls.Add(txtGoodsCode);
            gbManual.Controls.Add(label1);
            gbManual.Font = new Font("微软雅黑", 10F);
            gbManual.Location = new Point(28, 27);
            gbManual.Name = "gbManual";
            gbManual.Size = new Size(1354, 272);
            gbManual.TabIndex = 0;
            gbManual.TabStop = false;
            gbManual.Text = "人工指定移库 (传统模式)";
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(944, 39);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(178, 40);
            btnConfirm.TabIndex = 8;
            btnConfirm.Text = "🔄 确认移库";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // txtQty
            // 
            txtQty.Location = new Point(606, 161);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(217, 38);
            txtQty.TabIndex = 7;
            // 
            // txtToLoc
            // 
            txtToLoc.Location = new Point(189, 165);
            txtToLoc.Name = "txtToLoc";
            txtToLoc.Size = new Size(217, 38);
            txtToLoc.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(498, 164);
            label4.Name = "label4";
            label4.Size = new Size(68, 31);
            label4.TabIndex = 6;
            label4.Text = "数量:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 164);
            label3.Name = "label3";
            label3.Size = new Size(116, 31);
            label3.TabIndex = 4;
            label3.Text = "目标库位:";
            // 
            // txtFromLoc
            // 
            txtFromLoc.Location = new Point(607, 41);
            txtFromLoc.Name = "txtFromLoc";
            txtFromLoc.Size = new Size(217, 38);
            txtFromLoc.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(498, 44);
            label2.Name = "label2";
            label2.Size = new Size(92, 31);
            label2.TabIndex = 2;
            label2.Text = "源库位:";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Location = new Point(189, 41);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(217, 38);
            txtGoodsCode.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 41);
            label1.Name = "label1";
            label1.Size = new Size(116, 31);
            label1.TabIndex = 0;
            label1.Text = "物料编码:";
            // 
            // gbSmart
            // 
            gbSmart.Controls.Add(btnExecuteSmart);
            gbSmart.Controls.Add(dgvAdvice);
            gbSmart.Controls.Add(btnScan);
            gbSmart.Font = new Font("微软雅黑", 10F);
            gbSmart.Location = new Point(28, 343);
            gbSmart.Name = "gbSmart";
            gbSmart.Size = new Size(1354, 444);
            gbSmart.TabIndex = 1;
            gbSmart.TabStop = false;
            gbSmart.Text = "智能碎片整理引擎 (系统建议模式)";
            // 
            // btnExecuteSmart
            // 
            btnExecuteSmart.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            btnExecuteSmart.ForeColor = Color.DarkGreen;
            btnExecuteSmart.Location = new Point(1032, 375);
            btnExecuteSmart.Name = "btnExecuteSmart";
            btnExecuteSmart.Size = new Size(285, 45);
            btnExecuteSmart.TabIndex = 2;
            btnExecuteSmart.Text = "✅ 一键下发合并任务";
            btnExecuteSmart.UseVisualStyleBackColor = true;
            btnExecuteSmart.Click += btnExecuteSmart_Click;
            // 
            // dgvAdvice
            // 
            dgvAdvice.AllowUserToAddRows = false;
            dgvAdvice.AllowUserToDeleteRows = false;
            dgvAdvice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAdvice.Location = new Point(20, 85);
            dgvAdvice.Name = "dgvAdvice";
            dgvAdvice.ReadOnly = true;
            dgvAdvice.RowHeadersWidth = 51;
            dgvAdvice.RowTemplate.Height = 35;
            dgvAdvice.Size = new Size(1297, 250);
            dgvAdvice.TabIndex = 1;
            // 
            // btnScan
            // 
            btnScan.Location = new Point(20, 380);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(279, 36);
            btnScan.TabIndex = 0;
            btnScan.Text = "🔍 智能扫描全仓碎片";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // MoveStockForm
            // 
            ClientSize = new Size(1414, 818);
            Controls.Add(gbSmart);
            Controls.Add(gbManual);
            Name = "MoveStockForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "库位移库与碎片整理";
            gbManual.ResumeLayout(false);
            gbManual.PerformLayout();
            gbSmart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAdvice).EndInit();
            ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFromLoc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtToLoc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnConfirm;

        private System.Windows.Forms.GroupBox gbSmart;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView dgvAdvice;
        private System.Windows.Forms.Button btnExecuteSmart;
    }
}