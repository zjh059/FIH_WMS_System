using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

//产品BOM表
//这个类用于告诉系统，组装一个成品需要哪些原材料，以及分别需要多少个。
namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 产品物料清单 (BOM) 模型
    /// 用于维护成品与原材料之间的构成关系
    /// </summary>

    [SugarTable("ProductBOM")]  // 告诉系统映射数据库的 ProductBOM 表
    public class ProductBOM
    {
        // 告诉系统这是主键(IsPrimaryKey)，并且是数据库自动增加的(IsIdentity)
        // 主键及自增标识
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 父产品编号 (例如：成品手机的编码)
        /// </summary>
        public string ParentGoodsCode { get; set; }

        /// <summary>
        /// 子物料编号 (例如：屏幕、螺丝的编码)
        /// </summary>
        public string ChildGoodsCode { get; set; }

        /// <summary>
        /// 组成一个父产品所需的子物料数量
        /// 使用 decimal 防止出现半个（比如 0.5kg 的锡膏）
        /// </summary>
        public decimal RequiredQty { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}