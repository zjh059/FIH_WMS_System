using System;
using System.Drawing;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class WarningForm : Form
    {
        private WmsService wms = new WmsService();

        public WarningForm()
        {
            InitializeComponent();
        }

        private void WarningForm_Load(object sender, EventArgs e)
        {
            // 呼叫大脑获取预警数据
            var warnings = wms.GetLowStockWarnings();
            dgv.DataSource = warnings;

            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["GoodsCode"].HeaderText = "物料编码";
                dgv.Columns["GoodsName"].HeaderText = "物料名称";
                dgv.Columns["SafeQty"].HeaderText = "安全库存线";
                dgv.Columns["CurrentTotalQty"].HeaderText = "当前可用总数";

                // 触目惊心的标红处理，提醒管理员严重性
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.DefaultCellStyle.Font = new Font("微软雅黑", 10, FontStyle.Bold);
                }
            }
        }

        private void BtnAutoOrder_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0) return;

            if (MessageBox.Show("是否根据当前系统缺口，自动生成采购入库单据？\n\n单据生成后将流转至采购部门。", "自动补单确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool success = wms.AutoGeneratePurchaseOrder();
                if (success)
                {
                    Utils.VoiceHelper.Speak("缺料危机解除，采购补货单已成功下发。");
                    MessageBox.Show("🎉 采购补货单（入库计划）已成功下发至系统订单库！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // 完成后自动关闭弹窗
                }
                else
                {
                    MessageBox.Show("单据生成失败，请检查数据库连接状态。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}