using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

//订单明细表
//之前我们只有订单的“皮”（WmsOrder 表头），现在加入订单的“瓤”（明细），系统才知道具体要搬什么货。
namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 仓储单据明细 模型
    /// 记录每次出库/入库任务的具体物料和数量要求
    /// </summary>

    [SugarTable("WmsOrderDetail")]// 告诉系统映射数据库的 WmsOrderDetail 表
    public class WmsOrderDetail
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 关联 WmsOrder 表的单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 需要处理的物料编码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 计划需要出库/入库的数量
        /// </summary>
        public int PlanQty { get; set; }

        /// <summary>
        /// 实际已经出库/入库的数量 (初始为0)
        /// </summary>
        public int ActualQty { get; set; }

        /// <summary>
        /// 单行明细的执行状态：0=待处理, 1=部分完成, 2=已完成
        /// </summary>
        public int Status { get; set; } = 0;
    }
}