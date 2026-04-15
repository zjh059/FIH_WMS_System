namespace FIH_WMS_System.UI
{
    partial class AgvMonitorForm
    {
        private System.Windows.Forms.ContextMenuStrip menuAgv;
        private System.Windows.Forms.ToolStripMenuItem menuViewTrace;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            //初始化 components 容器！
            this.components = new System.ComponentModel.Container();

            //要让用户在 AgvMonitorForm（AGV 监控台）里，右键点击某个任务，就能直接跳出对应的日志。
            //实例化并绑定
            this.menuAgv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuViewTrace = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAgv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.menuViewTrace });
            this.menuViewTrace.Text = "🔍 查看该任务运行轨迹";
            this.menuViewTrace.Click += new System.EventHandler(this.menuViewTrace_Click);

            // 将菜单绑定到表格
            //this.dgvTasks.ContextMenuStrip = this.menuAgv;
            //转到业务层去了



            components = new System.ComponentModel.Container();
            dgvTasks = new DataGridView();
            btnRefresh = new Button();
            btnComplete = new Button();
            labelTitle = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            SuspendLayout();
            // 
            // dgvTasks
            // 
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTasks.Location = new Point(25, 75);
            dgvTasks.Name = "dgvTasks";
            dgvTasks.ReadOnly = true;
            dgvTasks.RowHeadersWidth = 51;
            dgvTasks.RowTemplate.Height = 35;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.Size = new Size(1275, 722);
            dgvTasks.TabIndex = 3;
            dgvTasks.CellFormatting += dgvTasks_CellFormatting;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Font = new Font("微软雅黑", 10F);
            btnRefresh.Location = new Point(904, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(153, 36);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "🔄 刷新状态";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnComplete
            // 
            btnComplete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnComplete.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            btnComplete.ForeColor = Color.DarkGreen;
            btnComplete.Location = new Point(1072, 20);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new Size(228, 36);
            btnComplete.TabIndex = 2;
            btnComplete.Text = "✅ 模拟小车送达";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(40, 80, 150);
            labelTitle.Location = new Point(20, 20);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(502, 44);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "🤖 WCS - AGV 小车调度控制台";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 3000;
            timer1.Tick += timer1_Tick;
            // 
            // AgvMonitorForm
            // 
            ClientSize = new Size(1320, 822);
            Controls.Add(dgvTasks);
            Controls.Add(btnComplete);
            Controls.Add(btnRefresh);
            Controls.Add(labelTitle);
            Name = "AgvMonitorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "AGV 小车调度与监控";
            Load += AgvMonitorForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Timer timer1;
    }
}