using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// 盘点窗体 CheckStockForm
    /// 仓库在实际运营中，经常会出现“电脑上显示有100个，去货架上一数只有98个”的情况（可能因为损耗、错发等）。
    /// 盘点的作用就是：以实物数量为准，强制修正系统数量，并自动记一笔“盘盈（多出来了）”或“盘亏（少了）”的流水账 。
    /// </summary>
    public partial class CheckStockForm : Form
    {
        public string InputGoodsCode { get; set; }
        public string InputLocCode { get; set; }
        public string InputBatchNo { get; set; }
        public int InputPhysicalQty { get; set; }

        public CheckStockForm()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputLocCode = txtLocCode.Text.Trim();
                InputBatchNo = txtBatchNo.Text.Trim();
                InputPhysicalQty = int.Parse(txtPhysicalQty.Text.Trim());

                if (string.IsNullOrEmpty(InputGoodsCode) || string.IsNullOrEmpty(InputLocCode) || string.IsNullOrEmpty(InputBatchNo))
                {
                    MessageBox.Show("警告：商品编码、库位、批次号都必须填写才能精准锁定盘点目标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (InputPhysicalQty < 0)
                {
                    MessageBox.Show("警告：实盘数量不能小于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：实盘数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}