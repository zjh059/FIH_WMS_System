using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FIH_WMS_System.Models;

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
            DrawMap();
        }

        // ==========================================
        // 核心：终极重绘，使用规整的纯文本圆点指示器
        // ==========================================
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

                // 👇 核心新增：生成右键菜单 (ContextMenuStrip)
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem lockItem = new ToolStripMenuItem(status == 0 ? "🔒 设为锁定 (停用此货架)" : "✅ 解除锁定 (恢复正常运作)");
                lockItem.Click += (s, e) =>
                {
                    // 呼叫大脑修改数据库状态，然后立即刷新地图！
                    wms.ToggleLocationStatus(loc.Code, status == 0 ? 1 : 0);
                    DrawMap();
                };
                cms.Items.Add(lockItem);

                // 将做好的菜单挂载给这个货架按钮
                btn.ContextMenuStrip = cms;

                flowLayoutPanel1.Controls.Add(btn);
            }



        }
    }
}