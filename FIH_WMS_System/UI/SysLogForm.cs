using System;
using System.Drawing;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class SysLogForm : Form
    {
        private WmsService wms = new WmsService();

        public SysLogForm()
        {
            InitializeComponent();
        }

        private void SysLogForm_Load(object sender, EventArgs e)
        {
            // 1. 深度美化表格样式（放在代码里更好控制颜色）
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 160, 255);
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            // 2. 加载最新数据
            LoadLogData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogData();
        }

        private void LoadLogData()
        {
            var dt = wms.GetOperationLogs();
            dgvLogs.DataSource = dt;

            // 美化列宽比例：让“详细操作记录”这一列尽量宽，展示更多内容
            if (dgvLogs.Columns.Count > 0)
            {
                dgvLogs.Columns["日志编号"].FillWeight = 10;
                dgvLogs.Columns["操作人账号"].FillWeight = 15;
                dgvLogs.Columns["操作模块"].FillWeight = 15;
                dgvLogs.Columns["详细操作记录"].FillWeight = 40;
                dgvLogs.Columns["操作时间"].FillWeight = 20;
            }
        }
    }
}