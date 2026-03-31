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
        }

        private void OutStockForm_Load(object sender, EventArgs e)
        {
            var strategyDict = new Dictionary<OutboundStrategy, string>
            {
                { OutboundStrategy.Manual, "Manual-直接人工指定" },
                { OutboundStrategy.FIFO, "FIFO-先进先出 (防过期)" },
                { OutboundStrategy.LIFO, "LIFO-后进先出" },
                { OutboundStrategy.NearestFirst, "NearestFirst-就近原则" },

                { OutboundStrategy.LeastQuantityFirst, "LeastQty-存量最少优先 (清空碎片)" }, 
                { OutboundStrategy.MostQuantityFirst, "MostQty-存量充足优先 (减少搬运)" }

            };

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
                    Utils.VoiceHelper.Speak("扫码成功");
                    txtGoodsCode.Text = scannedCode;
                    txtScanner.Clear();
                    txtQty.Focus();
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
        // 核心：一键派发 BOM 出库工单
        // ==========================================
        private void btnExecuteBOM_Click(object sender, EventArgs e)
        {
            if (currentBOMReqs.Count == 0) return;

            // 检查是否有缺料情况
            if (currentBOMReqs.Any(r => !r.IsEnough))
            {
                //语音播报警告
                Utils.VoiceHelper.Speak("齐套检查失败，严禁盲目发料！");

                MessageBox.Show("警告：底层原材料库存不足以支撑该工单生产！\n出库任务已被拦截，请先安排采购入库。", "齐套性拦截", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}