using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FIH_WMS_System.Models;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class AddPurchaseOrderForm : Form
    {
        private WmsService wms = new WmsService();
        // 用于暂时存放用户添加的物料清单
        private List<WmsOrderDetail> tempDetails = new List<WmsOrderDetail>();

        public AddPurchaseOrderForm()
        {
            InitializeComponent();
        }

        private void AddPurchaseOrderForm_Load(object sender, EventArgs e)
        {
            LoadGoods();
        }

        private void LoadGoods()
        {
            // 从数据库拉取物料给下拉框
            var dt = wms.GetAllGoods();
            cmbGoods.DataSource = dt;
            cmbGoods.DisplayMember = "物料名称";
            cmbGoods.ValueMember = "物料编码";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cmbGoods.SelectedValue == null) return;

            string code = cmbGoods.SelectedValue.ToString();
            string name = cmbGoods.Text;
            int qty = (int)numQty.Value;

            // 检查清单中是否已经有这个物料了，如果有就直接增加数量
            var existItem = tempDetails.FirstOrDefault(x => x.GoodsCode == code);
            if (existItem != null)
            {
                existItem.PlanQty += qty;
            }
            else
            {
                tempDetails.Add(new WmsOrderDetail { GoodsCode = code, PlanQty = qty });
            }

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvDetails.DataSource = null;
            dgvDetails.DataSource = tempDetails.Select(x => new {
                物料编码 = x.GoodsCode,
                计划采购数量 = x.PlanQty
            }).ToList();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (tempDetails.Count == 0)
            {
                MessageBox.Show("请先添加至少一条物料明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool ok = wms.CreateManualPurchaseOrder(tempDetails);
            if (ok)
            {
                Utils.VoiceHelper.Speak("手工采购单创建成功");
                MessageBox.Show("🎉 采购单据生成成功！已下发至管理中心。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // 关闭窗口
            }
            else
            {
                MessageBox.Show("生成失败，请检查数据库连接状态！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}