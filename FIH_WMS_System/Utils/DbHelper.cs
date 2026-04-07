using SqlSugar;
using System;

namespace FIH_WMS_System.Utils
{
    /// <summary>
    /// 全局数据库操作大管家 (基于 SqlSugar)
    /// </summary>
    public class DbHelper
    {
        // ⚠️ 默认使用 Windows 身份验证连接本机的 SQL Server。
        // 若 SQL Server 需要账号密码，改成类似："Server=.;Database=FIH_WMS_DB;User ID=sa;Password=密码;TrustServerCertificate=True;"
        //public static readonly string ConnectionString = "Server=.;Database=FIH_WMS_DB;Trusted_Connection=True;TrustServerCertificate=True;";
        public static readonly string ConnectionString = "Server=.;Database=FIH_WMS_DB;User ID=sa;Password=123456;TrustServerCertificate=True;";

        /// <summary>
        /// 获取数据库操作客户端 (自带连接池自动释放，极其稳定)
        /// </summary>
        public static SqlSugarClient Db
        {
            get
            {
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = ConnectionString,
                    DbType = DbType.SqlServer,       // 指定数据库类型为 SQL Server
                    IsAutoCloseConnection = true,    // 自动释放和关闭连接，极大减少内存泄漏
                    InitKeyType = InitKeyType.Attribute // 告诉 SqlSugar 从实体类的特性中读取主键信息
                });

                // 可选择，在这里打印执行的 SQL 语句，方便开发联调时排错
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    System.Diagnostics.Debug.WriteLine(sql); // 会在 VS 的输出窗口看到底层跑了什么 SQL
                };

                return db;
            }
        }
    }
}