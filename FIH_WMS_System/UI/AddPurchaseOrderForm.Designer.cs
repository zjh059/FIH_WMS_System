namespace FIH_WMS_System.UI
{
    partial class AddPurchaseOrderForm
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
            lblGoods = new Label();
            cmbGoods = new ComboBox();
            lblQty = new Label();
            numQty = new NumericUpDown();
            btnAdd = new Button();
            dgvDetails = new DataGridView();
            btnSubmit = new Button();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();
            // 
            // lblGoods
            // 
            lblGoods.AutoSize = true;
            lblGoods.Location = new Point(32, 30);
            lblGoods.Name = "lblGoods";
            lblGoods.Size = new Size(164, 31);
            lblGoods.TabIndex = 0;
            lblGoods.Text = "选择采购物料:";
            // 
            // cmbGoods
            // 
            cmbGoods.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGoods.FormattingEnabled = true;
            cmbGoods.Location = new Point(257, 27);
            cmbGoods.Name = "cmbGoods";
            cmbGoods.Size = new Size(734, 38);
            cmbGoods.TabIndex = 1;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Location = new Point(68, 99);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(116, 31);
            lblQty.TabIndex = 2;
            lblQty.Text = "采购数量:";
            // 
            // numQty
            // 
            numQty.Location = new Point(257, 97);
            numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(734, 38);
            numQty.TabIndex = 3;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.SteelBlue;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(32, 740);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(219, 50);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "➕ 加入清单";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // dgvDetails
            // 
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BackgroundColor = Color.WhiteSmoke;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Location = new Point(32, 179);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersWidth = 51;
            dgvDetails.RowTemplate.Height = 29;
            dgvDetails.Size = new Size(959, 510);
            dgvDetails.TabIndex = 5;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.MediumSeaGreen;
            btnSubmit.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(772, 739);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(219, 50);
            btnSubmit.TabIndex = 6;
            btnSubmit.Text = "✅ 确认生成采购单";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += BtnSubmit_Click;
            // 
            // AddPurchaseOrderForm
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1021, 822);
            Controls.Add(btnSubmit);
            Controls.Add(dgvDetails);
            Controls.Add(btnAdd);
            Controls.Add(numQty);
            Controls.Add(lblQty);
            Controls.Add(cmbGoods);
            Controls.Add(lblGoods);
            Font = new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Name = "AddPurchaseOrderForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📝 手工录入采购工单";
            Load += AddPurchaseOrderForm_Load;
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGoods;
        private System.Windows.Forms.ComboBox cmbGoods;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Button btnSubmit;
    }
}