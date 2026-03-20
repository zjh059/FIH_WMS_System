namespace FIH_WMS_System.UI
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微软雅黑", 16F, FontStyle.Bold);
            label1.Location = new Point(83, 30);
            label1.Name = "label1";
            label1.Size = new Size(402, 50);
            label1.TabIndex = 5;
            label1.Text = "FIH 智能仓储登录系统";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(120, 110);
            label2.Name = "label2";
            label2.Size = new Size(117, 28);
            label2.TabIndex = 4;
            label2.Text = "登录账号：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(120, 170);
            label3.Name = "label3";
            label3.Size = new Size(117, 28);
            label3.TabIndex = 3;
            label3.Text = "登录密码：";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(256, 107);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(180, 34);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(256, 167);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(180, 34);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(187, 235);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(160, 45);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "🚀 登录系统";
            btnLogin.Click += btnLogin_Click;
            // 
            // LoginForm
            // 
            ClientSize = new Size(584, 320);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "系统登录";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
    }
}