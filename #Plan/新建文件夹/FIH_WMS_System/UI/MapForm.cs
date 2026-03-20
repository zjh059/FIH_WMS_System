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
    /// 2D 库位地图数据源 (可视化屏)
    ///服务层增加“扫描仓库”的大脑
    ///我们需要一次性把所有货架、货架上有没有东西、有什么东西全部查出来。
    /// </summary>
    public partial class MapForm : Form
    {
        private Services.WmsService wms = new Services.WmsService();

        public MapForm()
        {
            InitializeComponent();
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            DrawMap(); // 窗口一加载，就开始画图！
        }

        private void DrawMap()
        {
            // 1. 先清空画板
            flowLayoutPanel1.Controls.Clear();

            // 2. 拿到大脑传来的全仓数据
            var mapData = wms.GetLocationMapData();

            // 3. 循环遍历每一个库位，动态生成一个个“物理货架”方块
            foreach (var loc in mapData)
            {
                // 动态实例化一个按钮当做方块
                Button btn = new Button();

                //btn.Size = new Size(160, 120); // 方块的长宽
                btn.Size = new Size(180, 140); // 微调(方块的长宽)

                btn.FlatStyle = FlatStyle.Flat; // 扁平化风格
                btn.Font = new Font("微软雅黑", 10F, FontStyle.Bold);

                btn.Margin = new Padding(10); // 给方块之间留点间隙

                bool isUsed = loc.IsUsed;

                if (isUsed)
                {
                    // 有货：经典科技蓝 (去掉了Emoji，使用 \r\n 标准换行)
                    btn.BackColor = Color.FromArgb(80, 160, 255);
                    btn.ForeColor = Color.White;
                    btn.Text = $"{loc.Code}\r\n\r\n物料: {loc.GoodsName}\r\n数量: {loc.Qty}";
                }
                else
                {
                    // 空闲：浅绿色
                    btn.BackColor = Color.FromArgb(238, 250, 240);
                    btn.ForeColor = Color.FromArgb(10, 160, 80);
                    btn.Text = $"{loc.Code}\r\n\r\n[ 空闲待命 ]";
                }
                // 把画好的格子塞进流式布局容器里，它会自动帮我们排版！
                flowLayoutPanel1.Controls.Add(btn);
            }
        }
    }
}
