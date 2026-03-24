namespace FIH_WMS_System.UI
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uiBarChart1 = new Sunny.UI.UIBarChart();
            this.uiPieChart1 = new Sunny.UI.UIPieChart();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1 (核心：自适应网格)
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F)); // 左边永远50%
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F)); // 右边永远50%
            this.tableLayoutPanel1.Controls.Add(this.uiBarChart1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.uiPieChart1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1268, 840);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uiBarChart1
            // 
            this.uiBarChart1.Dock = System.Windows.Forms.DockStyle.Fill; // 填满左边的格子
            this.uiBarChart1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiBarChart1.Location = new System.Drawing.Point(10, 10); // 加一点外边距，防止贴边
            this.uiBarChart1.Name = "uiBarChart1";
            this.uiBarChart1.Size = new System.Drawing.Size(614, 820);
            this.uiBarChart1.TabIndex = 0;
            this.uiBarChart1.Text = "uiBarChart1";
            // 
            // uiPieChart1
            // 
            this.uiPieChart1.Dock = System.Windows.Forms.DockStyle.Fill; // 填满右边的格子
            this.uiPieChart1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPieChart1.Location = new System.Drawing.Point(644, 10);
            this.uiPieChart1.Name = "uiPieChart1";
            this.uiPieChart1.Size = new System.Drawing.Size(614, 820);
            this.uiPieChart1.TabIndex = 1;
            this.uiPieChart1.Text = "uiPieChart1";
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1268, 840);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据分析大屏";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sunny.UI.UIBarChart uiBarChart1;
        private Sunny.UI.UIPieChart uiPieChart1;
    }
}