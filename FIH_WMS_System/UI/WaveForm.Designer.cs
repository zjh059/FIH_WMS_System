namespace FIH_WMS_System.UI
{
    partial class WaveForm
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
            label1 = new Label();
            txtPCode = new TextBox();
            label2 = new Label();
            txtQty = new TextBox();
            btnAdd = new Button();
            listOrders = new ListBox();
            btnAnalyze = new Button();
            btnExecute = new Button();
            dgvWave = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvWave).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 25);
            label1.Name = "label1";
            label1.Size = new Size(116, 31);
            label1.TabIndex = 0;
            label1.Text = "成品工单:";
            // 
            // txtPCode
            // 
            txtPCode.Location = new Point(156, 22);
            txtPCode.Name = "txtPCode";
            txtPCode.Size = new Size(530, 39);
            txtPCode.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 96);
            label2.Name = "label2";
            label2.Size = new Size(116, 31);
            label2.TabIndex = 2;
            label2.Text = "生产数量:";
            // 
            // txtQty
            // 
            txtQty.Location = new Point(156, 93);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(530, 39);
            txtQty.TabIndex = 3;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.LightBlue;
            btnAdd.Location = new Point(727, 25);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(188, 107);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "➕ 录入本波次";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // listOrders
            // 
            listOrders.FormattingEnabled = true;
            listOrders.ItemHeight = 31;
            listOrders.Location = new Point(24, 194);
            listOrders.Name = "listOrders";
            listOrders.Size = new Size(891, 66);
            listOrders.TabIndex = 5;
            // 
            // btnAnalyze
            // 
            btnAnalyze.BackColor = Color.Orange;
            btnAnalyze.Location = new Point(24, 288);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(237, 98);
            btnAnalyze.TabIndex = 6;
            btnAnalyze.Text = "⚙️ 智能合并波次物料 (BOM)";
            btnAnalyze.UseVisualStyleBackColor = false;
            btnAnalyze.Click += BtnAnalyze_Click;
            // 
            // btnExecute
            // 
            btnExecute.BackColor = Color.MediumSeaGreen;
            btnExecute.ForeColor = Color.White;
            btnExecute.Location = new Point(678, 288);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(237, 98);
            btnExecute.TabIndex = 7;
            btnExecute.Text = "🚀 一键下发波次拣货任务 (呼叫AGV)";
            btnExecute.UseVisualStyleBackColor = false;
            btnExecute.Click += BtnExecute_Click;
            // 
            // dgvWave
            // 
            dgvWave.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvWave.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWave.Location = new Point(24, 419);
            dgvWave.Name = "dgvWave";
            dgvWave.RowHeadersWidth = 51;
            dgvWave.RowTemplate.Height = 27;
            dgvWave.Size = new Size(891, 368);
            dgvWave.TabIndex = 8;
            // 
            // WaveForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(937, 800);
            Controls.Add(dgvWave);
            Controls.Add(btnExecute);
            Controls.Add(btnAnalyze);
            Controls.Add(listOrders);
            Controls.Add(btnAdd);
            Controls.Add(txtQty);
            Controls.Add(label2);
            Controls.Add(txtPCode);
            Controls.Add(label1);
            Font = new Font("微软雅黑", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = new Padding(4, 5, 4, 5);
            Name = "WaveForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🌊 波次出库管理中心 (Wave Consolidation)";
            ((System.ComponentModel.ISupportInitialize)dgvWave).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox listOrders;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dgvWave;
    }
}