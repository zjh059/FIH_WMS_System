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
            this.uiBarChart1 = new Sunny.UI.UIBarChart();
            this.uiPieChart1 = new Sunny.UI.UIPieChart(); // 👈 增加饼图控件
            this.SuspendLayout();

            // 
            // uiBarChart1 (柱状图，放在左边)
            // 
            this.uiBarChart1.Dock = System.Windows.Forms.DockStyle.Left; // 👈 关键：停靠在左边，不再是 Fill！
            this.uiBarChart1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiBarChart1.Location = new System.Drawing.Point(0, 0);
            this.uiBarChart1.Name = "uiBarChart1";
            this.uiBarChart1.Size = new System.Drawing.Size(600, 840); // 👈 宽度设为 600，刚好占左边一半
            this.uiBarChart1.TabIndex = 0;
            this.uiBarChart1.Text = "uiBarChart1";

            // 
            // uiPieChart1 (饼状图，占满剩下的右边)
            // 
            this.uiPieChart1.Dock = System.Windows.Forms.DockStyle.Fill; // 👈 关键：左边被占了600，剩下的全归饼图！
            this.uiPieChart1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPieChart1.Location = new System.Drawing.Point(600, 0);
            this.uiPieChart1.Name = "uiPieChart1";
            this.uiPieChart1.Size = new System.Drawing.Size(668, 840);
            this.uiPieChart1.TabIndex = 1;
            this.uiPieChart1.Text = "uiPieChart1";

            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1268, 840);
            this.Controls.Add(this.uiPieChart1); // 👈 把饼图加进窗口
            this.Controls.Add(this.uiBarChart1);
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据分析大屏";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.ResumeLayout(false);
        }

        private Sunny.UI.UIBarChart uiBarChart1;
        private Sunny.UI.UIPieChart uiPieChart1; // 👈 声明饼图变量
    }
}