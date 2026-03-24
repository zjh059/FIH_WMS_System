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
    public partial class CheckStockForm : Form
    {
        // 实例化服务层和当前盘点清单缓存
        private WmsService wms = new WmsService();
        private List<StockCountItem> currentCountList = new List<StockCountItem>();

        public CheckStockForm()
        {
            InitializeComponent();
        }

        // 1. 窗口加载时，初始化下拉选项
        private void CheckStockForm_Load(object sender, EventArgs e)
        {
            cmbCountType.Items.Add("0 - 按指定库位盘点");
            cmbCountType.Items.Add("1 - 按指定物料盘点");
            cmbCountType.SelectedIndex = 0; // 默认选中第一个

        }

        // 2. 点击“生成盘点清单”按钮
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int countType = cmbCountType.SelectedIndex;
            string keyword = txtKeyword.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("请输入要盘点的库位编码或物料编码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 呼叫服务层获取数据
            currentCountList = wms.GenerateCountList(countType, keyword);

            if (currentCountList.Count == 0)
            {
                MessageBox.Show("未找到符合条件的库存记录，请检查输入是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvCountList.DataSource = null;
                return;
            }

            // 绑定到表格
            dgvCountList.DataSource = null;
            dgvCountList.DataSource = currentCountList;

            // 格式化表格，让它看起来更直观，并限制只允许修改“实盘数量”
            FormatGrid();
        }

        // 格式化 DataGridView (美化与权限控制)
        private void FormatGrid()
        {
            foreach (DataGridViewColumn col in dgvCountList.Columns)
            {
                // 只有“PhysicalQty(实盘数量)”允许编辑，其他全部只读！
                if (col.Name == "PhysicalQty")
                {
                    col.HeaderText = "✍️ 实盘数量 (点此修改)";
                    col.DefaultCellStyle.BackColor = Color.LightYellow; // 亮黄色背景提示可填
                    col.DefaultCellStyle.Font = new Font("微软雅黑", 10, FontStyle.Bold);
                    col.DefaultCellStyle.ForeColor = Color.Red;
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }

            // 隐藏内部ID，设置中文表头
            dgvCountList.Columns["StockId"].Visible = false;
            dgvCountList.Columns["GoodsCode"].HeaderText = "物料编码";
            dgvCountList.Columns["GoodsName"].HeaderText = "物料名称";
            dgvCountList.Columns["LocationCode"].HeaderText = "所在库位";
            dgvCountList.Columns["BatchNo"].HeaderText = "批次号";
            dgvCountList.Columns["SystemQty"].HeaderText = "系统账面数";
            dgvCountList.Columns["Difference"].HeaderText = "差异数量";

            //dgvCountList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //强制让所有列按比例撑满整个表格宽度！
            dgvCountList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // 3. 提交盘点结果并一键平账
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (currentCountList == null || currentCountList.Count == 0)
            {
                MessageBox.Show("请先生成盘点清单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 强制结束表格中正在编辑的单元格，确保最新的数字写进了内存中
            dgvCountList.EndEdit();

            DialogResult result = MessageBox.Show(
                "确认提交当前的盘点结果吗？\n\n系统将自动比对差异，修改系统库存量，并生成对应的流水账单！",
                "一键平账确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 调用服务层的一键批量平账功能
                bool success = wms.SubmitBatchCountResult(currentCountList);
                if (success)
                {
                    // 成功后，给主界面发送 OK 信号
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("提交失败！数据库发生异常，请重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}