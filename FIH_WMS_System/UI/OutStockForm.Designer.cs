namespace FIH_WMS_System.UI
{
    partial class OutStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            labelScanner = new Label();
            txtScanner = new TextBox();
            label1 = new Label();
            txtGoodsCode = new TextBox();
            label2 = new Label();
            txtQty = new TextBox();
            label3 = new Label();
            txtLocCode = new TextBox();
            cmbStrategy = new ComboBox();
            btnConfirm = new Button();
            lblProduct = new Label();
            txtProductCode = new TextBox();
            lblProduceQty = new Label();
            txtProduceQty = new TextBox();
            btnAnalyze = new Button();
            dgvBOM = new DataGridView();
            tabControl1 = new TabControl();
            tpStandard = new TabPage();
            tableLayoutPanelStandard = new TableLayoutPanel();
            lblStrategy = new Label();
            tpBOM = new TabPage();
            tableLayoutPanelBOM = new TableLayoutPanel();
            btnExecuteBOM = new Button();
            tpAdditional = new TabPage();
            tableLayoutPanelAdd = new TableLayoutPanel();
            labelAddWO = new Label();
            txtAddWO = new TextBox();
            labelAddGoods = new Label();
            txtAddGoodsCode = new TextBox();
            labelAddQty = new Label();
            txtAddQty = new TextBox();
            labelAddStrategy = new Label();
            cmbStrategyAdd = new ComboBox();
            btnExecuteAdd = new Button();
            tpCart = new TabPage();
            btnDispatchCart = new Button();
            btnBindCart = new Button();
            txtCartQty = new TextBox();
            lblCartQty = new Label();
            txtCartReel = new TextBox();
            lblCartReel = new Label();
            cmbCart = new ComboBox();
            lblCart = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBOM).BeginInit();
            tabControl1.SuspendLayout();
            tpStandard.SuspendLayout();
            tableLayoutPanelStandard.SuspendLayout();
            tpBOM.SuspendLayout();
            tableLayoutPanelBOM.SuspendLayout();
            tpAdditional.SuspendLayout();
            tableLayoutPanelAdd.SuspendLayout();
            tpCart.SuspendLayout();
            SuspendLayout();
            // 
            // labelScanner
            // 
            labelScanner.Anchor = AnchorStyles.Right;
            labelScanner.AutoSize = true;
            labelScanner.Location = new Point(27, 24);
            labelScanner.Margin = new Padding(5, 0, 5, 0);
            labelScanner.Name = "labelScanner";
            labelScanner.Size = new Size(114, 64);
            labelScanner.TabIndex = 0;
            labelScanner.Text = "条码枪扫描区:";
            // 
            // txtScanner
            // 
            txtScanner.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtScanner.BackColor = SystemColors.Info;
            txtScanner.Location = new Point(151, 36);
            txtScanner.Margin = new Padding(5, 6, 5, 6);
            txtScanner.Name = "txtScanner";
            txtScanner.Size = new Size(1149, 40);
            txtScanner.TabIndex = 1;
            txtScanner.KeyDown += txtScanner_KeyDown;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(21, 152);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(120, 32);
            label1.TabIndex = 2;
            label1.Text = "物料编码:";
            // 
            // txtGoodsCode
            // 
            txtGoodsCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtGoodsCode.Location = new Point(151, 148);
            txtGoodsCode.Margin = new Padding(5, 6, 5, 6);
            txtGoodsCode.Name = "txtGoodsCode";
            txtGoodsCode.Size = new Size(1149, 40);
            txtGoodsCode.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(21, 264);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(120, 32);
            label2.TabIndex = 4;
            label2.Text = "出库数量:";
            // 
            // txtQty
            // 
            txtQty.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtQty.Location = new Point(151, 260);
            txtQty.Margin = new Padding(5, 6, 5, 6);
            txtQty.Name = "txtQty";
            txtQty.Size = new Size(1149, 40);
            txtQty.TabIndex = 5;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(16, 360);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(125, 64);
            label3.TabIndex = 6;
            label3.Text = "推荐/指定库位:";
            // 
            // txtLocCode
            // 
            txtLocCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtLocCode.Location = new Point(151, 372);
            txtLocCode.Margin = new Padding(5, 6, 5, 6);
            txtLocCode.Name = "txtLocCode";
            txtLocCode.Size = new Size(1149, 40);
            txtLocCode.TabIndex = 7;
            // 
            // cmbStrategy
            // 
            cmbStrategy.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbStrategy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStrategy.FormattingEnabled = true;
            cmbStrategy.Location = new Point(151, 484);
            cmbStrategy.Margin = new Padding(5, 6, 5, 6);
            cmbStrategy.Name = "cmbStrategy";
            cmbStrategy.Size = new Size(1149, 39);
            cmbStrategy.TabIndex = 9;
            // 
            // btnConfirm
            // 
            btnConfirm.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            btnConfirm.Location = new Point(146, 588);
            btnConfirm.Margin = new Padding(0, 28, 0, 0);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(406, 84);
            btnConfirm.TabIndex = 10;
            btnConfirm.Text = "确认出库并通过AGV叫料";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // lblProduct
            // 
            lblProduct.Anchor = AnchorStyles.Right;
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(8, 24);
            lblProduct.Margin = new Padding(5, 0, 5, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(114, 64);
            lblProduct.TabIndex = 0;
            lblProduct.Text = "工单成品编码:";
            // 
            // txtProductCode
            // 
            txtProductCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtProductCode.Location = new Point(132, 36);
            txtProductCode.Margin = new Padding(5, 6, 5, 6);
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Size = new Size(905, 40);
            txtProductCode.TabIndex = 1;
            // 
            // lblProduceQty
            // 
            lblProduceQty.Anchor = AnchorStyles.Right;
            lblProduceQty.AutoSize = true;
            lblProduceQty.Location = new Point(8, 136);
            lblProduceQty.Margin = new Padding(5, 0, 5, 0);
            lblProduceQty.Name = "lblProduceQty";
            lblProduceQty.Size = new Size(114, 64);
            lblProduceQty.TabIndex = 3;
            lblProduceQty.Text = "计划生产数量:";
            // 
            // txtProduceQty
            // 
            txtProduceQty.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtProduceQty.Location = new Point(132, 148);
            txtProduceQty.Margin = new Padding(5, 6, 5, 6);
            txtProduceQty.Name = "txtProduceQty";
            txtProduceQty.Size = new Size(905, 40);
            txtProduceQty.TabIndex = 4;
            // 
            // btnAnalyze
            // 
            btnAnalyze.Dock = DockStyle.Fill;
            btnAnalyze.Location = new Point(1047, 9);
            btnAnalyze.Margin = new Padding(5, 9, 5, 9);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(253, 94);
            btnAnalyze.TabIndex = 2;
            btnAnalyze.Text = "⚡ 分析BOM";
            btnAnalyze.UseVisualStyleBackColor = true;
            btnAnalyze.Click += btnAnalyze_Click;
            // 
            // dgvBOM
            // 
            dgvBOM.AllowUserToAddRows = false;
            dgvBOM.BackgroundColor = SystemColors.Window;
            dgvBOM.ColumnHeadersHeight = 35;
            tableLayoutPanelBOM.SetColumnSpan(dgvBOM, 3);
            dgvBOM.Dock = DockStyle.Fill;
            dgvBOM.Location = new Point(5, 243);
            dgvBOM.Margin = new Padding(5, 19, 5, 19);
            dgvBOM.Name = "dgvBOM";
            dgvBOM.RowHeadersWidth = 51;
            dataGridViewCellStyle1.BackColor = Color.LemonChiffon;
            dgvBOM.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvBOM.Size = new Size(1295, 581);
            dgvBOM.TabIndex = 5;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpStandard);
            tabControl1.Controls.Add(tpBOM);
            tabControl1.Controls.Add(tpAdditional);
            tabControl1.Controls.Add(tpCart);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            tabControl1.Location = new Point(16, 19);
            tabControl1.Margin = new Padding(5, 6, 5, 6);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1377, 961);
            tabControl1.TabIndex = 0;
            // 
            // tpStandard
            // 
            tpStandard.BackColor = SystemColors.Control;
            tpStandard.Controls.Add(tableLayoutPanelStandard);
            tpStandard.Location = new Point(4, 40);
            tpStandard.Margin = new Padding(5, 6, 5, 6);
            tpStandard.Name = "tpStandard";
            tpStandard.Padding = new Padding(32, 37, 32, 37);
            tpStandard.Size = new Size(1369, 917);
            tpStandard.TabIndex = 0;
            tpStandard.Text = "📦 常规单品出库";
            // 
            // tableLayoutPanelStandard
            // 
            tableLayoutPanelStandard.ColumnCount = 2;
            tableLayoutPanelStandard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.2353563F));
            tableLayoutPanelStandard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 88.76464F));
            tableLayoutPanelStandard.Controls.Add(lblStrategy, 0, 4);
            tableLayoutPanelStandard.Controls.Add(labelScanner, 0, 0);
            tableLayoutPanelStandard.Controls.Add(txtScanner, 1, 0);
            tableLayoutPanelStandard.Controls.Add(label1, 0, 1);
            tableLayoutPanelStandard.Controls.Add(txtGoodsCode, 1, 1);
            tableLayoutPanelStandard.Controls.Add(label2, 0, 2);
            tableLayoutPanelStandard.Controls.Add(txtQty, 1, 2);
            tableLayoutPanelStandard.Controls.Add(label3, 0, 3);
            tableLayoutPanelStandard.Controls.Add(txtLocCode, 1, 3);
            tableLayoutPanelStandard.Controls.Add(cmbStrategy, 1, 4);
            tableLayoutPanelStandard.Controls.Add(btnConfirm, 1, 5);
            tableLayoutPanelStandard.Dock = DockStyle.Fill;
            tableLayoutPanelStandard.Location = new Point(32, 37);
            tableLayoutPanelStandard.Margin = new Padding(5, 6, 5, 6);
            tableLayoutPanelStandard.Name = "tableLayoutPanelStandard";
            tableLayoutPanelStandard.RowCount = 6;
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelStandard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelStandard.Size = new Size(1305, 843);
            tableLayoutPanelStandard.TabIndex = 0;
            // 
            // lblStrategy
            // 
            lblStrategy.Anchor = AnchorStyles.Right;
            lblStrategy.AutoSize = true;
            lblStrategy.Location = new Point(21, 488);
            lblStrategy.Margin = new Padding(5, 0, 5, 0);
            lblStrategy.Name = "lblStrategy";
            lblStrategy.Size = new Size(120, 32);
            lblStrategy.TabIndex = 11;
            lblStrategy.Text = "出库策略:";
            // 
            // tpBOM
            // 
            tpBOM.BackColor = SystemColors.Control;
            tpBOM.Controls.Add(tableLayoutPanelBOM);
            tpBOM.Location = new Point(4, 40);
            tpBOM.Margin = new Padding(5, 6, 5, 6);
            tpBOM.Name = "tpBOM";
            tpBOM.Padding = new Padding(32, 37, 32, 37);
            tpBOM.Size = new Size(1369, 917);
            tpBOM.TabIndex = 1;
            tpBOM.Text = "⚙️ 工单 BOM 齐套领料";
            // 
            // tableLayoutPanelBOM
            // 
            tableLayoutPanelBOM.ColumnCount = 3;
            tableLayoutPanelBOM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.797657F));
            tableLayoutPanelBOM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.12779F));
            tableLayoutPanelBOM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelBOM.Controls.Add(btnExecuteBOM, 2, 1);
            tableLayoutPanelBOM.Controls.Add(lblProduct, 0, 0);
            tableLayoutPanelBOM.Controls.Add(txtProductCode, 1, 0);
            tableLayoutPanelBOM.Controls.Add(btnAnalyze, 2, 0);
            tableLayoutPanelBOM.Controls.Add(lblProduceQty, 0, 1);
            tableLayoutPanelBOM.Controls.Add(txtProduceQty, 1, 1);
            tableLayoutPanelBOM.Controls.Add(dgvBOM, 0, 2);
            tableLayoutPanelBOM.Dock = DockStyle.Fill;
            tableLayoutPanelBOM.Location = new Point(32, 37);
            tableLayoutPanelBOM.Margin = new Padding(5, 6, 5, 6);
            tableLayoutPanelBOM.Name = "tableLayoutPanelBOM";
            tableLayoutPanelBOM.RowCount = 3;
            tableLayoutPanelBOM.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelBOM.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelBOM.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelBOM.Size = new Size(1305, 843);
            tableLayoutPanelBOM.TabIndex = 0;
            // 
            // btnExecuteBOM
            // 
            btnExecuteBOM.Dock = DockStyle.Fill;
            btnExecuteBOM.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            btnExecuteBOM.Location = new Point(1047, 121);
            btnExecuteBOM.Margin = new Padding(5, 9, 5, 9);
            btnExecuteBOM.Name = "btnExecuteBOM";
            btnExecuteBOM.Size = new Size(253, 94);
            btnExecuteBOM.TabIndex = 7;
            btnExecuteBOM.Text = "🚀 一键全套出库\r\n并呼叫AGV";
            btnExecuteBOM.UseVisualStyleBackColor = true;
            // 
            // tpAdditional
            // 
            tpAdditional.BackColor = SystemColors.Control;
            tpAdditional.Controls.Add(tableLayoutPanelAdd);
            tpAdditional.Location = new Point(4, 40);
            tpAdditional.Margin = new Padding(5, 6, 5, 6);
            tpAdditional.Name = "tpAdditional";
            tpAdditional.Padding = new Padding(32, 37, 32, 37);
            tpAdditional.Size = new Size(1369, 917);
            tpAdditional.TabIndex = 2;
            tpAdditional.Text = "🚑 产线追加/补料";
            // 
            // tableLayoutPanelAdd
            // 
            tableLayoutPanelAdd.ColumnCount = 2;
            tableLayoutPanelAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.436635F));
            tableLayoutPanelAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 89.56336F));
            tableLayoutPanelAdd.Controls.Add(labelAddWO, 0, 0);
            tableLayoutPanelAdd.Controls.Add(txtAddWO, 1, 0);
            tableLayoutPanelAdd.Controls.Add(labelAddGoods, 0, 1);
            tableLayoutPanelAdd.Controls.Add(txtAddGoodsCode, 1, 1);
            tableLayoutPanelAdd.Controls.Add(labelAddQty, 0, 2);
            tableLayoutPanelAdd.Controls.Add(txtAddQty, 1, 2);
            tableLayoutPanelAdd.Controls.Add(labelAddStrategy, 0, 3);
            tableLayoutPanelAdd.Controls.Add(cmbStrategyAdd, 1, 3);
            tableLayoutPanelAdd.Controls.Add(btnExecuteAdd, 1, 4);
            tableLayoutPanelAdd.Dock = DockStyle.Fill;
            tableLayoutPanelAdd.Location = new Point(32, 37);
            tableLayoutPanelAdd.Margin = new Padding(5, 6, 5, 6);
            tableLayoutPanelAdd.Name = "tableLayoutPanelAdd";
            tableLayoutPanelAdd.RowCount = 5;
            tableLayoutPanelAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            tableLayoutPanelAdd.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelAdd.Size = new Size(1305, 843);
            tableLayoutPanelAdd.TabIndex = 0;
            // 
            // labelAddWO
            // 
            labelAddWO.Anchor = AnchorStyles.Right;
            labelAddWO.AutoSize = true;
            labelAddWO.Location = new Point(9, 24);
            labelAddWO.Margin = new Padding(5, 0, 5, 0);
            labelAddWO.Name = "labelAddWO";
            labelAddWO.Size = new Size(122, 64);
            labelAddWO.TabIndex = 0;
            labelAddWO.Text = "关联工单(选填):";
            // 
            // txtAddWO
            // 
            txtAddWO.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddWO.Location = new Point(141, 36);
            txtAddWO.Margin = new Padding(5, 6, 5, 6);
            txtAddWO.Name = "txtAddWO";
            txtAddWO.Size = new Size(1159, 40);
            txtAddWO.TabIndex = 1;
            // 
            // labelAddGoods
            // 
            labelAddGoods.Anchor = AnchorStyles.Right;
            labelAddGoods.AutoSize = true;
            labelAddGoods.Location = new Point(17, 136);
            labelAddGoods.Margin = new Padding(5, 0, 5, 0);
            labelAddGoods.Name = "labelAddGoods";
            labelAddGoods.Size = new Size(114, 64);
            labelAddGoods.TabIndex = 2;
            labelAddGoods.Text = "追加物料编码:";
            // 
            // txtAddGoodsCode
            // 
            txtAddGoodsCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddGoodsCode.Location = new Point(141, 148);
            txtAddGoodsCode.Margin = new Padding(5, 6, 5, 6);
            txtAddGoodsCode.Name = "txtAddGoodsCode";
            txtAddGoodsCode.Size = new Size(1159, 40);
            txtAddGoodsCode.TabIndex = 3;
            // 
            // labelAddQty
            // 
            labelAddQty.Anchor = AnchorStyles.Right;
            labelAddQty.AutoSize = true;
            labelAddQty.Location = new Point(11, 264);
            labelAddQty.Margin = new Padding(5, 0, 5, 0);
            labelAddQty.Name = "labelAddQty";
            labelAddQty.Size = new Size(120, 32);
            labelAddQty.TabIndex = 4;
            labelAddQty.Text = "追加数量:";
            // 
            // txtAddQty
            // 
            txtAddQty.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddQty.Location = new Point(141, 260);
            txtAddQty.Margin = new Padding(5, 6, 5, 6);
            txtAddQty.Name = "txtAddQty";
            txtAddQty.Size = new Size(1159, 40);
            txtAddQty.TabIndex = 5;
            // 
            // labelAddStrategy
            // 
            labelAddStrategy.Anchor = AnchorStyles.Right;
            labelAddStrategy.AutoSize = true;
            labelAddStrategy.Location = new Point(11, 376);
            labelAddStrategy.Margin = new Padding(5, 0, 5, 0);
            labelAddStrategy.Name = "labelAddStrategy";
            labelAddStrategy.Size = new Size(120, 32);
            labelAddStrategy.TabIndex = 8;
            labelAddStrategy.Text = "出库策略:";
            // 
            // cmbStrategyAdd
            // 
            cmbStrategyAdd.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbStrategyAdd.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStrategyAdd.FormattingEnabled = true;
            cmbStrategyAdd.Location = new Point(141, 372);
            cmbStrategyAdd.Margin = new Padding(5, 6, 5, 6);
            cmbStrategyAdd.Name = "cmbStrategyAdd";
            cmbStrategyAdd.Size = new Size(1159, 39);
            cmbStrategyAdd.TabIndex = 7;
            // 
            // btnExecuteAdd
            // 
            btnExecuteAdd.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold);
            btnExecuteAdd.Location = new Point(136, 476);
            btnExecuteAdd.Margin = new Padding(0, 28, 0, 0);
            btnExecuteAdd.Name = "btnExecuteAdd";
            btnExecuteAdd.Size = new Size(406, 84);
            btnExecuteAdd.TabIndex = 8;
            btnExecuteAdd.Text = "🚑 确认追加领料并呼叫AGV";
            btnExecuteAdd.UseVisualStyleBackColor = true;
            btnExecuteAdd.Click += btnExecuteAdd_Click;
            // 
            // tpCart
            // 
            tpCart.BackColor = SystemColors.Control;
            tpCart.Controls.Add(btnDispatchCart);
            tpCart.Controls.Add(btnBindCart);
            tpCart.Controls.Add(txtCartQty);
            tpCart.Controls.Add(lblCartQty);
            tpCart.Controls.Add(txtCartReel);
            tpCart.Controls.Add(lblCartReel);
            tpCart.Controls.Add(cmbCart);
            tpCart.Controls.Add(lblCart);
            tpCart.Location = new Point(4, 40);
            tpCart.Name = "tpCart";
            tpCart.Padding = new Padding(32, 37, 32, 37);
            tpCart.Size = new Size(1369, 917);
            tpCart.TabIndex = 3;
            tpCart.Text = "\U0001f6d2 移动料车装载发车";
            // 
            // btnDispatchCart
            // 
            btnDispatchCart.BackColor = Color.MediumSeaGreen;
            btnDispatchCart.ForeColor = Color.White;
            btnDispatchCart.Location = new Point(925, 490);
            btnDispatchCart.Name = "btnDispatchCart";
            btnDispatchCart.Size = new Size(347, 142);
            btnDispatchCart.TabIndex = 0;
            btnDispatchCart.Text = "🚀 满载发车(呼叫AGV)";
            btnDispatchCart.UseVisualStyleBackColor = false;
            btnDispatchCart.Click += btnDispatchCart_Click;
            // 
            // btnBindCart
            // 
            btnBindCart.BackColor = Color.SteelBlue;
            btnBindCart.ForeColor = Color.White;
            btnBindCart.Location = new Point(51, 490);
            btnBindCart.Name = "btnBindCart";
            btnBindCart.Size = new Size(347, 142);
            btnBindCart.TabIndex = 1;
            btnBindCart.Text = "🔗 确认装车绑定";
            btnBindCart.UseVisualStyleBackColor = false;
            btnBindCart.Click += btnBindCart_Click;
            // 
            // txtCartQty
            // 
            txtCartQty.Location = new Point(266, 270);
            txtCartQty.Name = "txtCartQty";
            txtCartQty.Size = new Size(1006, 40);
            txtCartQty.TabIndex = 2;
            // 
            // lblCartQty
            // 
            lblCartQty.AutoSize = true;
            lblCartQty.Location = new Point(101, 273);
            lblCartQty.Name = "lblCartQty";
            lblCartQty.Size = new Size(120, 32);
            lblCartQty.TabIndex = 3;
            lblCartQty.Text = "装车数量:";
            // 
            // txtCartReel
            // 
            txtCartReel.Location = new Point(266, 167);
            txtCartReel.Name = "txtCartReel";
            txtCartReel.Size = new Size(1006, 40);
            txtCartReel.TabIndex = 4;
            // 
            // lblCartReel
            // 
            lblCartReel.AutoSize = true;
            lblCartReel.Location = new Point(51, 170);
            lblCartReel.Name = "lblCartReel";
            lblCartReel.Size = new Size(170, 32);
            lblCartReel.TabIndex = 5;
            lblCartReel.Text = "扫描物料条码:";
            // 
            // cmbCart
            // 
            cmbCart.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCart.Location = new Point(266, 68);
            cmbCart.Name = "cmbCart";
            cmbCart.Size = new Size(1006, 39);
            cmbCart.TabIndex = 6;
            // 
            // lblCart
            // 
            lblCart.AutoSize = true;
            lblCart.Location = new Point(51, 71);
            lblCart.Name = "lblCart";
            lblCart.Size = new Size(170, 32);
            lblCart.TabIndex = 7;
            lblCart.Text = "选择空闲料车:";
            // 
            // OutStockForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1409, 999);
            Controls.Add(tabControl1);
            Margin = new Padding(5, 6, 5, 6);
            Name = "OutStockForm";
            Padding = new Padding(16, 19, 16, 19);
            StartPosition = FormStartPosition.CenterParent;
            Text = "智能制造 - 工单出库齐套管理中心";
            ((System.ComponentModel.ISupportInitialize)dgvBOM).EndInit();
            tabControl1.ResumeLayout(false);
            tpStandard.ResumeLayout(false);
            tableLayoutPanelStandard.ResumeLayout(false);
            tableLayoutPanelStandard.PerformLayout();
            tpBOM.ResumeLayout(false);
            tableLayoutPanelBOM.ResumeLayout(false);
            tableLayoutPanelBOM.PerformLayout();
            tpAdditional.ResumeLayout(false);
            tableLayoutPanelAdd.ResumeLayout(false);
            tableLayoutPanelAdd.PerformLayout();
            tpCart.ResumeLayout(false);
            tpCart.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpStandard;
        private System.Windows.Forms.TabPage tpBOM;
        private System.Windows.Forms.TabPage tpAdditional;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStandard;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBOM;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdd;
        private System.Windows.Forms.Label labelScanner;
        private System.Windows.Forms.TextBox txtScanner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLocCode;
        private System.Windows.Forms.Label labelAddStrategy;
        private System.Windows.Forms.ComboBox cmbStrategy;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProduceQty;
        private System.Windows.Forms.TextBox txtProduceQty;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.DataGridView dgvBOM;
        private System.Windows.Forms.Label labelAddWO;
        private System.Windows.Forms.TextBox txtAddWO;
        private System.Windows.Forms.Label labelAddGoods;
        private System.Windows.Forms.TextBox txtAddGoodsCode;
        private System.Windows.Forms.Label labelAddQty;
        private System.Windows.Forms.TextBox txtAddQty;
        private System.Windows.Forms.ComboBox cmbStrategyAdd;
        private System.Windows.Forms.Button btnExecuteAdd;
        private Button btnExecuteBOM;
        private Label lblStrategy;


        // =====================================
        // 以下为料车专属控件声明
        // =====================================
        private System.Windows.Forms.TabPage tpCart;
        private System.Windows.Forms.Label lblCart;
        private System.Windows.Forms.ComboBox cmbCart;
        private System.Windows.Forms.Label lblCartReel;
        private System.Windows.Forms.TextBox txtCartReel;
        private System.Windows.Forms.Label lblCartQty;
        private System.Windows.Forms.TextBox txtCartQty;
        private System.Windows.Forms.Button btnBindCart;
        private System.Windows.Forms.Button btnDispatchCart;
    }
}