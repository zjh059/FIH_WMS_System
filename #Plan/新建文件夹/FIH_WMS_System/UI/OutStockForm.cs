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

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// 入库窗体界面
    /// </summary>
    public partial class OutStockForm : Form
    {
        public OutStockForm()
        {
            InitializeComponent();
        }


        // 1. 定义三个“公开的口袋”，用来装用户输入的数据，方便主窗口一会来拿
        public string InputGoodsCode { get; set; }
        public int InputQty { get; set; }
        public string InputLocCode { get; set; }


        // 确认按钮的点击事件
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                // 2. 把文本框里的字，装进我们的口袋里
                InputGoodsCode = txtGoodsCode.Text;
                InputLocCode = txtLocCode.Text;

                // 数量必须是数字，所以用 int.Parse 转换一下
                InputQty = int.Parse(txtQty.Text);

                // 3. 极其重要的一句！告诉系统：“这个弹窗的任务圆满完成 (OK)！”
                // 这句话执行后，弹窗会自动关闭，并给主窗口发个 OK 的信号。
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：出库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
