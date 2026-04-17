using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 出入库单据表（宏观任务指令）
    /// </summary>

    [SugarTable("WmsOrder")]// 告诉系统映射数据库的 WmsOrder 表
    public class WmsOrder
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string OrderNo { get; set; } = string.Empty; // 单据编号
        public int OrderType { get; set; }                  // 0: 入库单, 1: 出库单
        public int Status { get; set; }                     // 0: 新建待处理, 1: 执行中, 2: 已完成
        public DateTime CreateTime { get; set; }            // 创建时间

        
        public string WaveNo { get; set; }                  //波次号字段 (可为空)
    }
}
