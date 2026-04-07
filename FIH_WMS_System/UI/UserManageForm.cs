using System;
using System.Windows.Forms;
using FIH_WMS_System.Services;

namespace FIH_WMS_System.UI
{
    public partial class UserManageForm : Form
    {
        private WmsService wms = new WmsService();

        public UserManageForm()
        {
            InitializeComponent();
            this.Load += UserManageForm_Load;
        }

        private void UserManageForm_Load(object sender, EventArgs e)
        {
            cmbRole.SelectedIndex = 0; // 默认选中"操作员"
            LoadUserData();
        }

        private void LoadUserData()
        {
            dgvUsers.DataSource = null;
            dgvUsers.Columns.Clear();
            dgvUsers.DataSource = wms.GetAllUsers();
        }

        // 新增账号
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text.Trim();
            string p = txtPassword.Text.Trim();
            string r = cmbRole.Text;

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("账号和密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (wms.AddUser(u, p, r))
            {
                MessageBox.Show($"开户成功！新成员【{u}】的身份为：{r}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Clear();
                txtPassword.Clear();
                LoadUserData();
            }
            else
            {
                MessageBox.Show("账号已存在，请换一个账号名！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 删除账号
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            string uName = dgvUsers.SelectedRows[0].Cells["登录账号"].Value.ToString();
            int uId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["用户编号"].Value);

            // 1. 保护超级管理员
            if (uName.ToLower() == "admin"|| uName.ToLower() == "t")
            {
                MessageBox.Show($"超级管理员 【{uName}】 账号禁止被删除！", "越权警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 👇 【新增】：保护当前登录者自己（防止数字自杀）
            if (uName == Program.CurrentUsername)
            {
                MessageBox.Show("危险操作！您不能在系统登录状态下删除自己的账号！\n如需删除，请使用其他管理员账号登录后操作。", "操作拒绝", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // 确认并删除
            if (MessageBox.Show($"您确定要永久删除账号【{uName}】吗？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                wms.DeleteUser(uId);
                LoadUserData();
            }
        }

        // 重置密码
        private void btnResetPwd_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            string uName = dgvUsers.SelectedRows[0].Cells["登录账号"].Value.ToString();
            int uId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["用户编号"].Value);

            if (MessageBox.Show($"您即将重置账号【{uName}】的密码。\n\n系统将其恢复为初始密码：123\n是否继续？", "重置确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                wms.ResetPassword(uId, "123");
                MessageBox.Show($"操作成功！【{uName}】的密码已被重置为 123。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}