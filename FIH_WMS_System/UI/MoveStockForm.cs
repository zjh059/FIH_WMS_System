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
    public partial class MoveStockForm : Form
    {
        // 四个“口袋”，装移库需要的关键信息
        public string InputGoodsCode { get; set; }
        public string InputFromLoc { get; set; }
        public string InputToLoc { get; set; }
        public int InputQty { get; set; }

        public MoveStockForm()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputFromLoc = txtFromLoc.Text.Trim();
                InputToLoc = txtToLoc.Text.Trim();
                InputQty = int.Parse(txtQty.Text.Trim());

                // 简单的必填校验
                if (string.IsNullOrEmpty(InputGoodsCode) || string.IsNullOrEmpty(InputFromLoc) || string.IsNullOrEmpty(InputToLoc))
                {
                    MessageBox.Show("警告：商品编码和新旧库位都必须填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (InputFromLoc == InputToLoc)
                {
                    MessageBox.Show("警告：源库位和目标库位不能相同！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：移库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}