using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{

    /// <summary>
    /// 物料信息表
    /// </summary>
    public class Goods
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;    // 物料编码
        public string Name { get; set; } = string.Empty;    // 名称
        public string Spec { get; set; } = string.Empty;    // 规格
        public decimal Price { get; set; }                  // 单价
        public string Category { get; set; } = string.Empty;// 物料分类

        public int SafeQty { get; set; }// 安全库存报警线
    }
}
