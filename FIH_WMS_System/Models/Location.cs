using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库位 模型
    /// 库位信息表
    /// </summary>
    public class Location
    {
        public int Id { get; set; }

        /// <summary>
        /// 库位编码 (如：A-01-01)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 所属区域 (如：A区, SMT区)
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 是否被使用 (旧版字段保留，兼容老代码)
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 【新增字段】库位高级状态
        /// 0:正常空闲, 1:已被占用(部分或全部), 2:被锁定(如正在盘点), 3:停用(如货架损坏维修)
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
