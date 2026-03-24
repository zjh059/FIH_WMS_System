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
    public partial class BaseDataForm : Form
    {
        private WmsService wms = new WmsService();

        public BaseDataForm()
        {
            InitializeComponent();
        }

        private void BaseDataForm_Load(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void LoadGoodsData()
        {
            // 加载物料档案到表格
            dgvGoods.DataSource = null;
            dgvGoods.DataSource = wms.GetAllGoods();
            dgvGoods.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // 单条新增物料档案
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("物料编码和物料名称是必填项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 调用大脑新增档案
            bool success = wms.AddNewGoods(code, name, txtSpec.Text.Trim(), txtCategory.Text.Trim());

            if (success)
            {
                Utils.VoiceHelper.Speak("档案建立成功");
                MessageBox.Show($"成功！物料【{code}】已加入系统基础档案库。\n现在您可以去进行入库操作了。", "建档成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 清空输入框并刷新表格
                txtCode.Clear(); txtName.Clear(); txtSpec.Clear(); txtCategory.Clear();
                LoadGoodsData();
            }
            else
            {
                MessageBox.Show($"失败！系统中已经存在编码为【{code}】的物料，不可重复建档。", "编码冲突", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 执行 Excel 批量导入
        // ==========================================
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            // 1. 弹出文件选择框，让管理员选 Excel 表格
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel 文件|*.xlsx;*.xls";
            ofd.Title = "请选择包含【物料基础数据】的 Excel 文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 2. 呼叫工具类，解析 Excel 内容
                    var importList = Utils.ExcelHelper.ImportGoodsFromExcel(ofd.FileName);

                    if (importList.Count == 0)
                    {
                        MessageBox.Show("没有读取到任何有效数据！\n请确保：\n第一列是【编码】，第二列是【名称】。", "格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 3. 呼叫大脑，将几百上千条数据一次性灌入数据库！
                    var result = wms.BatchAddGoods(importList);

                    // 4. 完美收尾：播报战绩并刷新界面
                    Utils.VoiceHelper.Speak($"批量导入完成，成功导入 {result.successCount} 条，跳过重复 {result.failCount} 条。");

                    MessageBox.Show($"📊 批量导入战报！\n\n✅ 成功新增物料: {result.successCount} 种\n⚠️ 跳过重复物料: {result.failCount} 种", "初始化完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadGoodsData(); // 瞬间刷新下方的数据表格，让你看到战果！
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入失败！请检查 Excel 文件格式是否符合要求，或该文件是否正在被另一个程序打开占用。\n\n底报信息：{ex.Message}", "系统异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



    }
}