using System;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class BaseDataForm : Form
    {
        private WmsService wms = new WmsService();

        public BaseDataForm()
        {
            InitializeComponent();
            this.Load += BaseDataForm_Load;
        }

        private void BaseDataForm_Load(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void LoadGoodsData()
        {
            // 👇 每次刷新前先强行清空旧数据和旧列，防止重复拼接！
            dgvData.DataSource = null;
            dgvData.Columns.Clear();

            dgvData.DataSource = wms.GetAllGoods();
        }

        // 单条新增物料档案
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            string name = txtName.Text.Trim();
            string spec = txtSpec.Text.Trim();
            string category = txtCategory.Text.Trim();

            // 👇 现在可以读取真实的品牌输入框了！
            string brand = txtBrand.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("物料编码和物料名称是必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 👇 真正地把 brand 参数传给大脑！
            bool success = wms.AddNewGoods(code, name, spec, category, brand);

            if (success)
            {
                Utils.VoiceHelper.Speak("档案建立成功");
                MessageBox.Show($"成功！物料【{code}】已加入系统基础档案库。\n现在您可以去进行入库操作了。", "建档成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 清空输入框并刷新表格
                txtCode.Clear(); txtName.Clear(); txtSpec.Clear(); txtCategory.Clear(); txtBrand.Clear();
                LoadGoodsData();
            }
            else
            {
                MessageBox.Show($"失败！系统中已经存在编码为【{code}】的物料，不可重复建档。", "编码冲突", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}