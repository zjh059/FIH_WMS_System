using FIH_WMS_System.UI;
using Sunny.UI;

namespace FIH_WMS_System
{
    public partial class MainForm : UIForm
    {
        //演示：在主窗口中直接使用服务层，简化设计
        // 1. 在窗口级别，声明并实例化我们的核心服务（主厨）
        private Services.WmsService wms = new Services.WmsService();
        public MainForm()
        {
            InitializeComponent();

            /*            wms.AddGoods("G001", "原材料A", "50kg/桶", 10.5m);
                        wms.AddGoods("G002", "成品B", "100个/箱", 300m);
                        wms.InStock("G001", 10, "A-01-01");
                        wms.InStock("G002", 5, "B-01-01");*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }


        private void uiButton1_Click(object sender, EventArgs e)
        {
            //1. 召唤入库子窗口
            InStockForm form = new InStockForm();

            // 2. 只有当子窗口返回了 OK 信号（说明用户点了确认并且输入没报错）
            //form.ShowDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                // 3. 从子窗口的口袋里，把刚才填的数据拿出来，交给Service层去入库！
                bool success = wms.InStock(form.InputGoodsCode, form.InputQty, form.InputLocCode);

                if (success)
                {

                    Utils.VoiceHelper.Speak("商品入库成功，已为您生成专属物料二维码。");

                    MessageBox.Show("🎉 入库成功！数据已更新。", "系统提示");



                    // 【扫码核心：动态生成并展示二维码贴纸！】
                    // 我们把 商品编码 藏在二维码里，将来扫码枪一扫就能读出来
                    string qrContent = form.InputGoodsCode;
                    Bitmap qrImage = Utils.BarcodeHelper.GenerateQRCode(qrContent);



                    // “凭空”捏造一个小窗口，用来展示打印出来的二维码
                    Form qrForm = new Form();
                    qrForm.Text = "🖨️ 请将此标签打印并贴在货物上";
                    qrForm.Size = new System.Drawing.Size(300, 350);
                    qrForm.StartPosition = FormStartPosition.CenterParent;
                    qrForm.BackColor = Color.White;

                    // 加一个图片控件装载二维码
                    PictureBox pb = new PictureBox();
                    pb.Image = qrImage;
                    pb.SizeMode = PictureBoxSizeMode.CenterImage;
                    pb.Dock = DockStyle.Fill;

                    qrForm.Controls.Add(pb);
                    qrForm.ShowDialog(); // 弹出这张虚拟的“打印贴纸”

                    // 4.成功后，自动点一下“库存查询”按钮，刷新右边表格
                    uiButton3_Click(null, null);
                }
                else
                {
                    MessageBox.Show("❌ 入库失败：可能商品编码或库位不存在！", "系统提示");
                }
            }
        }

        // 出库按钮的事件处理器，包含了人工指定和智能出库两种场景的逻辑，以及BOM模式的特殊处理
        private void uiButton2_Click(object sender, EventArgs e)
        {
            OutStockForm form = new OutStockForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                //判断如果是BOM模式完成的，直接语音播报 + 刷新！
                if (form.IsBOMOutCompleted)
                {
                    Utils.VoiceHelper.Speak("产线工单出库已下发，AGV正在为您备料。");
                    MessageBox.Show("🚀 BOM 智能出库执行成功！相关 AGV 任务已生成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    uiButton3_Click(null, null); // 刷新表格
                    return; // 搞定收工，直接退出，不要往下执行老逻辑了
                }


                // --- 场景 A：人工指定模式 ---
                if (form.InputStrategy == Services.OutboundStrategy.Manual)
                {
                    if (string.IsNullOrEmpty(form.InputLocCode))
                    {
                        MessageBox.Show("人工指定策略下，请务必填写【出库库位】！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool success = wms.OutStock(form.InputGoodsCode, form.InputQty, form.InputLocCode);
                    if (success)
                    {
                        MessageBox.Show("🎉 人工出库成功！数据已更新。", "系统提示");
                        uiButton3_Click(null, null); // ✅ 成功后刷新表格！
                    }
                    else
                    {
                        MessageBox.Show("❌ 出库失败：请检查库位是否存在或库存是否充足！", "系统警告");
                    }
                }

                // --- 场景 B：智能出库模式 ---
                else
                {
                    // 1. 找智能大脑要拣货建议
                    var adviceList = wms.GetOutboundPickAdvice(form.InputGoodsCode, form.InputQty, form.InputStrategy);
                    int totalAvailable = adviceList.Sum(x => x.Qty);

                    if (adviceList.Count == 0 || totalAvailable < form.InputQty)
                    {
                        MessageBox.Show($"库存严重不足！\n当前可用总数仅剩: {totalAvailable} 个。", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 2. 拼接提示给操作员看
                    string adviceMsg = $"系统为您生成了最佳拣货路径：\n\n";
                    foreach (var pick in adviceList)
                    {
                        adviceMsg += $"👉 去库位【{pick.LocationCode}】取出 {pick.Qty} 个 (批次:{pick.BatchNo})\n";
                    }
                    adviceMsg += "\n是否确认立即出库，并指派 AGV 小车？";

                    // 3. 弹窗确认
                    if (MessageBox.Show(adviceMsg, "智能出库确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        string autoOrderNo = "AUTO-OUT-" + DateTime.Now.ToString("HHmmss");
                        bool success = wms.SmartOutStock(form.InputGoodsCode, form.InputQty, autoOrderNo, form.InputStrategy);

                        if (success)
                        {
                            MessageBox.Show("🎉 智能出库执行成功！相关 AGV 任务已生成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            uiButton3_Click(null, null); // ✅ 成功后瞬间刷新表格！
                        }
                        else
                        {
                            MessageBox.Show("出库执行失败，请重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            // 1. 向Models要出所有的库存数据
            var rawStock = wms.GetStockList();

            // 2. ✨ LINQ showTime
            // 因为库存对象里包含了商品和库位，直接展示会很难看。
            // 我们用 Select 重新“包装”一下，变成中文表头
            var displayList = rawStock.Select(s => new
            {
                库存编号 = s.Id,
                商品名称 = s.Goods?.Name,
                商品编码 = s.Goods?.Code,
                所在库位 = s.Location?.Code,
                库存数量 = s.Qty,
                批次号 = s.BatchNo,                           // 👈 【新增展示】
                入库时间 = s.InStockTime.ToString("yyyy-MM-dd") // 👈 【新增展示】
            }).ToList();

            // 3. 把包装好的数据直接“喂”给表格控件！(假设你的表格叫 dataGridView1)
            dataGridView1.DataSource = null;// 先清空一下，强制刷新
            dataGridView1.DataSource = displayList;//绑定新数据

            // 让表格根据内容自动调整列宽
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void uiButton4_Click(object sender, EventArgs e)
        {
            // 1. 向Models要出入库的流水日志
            var rawRecords = wms.GetRecords();

            // 2. LINQ：翻译并包装数据
            var displayList = rawRecords.Select(r => new
            {
                记录编号 = r.Id,
                单据号 = r.OrderNo,               // 👈 【新增展示】查账必备
                // ⚠️ 三元表达式：如果是 InStock 就显示"入库"，否则显示"出库"
                操作类型 = r.Type == FIH_WMS_System.Models.RecordType.InStock ? "▲ 入库 (+)" : "▼ 出库 (-)",
                商品名称 = r.Goods?.Name,
                所在库位 = r.Location?.Code,      // 👈 【新增展示】明确货物在哪进出的
                变动数量 = r.Qty,
                批次号 = r.BatchNo,               // 👈 【新增展示】FIFO的核心依据
                操作时间 = r.OperateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                操作人 = r.Operator               // 👈 【新增展示】
            }).ToList();

            // 3. 喂给同一个表格
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displayList;

            // 👇 这句赋予“操作时间”列特权，让它根据里面的字数自动撑开！
            dataGridView1.Columns["操作时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //“记录编号”那一列太宽了浪费，也可以加一句
            //dataGridView1.Columns["记录编号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Columns["商品名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["所在库位"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["变动数量"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        //🔄 库位移库与碎片整理
        private void uiButton5_Click(object sender, EventArgs e)
        {
            UI.MoveStockForm form = new UI.MoveStockForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // ---- 场景 A：智能碎片整理模式 ----
                if (form.IsSmartMoveCompleted)
                {
                    // 智能移库已经在子窗体内自己循环执行完毕了，这里只需要播报和刷新即可
                    Utils.VoiceHelper.Speak("智能碎片整理完成，小车已前往调度。");
                    MessageBox.Show("🎉 智能碎片整理成功！全仓库存已优化。", "系统提示");
                    uiButton3_Click(null, null); // 刷新库存查询表格
                }
                // ---- 场景 B：传统人工移库模式 ----
                else
                {
                    bool success = wms.MoveStock(form.InputGoodsCode, form.InputFromLoc, form.InputToLoc, form.InputQty);

                    if (success)
                    {
                        Utils.VoiceHelper.Speak("移库操作已完成，流水账目已记录。");
                        MessageBox.Show("🎉 人工移库成功！货品已转移，流水已记录。", "系统提示");
                        uiButton3_Click(null, null); // 刷新库存查询表格
                    }
                    else
                    {
                        MessageBox.Show("❌ 移库失败：可能源库位不存在该商品，或者库存数量不足！", "系统警告");
                    }
                }
            }
        }

        //📋 库存盘点
        private void uiButton6_Click(object sender, EventArgs e)
        {
            UI.CheckStockForm form = new UI.CheckStockForm();

            // 如果子窗体成功处理完盘点，并传回了 OK 信号
            if (form.ShowDialog() == DialogResult.OK)
            {
                // 主界面只需要负责弹窗庆祝，并刷新总表格即可！
                MessageBox.Show("🎉 盘点完成！系统账目已修正，盘点流水已生成。", "系统提示");
                uiButton3_Click(null, null); // 瞬间刷新右侧的主展示表格
            }
        }


        //查询 AGV 任务列表
        private void uiButton7_Click(object sender, EventArgs e)
        {
            // 1. 获取所有下发给小车的任务
            var tasks = wms.GetAgvTasks();

            // 2. 将枯燥的数字状态“翻译”成人类和车间工人能看懂的炫酷状态
            var displayList = tasks.Select(t => new
            {
                任务ID = t.Id,
                任务单号 = t.TaskNo,
                任务类型 = t.TaskType == 1 ? "📤 出库搬运" : (t.TaskType == 0 ? "📥 入库搬运" : "🔄 移库搬运"),
                当前状态 = t.Status == 0 ? "⏳ 待分配" : (t.Status == 3 ? "✅ 已完成" : "🚀 运送中"),
                商品名称 = t.Goods?.Name,
                搬运数量 = t.Qty,
                起点 = t.FromLocation,
                终点 = t.ToLocation,
                下发时间 = t.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                完成时间 = t.FinishTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "尚未完成"
            }).ToList();

            // 3. 绑定到表格展示
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displayList;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        //模拟 AGV 小车完成任务反馈
        private void btnSimulateAgv_Click(object sender, EventArgs e)
        {
            // 判断用户有没有在表格里选中某一行
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // 从选中的那一行里，抓取“任务ID”
                    int taskId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["任务ID"].Value);
                    string status = dataGridView1.SelectedRows[0].Cells["当前状态"].Value.ToString();

                    if (status.Contains("已完成"))
                    {
                        Utils.VoiceHelper.Speak("Warning，操作异常！");
                        MessageBox.Show("这个任务小车已经完成，无须重复确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 呼叫服务层，完结这个任务！
                    if (wms.CompleteAgvTask(taskId))
                    {

                        // 👇 触发语音播报
                        Utils.VoiceHelper.Speak("AGV小车已成功将货物搬运至终点，任务完成！");

                        MessageBox.Show("🚗 小车已成功将货物搬运至终点！", "WCS系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // 刷新一下调度中心列表，让你看到状态变成“已完成”
                        uiButton7_Click(null, null);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请确保您当前查看的是【AGV调度中心】的列表，并整行选中了一个任务！", "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("请先在表格中点击最左侧的箭头，选中一行小车任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //“数据分析（库存占比分析）”统计看板
        private void uiButton8_Click(object sender, EventArgs e)
        {
            UI.DashboardForm form = new UI.DashboardForm();
            form.ShowDialog();
        }

        //2D 库位可视化监控屏
        private void uiButton9_Click(object sender, EventArgs e)
        {
            UI.MapForm form = new UI.MapForm();
            form.ShowDialog();
        }



        // 窗口加载时触发的事件
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. 在左上角的标题栏，显示当前登录人的名字和身份！
            this.Text = $"FIH 智能仓储管理系统 - 当前登录：【{Program.CurrentUsername}】 ({Program.CurrentRole})";

            // 2. 权限校验：如果是普通操作员，把高级功能的按钮藏起来！
            if (Program.CurrentRole == "操作员")
            {
                // 假设 uiButton7 是AGV，uiButton8 是大屏，uiButton9 是2D地图
                // 请根据界面上实际的按钮名字修改！
                uiButton7.Visible = false; // 隐藏 AGV调度中心
                uiButton8.Visible = false; // 隐藏 数据大屏看板
                uiButton9.Visible = false; // 隐藏 2D库位监控

                // 或许！ 也可以把盘点、移库的按钮给禁用掉
                // uiButton5.Enabled = false; 
            }
        }

        private void btnLogout_Click(object sender, EventArgs e) // 假设你的按钮叫 btnLogout，请以你实际双击出来的名字为准
        {
            // 1. 弹出一个确认框，防止手抖误触
            DialogResult result = MessageBox.Show(
                "您确定要登出当前账号，并返回登录界面吗？",
                "登出提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // 2. 如果用户点击了“是”
            if (result == DialogResult.Yes)
            {
                // Application.Restart() 可以关闭当前程序，并瞬间重新启动它
                // 这样可以自动跳回 Program.cs 里的 LoginForm 登录页，
                // 还能清空所有的全局变量（就像Program.CurrentUsername），防止上一个人的状态残留
                Application.Restart();
            }
        }

        //一键导出--表格处理
        private void btnExportExcel_Click(object sender, EventArgs e) 
        {
            Utils.ExcelHelper.ExportToExcel(dataGridView1, "FIH智能仓储报表");
        }


    }
}
