namespace FIH_WMS_System.UI
{
    partial class BaseDataForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnImportExcel = new Button();
            btnAdd = new Button();
            txtCategory = new TextBox();
            label4 = new Label();
            txtSpec = new TextBox();
            label3 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtCode = new TextBox();
            label1 = new Label();
            dgvGoods = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGoods).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnImportExcel);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(txtCategory);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtSpec);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtCode);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("微软雅黑", 10F);
            groupBox1.Location = new Point(20, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1213, 190);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "✍️ 手动建档 / Excel批量导入";
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.FromArgb(30, 150, 80);
            btnImportExcel.ForeColor = Color.White;
            btnImportExcel.Location = new Point(926, 114);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(262, 40);
            btnImportExcel.TabIndex = 9;
            btnImportExcel.Text = "📊 从 Excel 批量导入";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(40, 120, 200);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(926, 53);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(262, 40);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "➕ 确认新增物料";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtCategory
            // 
            txtCategory.Location = new Point(567, 116);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(278, 38);
            txtCategory.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(445, 119);
            label4.Name = "label4";
            label4.Size = new Size(96, 31);
            label4.TabIndex = 6;
            label4.Text = "  分类  :";
            // 
            // txtSpec
            // 
            txtSpec.Location = new Point(142, 116);
            txtSpec.Name = "txtSpec";
            txtSpec.Size = new Size(278, 38);
            txtSpec.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 119);
            label3.Name = "label3";
            label3.Size = new Size(96, 31);
            label3.TabIndex = 4;
            label3.Text = "  规格  :";
            // 
            // txtName
            // 
            txtName.Location = new Point(567, 55);
            txtName.Name = "txtName";
            txtName.Size = new Size(278, 38);
            txtName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(445, 58);
            label2.Name = "label2";
            label2.Size = new Size(116, 31);
            label2.TabIndex = 2;
            label2.Text = "物料名称:";
            // 
            // txtCode
            // 
            txtCode.Location = new Point(142, 55);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(278, 38);
            txtCode.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 58);
            label1.Name = "label1";
            label1.Size = new Size(116, 31);
            label1.TabIndex = 0;
            label1.Text = "物料编码:";
            // 
            // dgvGoods
            // 
            dgvGoods.AllowUserToAddRows = false;
            dgvGoods.AllowUserToDeleteRows = false;
            dgvGoods.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGoods.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGoods.Location = new Point(20, 228);
            dgvGoods.Name = "dgvGoods";
            dgvGoods.ReadOnly = true;
            dgvGoods.RowHeadersWidth = 51;
            dgvGoods.RowTemplate.Height = 35;
            dgvGoods.Size = new Size(1213, 442);
            dgvGoods.TabIndex = 1;
            // 
            // BaseDataForm
            // 
            ClientSize = new Size(1262, 682);
            Controls.Add(dgvGoods);
            Controls.Add(groupBox1);
            Name = "BaseDataForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "⚙️ 基础物料档案管理";
            Load += BaseDataForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGoods).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.DataGridView dgvGoods;
    }
}