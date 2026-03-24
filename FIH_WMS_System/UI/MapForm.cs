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
                btn.FlatAppearance.BorderSize = 1; // 加上一个细细的灰色边框，更有质感
                btn.FlatAppearance.BorderColor = Color.LightGray; // 边框颜色

                // 使用专门适合在文本前面放圆点的字体和大小
                btn.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
                btn.Margin = new Padding(12);
                btn.Padding = new Padding(10); // 加上内边距，让内容更呼吸

                // 核心：不需要 TextImageRelation，直接让文字整体居中
                btn.TextAlign = ContentAlignment.MiddleCenter;

                bool isUsed = loc.IsUsed;
                int qty = loc.Qty != null ? (int)loc.Qty : 0;
                string goodsName = loc.GoodsName;

                // 定义 Unicode 圆点字符
                string dot = "● ";

                if (!isUsed)
                {
                    // --- 状态 1：空闲待命 (整块浅绿色预警) ---
                    btn.BackColor = Color.FromArgb(220, 245, 230); // 浅绿底色
                    btn.ForeColor = Color.FromArgb(0, 130, 50);    // 深绿文字

                    btn.Text = $"{dot}{loc.Code}\n\n空闲待命";
                    toolTip.SetToolTip(btn, "当前货架完全空闲，可用于入库分配。");
                }
                else
                {
                    if (qty < 50 && qty > 0)
                    {
                        // --- 状态 2：零星碎片 (整块浅黄色预警) ---
                        btn.BackColor = Color.FromArgb(255, 245, 220); // 浅黄底色
                        btn.ForeColor = Color.FromArgb(180, 100, 0);   // 暗橙文字

                        btn.Text = $"{dot}{loc.Code}\n\n零星碎片\n{qty} 个";
                        string info = $"物料: {goodsName}\n数量: {qty}\n建议: 可使用智能引擎进行库位合并。";
                        toolTip.SetToolTip(btn, info);
                    }
                    else
                    {
                        // --- 状态 3：已占用 (整块浅红色预警) ---
                        btn.BackColor = Color.FromArgb(255, 230, 230); // 浅红底色
                        btn.ForeColor = Color.FromArgb(180, 20, 20);   // 深红文字

                        btn.Text = $"{dot}{loc.Code}\n\n已占用\n{qty} 个";
                        string info = $"物料: {goodsName}\n当前总数量: {qty}";
                        toolTip.SetToolTip(btn, info);
                    }
                }

                btn.Click += (s, e) =>
                {
                    MessageBox.Show($"库位编码: {loc.Code}\n存放物料: {(string.IsNullOrEmpty(goodsName) ? "无" : goodsName)}\n数量: {qty}", "库位详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
    }
}