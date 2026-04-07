using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库存操作记录(流水账) 模型
    /// 无论入库、出库、移库、盘点，任何库存数量的变化都要在这里记录一笔
    /// </summary>

    [SugarTable("StockRecord")]  // 告诉系统映射数据库的 StockRecord 表
    public class StockRecord
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        /// <summary>
        /// 主键，数据库自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关联的单据号 (比如入库单号、出库单号或盘点单号)
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 记录类型 (如：0表示入库, 1表示出库, 2表示盘点调整等)
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 【关键修复】：兼容原有 UI 代码的 Type 属性，做个巧妙的转换
        /// 这样 MainForm.cs 里的 r.Type 就能继续正常工作
        /// </summary>
        public RecordType Type
        {
            get { return (RecordType)this.RecordType; }
            set { this.RecordType = (int)value; }
        }



        /// <summary>
        /// 物料编码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 物料实体对象 (导航属性，方便UI显示)
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Goods? Goods { get; set; }

        /// <summary>
        /// 操作涉及的库位编码
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string LocationCode { get; set; }

        /// <summary>
        /// 库位实体对象 (导航属性，方便UI显示)
        /// </summary>
        public Location? Location { get; set; }

        /// <summary>
        /// 本次操作变动的数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 变动涉及的批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 本次操作发生的时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作人 (比如是具体的员工账号，或者是"System"/"AGV"代表自动操作)
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 【新增字段】本次操作对应的唯一条码编号 (ReelId)
        /// 方便后续追踪某个特定条码的完整生命周期(什么时候入库，什么时候出库)
        /// </summary>
        public string ReelId { get; set; }
    }
}
