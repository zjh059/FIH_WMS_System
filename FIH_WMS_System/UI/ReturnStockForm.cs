using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class ReturnStockForm : Form
    {
        private WmsService wms = new WmsService();

        public ReturnStockForm()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string code = txtGoodsCode.Text.Trim();
            int qty = (int)numQty.Value;

            //获取输入的关联工单号
            string relatedOrder = txtOrder.Text.Trim();

            if (string.IsNullOrEmpty(code) || qty <= 0)
            {
                MessageBox.Show("请输入正确的物料编码和数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 呼叫后台算法，并把 relatedOrder 传进去
            string targetLoc = wms.SmartReturnMaterial(code, qty, relatedOrder);

            if (targetLoc == "ERROR_GOODS")
            {
                MessageBox.Show("系统基础档案中不存在该物料，拦截退料！", "拦截", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (targetLoc == "ERROR_FULL")
            {
                MessageBox.Show("仓库已满，无法接纳新的退料！", "拦截", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Utils.VoiceHelper.Speak("余料清点完毕，AGV小车正在前往产线接驳口");
                MessageBox.Show($"🎉 返料指令下发成功！\n系统已为您智能分配碎片合并库位：【{targetLoc}】\nAGV 小车已出发前往产线接驳口进行回收。", "调度成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // 成功后关闭窗口
            }
        }
    }
}