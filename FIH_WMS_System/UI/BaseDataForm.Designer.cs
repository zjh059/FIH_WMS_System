namespace FIH_WMS_System.UI
{
    partial class BaseDataForm
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
            txtCode = new TextBox();
            label2 = new Label();
            txtName = new TextBox();
            label3 = new Label();
            txtBrand = new TextBox();
            label4 = new Label();
            txtCategory = new TextBox();
            label5 = new Label();
            txtSpec = new TextBox();
            btnAdd = new Button();
            dgvData = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 35);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(101, 28);
            label1.TabIndex = 11;
            label1.Text = "物料编码:";
            // 
            // txtCode
            // 
            txtCode.Location = new Point(130, 32);
            txtCode.Margin = new Padding(4, 4, 4, 4);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(259, 34);
            txtCode.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(403, 35);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(101, 28);
            label2.TabIndex = 9;
            label2.Text = "物料名称:";
            // 
            // txtName
            // 
            txtName.Location = new Point(504, 32);
            txtName.Margin = new Padding(4, 4, 4, 4);
            txtName.Name = "txtName";
            txtName.Size = new Size(259, 34);
            txtName.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Blue;
            label3.Location = new Point(780, 35);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(101, 28);
            label3.TabIndex = 7;
            label3.Text = "所属品牌:";
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(907, 32);
            txtBrand.Margin = new Padding(4, 4, 4, 4);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(191, 34);
            txtBrand.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(29, 105);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(101, 28);
            label4.TabIndex = 5;
            label4.Text = "物料分类:";
            // 
            // txtCategory
            // 
            txtCategory.Location = new Point(130, 102);
            txtCategory.Margin = new Padding(4, 4, 4, 4);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(259, 34);
            txtCategory.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(403, 105);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(101, 28);
            label5.TabIndex = 3;
            label5.Text = "规格型号:";
            // 
            // txtSpec
            // 
            txtSpec.Location = new Point(504, 102);
            txtSpec.Margin = new Padding(4, 4, 4, 4);
            txtSpec.Name = "txtSpec";
            txtSpec.Size = new Size(259, 34);
            txtSpec.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.MediumSeaGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(907, 92);
            btnAdd.Margin = new Padding(4, 4, 4, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(192, 49);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ 增加物料档案";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // dgvData
            // 
            dgvData.AllowUserToAddRows = false;
            dgvData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.BackgroundColor = Color.WhiteSmoke;
            dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData.Location = new Point(29, 168);
            dgvData.Margin = new Padding(4, 4, 4, 4);
            dgvData.Name = "dgvData";
            dgvData.ReadOnly = true;
            dgvData.RowHeadersWidth = 51;
            dgvData.Size = new Size(1069, 420);
            dgvData.TabIndex = 0;
            // 
            // BaseDataForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1127, 616);
            Controls.Add(dgvData);
            Controls.Add(btnAdd);
            Controls.Add(txtSpec);
            Controls.Add(label5);
            Controls.Add(txtCategory);
            Controls.Add(label4);
            Controls.Add(txtBrand);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(txtCode);
            Controls.Add(label1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "BaseDataForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "⚙️ 基础物料档案管理";
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvData;
    }
}