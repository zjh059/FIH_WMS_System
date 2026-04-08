using FIH_WMS_System.UI;
using Sunny.UI;

namespace FIH_WMS_System
{
    public partial class MainForm : UIForm
    {
        //在窗口声明并实例化核心服务
        private Services.WmsService wms = new Services.WmsService();
        public MainForm()
        {
            InitializeComponent();

            /*            wms.AddGoods("G001", "原材料A", "50kg/桶", 10.5m);
                        wms.AddGoods("G002", "成品B", "100个/箱", 300m);
                        wms.InStock("G001", 10, "A-01-01");
                        wms.InStock("G002", 5, "B-01-01");*/

            //  1. 禁用字体自动缩放，防止在高分屏下控件乱缩
            this.AutoScaleMode = AutoScaleMode.None;

            //  2. 强行撑开标题栏高度（默认通常是35，设为45左右，右上角的按钮会自动变大！）
            //this.TitleHeight = 65;
        }

        // 窗口加载时触发的事件
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. 左上角的标题栏显示当前登录人的名字和身份
            this.Text = $"FIH 智能仓储管理系统 - 当前登录：【{Program.CurrentUsername}】 ({Program.CurrentRole})";

            //把表格的锚点钉死在上下左右四个边
            // 这样只要窗口一拉伸，表格就会填满空白区域
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            label1.Anchor = AnchorStyles.Top | AnchorStyles.Top;
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            //让这三个按钮钉在窗口的“底部”和“右侧”
            // 这样窗口放大时，它们会乖乖跟着往下跑，绝对不会被表格压到！
            btnAgvMonitor.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSimulateAgv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportExcel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;




            // 2. 权限校验：如果是普通操作员，锁死危险的改账操作
            if (Program.CurrentRole == "操作员")
            {
                // 禁用移库 (uiButton5)
                uiButton5.Enabled = false;
                uiButton5.Text = "🚫 无权限(移库)";

                // 禁用盘点 (uiButton6)
                uiButton6.Enabled = false;
                uiButton6.Text = "🚫 无权限(盘点)";

                // 隐藏 AGV 手动调度中心 (控制危险)
                //uiButton7.Visible = false;
                uiButton7.Enabled = false;
                uiButton7.Text = "🚫禁用(AVG调度)";

                btnBaseData.Enabled = false;
                btnBaseData.Text = "🚫(⚙️数据管理)";

                btnUserManage.Enabled = false;
                btnUserManage.Text = "🚫(👥用户管理)";

                btnSysVoiceSetup.Enabled = false;
                btnSysVoiceSetup.Text = "🚫(🔊语音设置)";

                // （大屏看板 uiButton8 和 2D地图 uiButton9 可以保留看，如果不给看也可以设为 false）


            }

            // ==========================================
            // 【新增】登录时主动扫描安全库存预警！
            // ==========================================
            if (Program.CurrentRole == "管理员") // 只给管理员弹警告
            {
                var warnings = wms.GetLowStockWarnings();
                if (warnings.Count > 0)
                {
                    // 播放警报语音
                    Utils.VoiceHelper.Speak("系统警报：检测到部分物料低于安全库存底线，请立即处理！");

                    // 弹出我们刚才写好的预警窗口
                    UI.WarningForm warningForm = new UI.WarningForm();
                    warningForm.ShowDialog();
                }
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
            // 1. 召唤入库子窗口
            InStockForm form = new InStockForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                // 2. 智能入库引擎，获取系统颁发的“身份证号(ReelId)”
                string generatedReelId = wms.InStockWithLabel(form.InputGoodsCode, form.InputQty, form.InputLocCode);

                // 【核心新增：精准的错误弹窗拦截】
                if (generatedReelId == "ERROR_GOODS")
                {
                    Utils.VoiceHelper.Speak("警告，物料编码不存在");
                    MessageBox.Show($"❌ 入库拦截：系统基础物料库中不存在编码为【{form.InputGoodsCode}】的物料！\n请先在基础数据中登记该物料，再进行入库。", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (generatedReelId == "ERROR_LOCATION")
                {
                    Utils.VoiceHelper.Speak("警告，库位编码不存在");
                    MessageBox.Show($"❌ 入库拦截：仓库中不存在编码为【{form.InputLocCode}】的库位！\n请检查货架条码是否扫描正确。", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                else if (!string.IsNullOrEmpty(generatedReelId))
                {
                    Utils.VoiceHelper.Speak("物料入库成功，已为您生成追溯标签。");
                    MessageBox.Show("🎉 入库成功！数据已更新，正在生成标签...", "系统提示");

                    // ==========================================
                    // 3. 核心：用代码动态画一张“工业级物料追溯贴纸”！
                    // ==========================================
                    Bitmap labelImage = new Bitmap(420, 240); // 设定贴纸的物理尺寸
                    using (Graphics g = Graphics.FromImage(labelImage))
                    {
                        // 涂上纯白底色，并画一个黑色粗边框
                        g.Clear(Color.White);
                        g.DrawRectangle(new Pen(Color.Black, 3), 5, 5, 410, 230);

                        // 设置字体
                        Font titleFont = new Font("微软雅黑", 14, FontStyle.Bold);
                        Font textFont = new Font("微软雅黑", 10, FontStyle.Bold);

                        // 画顶部公司抬头和分割线
                        g.DrawString("FIH 智能制造 - 物料追溯标签", titleFont, Brushes.Black, 50, 15);
                        g.DrawLine(Pens.Black, 10, 45, 410, 45);

                        // 画左侧的具体入库明细
                        g.DrawString($"物料编码: {form.InputGoodsCode}", textFont, Brushes.Black, 20, 60);
                        g.DrawString($"入库数量: {form.InputQty} (PCS/桶)", textFont, Brushes.Black, 20, 90);
                        g.DrawString($"存放库位: {form.InputLocCode}", textFont, Brushes.Black, 20, 120);
                        g.DrawString($"入库人员: {Program.CurrentUsername}", textFont, Brushes.Black, 20, 150);
                        g.DrawString($"入库时间: {DateTime.Now:yyyy-MM-dd HH:mm}", textFont, Brushes.Black, 20, 180);

                        // 画右侧的二维码 (把 ReelId 藏在二维码里！)
                        Bitmap qrCode = Utils.BarcodeHelper.GenerateQRCode(generatedReelId);
                        g.DrawImage(qrCode, 260, 55, 130, 130);

                        // 在二维码下方打印出 ReelId 的明文
                        g.DrawString($"Reel ID: {generatedReelId}", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 250, 195);
                    }

                    // 4. 一个小窗口，用来展示此打印贴纸
                    Form qrForm = new Form();
                    qrForm.Text = "🖨️ 请将此标签使用斑马打印机打印并贴在货物上";
                    qrForm.Size = new System.Drawing.Size(460, 300);
                    qrForm.StartPosition = FormStartPosition.CenterParent;
                    qrForm.BackColor = Color.LightGray;

                    PictureBox pb = new PictureBox();
                    pb.Image = labelImage;
                    pb.SizeMode = PictureBoxSizeMode.CenterImage;
                    pb.Dock = DockStyle.Fill;

                    qrForm.Controls.Add(pb);
                    qrForm.ShowDialog(); // 弹出贴纸！

                    // 5. 成功后，自动点一下“库存查询”按钮，刷新右边表格
                    uiButton3_Click(null, null);
                }
                else
                {
                    MessageBox.Show("❌ 入库失败：可能物料编码或库位不存在！", "系统提示");
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

                    CheckLowStockWarning(); // 👈 【新增】：出库完立刻查水表！
                    return;
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
                        uiButton3_Click(null, null); // 刷新表格

                        CheckLowStockWarning(); // 👈 【新增】：出库完立刻查水表！
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
                            uiButton3_Click(null, null); // 刷新表格

                            CheckLowStockWarning(); // 👈 【新增】：出库完立刻查水表！
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
            var displayList = rawStock.Select(s => new
            {
                库存编号 = s.Id,
                唯一追溯码 = s.ReelId,
                物料名称 = s.Goods?.Name,
                物料编码 = s.Goods?.Code,
                所在库位 = s.Location?.Code,
                实际总数量 = s.Qty,               // 👈 改个名字，方便区分
                冻结数量 = s.FrozenQty,           // 👈 【新增展示】
                可用数量 = s.Qty - s.FrozenQty,   // 👈 【新增展示】真实能用的数量！
                批次号 = s.BatchNo,
                入库时间 = s.InStockTime.ToString("yyyy-MM-dd")
            }).ToList();

            // 3. 把包装好的数据直接给表格控件！
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displayList;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            // 加滑条
            // 让表格根据内容自动撑开，塞不下就出横向滚动条！
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // 卸载右键菜单，防止流水和调度界面被干扰
            dataGridView1.ContextMenuStrip = null;


            // ==========================================
            // 【新增】：给库存查询表格挂载“右键菜单”
            // ==========================================
            ContextMenuStrip stockMenu = new ContextMenuStrip();

            ToolStripMenuItem freezeItem = new ToolStripMenuItem("🔒 冻结该批次库存 (拦截出库)");
            freezeItem.Click += (s, ev) => {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int stockId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["库存编号"].Value);
                    wms.ToggleFreezeStock(stockId, true);
                    MessageBox.Show("已成功冻结该库存，它将不会被分配给任何出库任务！", "冻结成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    uiButton3_Click(null, null); // 刷新表格
                }
            };

            ToolStripMenuItem unfreezeItem = new ToolStripMenuItem("🔓 解除冻结 (恢复可用)");
            unfreezeItem.Click += (s, ev) => {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int stockId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["库存编号"].Value);
                    wms.ToggleFreezeStock(stockId, false);
                    MessageBox.Show("已解除冻结，恢复正常可用状态！", "解冻成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    uiButton3_Click(null, null); // 刷新表格
                }
            };

            stockMenu.Items.Add(freezeItem);
            stockMenu.Items.Add(unfreezeItem);

            // 把做好的菜单绑定给表格
            dataGridView1.ContextMenuStrip = stockMenu;
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
                唯一追溯码 = r.ReelId,
                操作类型 = r.Type == FIH_WMS_System.Models.RecordType.InStock ? "▲ 入库 (+)" : "▼ 出库 (-)",
                物料名称 = r.Goods?.Name,
                所在库位 = r.Location?.Code,      // 👈 【新增展示】明确货物在哪进出的
                变动数量 = r.Qty,
                批次号 = r.BatchNo,               // 👈 【新增展示】FIFO的核心依据
                操作时间 = r.OperateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                操作人 = r.Operator               // 👈 【新增展示】
            }).ToList();

            // 3. 喂给同一个表格
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displayList;


            // 加滑条
            // 让表格根据内容自动撑开，塞不下就出横向滚动条！
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // 卸载右键菜单，防止流水和调度界面被干扰
            dataGridView1.ContextMenuStrip = null;


            // 👇 这句赋予“操作时间”列特权，让它根据里面的字数自动撑开！
            dataGridView1.Columns["操作时间"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //“记录编号”那一列太宽了浪费，也可以加一句
            //dataGridView1.Columns["记录编号"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Columns["物料名称"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["所在库位"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["变动数量"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            // 卸载右键菜单，防止流水和调度界面被干扰
            dataGridView1.ContextMenuStrip = null;
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
                        MessageBox.Show("❌ 移库失败：可能源库位不存在该物料，或者库存数量不足！", "系统警告");
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
                物料名称 = t.Goods?.Name,
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


            // 卸载右键菜单，防止流水和调度界面被干扰
            dataGridView1.ContextMenuStrip = null;
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


        //一键导出--表格处理
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            Utils.ExcelHelper.ExportToExcel(dataGridView1, "FIH智能仓储报表");
        }

        // AGV 监控台
        private void btnAgvMonitor_Click(object sender, EventArgs e)
        {
            UI.AgvMonitorForm form = new UI.AgvMonitorForm();
            form.ShowDialog();
        }

        private void btnBaseData_Click(object sender, EventArgs e)
        {
            UI.BaseDataForm form = new UI.BaseDataForm();
            form.ShowDialog();
        }

        private void btnReturnStock_Click(object sender, EventArgs e)
        {
            UI.ReturnStockForm form = new UI.ReturnStockForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // 返料成功关闭窗口后，顺手刷新一下库存表格
                uiButton3_Click(null, null);
            }
        }

        private void btnWaveOut_Click(object sender, EventArgs e)
        {
            UI.WaveForm form = new UI.WaveForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                uiButton3_Click(null, null); // 执行完波次后，刷新库存表格
            }
        }



        // ==========================================
        // 核心监控：随时查验全仓安全库存！
        // ==========================================
        private void CheckLowStockWarning()
        {
            if (Program.CurrentRole == "管理员") // 只有管理员才配看到报警并处理采购单
            {
                var warnings = wms.GetLowStockWarnings();
                if (warnings.Count > 0)
                {
                    Utils.VoiceHelper.Speak("系统警报：检测到部分物料低于安全库存底线，请立即处理！");
                    UI.WarningForm warningForm = new UI.WarningForm();
                    warningForm.ShowDialog();
                }
            }
        }

        private void btnOrderCenter_Click(object sender, EventArgs e)
        {
            new UI.OrderCenterForm().ShowDialog();
        }
        private void btnUserManage_Click(object sender, EventArgs e)
        {
            new UI.UserManageForm().ShowDialog();
        }

        private void btnSysVoiceSetup_Click(object sender, EventArgs e)
        {
            new UI.SettingsForm().ShowDialog();
        }
    }
}
