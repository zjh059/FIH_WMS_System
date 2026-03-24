using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 盘点清单明细 模型
    /// 用于在盘点界面展示账面数量与实物数量的对比
    /// </summary>
    public class StockCountItem
    {
        public int StockId { get; set; }          // 唯一库存ID
        public string GoodsCode { get; set; }     // 物料编码
        public string GoodsName { get; set; }     // 物料名称
        public string LocationCode { get; set; }  // 所在库位
        public string BatchNo { get; set; }       // 批次号

        public int SystemQty { get; set; }        // 电脑账面数量

        /// <summary>
        /// 实际盘点数量 (默认初始值等于系统数量，用户可在表格中修改)
        /// </summary>
        public int PhysicalQty { get; set; }

        /// <summary>
        /// 差异数量 (实物 - 账面。正数代表盘盈，负数代表盘亏)
        /// 只读属性，自动计算
        /// </summary>
        public int Difference => PhysicalQty - SystemQty;
    }
}