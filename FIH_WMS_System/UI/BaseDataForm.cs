using System;
using System.Windows.Forms;
using FIH_WMS_System.Services;

using Dapper;
using Microsoft.Data.SqlClient;

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
            LoadGoodsData();    //  原有的物料加载
            LoadBomData();      //  新增：加载BOM数据
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


        /// <summary>
        /// 加载 BOM 关系树展示在表格中
        /// </summary>
        private void LoadBomData()
        {
            try
            {
                using (var db = new SqlConnection(Utils.DbHelper.ConnectionString))
                {
                    // 使用连表查询，把父编码和子编码对应的“中文名”也查出来，体验更好
                    string sql = @"
                SELECT 
                    b.Id AS 'BOM记录编号',
                    b.ParentGoodsCode AS '成品编码',
                    p.Name AS '成品名称',
                    b.ChildGoodsCode AS '子件物料编码',
                    c.Name AS '子件名称',
                    b.RequiredQty AS '单机消耗量',
                    b.CreateTime AS '绑定时间'
                FROM ProductBOM b
                LEFT JOIN Goods p ON b.ParentGoodsCode = p.Code
                LEFT JOIN Goods c ON b.ChildGoodsCode = c.Code
                ORDER BY b.ParentGoodsCode, b.ChildGoodsCode";

                    var list = db.Query(sql).ToList();
                    dgvBom.DataSource = list;

                    // 挂载右键删除菜单
                    AttachBomContextMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载 BOM 数据失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 绑定 BOM 关系按钮点击
        /// </summary>
        private void btnBomAdd_Click(object sender, EventArgs e)
        {
            string parent = txtBomParent.Text.Trim();
            string child = txtBomChild.Text.Trim();
            decimal qty = numBomQty.Value;

            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(child))
            {
                MessageBox.Show("父成品编码和子件原材料编码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (parent == child)
            {
                MessageBox.Show("父件和子件不能是同一个物料！（会引起死循环）", "逻辑错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var db = new SqlConnection(Utils.DbHelper.ConnectionString))
            {
                // 防呆1：检查这两个物料在基础档案里存不存在？
                int pExists = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Goods WHERE Code=@c", new { c = parent });
                int cExists = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Goods WHERE Code=@c", new { c = child });
                if (pExists == 0 || cExists == 0)
                {
                    MessageBox.Show("绑定失败：系统中不存在您输入的成品或子件物料编码！请先在【物料档案】中建立档案。", "档案缺失", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // 防呆2：检查是否已经绑定过该组合，如果绑定过就更新数量，没绑定过就插入
                var existBomId = db.QueryFirstOrDefault<int?>("SELECT Id FROM ProductBOM WHERE ParentGoodsCode=@p AND ChildGoodsCode=@c", new { p = parent, c = child });

                if (existBomId != null)
                {
                    db.Execute("UPDATE ProductBOM SET RequiredQty=@q WHERE Id=@id", new { q = qty, id = existBomId });
                    MessageBox.Show("检测到该 BOM 关系已存在，已为您更新单机消耗量。", "更新成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    db.Execute("INSERT INTO ProductBOM (ParentGoodsCode, ChildGoodsCode, RequiredQty, CreateTime) VALUES (@p, @c, @q, GETDATE())",
                        new { p = parent, c = child, q = qty });
                    MessageBox.Show("BOM 节点绑定成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadBomData(); // 刷新表格
        }

        /// <summary>
        /// 为 BOM 表格挂载右键删除菜单
        /// </summary>
        private void AttachBomContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            ToolStripMenuItem delItem = new ToolStripMenuItem("✂️ 解除并删除该 BOM 节点");
            delItem.ForeColor = System.Drawing.Color.Red;

            delItem.Click += (s, e) =>
            {
                if (dgvBom.SelectedRows.Count > 0)
                {
                    int bomId = Convert.ToInt32(dgvBom.SelectedRows[0].Cells["BOM记录编号"].Value);
                    string pName = dgvBom.SelectedRows[0].Cells["成品编码"].Value.ToString();
                    string cName = dgvBom.SelectedRows[0].Cells["子件物料编码"].Value.ToString();

                    if (MessageBox.Show($"确定要解除【{pName}】对【{cName}】的依赖关系吗？\n删除后波次运算将不再计算该子件。", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (var db = new SqlConnection(Utils.DbHelper.ConnectionString))
                        {
                            db.Execute("DELETE FROM ProductBOM WHERE Id=@id", new { id = bomId });
                        }
                        LoadBomData();
                    }
                }
                else
                {
                    MessageBox.Show("请先点击行头最左侧的箭头选中一整行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            cms.Items.Add(delItem);
            dgvBom.ContextMenuStrip = cms;
        }



    }
}