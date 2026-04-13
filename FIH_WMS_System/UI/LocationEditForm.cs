using System;
using System.Windows.Forms;

namespace FIH_WMS_System.UI
{
    // 定义枚举告诉窗体作用
    // SingleAdd：新增单个库位，BatchAdd：批量扩建库位，Edit：编辑现有库位

    //在MapForm库位管理员进行库位操作
    public enum LocationEditMode { SingleAdd, BatchAdd, Edit }

    public partial class LocationEditForm : Form
    {
        // 暴露出去给外面的 MapForm 读取的“口袋”
        public string InputCode { get; private set; }
        public string InputArea { get; private set; }
        public int InputCapacity { get; private set; }
        public int InputBatchCount { get; private set; }

        public LocationEditForm(LocationEditMode mode, string defaultCode = "", string defaultArea = "A区", int defaultCap = 500)
        {
            InitializeComponent();

            // 根据模式自动变形
            switch (mode)
            {
                case LocationEditMode.SingleAdd:
                    this.Text = "➕ 新增单库位";
                    lblBatchCount.Visible = numBatchCount.Visible = false;
                    break;
                case LocationEditMode.BatchAdd:
                    this.Text = "🚀 批量扩建库位";
                    lblCode.Text = "库位前缀：";
                    txtCode.PlaceholderText = "如 A-02-"; // 提示用户
                    break;
                case LocationEditMode.Edit:
                    this.Text = "✏️ 编辑库位信息";
                    lblBatchCount.Visible = numBatchCount.Visible = false;
                    break;
            }

            // 填充默认值
            txtCode.Text = defaultCode;
            txtArea.Text = defaultArea;
            numCapacity.Value = defaultCap;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("库位编码/前缀不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 把值装进口袋里
            InputCode = txtCode.Text.Trim();
            InputArea = txtArea.Text.Trim();
            InputCapacity = (int)numCapacity.Value;
            InputBatchCount = (int)numBatchCount.Value;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}