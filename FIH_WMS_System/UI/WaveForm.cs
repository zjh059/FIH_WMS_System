using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FIH_WMS_System.Services;
using FIH_WMS_System.Models;

namespace FIH_WMS_System.UI
{
    public partial class WaveForm : Form
    {
        private WmsService wms = new WmsService();
        // 字典：用来暂时存放被装进这个波次里的所有【工单号】和【生产数量】
        private Dictionary<string, int> waveOrders = new Dictionary<string, int>();
        private List<BOMRequirement> waveBOMs = new List<BOMRequirement>();

        // 纯代码定义的 UI 控件
        private ListBox listOrders = new ListBox();
        private DataGridView dgvWave = new DataGridView();
        private TextBox txtPCode = new TextBox();
        private TextBox txtQty = new TextBox();

        public WaveForm()
        {
            InitializeComponent();
            // 1. 初始化窗体属性
            this.Text = "🌊 波次出库管理中心 (Wave Consolidation)";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("微软雅黑", 10F);

            // 2. 绘制上半部分：添加波次工单区
            Label lbl1 = new Label { Text = "成品工单:", Location = new Point(20, 25), AutoSize = true };
            txtPCode.Location = new Point(100, 20); txtPCode.Size = new Size(120, 30);

            Label lbl2 = new Label { Text = "生产数量:", Location = new Point(240, 25), AutoSize = true };
            txtQty.Location = new Point(320, 20); txtQty.Size = new Size(80, 30);

            Button btnAdd = new Button { Text = "➕ 录入本波次", Location = new Point(420, 18), Size = new Size(120, 32), BackColor = Color.LightBlue };
            btnAdd.Click += BtnAdd_Click;

            listOrders.Location = new Point(20, 60); listOrders.Size = new Size(520, 100);

            // 3. 绘制下半部分：波次分析与一键执行区
            Button btnAnalyze = new Button { Text = "⚙️ 智能合并波次物料 (BOM)", Location = new Point(20, 180), Size = new Size(250, 40), BackColor = Color.Orange };
            btnAnalyze.Click += BtnAnalyze_Click;

            Button btnExecute = new Button { Text = "🚀 一键下发波次拣货任务 (呼叫AGV)", Location = new Point(290, 180), Size = new Size(300, 40), BackColor = Color.MediumSeaGreen, ForeColor = Color.White };
            btnExecute.Click += BtnExecute_Click;

            dgvWave.Location = new Point(20, 230); dgvWave.Size = new Size(740, 310);
            dgvWave.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // 4. 将画好的控件一把抓，全塞进窗体里显示出来
            this.Controls.Add(lbl1); this.Controls.Add(txtPCode);
            this.Controls.Add(lbl2); this.Controls.Add(txtQty);
            this.Controls.Add(btnAdd); this.Controls.Add(listOrders);
            this.Controls.Add(btnAnalyze); this.Controls.Add(btnExecute);
            this.Controls.Add(dgvWave);
        }

        // ===============================================
        // 以下是波次业务逻辑代码 (无需修改)
        // ===============================================

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string code = txtPCode.Text.Trim();
            if (string.IsNullOrEmpty(code) || !int.TryParse(txtQty.Text, out int qty)) return;

            if (waveOrders.ContainsKey(code)) waveOrders[code] += qty;
            else waveOrders.Add(code, qty);

            txtPCode.Clear(); txtQty.Clear();
            listOrders.Items.Clear();
            foreach (var kvp in waveOrders) listOrders.Items.Add($"待产任务 - 组装成品: 【{kvp.Key}】, 需下线数量: {kvp.Value} 台");
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            if (waveOrders.Count == 0) return;

            waveBOMs = wms.AnalyzeWaveBOM(waveOrders);
            dgvWave.DataSource = waveBOMs;
            dgvWave.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewRow row in dgvWave.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["IsEnough"].Value))
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (waveBOMs.Count == 0) return;
            if (waveBOMs.Any(r => !r.IsEnough))
            {
                Utils.VoiceHelper.Speak("波次齐套检查失败");
                MessageBox.Show("合并后的波次物料存在缺口，无法齐套发料，已被拦截！", "齐套性拦截", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string waveNo = "WAVE-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            bool success = wms.ExecuteBOMOutbound(waveBOMs, waveNo, OutboundStrategy.FIFO);

            if (success)
            {
                Utils.VoiceHelper.Speak("波次出库任务已派发，AGV 正在合并拣货");
                MessageBox.Show($"🎉 波次合并出库任务生成成功！波次号：{waveNo}\nAGV 小车正在按 FIFO 规则为您合并备料。", "智能调度");
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}