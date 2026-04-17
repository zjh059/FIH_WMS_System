using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class InboundWaveConsolidationForm : Form
    {
        private WmsService wms = new WmsService();

        public InboundWaveConsolidationForm()
        {
            InitializeComponent();
        }

        private void InboundWaveConsolidationForm_Load(object sender, EventArgs e)
        {
            LoadPendingOrders();
        }

        private void LoadPendingOrders()
        {
            // 获取所有未完成的入库单
            var orders = wms.GetPendingInboundOrders();
            clbOrders.Items.Clear();
            foreach (var o in orders)
            {
                clbOrders.Items.Add(o);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 获取用户勾选的所有单号
            var selectedOrders = clbOrders.CheckedItems.Cast<string>().ToList();

            if (selectedOrders.Count < 2)
            {
                MessageBox.Show("请至少勾选两个采购单进行合并！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 生成宏观波次号
            string waveName = "WAVE-IN-" + DateTime.Now.ToString("HHmmss");

            // 呼叫大脑进行合并
            if (wms.ConsolidateInboundOrders(selectedOrders, waveName))
            {
                Utils.VoiceHelper.Speak("入库波次合并成功");
                MessageBox.Show($"🎉 波次【{waveName}】已创建！\n\n这些单据的物料在执行入库时，将被智能引擎自动分配到相近的货架区域。", "合并成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("波次合并失败，请检查数据库连接状态！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}