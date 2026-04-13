using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;


namespace FIH_WMS_System.Models
{

    /// <summary>
    /// 物料信息表
    /// </summary>

    [SugarTable("Goods")]  // 告诉系统映射数据库的 Goods 表

    public class Goods
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;    // 物料编码
        public string Name { get; set; } = string.Empty;    // 名称
        public string Spec { get; set; } = string.Empty;    // 规格
        public decimal Price { get; set; }                  // 单价
        public string Category { get; set; } = string.Empty;// 物料分类

        public int SafeQty { get; set; }// 安全库存报警线

        
        public string Brand { get; set; } = string.Empty;//品牌字段

        public int ShelfLifeDays { get; set; }//新增：保质期(天数)。0代表无保质期限制
    }
}

