namespace FIH_WMS_System.UI
{
    partial class MapForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            panelTop = new Panel();
            btnAddSingle = new Button();
            btnBatchAdd = new Button();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 84);
            flowLayoutPanel1.Margin = new Padding(4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(29, 28, 29, 28);
            flowLayoutPanel1.Size = new Size(1271, 756);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnAddSingle);
            panelTop.Controls.Add(btnBatchAdd);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(4);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1271, 84);
            panelTop.TabIndex = 1;
            // 
            // btnAddSingle
            // 
            btnAddSingle.BackColor = Color.MediumSeaGreen;
            btnAddSingle.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnAddSingle.ForeColor = Color.White;
            btnAddSingle.Location = new Point(38, 13);
            btnAddSingle.Margin = new Padding(4);
            btnAddSingle.Name = "btnAddSingle";
            btnAddSingle.Size = new Size(217, 56);
            btnAddSingle.TabIndex = 0;
            btnAddSingle.Text = "➕ 新增单库位";
            btnAddSingle.UseVisualStyleBackColor = false;
            btnAddSingle.Click += btnAddSingle_Click;
            // 
            // btnBatchAdd
            // 
            btnBatchAdd.BackColor = Color.SteelBlue;
            btnBatchAdd.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnBatchAdd.ForeColor = Color.White;
            btnBatchAdd.Location = new Point(1041, 13);
            btnBatchAdd.Margin = new Padding(4);
            btnBatchAdd.Name = "btnBatchAdd";
            btnBatchAdd.Size = new Size(217, 56);
            btnBatchAdd.TabIndex = 1;
            btnBatchAdd.Text = "🚀 批量扩建库位";
            btnBatchAdd.UseVisualStyleBackColor = false;
            btnBatchAdd.Click += btnBatchAdd_Click;
            // 
            // MapForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1271, 840);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panelTop);
            Margin = new Padding(4);
            Name = "MapForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "2D 仓储库位可视化监控屏";
            Load += MapForm_Load;
            panelTop.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnAddSingle;
        private System.Windows.Forms.Button btnBatchAdd;

    }
}   