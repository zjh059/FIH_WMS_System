using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Generic;
using FIH_WMS_System.Services;
using FIH_WMS_System.Models;

namespace FIH_WMS_System.UI
{
    public partial class OutStockForm : Form
    {
        private WmsService wms = new WmsService();
        private List<BOMRequirement> currentBOMReqs = new List<BOMRequirement>();

        // 保留给主界面调用的常规模板参数
        public string InputGoodsCode { get; set; }
        public int InputQty { get; set; }
        public string InputLocCode { get; set; }
        public OutboundStrategy InputStrategy { get; set; }

        // 【新增】：告诉主界面，我们是不是通过BOM模式成功出库了
        public bool IsBOMOutCompleted { get; set; } = false;

        public OutStockForm()
        {
            InitializeComponent();
            this.Load += OutStockForm_Load; //  新增:确保窗口打开时会执行数据加载
        }

        private void OutStockForm_Load(object sender, EventArgs e)
        {
            var strategyDict = new Dictionary<OutboundStrategy, string>
            {
                { OutboundStrategy.Manual, "Manual-直接人工指定" },
                { OutboundStrategy.FIFO, "FIFO-先进先出 (防过期)" },

                { OutboundStrategy.FEFO, "FEFO-近效期优先 (防过期)" }, // 新增


                { OutboundStrategy.LIFO, "LIFO-后进先出" },
                { OutboundStrategy.NearestFirst, "NearestFirst-就近原则" },

                { OutboundStrategy.LeastQuantityFirst, "LeastQty-存量最少优先 (清空碎片)" }, 
                { OutboundStrategy.MostQuantityFirst, "MostQty-存量充足优先 (减少搬运)" },

                { OutboundStrategy.ByReelId, "ReelId-一物一码精准出库" } // 新增

            };



            // 1. 给第一页的单品出库下拉框绑定数据
            cmbStrategy.DataSource = new BindingSource(strategyDict, null);
            cmbStrategy.DisplayMember = "Value";
            cmbStrategy.ValueMember = "Key";
            cmbStrategy.SelectedValue = OutboundStrategy.FIFO;

            // 2. 给第三页的追加补料下拉框也绑定数据 (注意必须 new 一个新的 BindingSource，否则两页的下拉框会互相干扰联动)
            if (cmbStrategyAdd != null)
            {
                cmbStrategyAdd.DataSource = new BindingSource(strategyDict, null);
                cmbStrategyAdd.DisplayMember = "Value";
                cmbStrategyAdd.ValueMember = "Key";
                cmbStrategyAdd.SelectedValue = OutboundStrategy.FIFO;
            }




            cmbStrategy.DataSource = new BindingSource(strategyDict, null);
            cmbStrategy.DisplayMember = "Value";
            cmbStrategy.ValueMember = "Key";
            cmbStrategy.SelectedValue = OutboundStrategy.FIFO;
        }

        // 常规单品出库，仅收集数据并返回 OK 交给 MainForm 处理
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputLocCode = txtLocCode.Text.Trim();
                InputQty = int.Parse(txtQty.Text.Trim());
                InputStrategy = (OutboundStrategy)cmbStrategy.SelectedValue;

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：出库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtScanner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string scannedCode = txtScanner.Text.Trim();
                if (!string.IsNullOrEmpty(scannedCode))
                {
                    System.Media.SystemSounds.Beep.Play();

                    // 【新增智能识别】：判断扫进来的是物料码还是长长的工业追溯码 (ReelId)
                    // 比如 XX-化工-G001-20260415-1815 包含 "-" 且长度分段较多
                    if (scannedCode.Contains("-") && scannedCode.Split('-').Length >= 4)
                    {
                        Utils.VoiceHelper.Speak("已识别为物料追溯码，自动开启一物一码出库");
                        txtGoodsCode.Text = scannedCode; // 把 ReelId 填进文本框
                        cmbStrategy.SelectedValue = OutboundStrategy.ByReelId; // 自动切策略！
                    }
                    else
                    {
                        Utils.VoiceHelper.Speak("扫码成功");
                        txtGoodsCode.Text = scannedCode; // 普通物料码
                        cmbStrategy.SelectedValue = OutboundStrategy.FIFO; // 默认 FIFO
                    }

                    txtScanner.Clear();
                    txtQty.Focus(); // 焦点自动跳到数量框
                }
            }
        }

        // ==========================================
        // 核心：分析 BOM 齐套性
        // ==========================================
        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            string productCode = txtProductCode.Text.Trim();
            if (string.IsNullOrEmpty(productCode) || !int.TryParse(txtProduceQty.Text.Trim(), out int produceQty))
            {
                MessageBox.Show("请输入正确的成品编码和生产数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 调用大脑解析
            currentBOMReqs = wms.AnalyzeBOM(productCode, produceQty);

            if (currentBOMReqs.Count == 0)
            {
                MessageBox.Show("该产品未配置底层 BOM 结构，或产品编码不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvBOM.DataSource = null;
                return;
            }

            dgvBOM.DataSource = currentBOMReqs;
            FormatBOMGrid();
        }

        private void FormatBOMGrid()
        {
            dgvBOM.Columns["ChildGoodsCode"].HeaderText = "所需原材料编码";
            dgvBOM.Columns["ChildGoodsName"].HeaderText = "物料名称";
            dgvBOM.Columns["RequiredTotalQty"].HeaderText = "生产需消耗总数";
            dgvBOM.Columns["CurrentAvailableQty"].HeaderText = "当前全仓可用总数";
            dgvBOM.Columns["IsEnough"].HeaderText = "库存是否满足?";

            dgvBOM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 如果数量不够，标红警告！
            foreach (DataGridViewRow row in dgvBOM.Rows)
            {
                bool isEnough = Convert.ToBoolean(row.Cells["IsEnough"].Value);
                if (!isEnough)
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }
        }

        // ==========================================
        // 核心：一键派发 BOM 出库工单 (升级)
        // ==========================================
        private void btnExecuteBOM_Click(object sender, EventArgs e)
        {
            if (currentBOMReqs.Count == 0) return;

            // 检查是否有缺料情况
            if (currentBOMReqs.Any(r => !r.IsEnough))
            {
                Utils.VoiceHelper.Speak("齐套检查失败，已为您计算缺口并准备生成采购单。");

                // 👇 核心升级：不再是单纯的报错，而是提供一键采购的选项！
                DialogResult shortResult = MessageBox.Show(
                    "警告：底层原材料库存不足以支撑该工单生产！出库任务已被拦截。\n\n" +
                    "是否需要系统自动根据缺口数量，为您一键生成【采购入库单】？",
                    "齐套性拦截与智能补货",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (shortResult == DialogResult.Yes)
                {
                    bool purSuccess = wms.GeneratePurchaseOrderByBOM(currentBOMReqs);
                    if (purSuccess)
                    {
                        Utils.VoiceHelper.Speak("采购补货单已成功下发至订单中心。");
                        MessageBox.Show("🎉 BOM 缺料采购单已成功生成！\n请前往【单据管理中心】查看，并等待采购入库后再次尝试发料。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("采购单生成失败，请检查数据库连接状态。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return;
            }

            string orderNo = "WO-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            OutboundStrategy strategy = (OutboundStrategy)cmbStrategy.SelectedValue; // 沿用上面选择的策略（通常是FIFO）

            DialogResult result = MessageBox.Show($"确认下发工单【{orderNo}】？\n\n系统将按照【{strategy}】策略为您锁定各散落货架的原材料，并指派 AGV 备料至产线接驳口。", "智能调度", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = wms.ExecuteBOMOutbound(currentBOMReqs, orderNo, strategy);

                if (success)
                {
                    IsBOMOutCompleted = true; // 设为true，告诉主窗体我们是用BOM成功出库的
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("出库执行失败，可能部分库存刚被其他系统占用！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // 核心：执行产线追加需求单
        // ==========================================
        private void btnExecuteAdd_Click(object sender, EventArgs e)
        {
            // 获取输入框的值（稍后你在界面上拖出这三个框，把 Name 命名成这样就行）
            string woNo = txtAddWO.Text.Trim();
            string goodsCode = txtAddGoodsCode.Text.Trim();

            if (string.IsNullOrEmpty(goodsCode) || !int.TryParse(txtAddQty.Text.Trim(), out int qty) || qty <= 0)
            {
                MessageBox.Show("请输入正确的追加物料编码和数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 默认采用先进先出策略防过期
            OutboundStrategy strategy = OutboundStrategy.FIFO;

            DialogResult result = MessageBox.Show(
                $"确认生成追加领料单并呼叫 AGV 吗？\n\n关联工单：{(string.IsNullOrEmpty(woNo) ? "无" : woNo)}\n追加物料：{goodsCode}\n追加数量：{qty}",
                "追加出库确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 呼叫大脑处理
                bool success = wms.ExecuteAdditionalOutbound(woNo, goodsCode, qty, strategy);

                if (success)
                {
                    Utils.VoiceHelper.Speak("产线追加物料单已下发，AGV正在为您备料。");
                    MessageBox.Show("🎉 追加需求单下发成功！\n系统已扣减库存，AGV 正在前往取货。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 标记出库成功，关闭窗口并让主界面刷新表格
                    this.IsBOMOutCompleted = true;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("❌ 出库失败：可能是仓库中该物料库存已不足，或已被其他任务冻结占用！", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




    }
}