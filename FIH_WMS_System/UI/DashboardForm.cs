using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sunny.UI;

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// “各物料库存量统计”的柱状图
    /// DashboardForm (看板窗体)
    /// </summary>
    public partial class DashboardForm : Form
    {
        private Services.WmsService wms = new Services.WmsService();

        public DashboardForm()
        {
            InitializeComponent();
        }
private void DashboardForm_Load(object sender, EventArgs e)
        {
            // ==========================================
            // 模块一：柱状图 (防遮挡)
            // ==========================================
            var rawChartData = wms.GetStockChartData();
            
            // 只取库存量最大的前 15 种物料进行展示，防止数据量爆炸导致柱子细
            var chartData = rawChartData.OrderByDescending(x => x.Value).Take(15).ToList();

            UIBarOption option = new UIBarOption();
            option.Title = new UITitle();
            option.Title.Text = "FIH 全仓物料总库存看板 (Top 15)";
            option.Title.SubText = "实时数据统计";

            // 【核心优化 2】：增加四个方向的内边距，绝对防止 X/Y 轴文字被控件边缘切掉！
            option.Grid.Left = 80;   // 给左侧 Y 轴数字留空间
            option.Grid.Right = 30;  // 右侧防贴边
            option.Grid.Bottom = 80; // 给底部 X 轴的物料长名字留足空间

            option.XAxis.Name = "物料名称";
            option.YAxis.Name = "数量";

            UIBarSeries series = new UIBarSeries();
            series.Name = "实时库存";
            
            foreach (var item in chartData)
            {
                // 如果名字太长，可以只截取前 6 个字防止重叠
                option.XAxis.Data.Add(item.Key); 
                series.AddData(item.Value);
            }
            
            option.Series.Add(series);
            uiBarChart1.SetOption(option);

            // ==========================================
            // 模块二：库位占比饼图 (由于固定只有两项，不会溢出)
            // ==========================================
            var locData = wms.GetLocationUsage();

            UIPieOption optionPie = new UIPieOption();
            optionPie.Title = new UITitle();
            optionPie.Title.Text = "仓库库位占用率分析";
            optionPie.Title.SubText = "空闲 vs 已占用";

            optionPie.Legend = new UILegend();
            optionPie.Legend.Orient = UIOrient.Vertical;
            optionPie.Legend.Left = UILeftAlignment.Left;

            UIPieSeries seriesPie = new UIPieSeries();
            seriesPie.Name = "库位状态";

            seriesPie.Label.Show = true;
            seriesPie.Label.Formatter = "{b} : {c} ({d}%)";

            foreach (var item in locData)
            {
                seriesPie.AddData(item.Key, item.Value);
                optionPie.Legend.AddData(item.Key);
            }

            optionPie.Series.Add(seriesPie);
            uiPieChart1.SetOption(optionPie);
        }



    }
}
