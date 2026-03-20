using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 出入库单据表（宏观任务指令）
    /// </summary>
    public class WmsOrder
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = string.Empty; // 单据编号
        public int OrderType { get; set; }                  // 0: 入库单, 1: 出库单
        public int Status { get; set; }                     // 0: 新建待处理, 1: 执行中, 2: 已完成
        public DateTime CreateTime { get; set; }            // 创建时间
    }
}
