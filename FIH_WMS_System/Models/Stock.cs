using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库存 模型
    /// 记录当前仓库里每个库位具体存放了什么物料，以及数量和批次信息
    /// </summary>
    
    [SugarTable("Stock")]// 告诉系统映射数据库的 Stock 表

    public class Stock
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]


        /// <summary>
        /// 主键，数据库自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 物料编码 (与数据库交互的实际存储字段)
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 物料实体对象 (导航属性)
        /// 保留原有的设计，方便在 Services/UI 层直接调用 Stock.Goods.Name
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Goods? Goods { get; set; }

        /// <summary>
        /// 库位编码 (与数据库交互的实际存储字段)
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 库位实体对象 (导航属性)
        /// 保留原有的设计，方便直接获取库位所在区域等信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Location? Location { get; set; }

        /// <summary>
        /// 当前可用库存数量 (实际放在物理货架上的数量)
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 批次号，用于追踪同一批生产的物料
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 生产日期，用于“按生产日期先入先出”的规则引擎计算
        /// </summary>
        public DateTime? ProduceDate { get; set; }

        /// <summary>
        /// 该物料最初的入库时间，用于“先进先出(FIFO)”规则计算
        /// </summary>
        public DateTime InStockTime { get; set; }

        /// <summary>
        /// 【新增字段】唯一条码编号 (ReelId)
        /// 满足文档要求：支持原厂物料条码直接识别和追踪，一物一码
        /// </summary>
        public string ReelId { get; set; }

        /// <summary>
        /// 【新增字段】冻结数量 
        /// 作用：当出库单已经生成，但AGV或工人还没把货拿走时，先“冻结”这部分数量。
        /// 真正可用的数量 = Qty - FrozenQty。这样可以防止同一个库位的物料被分配给两个不同的订单。
        /// </summary>
        public int FrozenQty { get; set; } = 0;
    }
}