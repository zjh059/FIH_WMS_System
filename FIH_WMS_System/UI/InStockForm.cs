using FIH_WMS_System.Models;
using FIH_WMS_System.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Generic; // 引用来使用 Dictionary

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// 入库窗体界面
    /// </summary>
    public partial class InStockForm : Form
    {
        private WmsService wmsService = new WmsService();

        public DateTime? InputProduceDate { get; set; }

        public InStockForm()
        {
            InitializeComponent();
        }


        // 1. 定义三个“公开的口袋”，用来装用户输入的数据，方便主窗口一会来拿
        public string InputGoodsCode { get; set; }
        public int InputQty { get; set; }
        public string InputLocCode { get; set; }


        /*        private void btnConfirm_Click(object sender, EventArgs e)
                {
                    try
                    {
                        // 2. 把文本框里的字，装进我们的口袋里
                        InputGoodsCode = txtGoodsCode.Text;
                        InputLocCode = txtLocCode.Text;

                        // 数量必须是数字，所以用 int.Parse 转换一下
                        InputQty = int.Parse(txtQty.Text);

                        // 3. 极其重要的一句！告诉系统：“这个弹窗的任务圆满完成 (OK)！”
                        // 这句话执行后，弹窗会自动关闭，并给主窗口发个 OK 的信号。
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("警告：入库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }*/


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputGoodsCode = txtGoodsCode.Text.Trim();
                InputLocCode = txtLocCode.Text.Trim();
                InputQty = int.Parse(txtQty.Text);

                //新增：如果勾选了生产日期，就把日期装进口袋，否则给 null
                if (dtpProduceDate.Checked)
                {
                    InputProduceDate = dtpProduceDate.Value.Date;
                }
                else
                {
                    InputProduceDate = null;
                }

                // 校验必填项
                if (string.IsNullOrEmpty(InputGoodsCode))
                {
                    MessageBox.Show("物料编码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 👇 【核心改动：触发智能分配】
                // 如果用户偷懒没填库位，就用 WmsService 帮他找！
                if (string.IsNullOrEmpty(InputLocCode))
                {
                    Services.WmsService wms = new Services.WmsService();
                    //string autoLoc = wms.GetRecommendLocation(InputGoodsCode);

                    //去下拉框抓取当前选中的入库策略
                    InboundStrategy currentStrategy = (InboundStrategy)cmbStrategy.SelectedValue;
                    // 【修改】：把 InputGoodsCode(物料)、InputQty(数量)、currentStrategy(策略) 一起传给大脑防爆仓！
                    string autoLoc = wms.GetRecommendLocation(InputGoodsCode, InputQty, currentStrategy);

                    if (string.IsNullOrEmpty(autoLoc))
                    {
                        MessageBox.Show("系统警告：仓库已满，无法智能分配库位！请先清理库存。", "爆仓警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 直接中止入库
                    }
                    else
                    {
                        // 帮用户把找到的库位填进口袋里
                        InputLocCode = autoLoc;
                        MessageBox.Show($"触发智能分配！\n系统已自动将物料分配至库位：【{autoLoc}】", "智能引擎", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                MessageBox.Show("警告：入库数量必须填入纯数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        // 1. 窗体加载时，初始化“入库策略”下拉框
        //用字典把英文枚举翻译成中文显示
        private void InStockForm_Load(object sender, EventArgs e)
        {
            // 创建一个字典，左边是系统认识的枚举，右边是人看的中文
            //Dictionary<InboundStrategy, string>：这是一个键值对集合
            //WinForms 的下拉框就知道：当用户看到并点击“3. 按空库位入库 (不混放)”时，它实际选中的底层数据是 InboundStrategy.EmptyLocationFirst
            var strategyDict = new Dictionary<InboundStrategy, string>
            {
                { InboundStrategy.Manual, "Manual-直接人工指定" },
                { InboundStrategy.SameMaterialMerge, "SameMaterialMerge-按未满库位入库 (推荐合并)" },
                { InboundStrategy.EmptyLocationFirst, "EmptyLocationFirst-按空库位入库 (不混放)" },
                { InboundStrategy.NearestFirst, "NearestFirst-按就近库位入库 (最高效)" }
            };

            // 绑定到下拉框
            cmbStrategy.DataSource = new BindingSource(strategyDict, null);
            cmbStrategy.DisplayMember = "Value"; // 表面上给用户看字典的 Value（中文）
            cmbStrategy.ValueMember = "Key";     // 骨子里给代码传字典的 Key（枚举）

            // 默认选中 "按未满库位入库"
            cmbStrategy.SelectedValue = InboundStrategy.SameMaterialMerge;

            // 绑定扫码枪(回车键)监听事件
            txtGoodsCode.KeyDown += new KeyEventHandler(txtGoodsCode_KeyDown);
            txtQty.KeyDown += new KeyEventHandler(txtQty_KeyDown);
        }



        // 2. 触发智能推荐
        private void btnRecommend_Click(object sender, EventArgs e)
        {
            string goodsCode = txtGoodsCode.Text.Trim();

            if (string.IsNullOrEmpty(goodsCode))
            {
                MessageBox.Show("请先输入物料编码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取用户当前选择的策略
            //InboundStrategy selectedStrategy = (InboundStrategy)cmbStrategy.SelectedItem;

            // 【关键修改】：因为用了字典绑定，这里要用 SelectedValue 获取背后的枚举
            //SelectedValue：配合上面的绑定方式，我们现在需要用 SelectedValue 而不是 SelectedItem 来抓取真正的枚举值传给 WmsService
            InboundStrategy selectedStrategy = (InboundStrategy)cmbStrategy.SelectedValue;

            // 召唤服务层的大脑
            //string recommendLoc = wmsService.GetRecommendLocation(goodsCode, selectedStrategy);

            // 【新增】：尝试读取当前界面的数量。如果还没填(比如扫码枪刚扫完)，就默认为0(仅去探测寻找库位)。
            int currentQty = 0;
            int.TryParse(txtQty.Text.Trim(), out currentQty);
            // 召唤服务层的大脑 【修改】：把 currentQty(入库数量) 也传进去！
            string recommendLoc = wmsService.GetRecommendLocation(goodsCode, currentQty, selectedStrategy);

            if (!string.IsNullOrEmpty(recommendLoc))
            {
                txtLocCode.Text = recommendLoc;
                //MessageBox.Show($"💡 智能引擎已为您推荐最佳库位：【{recommendLoc}】\n策略：{selectedStrategy}",
                //                "智能推荐成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string strategyName = cmbStrategy.Text; // 抓取下拉框表面显示的中文文字
                MessageBox.Show($"💡 智能引擎已为您推荐最佳库位：【{recommendLoc}】\n策略：{strategyName}",
                                "智能推荐成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 如果是人工指定，或者仓库真的爆满没位置了
                if (selectedStrategy == InboundStrategy.Manual)
                {
                    MessageBox.Show("您选择了【人工指定】，请手动输入目标库位。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("⚠️ 仓库当前可能已满，或未找到符合策略的可用库位，请人工确认！", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        // ==========================================
        // 扫码枪专属：扫完物料码，自动触发智能分配库位！
        // ==========================================
        private void txtGoodsCode_KeyDown(object sender, KeyEventArgs e)
        {
            // 如果监听到回车键 (扫码枪的默认行为)
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // 消除系统默认的 '叮' 提示音

                // 1. 瞬间自动点击“智能推荐”按钮
                btnRecommend_Click(null, null);

                // 2. 将光标自动跳到“数量”框，让工人可以直接填数量！
                txtQty.Focus();
            }
        }

        // ==========================================
        // 扫码枪专属：填完数量敲回车，自动提交入库！
        // ==========================================
        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                // 瞬间自动点击“确认入库”按钮
                btnConfirm_Click(null, null);
            }
        }



    }
}
