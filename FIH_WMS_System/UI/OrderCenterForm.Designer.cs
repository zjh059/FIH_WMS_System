namespace FIH_WMS_System.UI
{
    partial class OrderCenterForm
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
            lblOrders = new Label();
            dgvOrders = new DataGridView();
            lblDetails = new Label();
            dgvDetails = new DataGridView();
            btnAddPurchaseOrder = new Button();
            btnWaveConsolidate = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();
            // 
            // lblOrders
            // 
            lblOrders.AutoSize = true;
            lblOrders.Font = new Font("微软雅黑", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblOrders.Location = new Point(29, 28);
            lblOrders.Margin = new Padding(4, 0, 4, 0);
            lblOrders.Name = "lblOrders";
            lblOrders.Size = new Size(380, 34);
            lblOrders.TabIndex = 0;
            lblOrders.Text = "📌 主单据列表 (WmsOrder)：";
            // 
            // dgvOrders
            // 
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.BackgroundColor = Color.WhiteSmoke;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(29, 89);
            dgvOrders.Margin = new Padding(4);
            dgvOrders.MultiSelect = false;
            dgvOrders.Name = "dgvOrders";
            dgvOrders.ReadOnly = true;
            dgvOrders.RowHeadersWidth = 51;
            dgvOrders.RowTemplate.Height = 29;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.Size = new Size(1358, 493);
            dgvOrders.TabIndex = 1;
            dgvOrders.SelectionChanged += dgvOrders_SelectionChanged;
            // 
            // lblDetails
            // 
            lblDetails.AutoSize = true;
            lblDetails.Font = new Font("微软雅黑", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblDetails.Location = new Point(29, 623);
            lblDetails.Margin = new Padding(4, 0, 4, 0);
            lblDetails.Name = "lblDetails";
            lblDetails.Size = new Size(562, 34);
            lblDetails.TabIndex = 2;
            lblDetails.Text = "📦 单据包含的物料明细 (WmsOrderDetail)：";
            // 
            // dgvDetails
            // 
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BackgroundColor = Color.WhiteSmoke;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Location = new Point(29, 687);
            dgvDetails.Margin = new Padding(4);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersWidth = 51;
            dgvDetails.RowTemplate.Height = 29;
            dgvDetails.Size = new Size(1358, 467);
            dgvDetails.TabIndex = 3;
            // 
            // btnAddPurchaseOrder
            // 
            btnAddPurchaseOrder.BackColor = Color.Orange;
            btnAddPurchaseOrder.Font = new Font("微软雅黑", 9.857143F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnAddPurchaseOrder.ForeColor = Color.White;
            btnAddPurchaseOrder.Location = new Point(1156, 26);
            btnAddPurchaseOrder.Name = "btnAddPurchaseOrder";
            btnAddPurchaseOrder.Size = new Size(231, 40);
            btnAddPurchaseOrder.TabIndex = 4;
            btnAddPurchaseOrder.Text = "➕ 手工录入采购单";
            btnAddPurchaseOrder.UseVisualStyleBackColor = false;
            btnAddPurchaseOrder.Click += btnAddPurchaseOrder_Click;
            // 
            // btnWaveConsolidate
            // 
            btnWaveConsolidate.BackColor = Color.LightSeaGreen;
            btnWaveConsolidate.Font = new Font("微软雅黑", 9.857143F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnWaveConsolidate.ForeColor = Color.White;
            btnWaveConsolidate.Location = new Point(901, 26);
            btnWaveConsolidate.Name = "btnWaveConsolidate";
            btnWaveConsolidate.Size = new Size(231, 40);
            btnWaveConsolidate.TabIndex = 5;
            btnWaveConsolidate.Text = "🌊 入库波次合并";
            btnWaveConsolidate.UseVisualStyleBackColor = false;
            btnWaveConsolidate.Click += btnWaveConsolidate_Click;
            // 
            // OrderCenterForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1444, 1184);
            Controls.Add(btnWaveConsolidate);
            Controls.Add(btnAddPurchaseOrder);
            Controls.Add(dgvDetails);
            Controls.Add(lblDetails);
            Controls.Add(dgvOrders);
            Controls.Add(lblOrders);
            Margin = new Padding(4);
            Name = "OrderCenterForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📑 宏观单据流转中心 (ERP 订单视图)";
            Load += OrderCenterForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.DataGridView dgvDetails;
        private Button btnAddPurchaseOrder;
        //private Button button1;
        private Button btnWaveConsolidate;

        //private System.Windows.Forms.Button btnAddPurchaseOrder;
    }
}