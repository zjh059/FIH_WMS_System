using System;
using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 系统操作审计日志表
    /// 用于记录用户的关键操作行为
    /// </summary>
    [SugarTable("SysOperationLog")]
    public class SysOperationLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 操作人账号
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 操作模块/类型 (如：系统登录、基础数据、权限管理等)
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// 具体操作描述 (如：新增了物料编码 G005)
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OperateTime { get; set; } = DateTime.Now;
    }
}