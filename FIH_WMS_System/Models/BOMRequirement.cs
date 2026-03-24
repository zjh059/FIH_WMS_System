using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FIH_WMS_System.Models
{
    /// <summary>
    /// BOM 展开与齐套性分析 模型
    /// </summary>
    public class BOMRequirement
    {
        public string ChildGoodsCode { get; set; }     // 子物料编码 (如: 原材料A)
        public string ChildGoodsName { get; set; }     // 子物料名称
        public int RequiredTotalQty { get; set; }      // 生产该批次总共需要的数量
        public int CurrentAvailableQty { get; set; }   // 仓库当前实际可用的数量 (扣除已冻结的)

        /// <summary>
        /// 齐套性检测：当前库存够不够用？
        /// </summary>
        public bool IsEnough => CurrentAvailableQty >= RequiredTotalQty;
    }
}