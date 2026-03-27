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

        public WaveForm()
        {
            InitializeComponent(); // 让 Designer.cs 去干排版的活儿
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string code = txtPCode.Text.Trim();
            if (string.IsNullOrEmpty(code) || !int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("工单编码不能为空，且数量必须大于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 将需求工单加入波次池 (如果同一工单重复录入，数量累加)
            if (waveOrders.ContainsKey(code)) waveOrders[code] += qty;
            else waveOrders.Add(code, qty);

            // 清空输入框，刷新列表
            txtPCode.Clear();
            txtQty.Clear();
            listOrders.Items.Clear();

            foreach (var kvp in waveOrders)
            {
                listOrders.Items.Add($"待产任务 - 组装成品: 【{kvp.Key}】, 需下线数量: {kvp.Value} 台");
            }
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            if (waveOrders.Count == 0)
            {
                MessageBox.Show("请先录入至少一个波次工单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 呼叫波次大脑，进行同类项合并计算！
            waveBOMs = wms.AnalyzeWaveBOM(waveOrders);
            dgvWave.DataSource = waveBOMs;
            dgvWave.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 木桶短板防呆：缺料行标红
            foreach (DataGridViewRow row in dgvWave.Rows)
            {
                if (row.Cells["IsEnough"].Value != null && !Convert.ToBoolean(row.Cells["IsEnough"].Value))
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (waveBOMs.Count == 0) return;

            // 发车前的最后一次安检
            if (waveBOMs.Any(r => !r.IsEnough))
            {
                Utils.VoiceHelper.Speak("波次齐套检查失败");
                MessageBox.Show("合并后的波次物料存在缺口，无法齐套发料，已被拦截！\n请通知采购补齐红色高亮的物料。", "齐套性拦截", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string waveNo = "WAVE-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // 复用底层智能出库逻辑
            bool success = wms.ExecuteBOMOutbound(waveBOMs, waveNo, OutboundStrategy.FIFO);

            if (success)
            {
                Utils.VoiceHelper.Speak("波次出库任务已派发，AGV 正在合并拣货");
                MessageBox.Show($"🎉 波次合并出库任务生成成功！波次号：{waveNo}\nAGV 小车正在按 FIFO 规则为您合并备料。", "智能调度");
                this.DialogResult = DialogResult.OK; // 关闭窗口返回主界面
            }
            else
            {
                MessageBox.Show("波次执行失败，可能是底层库存发生变动导致扣减异常。", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}