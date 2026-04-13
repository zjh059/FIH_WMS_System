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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvData = new DataGridView();
            btnAdd = new Button();
            txtSpec = new TextBox();
            label5 = new Label();
            txtCategory = new TextBox();
            label4 = new Label();
            txtBrand = new TextBox();
            label3 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtCode = new TextBox();
            label1 = new Label();
            tabPage2 = new TabPage();
            label9 = new Label();
            dgvBom = new DataGridView();
            gbBomAdd = new GroupBox();
            btnBomAdd = new Button();
            numBomQty = new NumericUpDown();
            label8 = new Label();
            txtBomChild = new TextBox();
            label7 = new Label();
            txtBomParent = new TextBox();
            label6 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBom).BeginInit();
            gbBomAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBomQty).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("微软雅黑", 10.5F);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1434, 1061);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvData);
            tabPage1.Controls.Add(btnAdd);
            tabPage1.Controls.Add(txtSpec);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(txtCategory);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(txtBrand);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(txtName);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(txtCode);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 40);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1426, 1017);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "📦 物料基础档案";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            dgvData.ColumnHeadersHeight = 40;
            dgvData.Location = new Point(29, 423);
            dgvData.Name = "dgvData";
            dgvData.RowHeadersWidth = 72;
            dgvData.Size = new Size(1369, 563);
            dgvData.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.Location = new Point(1184, 35);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(214, 327);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "➕ 增加物料档案";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtSpec
            // 
            txtSpec.Location = new Point(200, 253);
            txtSpec.Name = "txtSpec";
            txtSpec.Size = new Size(927, 40);
            txtSpec.TabIndex = 2;
            // 
            // label5
            // 
            label5.Location = new Point(29, 256);
            label5.Name = "label5";
            label5.Size = new Size(125, 35);
            label5.TabIndex = 3;
            label5.Text = "规格型号:";
            // 
            // txtCategory
            // 
            txtCategory.Location = new Point(200, 179);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(927, 40);
            txtCategory.TabIndex = 4;
            // 
            // label4
            // 
            label4.Location = new Point(29, 182);
            label4.Name = "label4";
            label4.Size = new Size(125, 35);
            label4.TabIndex = 5;
            label4.Text = "物料分类:";
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(200, 322);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(927, 40);
            txtBrand.TabIndex = 6;
            // 
            // label3
            // 
            label3.Location = new Point(29, 325);
            label3.Name = "label3";
            label3.Size = new Size(125, 35);
            label3.TabIndex = 7;
            label3.Text = "所属品牌:";
            // 
            // txtName
            // 
            txtName.Location = new Point(200, 102);
            txtName.Name = "txtName";
            txtName.Size = new Size(927, 40);
            txtName.TabIndex = 8;
            // 
            // label2
            // 
            label2.Location = new Point(29, 105);
            label2.Name = "label2";
            label2.Size = new Size(125, 35);
            label2.TabIndex = 9;
            label2.Text = "物料名称:";
            // 
            // txtCode
            // 
            txtCode.Location = new Point(200, 32);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(927, 40);
            txtCode.TabIndex = 10;
            // 
            // label1
            // 
            label1.Location = new Point(29, 35);
            label1.Name = "label1";
            label1.Size = new Size(125, 35);
            label1.TabIndex = 11;
            label1.Text = "物料编码:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label9);
            tabPage2.Controls.Add(dgvBom);
            tabPage2.Controls.Add(gbBomAdd);
            tabPage2.Location = new Point(4, 40);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1426, 1017);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "⚙️ 产品 BOM 架构配置";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Gray;
            label9.Location = new Point(20, 338);
            label9.Name = "label9";
            label9.Size = new Size(747, 32);
            label9.TabIndex = 0;
            label9.Text = "💡 提示：右键点击表格中的某行可以解除绑定删除该 BOM 节点。";
            // 
            // dgvBom
            // 
            dgvBom.AllowUserToAddRows = false;
            dgvBom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBom.ColumnHeadersHeight = 40;
            dgvBom.Location = new Point(20, 373);
            dgvBom.Name = "dgvBom";
            dgvBom.RowHeadersWidth = 72;
            dgvBom.Size = new Size(1381, 574);
            dgvBom.TabIndex = 1;
            // 
            // gbBomAdd
            // 
            gbBomAdd.Controls.Add(btnBomAdd);
            gbBomAdd.Controls.Add(numBomQty);
            gbBomAdd.Controls.Add(label8);
            gbBomAdd.Controls.Add(txtBomChild);
            gbBomAdd.Controls.Add(label7);
            gbBomAdd.Controls.Add(txtBomParent);
            gbBomAdd.Controls.Add(label6);
            gbBomAdd.Location = new Point(20, 20);
            gbBomAdd.Name = "gbBomAdd";
            gbBomAdd.Size = new Size(1381, 296);
            gbBomAdd.TabIndex = 2;
            gbBomAdd.TabStop = false;
            gbBomAdd.Text = "添加 BOM 子件关系";
            // 
            // btnBomAdd
            // 
            btnBomAdd.BackColor = Color.SteelBlue;
            btnBomAdd.ForeColor = Color.White;
            btnBomAdd.Location = new Point(1170, 39);
            btnBomAdd.Name = "btnBomAdd";
            btnBomAdd.Size = new Size(191, 169);
            btnBomAdd.TabIndex = 0;
            btnBomAdd.Text = "🔗 绑定 BOM 关系";
            btnBomAdd.UseVisualStyleBackColor = false;
            btnBomAdd.Click += btnBomAdd_Click;
            // 
            // numBomQty
            // 
            numBomQty.DecimalPlaces = 4;
            numBomQty.Location = new Point(241, 168);
            numBomQty.Name = "numBomQty";
            numBomQty.Size = new Size(749, 40);
            numBomQty.TabIndex = 1;
            // 
            // label8
            // 
            label8.Location = new Point(20, 170);
            label8.Name = "label8";
            label8.Size = new Size(215, 36);
            label8.TabIndex = 2;
            label8.Text = "单机用量:";
            // 
            // txtBomChild
            // 
            txtBomChild.Location = new Point(241, 100);
            txtBomChild.Name = "txtBomChild";
            txtBomChild.Size = new Size(749, 40);
            txtBomChild.TabIndex = 3;
            // 
            // label7
            // 
            label7.Location = new Point(20, 103);
            label7.Name = "label7";
            label7.Size = new Size(215, 36);
            label7.TabIndex = 4;
            label7.Text = "子件(原材料)编码:";
            // 
            // txtBomParent
            // 
            txtBomParent.Location = new Point(241, 39);
            txtBomParent.Name = "txtBomParent";
            txtBomParent.Size = new Size(749, 40);
            txtBomParent.TabIndex = 5;
            // 
            // label6
            // 
            label6.Location = new Point(20, 43);
            label6.Name = "label6";
            label6.Size = new Size(215, 36);
            label6.TabIndex = 6;
            label6.Text = "父成品编码:";
            // 
            // BaseDataForm
            // 
            ClientSize = new Size(1434, 1061);
            Controls.Add(tabControl1);
            Name = "BaseDataForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "⚙️ 基础物料与 BOM 档案管理";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBom).EndInit();
            gbBomAdd.ResumeLayout(false);
            gbBomAdd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBomQty).EndInit();
            ResumeLayout(false);
        }


        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label1;
        // BOM 相关控件
        private System.Windows.Forms.GroupBox gbBomAdd;
        private System.Windows.Forms.Button btnBomAdd;
        private System.Windows.Forms.NumericUpDown numBomQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBomChild;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBomParent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvBom;
        private System.Windows.Forms.Label label9;
    }
}