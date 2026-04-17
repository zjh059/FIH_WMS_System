using System;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class OrderCenterForm : Form
    {
        private WmsService wms = new WmsService();

        public OrderCenterForm()
        {
            InitializeComponent();
        }

        private void OrderCenterForm_Load(object sender, EventArgs e)
        {
            // 窗体加载时，自动查出所有订单
            dgvOrders.DataSource = wms.GetAllOrders();
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            // 当用户点击上面表格的某一行时，自动刷新下面的明细！
            if (dgvOrders.SelectedRows.Count > 0)
            {
                // 获取当前选中行中名为"单据编号"的单元格内容
                string orderNo = dgvOrders.SelectedRows[0].Cells["单据编号"].Value.ToString();

                // 根据主单号查询并绑定明细
                dgvDetails.DataSource = wms.GetOrderDetails(orderNo);
            }
        }

        //// 手工录入按钮点击事件
        //private void btnAddPurchaseOrder_Click(object sender, EventArgs e)
        //{
        //    // 弹出手工建单窗口
        //    AddPurchaseOrderForm form = new AddPurchaseOrderForm();

        //    if (form.ShowDialog() == DialogResult.OK)
        //    {
        //        // 如果子窗口返回 OK（说明保存成功），立即刷新当前页面的订单列表
        //        dgvOrders.DataSource = wms.GetAllOrders();

        //        // 提示一下用户
        //        // Utils.VoiceHelper.Speak("单据列表已更新");
        //    }
        //}


        // 手工录入按钮点击事件
        private void btnAddPurchaseOrder_Click(object sender, EventArgs e)
        {
            // 弹出手工建单窗口
            AddPurchaseOrderForm form = new AddPurchaseOrderForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                // 如果子窗口返回 OK（说明保存成功），立即刷新当前页面的订单列表
                dgvOrders.DataSource = wms.GetAllOrders();

                //提示一下用户
                Utils.VoiceHelper.Speak("单据列表已更新");
            }
        }

        // 波次合并按钮点击事件
        private void btnWaveConsolidate_Click(object sender, EventArgs e)
        {
            InboundWaveConsolidationForm form = new InboundWaveConsolidationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // 合并完成后，刷新一下订单列表（以便你能看到底层数据可能的变化）
                dgvOrders.DataSource = wms.GetAllOrders();
            }
        }


    }
}