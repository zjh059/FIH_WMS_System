using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// AGV小车调度任务表 (WCS控制层)
    /// </summary>
    public class AgvTask
    {
        public int Id { get; set; }
        public string TaskNo { get; set; } = string.Empty;

        /// <summary>
        /// 0:入库, 1:出库, 2:移库
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 0:待下发, 1:前往取货, 2:运送中, 3:已完成, 4:故障
        /// </summary>
        public int Status { get; set; }

        public string GoodsCode { get; set; } = string.Empty;//要搬运的物料
        public int Qty { get; set; }                        //要搬运的数量

        public string FromLocation { get; set; } = string.Empty;//起始地点 (如 A-01-01)
        public string ToLocation { get; set; } = string.Empty;//目标地点 (如 产线接驳口1)

        public DateTime CreateTime { get; set; }            //指令下发时间
        public DateTime? FinishTime { get; set; }           //指令完成时间

        // 导航属性：方便在界面上显示物料的中文名
        public Goods? Goods { get; set; }
    }
}
