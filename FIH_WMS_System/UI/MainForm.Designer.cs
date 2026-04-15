namespace FIH_WMS_System
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            uiButton1 = new Sunny.UI.UIButton();
            uiButton2 = new Sunny.UI.UIButton();
            uiButton3 = new Sunny.UI.UIButton();
            uiButton4 = new Sunny.UI.UIButton();
            dataGridView1 = new Sunny.UI.UIDataGridView();
            uiButton5 = new Sunny.UI.UIButton();
            uiButton6 = new Sunny.UI.UIButton();
            uiButton7 = new Sunny.UI.UIButton();
            btnSimulateAgv = new Sunny.UI.UIButton();
            uiButton8 = new Sunny.UI.UIButton();
            uiButton9 = new Sunny.UI.UIButton();
            btnLogout = new Sunny.UI.UIButton();
            btnExportExcel = new Sunny.UI.UIButton();
            btnAgvMonitor = new Sunny.UI.UIButton();
            btnBaseData = new Sunny.UI.UIButton();
            btnReturnStock = new Sunny.UI.UIButton();
            btnWaveOut = new Sunny.UI.UIButton();
            btnOrderCenter = new Sunny.UI.UIButton();
            btnUserManage = new Sunny.UI.UIButton();
            btnSysVoiceSetup = new Sunny.UI.UIButton();
            btnSysLog = new Sunny.UI.UIButton();
            btnSetupLangs = new Sunny.UI.UIButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.images;
            pictureBox1.Location = new Point(3, 35);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(298, 143);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微软雅黑", 24F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(754, 85);
            label1.Name = "label1";
            label1.Size = new Size(593, 75);
            label1.TabIndex = 1;
            label1.Text = "FIH 智能仓储管理系统";
            // 
            // uiButton1
            // 
            uiButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton1.Location = new Point(53, 243);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Size = new Size(248, 61);
            uiButton1.TabIndex = 7;
            uiButton1.Text = "📥 物料入库";
            uiButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton1.Click += uiButton1_Click;
            // 
            // uiButton2
            // 
            uiButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton2.Location = new Point(53, 320);
            uiButton2.MinimumSize = new Size(1, 1);
            uiButton2.Name = "uiButton2";
            uiButton2.Size = new Size(248, 61);
            uiButton2.TabIndex = 8;
            uiButton2.Text = "📤 物料出库";
            uiButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton2.Click += uiButton2_Click;
            // 
            // uiButton3
            // 
            uiButton3.FillColor = Color.FromArgb(0, 0, 64);
            uiButton3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton3.Location = new Point(53, 398);
            uiButton3.MinimumSize = new Size(1, 1);
            uiButton3.Name = "uiButton3";
            uiButton3.Size = new Size(248, 61);
            uiButton3.TabIndex = 9;
            uiButton3.Text = "📊 库存查询";
            uiButton3.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton3.Click += uiButton3_Click;
            // 
            // uiButton4
            // 
            uiButton4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton4.Location = new Point(53, 474);
            uiButton4.MinimumSize = new Size(1, 1);
            uiButton4.Name = "uiButton4";
            uiButton4.Size = new Size(248, 61);
            uiButton4.TabIndex = 10;
            uiButton4.Text = "📝 流水记录";
            uiButton4.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton4.Click += uiButton4_Click;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridView1.GridColor = Color.FromArgb(80, 160, 255);
            dataGridView1.Location = new Point(397, 243);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.RowHeadersWidth = 72;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.SelectedIndex = -1;
            dataGridView1.Size = new Size(1476, 1056);
            dataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            dataGridView1.TabIndex = 11;
            // 
            // uiButton5
            // 
            uiButton5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton5.Location = new Point(53, 551);
            uiButton5.MinimumSize = new Size(1, 1);
            uiButton5.Name = "uiButton5";
            uiButton5.Size = new Size(248, 61);
            uiButton5.TabIndex = 12;
            uiButton5.Text = "🔄 库位移库";
            uiButton5.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton5.Click += uiButton5_Click;
            // 
            // uiButton6
            // 
            uiButton6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton6.Location = new Point(53, 627);
            uiButton6.MinimumSize = new Size(1, 1);
            uiButton6.Name = "uiButton6";
            uiButton6.Size = new Size(248, 61);
            uiButton6.TabIndex = 13;
            uiButton6.Text = "📋 库存盘点";
            uiButton6.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton6.Click += uiButton6_Click;
            // 
            // uiButton7
            // 
            uiButton7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton7.Location = new Point(53, 702);
            uiButton7.MinimumSize = new Size(1, 1);
            uiButton7.Name = "uiButton7";
            uiButton7.Size = new Size(248, 61);
            uiButton7.TabIndex = 14;
            uiButton7.Text = "🚗AGV调度中心";
            uiButton7.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton7.Click += uiButton7_Click;
            // 
            // btnSimulateAgv
            // 
            btnSimulateAgv.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSimulateAgv.Location = new Point(665, 1320);
            btnSimulateAgv.MinimumSize = new Size(1, 1);
            btnSimulateAgv.Name = "btnSimulateAgv";
            btnSimulateAgv.Size = new Size(424, 61);
            btnSimulateAgv.TabIndex = 15;
            btnSimulateAgv.Text = "✅ 模拟小车到达 (AVG完成任务)";
            btnSimulateAgv.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSimulateAgv.Click += btnSimulateAgv_Click;
            // 
            // uiButton8
            // 
            uiButton8.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton8.Location = new Point(53, 778);
            uiButton8.MinimumSize = new Size(1, 1);
            uiButton8.Name = "uiButton8";
            uiButton8.Size = new Size(248, 61);
            uiButton8.TabIndex = 16;
            uiButton8.Text = "📈数据大屏看板";
            uiButton8.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton8.Click += uiButton8_Click;
            // 
            // uiButton9
            // 
            uiButton9.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton9.Location = new Point(53, 855);
            uiButton9.MinimumSize = new Size(1, 1);
            uiButton9.Name = "uiButton9";
            uiButton9.Size = new Size(248, 61);
            uiButton9.TabIndex = 17;
            uiButton9.Text = "🗺️ 2D库位监控";
            uiButton9.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton9.Click += uiButton9_Click;
            // 
            // btnLogout
            // 
            btnLogout.FillColor = Color.Red;
            btnLogout.FillColor2 = Color.Silver;
            btnLogout.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnLogout.Location = new Point(1711, 99);
            btnLogout.MinimumSize = new Size(1, 1);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(162, 61);
            btnLogout.TabIndex = 18;
            btnLogout.Text = "🔙 系统登出";
            btnLogout.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnLogout.Click += btnLogout_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.FillColor = Color.DarkGreen;
            btnExportExcel.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnExportExcel.Location = new Point(1538, 1320);
            btnExportExcel.MinimumSize = new Size(1, 1);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(335, 61);
            btnExportExcel.TabIndex = 19;
            btnExportExcel.Text = "💾 导出当前数据至 Excel";
            btnExportExcel.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnAgvMonitor
            // 
            btnAgvMonitor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAgvMonitor.Location = new Point(397, 1320);
            btnAgvMonitor.MinimumSize = new Size(1, 1);
            btnAgvMonitor.Name = "btnAgvMonitor";
            btnAgvMonitor.Size = new Size(246, 61);
            btnAgvMonitor.TabIndex = 20;
            btnAgvMonitor.Text = "AGV 监控台";
            btnAgvMonitor.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAgvMonitor.Click += btnAgvMonitor_Click;
            // 
            // btnBaseData
            // 
            btnBaseData.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnBaseData.Location = new Point(53, 932);
            btnBaseData.MinimumSize = new Size(1, 1);
            btnBaseData.Name = "btnBaseData";
            btnBaseData.Size = new Size(248, 61);
            btnBaseData.TabIndex = 21;
            btnBaseData.Text = "⚙️ 基础数据管理";
            btnBaseData.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnBaseData.Click += btnBaseData_Click;
            // 
            // btnReturnStock
            // 
            btnReturnStock.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnReturnStock.Location = new Point(53, 1007);
            btnReturnStock.MinimumSize = new Size(1, 1);
            btnReturnStock.Name = "btnReturnStock";
            btnReturnStock.Size = new Size(248, 61);
            btnReturnStock.TabIndex = 22;
            btnReturnStock.Text = "🔄 产线返料";
            btnReturnStock.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnReturnStock.Click += btnReturnStock_Click;
            // 
            // btnWaveOut
            // 
            btnWaveOut.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnWaveOut.Location = new Point(53, 1083);
            btnWaveOut.MinimumSize = new Size(1, 1);
            btnWaveOut.Name = "btnWaveOut";
            btnWaveOut.Size = new Size(248, 61);
            btnWaveOut.TabIndex = 23;
            btnWaveOut.Text = "🌊 波次出库";
            btnWaveOut.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnWaveOut.Click += btnWaveOut_Click;
            // 
            // btnOrderCenter
            // 
            btnOrderCenter.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnOrderCenter.Location = new Point(53, 1159);
            btnOrderCenter.MinimumSize = new Size(1, 1);
            btnOrderCenter.Name = "btnOrderCenter";
            btnOrderCenter.Size = new Size(248, 61);
            btnOrderCenter.TabIndex = 24;
            btnOrderCenter.Text = "📑 单据管理中心";
            btnOrderCenter.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnOrderCenter.Click += btnOrderCenter_Click;
            // 
            // btnUserManage
            // 
            btnUserManage.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnUserManage.Location = new Point(53, 1238);
            btnUserManage.MinimumSize = new Size(1, 1);
            btnUserManage.Name = "btnUserManage";
            btnUserManage.Size = new Size(248, 61);
            btnUserManage.TabIndex = 25;
            btnUserManage.Text = "👤 系统权限与账号";
            btnUserManage.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnUserManage.Click += btnUserManage_Click;
            // 
            // btnSysVoiceSetup
            // 
            btnSysVoiceSetup.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSysVoiceSetup.Location = new Point(53, 1320);
            btnSysVoiceSetup.MinimumSize = new Size(1, 1);
            btnSysVoiceSetup.Name = "btnSysVoiceSetup";
            btnSysVoiceSetup.Size = new Size(248, 61);
            btnSysVoiceSetup.TabIndex = 26;
            btnSysVoiceSetup.Text = "⚙️ 系统参数设置";
            btnSysVoiceSetup.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSysVoiceSetup.Click += btnSysVoiceSetup_Click;
            // 
            // btnSysLog
            // 
            btnSysLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSysLog.FillColor = Color.Orchid;
            btnSysLog.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSysLog.Location = new Point(1490, 99);
            btnSysLog.MinimumSize = new Size(1, 1);
            btnSysLog.Name = "btnSysLog";
            btnSysLog.Size = new Size(173, 61);
            btnSysLog.TabIndex = 27;
            btnSysLog.Text = "🛡️ 系统日志";
            btnSysLog.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSysLog.Click += btnSysLog_Click;
            // 
            // btnSetupLangs
            // 
            btnSetupLangs.FillColor = Color.FromArgb(192, 64, 0);
            btnSetupLangs.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSetupLangs.Location = new Point(397, 99);
            btnSetupLangs.MinimumSize = new Size(1, 1);
            btnSetupLangs.Name = "btnSetupLangs";
            btnSetupLangs.Size = new Size(217, 61);
            btnSetupLangs.TabIndex = 28;
            btnSetupLangs.Text = "🔄 简/繁/英";
            btnSetupLangs.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSetupLangs.Click += btnSetupLangs_Click;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1967, 1452);
            Controls.Add(btnSetupLangs);
            Controls.Add(btnSysLog);
            Controls.Add(btnSysVoiceSetup);
            Controls.Add(btnUserManage);
            Controls.Add(btnOrderCenter);
            Controls.Add(btnWaveOut);
            Controls.Add(btnReturnStock);
            Controls.Add(btnBaseData);
            Controls.Add(btnAgvMonitor);
            Controls.Add(btnExportExcel);
            Controls.Add(btnLogout);
            Controls.Add(uiButton9);
            Controls.Add(uiButton8);
            Controls.Add(btnSimulateAgv);
            Controls.Add(uiButton7);
            Controls.Add(uiButton6);
            Controls.Add(uiButton5);
            Controls.Add(dataGridView1);
            Controls.Add(uiButton4);
            Controls.Add(uiButton3);
            Controls.Add(uiButton2);
            Controls.Add(uiButton1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "MainForm";
            Text = "WMS_Main_Form";
            ZoomScaleRect = new Rectangle(26, 26, 1489, 877);
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;

        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton4;
        private Sunny.UI.UIDataGridView dataGridView1;
        private Sunny.UI.UIButton uiButton5;
        private Sunny.UI.UIButton uiButton6;
        private Sunny.UI.UIButton uiButton7;
        private Sunny.UI.UIButton btnSimulateAgv;
        private Sunny.UI.UIButton uiButton8;
        private Sunny.UI.UIButton uiButton9;
        private Sunny.UI.UIButton btnLogout;
        private Sunny.UI.UIButton btnExportExcel;
        private Sunny.UI.UIButton btnAgvMonitor;
        private Sunny.UI.UIButton btnBaseData;
        private Sunny.UI.UIButton btnReturnStock;
        private Sunny.UI.UIButton btnWaveOut;
        private Sunny.UI.UIButton btnOrderCenter;
        private Sunny.UI.UIButton btnUserManage;
        private Sunny.UI.UIButton btnSysVoiceSetup;
        private Sunny.UI.UIButton btnSysLog;
        private Sunny.UI.UIButton btnSetupLangs;
    }
}
