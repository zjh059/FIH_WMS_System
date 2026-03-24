using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库位合并(碎片整理)建议 模型
    /// </summary>
    public class ConsolidationAdvice
    {
        public string GoodsCode { get; set; }      // 物料编码
        public string GoodsName { get; set; }      // 物料名称
        public string FromLocation { get; set; }   // 建议搬出的库位 (零星碎片的源头)
        public string ToLocation { get; set; }     // 建议搬入的库位 (数量较多的大本营)
        public int MoveQty { get; set; }           // 建议搬运的数量
    }
}