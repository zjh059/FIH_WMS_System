using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FIH_WMS_System.Models; // 引用 User 类型所在命名空间

namespace FIH_WMS_System.UI
{
    /// <summary>
    /// 登录界面
    /// </summary>
    public partial class LoginForm : Form
    {
        private Services.WmsService wms = new Services.WmsService();

        public LoginForm()
        {
            Utils.VoiceHelper.Speak("欢迎来到富士康智能仓储管理系统，请输入账号和密码！");
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pwd = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("账号和密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 调用大脑去校验
            var loginUser = wms.Login(user, pwd);

            if (loginUser is not null)
            {
                // 登录成功！把他的名字和角色记在全局变量里
                Program.CurrentUsername = loginUser.Username;
                Program.CurrentRole = loginUser.Role;

                //  触发语音播报
                Utils.VoiceHelper.Speak($"欢迎回来，{loginUser.Role}，{loginUser.Username}！");

                // 记录登录日志
                wms.AddOperationLog("系统登录", $"账号登录成功，角色：{loginUser.Role}");

                this.DialogResult = DialogResult.OK; // 告诉系统登录过关了
            }
            else
            {
                MessageBox.Show("账号或密码错误，请重试！", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}