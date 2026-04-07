using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 系统用户表
    /// </summary>

    [SugarTable("User")]// 告诉系统映射数据库的 User 表
    public class User
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
