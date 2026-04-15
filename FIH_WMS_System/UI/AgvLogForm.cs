using System;
using System.Drawing;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class AgvLogForm : Form
    {
        private WmsService wms = new WmsService();

        // 构造函数，支持直接传入单号进行筛选
        public AgvLogForm(string initTaskNo = "")
        {
            InitializeComponent();
            txtTaskFilter.Text = initTaskNo;
        }

        private void AgvLogForm_Load(object sender, EventArgs e)
        {
            // 美化表格标题
            dgvAgvLogs.EnableHeadersVisualStyles = false;
            dgvAgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            dgvAgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);

            RefreshData();
        }

        private void txtTaskFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        //private void RefreshData()
        //{
        //    dgvAgvLogs.DataSource = wms.GetAgvLogs(txtTaskFilter.Text.Trim());

        //    // 调整时间列宽
        //    if (dgvAgvLogs.Columns.Count > 4)
        //    {
        //        dgvAgvLogs.Columns["记录时间"].Width = 180;
        //    }
        //}


        //在 Designer.cs 里把这个表格的列宽自适应模式设置为了 Fill（自动填满整个表格宽度）。
        //在 Fill 模式下，表格的列宽是由系统根据百分比（FillWeight）动态计算的。这个时候强行用代码去写死某底列的绝对宽度（Width = 180），WinForms 底层的布局引擎就会逻辑打架，直接抛出内部的空引用异常（也就是 set_Thickness 崩溃）
        private void RefreshData()
        {
            dgvAgvLogs.DataSource = wms.GetAgvLogs(txtTaskFilter.Text.Trim());

            // 安全地调整列宽比例 (替换掉原来报错的 .Width = 180)
            if (dgvAgvLogs.Columns.Count > 0)
            {
                // 使用 FillWeight 按比例分配宽度，完美契合 AutoSizeColumnsMode.Fill
                if (dgvAgvLogs.Columns["序号"] != null) dgvAgvLogs.Columns["序号"].FillWeight = 10;
                if (dgvAgvLogs.Columns["任务单号"] != null) dgvAgvLogs.Columns["任务单号"].FillWeight = 25;
                if (dgvAgvLogs.Columns["动作描述"] != null) dgvAgvLogs.Columns["动作描述"].FillWeight = 35;
                if (dgvAgvLogs.Columns["当前位置"] != null) dgvAgvLogs.Columns["当前位置"].FillWeight = 15;
                if (dgvAgvLogs.Columns["记录时间"] != null) dgvAgvLogs.Columns["记录时间"].FillWeight = 20;
            }
        }



        private void btnExport_Click(object sender, EventArgs e)
        {
            // 这里可以复用你项目中的 ExcelHelper 导出功能
            MessageBox.Show("轨迹日志导出成功！(模拟动作)");
        }
    }
}