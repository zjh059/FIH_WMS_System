

namespace FIH_WMS_System
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>


        // 增加一个全局属性，用来记录当前是谁登录了系统
        public static string CurrentUsername = "";
        public static string CurrentRole = "";

        // 【新增】：全局系统参数
        public static bool EnableVoiceBroadcast = true; // 语音播报开关 (默认开启)
        public static int AgvRefreshInterval = 3000;    // AGV监控台刷新频率 (毫秒)

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. 先把登录窗口弹出来
            UI.LoginForm login = new UI.LoginForm();

            // 2. 如果登录窗口返回的是 OK (说明账号密码对了)
            if (login.ShowDialog() == DialogResult.OK)
            {
                // 3. 才允许启动主界面的大门！
                Application.Run(new MainForm());
            }
            else
            {
                // 否则直接退出程序
                Application.Exit();
            }
        }
    }



}