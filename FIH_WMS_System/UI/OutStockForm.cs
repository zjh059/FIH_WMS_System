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
    /// <summary>
    /// 入库窗体界面
    /// </summary>
    public partial class OutStockForm : Form
    {
        private WmsService wmsService = new WmsService();
        public OutStockForm()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. 窗体加载：绑定出库策略字典
        // ==========================================
        private void OutStockForm_Load(object sender, EventArgs e)
        {
            var strategyDict = new Dictionary<OutboundStrategy, string>
            {
                { OutboundStrategy.Manual, "Manual-直接人工指定库位" },
                { OutboundStrategy.FIFO, "FIFO-先进先出 (防过期)" },
                { OutboundStrategy.LIFO, "LIFO-后进先出 (最新批次)" },
                { OutboundStrategy.NearestFirst, "NearestFirst-就近原则出库" }
            };

            cmbStrategy.DataSource = new BindingSource(strategyDict, null);
            cmbStrategy.DisplayMember = "Value";
            cmbStrategy.ValueMember = "Key";

            // 默认选中 FIFO 先进先出
            cmbStrategy.SelectedValue = OutboundStrategy.FIFO;
        }


        // 1. 定义三个“公开的口袋”，用来装用户输入的数据，方便主窗口一会来拿
        public string InputGoodsCode { get; set; }
        public int InputQty { get; set; }
        public string InputLocCode { get; set; }


        // 确认按钮的点击事件
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            /*            try
                        {
                            // 2. 把文本框里的字，装进我们的口袋里
                            InputGoodsCode = txtGoodsCode.Text;
                            InputLocCode = txtLocCode.Text;

                            // 数量必须是数字，所以用 int.Parse 转换一下
                            InputQty = int.Parse(txtQty.Text);

                            // 3. 极其重要,告诉系统：“这个弹窗的任务圆满完成 (OK)！”
                            // 这句话执行后，弹窗会自动关闭，并给主窗口发个 OK 的信号。
                            this.DialogResult = DialogResult.OK;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("警告：出库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }*/

            string goodsCode = txtGoodsCode.Text.Trim();
            if (string.IsNullOrEmpty(goodsCode))
            {
                MessageBox.Show("请输入物料编码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQty.Text.Trim(), out int qty) || qty <= 0)
            {
                MessageBox.Show("请输入正确的出库数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OutboundStrategy selectedStrategy = (OutboundStrategy)cmbStrategy.SelectedValue;

            // --- 场景 A：人工指定模式 ---
            if (selectedStrategy == OutboundStrategy.Manual)
            {
                string locCode = txtLocCode.Text.Trim();
                if (string.IsNullOrEmpty(locCode))
                {
                    MessageBox.Show("人工指定策略下，请务必在上方填写【出库库位】！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 调用原本老版本的单库位出库方法
                bool success = wmsService.OutStock(goodsCode, qty, locCode);
                if (success)
                    MessageBox.Show("人工指定出库成功！并已呼叫AGV。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("出库失败，请检查该库位库存是否充足！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 场景 B：智能出库模式 ---
            // 1. 获取智能拣货建议 (先不扣库存，只是算一算)
            var adviceList = wmsService.GetOutboundPickAdvice(goodsCode, qty, selectedStrategy);

            int totalAvailable = adviceList.Sum(x => x.Qty);
            if (adviceList.Count == 0 || totalAvailable < qty)
            {
                MessageBox.Show($"库存严重不足！\n当前全仓可用总数仅剩: {totalAvailable} 个。", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. 拼接示信息--操作员
            string strategyName = cmbStrategy.Text;
            string adviceMsg = $"系统根据【{strategyName}】为您生成了最佳拣货路径：\n\n";

            foreach (var pick in adviceList)
            {
                adviceMsg += $"👉 去库位【{pick.LocationCode}】取出 {pick.Qty} 个 (批次:{pick.BatchNo})\n";
            }
            adviceMsg += "\n是否确认立即出库，并指派 AGV 小车前往这些库位取货？";

            // 3. 弹窗询问
            var result = MessageBox.Show(adviceMsg, "智能出库确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                // 4. 用户点赞同意，正式执行数据库扣减！
                string autoOrderNo = "AUTO-OUT-" + DateTime.Now.ToString("HHmmss");
                bool isSuccess = wmsService.SmartOutStock(goodsCode, qty, autoOrderNo, selectedStrategy);

                if (isSuccess)
                {
                    MessageBox.Show("🎉 智能出库执行成功！相关 AGV 任务已生成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 清空输入框，方便下一次扫码
                    txtGoodsCode.Text = "";
                    txtQty.Text = "";
                    txtLocCode.Text = "";
                }
                else
                {
                    MessageBox.Show("出库执行失败，可能由于并发导致库存变更，请重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



        }

        // 模拟扫码枪接收事件
        private void txtScanner_KeyDown(object sender, KeyEventArgs e)
        {
            // 扫码枪在扫完码后，会在极短的时间内自动发送一个“回车键 (Enter)”指令
            if (e.KeyCode == Keys.Enter)
            {
                string scannedCode = txtScanner.Text.Trim();

                if (!string.IsNullOrEmpty(scannedCode))
                {
                    // 1.模拟扫码枪物理音效
                    System.Media.SystemSounds.Beep.Play();
                    Utils.VoiceHelper.Speak("扫码成功");

                    // 2. 把扫到的条码内容，瞬间填入到下面的“商品编码”框里
                    txtGoodsCode.Text = scannedCode;

                    // 3. 自动清空扫码框，时刻准备扫描下一个箱子
                    txtScanner.Clear();

                    // 4.人机交互：把光标直接跳到“出库数量”框，不用鼠标，直接敲数字就能提交！
                    txtQty.Focus();
                }
            }
        }

    }


}
