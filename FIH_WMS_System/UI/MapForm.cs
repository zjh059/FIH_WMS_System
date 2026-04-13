using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FIH_WMS_System.Models;

using Dapper;
using Microsoft.Data.SqlClient;

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// 2D 库位地图数据源 (可视化监控屏 - 终极美化版)
    /// </summary>
    public partial class MapForm : Form
    {
        private Services.WmsService wms = new Services.WmsService();
        private ToolTip toolTip = new ToolTip(); // 悬停提示

        public MapForm()
        {
            InitializeComponent();
            InitializeToolTip(); // 仅初始化 ToolTip 即可，无需 ImageList
        }

        private void InitializeToolTip()
        {
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 100;
            toolTip.ToolTipTitle = "📦 库位详情";
            toolTip.ToolTipIcon = ToolTipIcon.Info;
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            // 权限判断：如果不是管理员，隐藏顶部的扩建按钮
            if (Program.CurrentRole == "操作员")
            {
                panelTop.Visible = false;
            }
            else
            {
                // 新增：为主面板背景（空白处）绑定右键菜单，仅管理员可用
                ContextMenuStrip bgMenu = new ContextMenuStrip();

                ToolStripMenuItem addSingleItem = new ToolStripMenuItem("➕ 新增单库位");
                addSingleItem.Click += btnAddSingle_Click; // 直接复用现成的方法！

                ToolStripMenuItem batchAddItem = new ToolStripMenuItem("🚀 批量扩建库位");
                batchAddItem.Click += btnBatchAdd_Click;   // 直接复用现成的方法！

                bgMenu.Items.Add(addSingleItem);
                bgMenu.Items.Add(batchAddItem);

                // 将菜单挂载到装载货架的那个大背景容器上
                flowLayoutPanel1.ContextMenuStrip = bgMenu;
            }
            DrawMap();
        }

        //重绘使用规整的纯文本圆点指示器
        private void DrawMap()
        {
            // 开启双缓冲，防止闪烁 (这一步非常关键)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            flowLayoutPanel1.Controls.Clear();

            var mapData = wms.GetLocationMapData();

            foreach (var loc in mapData)
            {
                Button btn = new Button();
                btn.Size = new Size(180, 140);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.LightGray;
                btn.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
                btn.Margin = new Padding(12);
                btn.Padding = new Padding(10);
                btn.TextAlign = ContentAlignment.MiddleCenter;

                bool isUsed = loc.IsUsed;
                int status = loc.Status != null ? (int)loc.Status : 0; // 提取物理状态
                int qty = loc.Qty != null ? (int)loc.Qty : 0;
                string goodsName = loc.GoodsName;
                string dot = "● ";

                // 核心视觉渲染：优先判断是否被锁定！
                if (status == 1)
                {
                    // --- 状态 4：锁定停用 (整块高级灰黑预警) ---
                    btn.BackColor = Color.FromArgb(200, 200, 200);
                    btn.ForeColor = Color.FromArgb(80, 80, 80);
                    btn.Text = $"🔒 {loc.Code}\n\n已锁定 (维修/停用)";
                    toolTip.SetToolTip(btn, "该库位已被管理员手动锁定，停止分配。");
                }
                else if (!isUsed)
                {
                    // --- 状态 1：空闲待命 ---
                    btn.BackColor = Color.FromArgb(220, 245, 230);
                    btn.ForeColor = Color.FromArgb(0, 130, 50);
                    btn.Text = $"{dot}{loc.Code}\n\n空闲待命";
                    toolTip.SetToolTip(btn, "当前货架完全空闲，可用于入库分配。");
                }
                else
                {
                    // --- 状态 2/3：零星碎片与已占用 ---
                    if (qty < 50 && qty > 0)
                    {
                        btn.BackColor = Color.FromArgb(255, 245, 220);
                        btn.ForeColor = Color.FromArgb(180, 100, 0);
                        btn.Text = $"{dot}{loc.Code}\n\n零星碎片\n{qty} 个";
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(255, 230, 230);
                        btn.ForeColor = Color.FromArgb(180, 20, 20);
                        btn.Text = $"{dot}{loc.Code}\n\n已占用\n{qty} 个";
                    }
                    toolTip.SetToolTip(btn, $"物料: {goodsName}\n数量: {qty}");
                }

                // 左键点击：查看详情
                btn.Click += (s, e) =>
                {
                    MessageBox.Show($"库位编码: {loc.Code}\n物理状态: {(status == 1 ? "🔒 已锁定" : "✅ 正常")}\n存放物料: {(string.IsNullOrEmpty(goodsName) ? "无" : goodsName)}\n数量: {qty}", "库位详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                ////核心：生成右键菜单 (ContextMenuStrip)
                //ContextMenuStrip cms = new ContextMenuStrip();
                //ToolStripMenuItem lockItem = new ToolStripMenuItem(status == 0 ? "🔒 设为锁定 (停用此货架)" : "✅ 解除锁定 (恢复正常运作)");

                //if (Program.CurrentRole == "操作员")
                //{
                //    lockItem.Enabled = false; // 变灰不可点
                //    lockItem.ToolTipText = "权限不足：仅管理员可更改库位物理状态";
                //}
                //else
                //{
                //    // 只有管理员才绑定点击事件
                //    lockItem.Click += (s, ev) =>
                //    {
                //        // 呼叫大脑修改数据库状态，然后立即刷新地图！
                //        wms.ToggleLocationStatus(loc.Code, status == 0 ? 1 : 0);
                //        DrawMap();
                //    };
                //}

                //cms.Items.Add(lockItem);

                //// 将做好的菜单挂载给这个货架按钮
                //btn.ContextMenuStrip = cms;



                // 核心：生成右键菜单 (ContextMenuStrip)
                ContextMenuStrip cms = new ContextMenuStrip();

                if (Program.CurrentRole == "操作员")
                {
                    // 操作员只能看，没有操作权限
                    ToolStripMenuItem noRightItem = new ToolStripMenuItem("🚫 权限不足");
                    noRightItem.Enabled = false;
                    cms.Items.Add(noRightItem);
                }
                else
                {
                    // 1. 停用/启用
                    ToolStripMenuItem lockItem = new ToolStripMenuItem(status == 0 ? "🔒 设为锁定 (停用此货架)" : "✅ 解除锁定 (恢复运作)");
                    lockItem.Click += (s, ev) =>
                    {
                        wms.ToggleLocationStatus(loc.Code, status == 0 ? 1 : 0);
                        DrawMap();
                    };

                    // 2. 编辑基本信息
                    ToolStripMenuItem editItem = new ToolStripMenuItem("✏️ 编辑库位信息");
                    editItem.Click += (s, ev) =>
                    {
                        string curArea = "A区";
                        int curCap = 500;

                        using (var db = new Microsoft.Data.SqlClient.SqlConnection(Utils.DbHelper.ConnectionString))
                        {
                            // 修复：显式告诉 Dapper 我要查的模型类型，或者只查单个字段，阻断 dynamic 传染
                            var locEntity = Dapper.SqlMapper.QueryFirstOrDefault<Models.Location>(db, "SELECT Area, MaxCapacity FROM Location WHERE Code=@c", new { c = loc.Code });
                            if (locEntity != null)
                            {
                                curArea = locEntity.Area;
                                curCap = locEntity.MaxCapacity;
                            }
                        }

                        LocationEditForm frm = new LocationEditForm(LocationEditMode.Edit, loc.Code, curArea, curCap);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            using (var db = new Microsoft.Data.SqlClient.SqlConnection(Utils.DbHelper.ConnectionString))
                            {
                                // 新增：防重校验核心逻辑
                                // 如果用户修改了库位编码，需要检查新编码是否已经存在
                                if (frm.InputCode != loc.Code)
                                {
                                    int exists = Dapper.SqlMapper.QueryFirstOrDefault<int>(db, "SELECT COUNT(1) FROM Location WHERE Code=@c", new { c = frm.InputCode });
                                    if (exists > 0)
                                    {
                                        MessageBox.Show($"修改失败！库位编码【{frm.InputCode}】已存在，不能重复录入。", "冲突警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return; // 拦截并终止后续操作
                                    }
                                }

                                // 校验通过，执行更新
                                Dapper.SqlMapper.Execute(db, "UPDATE Location SET Code=@newC, Area=@a, MaxCapacity=@cap WHERE Code=@oldC",
                                    new { newC = frm.InputCode, a = frm.InputArea, cap = frm.InputCapacity, oldC = loc.Code });
                            }
                            DrawMap();
                        }
                    };

                    // 3. 删除库位 (极其严格的安全防呆)
                    ToolStripMenuItem delItem = new ToolStripMenuItem("🗑️ 删除此库位");
                    delItem.ForeColor = Color.Red;
                    delItem.Click += (s, ev) =>
                    {
                        if (qty > 0)
                        {
                            MessageBox.Show("删除失败！该库位上还有存放的物料，请先移库清空后再删除。", "安全防呆", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                        if (MessageBox.Show($"警告：确定要永久删除库位【{loc.Code}】吗？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            using (var db = new SqlConnection(Utils.DbHelper.ConnectionString))
                            {
                                db.Execute("DELETE FROM Location WHERE Code=@c", new { c = loc.Code });
                            }
                            DrawMap();
                        }
                    };

                    cms.Items.Add(lockItem);
                    cms.Items.Add(new ToolStripSeparator()); // 分割线
                    cms.Items.Add(editItem);
                    cms.Items.Add(delItem);
                }

                // 将做好的菜单挂载给货架按钮
                btn.ContextMenuStrip = cms;


                flowLayoutPanel1.Controls.Add(btn);
            }



        }



        // 单条新增
        private void btnAddSingle_Click(object sender, EventArgs e)
        {
            LocationEditForm frm = new LocationEditForm(LocationEditMode.SingleAdd, "A-01-");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                using (var db = new Microsoft.Data.SqlClient.SqlConnection(Utils.DbHelper.ConnectionString))
                {
                    int exists = Dapper.SqlMapper.QueryFirstOrDefault<int>(db, "SELECT COUNT(1) FROM Location WHERE Code = @c", new { c = frm.InputCode });
                    if (exists > 0) { MessageBox.Show("库位编码已存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    Dapper.SqlMapper.Execute(db, "INSERT INTO Location (Code, Area, IsUsed, Status, PosX, PosY, MaxCapacity) VALUES (@c, @a, 0, 0, 0, 0, @cap)",
                        new { c = frm.InputCode, a = frm.InputArea, cap = frm.InputCapacity });
                }
                DrawMap();
            }
        }

        // 批量扩建
        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            LocationEditForm frm = new LocationEditForm(LocationEditMode.BatchAdd, "A-01-");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int successCount = 0;
                int currentIndex = 1; // 从 1 开始往后找

                using (var db = new Microsoft.Data.SqlClient.SqlConnection(Utils.DbHelper.ConnectionString))
                {
                    db.Open();
                    // 只要还没生成够用户要求的数量，就一直往后找可用的编号
                    while (successCount < frm.InputBatchCount)
                    {
                        string newCode = $"{frm.InputCode}{currentIndex:D2}";
                        int exists = Dapper.SqlMapper.QueryFirstOrDefault<int>(db, "SELECT COUNT(1) FROM Location WHERE Code = @c", new { c = newCode });

                        if (exists == 0) // 若不存在，说明这个编号可用，执行插入
                        {
                            Dapper.SqlMapper.Execute(db, "INSERT INTO Location (Code, Area, IsUsed, Status, PosX, PosY, MaxCapacity) VALUES (@c, @a, 0, 0, 0, 0, @cap)",
                                new { c = newCode, a = frm.InputArea, cap = frm.InputCapacity });
                            successCount++;
                        }

                        currentIndex++; // 索引自增（如果存在就跳过，如果成功插入也往下走）

                        if (currentIndex > 9999) break; // 安全锁：防止前缀不规范导致死循环崩溃
                    }
                }
                MessageBox.Show($"扩建完成！成功顺延新增 {successCount} 个库位。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DrawMap();
            }
        }



    }
}