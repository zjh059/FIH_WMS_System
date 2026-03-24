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
    public partial class MoveStockForm : Form
    {
        private WmsService wms = new WmsService();
        private List<ConsolidationAdvice> currentAdviceList = new List<ConsolidationAdvice>();

        // 保留给人工移库的四个“口袋”
        public string InputGoodsCode { get; set; }
        public string InputFromLoc { get; set; }
        public string InputToLoc { get; set; }
        public int InputQty { get; set; }

        // 【新增】：告诉主界面，我们是不是已经完成了“智能模式”的移库
        public bool IsSmartMoveCompleted { get; set; } = false;

        public MoveStockForm()
        {
            InitializeComponent();
        }

        // 1. 人工指定移库 (传统模式) - 逻辑保持不变
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputFromLoc = txtFromLoc.Text.Trim();
                InputToLoc = txtToLoc.Text.Trim();
                InputQty = int.Parse(txtQty.Text.Trim());

                if (string.IsNullOrEmpty(InputGoodsCode) || string.IsNullOrEmpty(InputFromLoc) || string.IsNullOrEmpty(InputToLoc))
                {
                    MessageBox.Show("警告：商品编码和新旧库位都必须填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (InputFromLoc == InputToLoc)
                {
                    MessageBox.Show("警告：源库位和目标库位不能相同！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK; // 关闭并交由 MainForm 处理
            }
            catch (Exception)
            {
                MessageBox.Show("警告：移库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 2. 智能扫描全仓碎片
        private void btnScan_Click(object sender, EventArgs e)
        {
            currentAdviceList = wms.GetConsolidationAdvice(); // 呼叫服务层的大脑

            if (currentAdviceList.Count == 0)
            {
                MessageBox.Show("太棒了！当前仓库状态极其健康，没有任何需要合并的零星碎片！", "扫描结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvAdvice.DataSource = null;
                return;
            }

            dgvAdvice.DataSource = currentAdviceList;
            FormatGrid(); // 美化表格
        }

        private void FormatGrid()
        {
            if (dgvAdvice.Columns["GoodsCode"] != null) dgvAdvice.Columns["GoodsCode"].HeaderText = "物料编码";
            if (dgvAdvice.Columns["GoodsName"] != null) dgvAdvice.Columns["GoodsName"].HeaderText = "物料名称";

            if (dgvAdvice.Columns["FromLocation"] != null)
            {
                dgvAdvice.Columns["FromLocation"].HeaderText = "源库位 (迁出)";
                dgvAdvice.Columns["FromLocation"].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (dgvAdvice.Columns["ToLocation"] != null)
            {
                dgvAdvice.Columns["ToLocation"].HeaderText = "目标大本营 (迁入)";
                dgvAdvice.Columns["ToLocation"].DefaultCellStyle.ForeColor = Color.Green;
                dgvAdvice.Columns["ToLocation"].DefaultCellStyle.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            }
            if (dgvAdvice.Columns["MoveQty"] != null) dgvAdvice.Columns["MoveQty"].HeaderText = "建议移动数量";

            dgvAdvice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // 3. 一键批量执行智能合并
        private void btnExecuteSmart_Click(object sender, EventArgs e)
        {
            if (currentAdviceList == null || currentAdviceList.Count == 0)
            {
                MessageBox.Show("没有可执行的碎片整理建议！请先进行扫描。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"即将自动执行 {currentAdviceList.Count} 条移库合并任务。\n系统将自动生成 AGV 调度任务，是否继续？",
                "智能合并确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int successCount = 0;
                foreach (var advice in currentAdviceList)
                {
                    // 直接在这里循环调用服务层进行移库
                    bool ok = wms.MoveStock(advice.GoodsCode, advice.FromLocation, advice.ToLocation, advice.MoveQty);
                    if (ok) successCount++;
                }

                if (successCount > 0)
                {
                    IsSmartMoveCompleted = true; // 告诉主窗体，智能模式搞定了
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("执行失败，可能部分库存已被其他单据冻结或占用！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}