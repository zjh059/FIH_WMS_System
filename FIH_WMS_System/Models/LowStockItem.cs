using System;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库存预警明细模型
    /// </summary>
    public class LowStockItem
    {
        public string GoodsCode { get; set; }
        public string GoodsName { get; set; }
        public int SafeQty { get; set; }
        public int CurrentTotalQty { get; set; }
    }
}