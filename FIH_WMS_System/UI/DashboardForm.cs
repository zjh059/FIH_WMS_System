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
            //1. 自动全屏最大化
            this.WindowState = FormWindowState.Maximized;


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



            //优化：开启悬停提示
            //option.ToolTip = new UIToolTip();
            //option.ToolTip.Visible = true;



            // 增加四个方向的内边距
            //option.Grid.Left = 80;
            //option.Grid.Right = 30;
            //option.Grid.Bottom = 80;
            // 把左边距调大
            option.Grid.Left = 120; 
            option.Grid.Right = 50;
            option.Grid.Bottom = 80;
            option.Grid.Top = 60;

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
            //option.Series[series.Name] = series;
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



            // ==========================================
            // 模块三：近7天出入库趋势折线图 (横跨底部)
            // ==========================================
            var trafficData = wms.Get7DaysTraffic();

            UILineOption optionLine = new UILineOption();
            optionLine.Title = new UITitle();
            optionLine.Title.Text = "近 7 天仓库流量趋势 (入库 vs 出库)";
            optionLine.Title.SubText = "按日统计吞吐量 (PCS)";



            //  1. 修复颜色混淆：强行给图表注入调色板 (入库:养眼绿，出库:警示红)
            //optionLine.Color = new System.Drawing.Color[] {
            //    System.Drawing.Color.MediumSeaGreen,
            //    System.Drawing.Color.IndianRed
            //};


            //解决两条线同色的问题！
            // 我们直接在 UI 控件本体上强行注入“自定义双色主题” (绿:入库，红:出库)
            //uiLineChart1.ChartStyleType = UIChartStyleType.Custom;
            //uiLineChart1.CustomColors = new System.Drawing.Color[] {
            //    System.Drawing.Color.MediumSeaGreen,
            //    System.Drawing.Color.IndianRed
            //};



            //“坐标系联动悬停”
            // 只要鼠标移入某一天，入库和出库的数值框会同时弹出来！
            //optionLine.ToolTip = new UIToolTip();
            optionLine.ToolTip.Visible = true;
            //optionLine.ToolTip.Trigger = UIToolTipTrigger.Axis;




            //  2. 修复图例重叠：把图例移到右上角去，别跟标题抢位置
            optionLine.Legend = new UILegend();
            optionLine.Legend.Orient = UIOrient.Horizontal;
            optionLine.Legend.Top = UITopAlignment.Top;
            optionLine.Legend.Left = UILeftAlignment.Right;

            // 增加画布内部的边距，给标题、图例和坐标轴文字留足呼吸空间
            //optionLine.Grid.Top = 60;
            //optionLine.Grid.Bottom = 40;
            //optionLine.Grid.Left = 60;
            //optionLine.Grid.Right = 40;
            // 重点加大顶部边距
            optionLine.Grid.Top = 100;  
            optionLine.Grid.Bottom = 80;
            optionLine.Grid.Left = 120;
            optionLine.Grid.Right = 50;




            optionLine.ToolTip.Visible = true; // 开启鼠标悬停提示
            //optionLine.Legend = new UILegend();
            //optionLine.Legend.AddData("入库总量");
            //optionLine.Legend.AddData("出库总量");



            //3、修复 X 轴只显示数字 0,1,2 的问题：必须声明这是“Category(类别)”轴！
            //optionLine.XAxis.Type = UIAxisType.Category;



            optionLine.XAxis.Name = "日期";
            //锁死X轴标签，强制只显示我们喂进去的文字，防止它乱算小数
            //optionLine.XAxis.AxisLabel.Interval = 0;
            optionLine.YAxis.Name = "变动数量";



            //强制将 X 轴设为类别轴，让底部的日期文字完美显示！
            optionLine.XAxisType = UIAxisType.Category;



            //UILineSeries seriesIn = new UILineSeries();
            //seriesIn.Name = "入库总量";
            //UILineSeries seriesOut = new UILineSeries();
            //seriesOut.Name = "出库总量";

            //修复：UILineSeries 强制要求在括号里传名字
            UILineSeries seriesIn = new UILineSeries("入库总量");
            UILineSeries seriesOut = new UILineSeries("出库总量");



            //让折线上的数据圆点稍微大一点
            seriesIn.SymbolSize = 6;
            seriesOut.SymbolSize = 6;



            // 核心防呆：生成最近7天的连续日期坐标轴
            // 如果某天既没入库也没出库，数据库查出来是缺了这天的，
            // 我们必须手动用循环补齐这 7 天的坐标轴，让它显示 0，而不是直接断层。

            //Sunny.UI 这个库在设计时有一点不统一：
            //柱状图(UIBarSeries)：可以直接 AddData(数值)，它会自动往后排。
            //折线图(UILineSeries)：它认为折线是由一个个“坐标点”组成的，所以强制要求同时传入 X 坐标和 Y 坐标，也就是 Add(x, y)
            //我们只需要在循环外面加一个索引变量 xIndex，然后每次把(xIndex, 数量) 传给它就可以了。
            int xIndex = 0; //  新增一个 X 轴的数字坐标索引

            for (int i = 6; i >= 0; i--)
            {
                //string dateStr = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                //optionLine.XAxis.Data.Add(dateStr); // 给 X 轴画上日期文字标签



                // 数据库比对用的完整日期 (如: 2026-04-13)
                string fullDateStr = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                // X 轴显示的精简日期，省空间 (如: 04-13)
                string shortDateStr = DateTime.Now.AddDays(-i).ToString("MM-dd");

                // 把精简版的日期塞给 X 轴作为标签
                optionLine.XAxis.Data.Add(shortDateStr);




                // 使用 LINQ 从数据库结果中匹配当前日期的总量，没找到就是 0
                //var inRecord = trafficData.FirstOrDefault(d => d.DateStr == dateStr && d.RecordType == 0);
                //var outRecord = trafficData.FirstOrDefault(d => d.DateStr == dateStr && d.RecordType == 1);
                var inRecord = trafficData.FirstOrDefault(d => d.DateStr == fullDateStr && d.RecordType == 0);
                var outRecord = trafficData.FirstOrDefault(d => d.DateStr == fullDateStr && d.RecordType == 1);



                //seriesIn.AddData(inRecord != null ? (int)inRecord.TotalQty : 0);
                //seriesOut.AddData(outRecord != null ? (int)outRecord.TotalQty : 0);
                //修复：把 AddData 改成普通的 Add
                //seriesIn.Add(inRecord != null ? (int)inRecord.TotalQty : 0);
                //seriesOut.Add(outRecord != null ? (int)outRecord.TotalQty : 0);

                // 安全地把 dynamic 里的数值提取出来，转成 double 格式
                double inValue = inRecord != null ? Convert.ToDouble(inRecord.TotalQty) : 0;
                double outValue = outRecord != null ? Convert.ToDouble(outRecord.TotalQty) : 0;



                //修复 2：防崩溃 Hack！破解 Sunny.UI 同值字典 Key 冲突 Bug
                // 如果两个值完全一样（比如都是 0 ），偷偷加上极小的小数差。
                // 这样界面悬停依然显示整数，但底层字典就不会因为 Key 重复而崩溃了！
                if (inValue == outValue)
                {
                    outValue += 0.000001;
                }




                //  终极修复：传入完整的 (X坐标, Y坐标)
                seriesIn.Add(xIndex, inValue);
                seriesOut.Add(xIndex, outValue);

                xIndex++; // 每画完一天，X 坐标往右挪一格
            }

            //optionLine.Series.Add(seriesIn);
            //optionLine.Series.Add(seriesOut);
            // 把名字作为 Key 传在前面，对象作为 Value 传在后面
            //optionLine.Series.Add(seriesIn.Name, seriesIn);
            //optionLine.Series.Add(seriesOut.Name, seriesOut);
            // 修复：使用并发字典专属的赋值方式
            optionLine.Series[seriesIn.Name] = seriesIn;
            optionLine.Series[seriesOut.Name] = seriesOut;


            uiLineChart1.SetOption(optionLine);
        }



    }
}
