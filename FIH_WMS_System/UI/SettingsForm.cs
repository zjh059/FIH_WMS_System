using System;
using System.Windows.Forms;

namespace FIH_WMS_System.UI
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 1. 读取当前内存里的参数，展示在界面上
            chkVoice.Checked = Program.EnableVoiceBroadcast;

            if (Program.AgvRefreshInterval == 1000) cmbAgvSpeed.SelectedIndex = 0;
            else if (Program.AgvRefreshInterval == 3000) cmbAgvSpeed.SelectedIndex = 1;
            else cmbAgvSpeed.SelectedIndex = 2;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 2. 将用户在界面的修改，写回到全局变量里
            Program.EnableVoiceBroadcast = chkVoice.Checked;

            if (cmbAgvSpeed.SelectedIndex == 0) Program.AgvRefreshInterval = 1000;
            else if (cmbAgvSpeed.SelectedIndex == 1) Program.AgvRefreshInterval = 3000;
            else Program.AgvRefreshInterval = 5000;

            MessageBox.Show("系统参数已保存并实时生效！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}