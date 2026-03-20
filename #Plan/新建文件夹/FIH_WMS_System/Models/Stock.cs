using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库存
    /// </summary>
    public class Stock
    {
        public int Id { get; set; }// 库存编号
        public Goods? Goods { get; set; }// 商品名称
        public Location? Location { get; set; }// 所在库位
        public int Qty { get; set; }                    // 库存数量


        // 👇【新增的三个核心字段】
        public string BatchNo { get; set; } = string.Empty; // 批次号
        public DateTime? ProduceDate { get; set; }          // 生产日期 (允许为空，因为有些商品可能没生产日期)
        public DateTime InStockTime { get; set; }           // 入库时间
    }

}
