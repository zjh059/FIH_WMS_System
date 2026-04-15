using System;
using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// AGV小车运行日志轨迹表
    /// </summary>
    [SugarTable("AgvLog")]
    public class AgvLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 关联的调度任务单号
        /// </summary>
        public string TaskNo { get; set; } = string.Empty;

        /// <summary>
        /// 动作指令 (如：接收任务、到达取货点、到达终点、开始充电)
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// 当前所在物理位置
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime LogTime { get; set; } = DateTime.Now;
    }
}