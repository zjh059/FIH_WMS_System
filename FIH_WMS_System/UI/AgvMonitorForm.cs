using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FIH_WMS_System.Models;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class AgvMonitorForm : Form
    {
        private WmsService wms = new WmsService();

        public AgvMonitorForm()
        {
            InitializeComponent();
        }

        private void AgvMonitorForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            Utils.VoiceHelper.Speak("AGV任务状态已更新");
        }

        private void LoadData()
        {
            // 呼叫服务层获取所有小车任务
            var tasks = wms.GetAgvTasks();
            dgvTasks.DataSource = tasks;
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvTasks.Columns["Goods"] != null) dgvTasks.Columns["Goods"].Visible = false;

            dgvTasks.Columns["Id"].HeaderText = "系统ID";
            dgvTasks.Columns["TaskNo"].HeaderText = "WCS 调度单号";
            dgvTasks.Columns["TaskType"].HeaderText = "任务类型";
            dgvTasks.Columns["Status"].HeaderText = "执行状态";
            dgvTasks.Columns["GoodsCode"].HeaderText = "物料编码";
            dgvTasks.Columns["Qty"].HeaderText = "搬运数量";
            dgvTasks.Columns["FromLocation"].HeaderText = "起点 (取货)";
            dgvTasks.Columns["ToLocation"].HeaderText = "终点 (送达)";
            dgvTasks.Columns["CreateTime"].HeaderText = "任务下发时间";
            dgvTasks.Columns["FinishTime"].HeaderText = "实际送达时间";

            dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==========================================
        // 核心亮点：动态渲染单元格状态 (将底层数字转换为好看的 UI)
        // ==========================================
        private void dgvTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTasks.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                int status = (int)e.Value;
                if (status == 3)
                {
                    e.Value = "✅ 已送达";
                    e.CellStyle.ForeColor = Color.DarkGray; // 完结的任务变灰
                }
                else
                {
                    e.Value = "🚚 运输中";
                    e.CellStyle.ForeColor = Color.DarkOrange; // 正在跑的任务高亮
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                e.FormattingApplied = true;
            }

            if (dgvTasks.Columns[e.ColumnIndex].Name == "TaskType" && e.Value != null)
            {
                int type = (int)e.Value;
                e.Value = type == 1 ? "出库/产线备料" : "内部移库合并";
                e.FormattingApplied = true;
            }
        }

        // ==========================================
        // 交互：模拟 AGV 物理到达，闭环任务
        // ==========================================
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先在表格中选中一条正在【运输中】的 AGV 任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["Id"].Value);
            int status = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["Status"].Value);

            if (status == 3)
            {
                MessageBox.Show("该任务已经送达了，请选择其他正在运输的任务。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 呼叫 WMS 大脑，将任务标记为已完成
            bool success = wms.CompleteAgvTask(taskId);

            if (success)
            {
                Utils.VoiceHelper.Speak("小车已到达指定位置");
                LoadData(); // 重新刷新表格，你会看到它瞬间变成绿色的“✅ 已送达”，并打上完成时间戳！
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 每隔3秒，定时器会自动执行这句代码，悄无声息地刷新 AGV 最新状态！
            LoadData();
        }
    }
}
