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
    /// “各商品库存量统计”的柱状图
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
            // 模块一：柱状图 
            // ==========================================
            var chartData = wms.GetStockChartData();
            UIBarOption option = new UIBarOption();
            option.Title = new UITitle();
            option.Title.Text = "FIH 仓储各物料总库存看板";
            option.Title.SubText = "实时数据统计";

            // 把图表绘制区域往右推120个像素，给数字留出空间
            //option.Grid = new UIGrid { Left = 120 }; 
            option.Grid.Left = 120;

            option.XAxis.Name = "商品名称";
            option.YAxis.Name = "数量 (件/桶)";

            UIBarSeries series = new UIBarSeries();
            series.Name = "实时库存";
            foreach (var item in chartData)
            {
                option.XAxis.Data.Add(item.Key);
                series.AddData(item.Value);
            }
            option.Series.Add(series);
            uiBarChart1.SetOption(option);

            // ==========================================
            // 模块二：全新的库位占比饼图
            // ==========================================
            var locData = wms.GetLocationUsage();

            UIPieOption optionPie = new UIPieOption();
            optionPie.Title = new UITitle();
            optionPie.Title.Text = "仓库库位占用率分析";
            optionPie.Title.SubText = "空闲 vs 已占用";

            // 开启图例（在饼图旁边显示带有颜色的方块说明）
            optionPie.Legend = new UILegend();
            optionPie.Legend.Orient = UIOrient.Vertical;
            optionPie.Legend.Left = UILeftAlignment.Left;

            UIPieSeries seriesPie = new UIPieSeries();
            seriesPie.Name = "库位状态";

            // 格式化标签：名称 : 数量 (百分比) -> 例：已占用 : 2 (66.67%)
            //seriesPie.Label = new UIPieSeriesLabel { Show = true, Formatter = "{b} : {c} ({d}%)" };
            seriesPie.Label.Show = true;
            seriesPie.Label.Formatter = "{b} : {c} ({d}%)";

            // 把刚才干净数据装进去
            foreach (var item in locData)
            {
                seriesPie.AddData(item.Key, item.Value);



                // 2. 👇 必须把名字也加到图例里，否则它找不到名字就会报错越界！
                optionPie.Legend.AddData(item.Key);
            }

            optionPie.Series.Add(seriesPie);
            uiPieChart1.SetOption(optionPie);
        }
    }
}
