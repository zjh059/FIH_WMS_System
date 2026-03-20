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
    public partial class InStockForm : Form
    {
        public InStockForm()
        {
            InitializeComponent();
        }


        // 1. 定义三个“公开的口袋”，用来装用户输入的数据，方便主窗口一会来拿
        public string InputGoodsCode { get; set; }
        public int InputQty { get; set; }
        public string InputLocCode { get; set; }


        /*        private void btnConfirm_Click(object sender, EventArgs e)
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
                        MessageBox.Show("警告：入库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }*/


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputLocCode = txtLocCode.Text.Trim();
                InputQty = int.Parse(txtQty.Text);

                // 校验必填项
                if (string.IsNullOrEmpty(InputGoodsCode))
                {
                    MessageBox.Show("商品编码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 👇 【核心改动：触发智能分配】
                // 如果用户偷懒没填库位，我们就召唤 WmsService 帮他找！
                if (string.IsNullOrEmpty(InputLocCode))
                {
                    Services.WmsService wms = new Services.WmsService();
                    string autoLoc = wms.GetRecommendLocation(InputGoodsCode);

                    if (string.IsNullOrEmpty(autoLoc))
                    {
                        MessageBox.Show("系统警告：仓库已满，无法智能分配库位！请先清理库存。", "爆仓警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 直接中止入库
                    }
                    else
                    {
                        // 帮用户把找到的库位填进口袋里
                        InputLocCode = autoLoc;
                        MessageBox.Show($"触发智能分配！\n系统已自动将商品分配至库位：【{autoLoc}】", "智能引擎", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：入库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }


}
