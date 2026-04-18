using System;
using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 料车装载明细表 (记录车上具体放了什么料)
    /// </summary>
    [SugarTable("MobileCartDetail")]
    public class MobileCartDetail
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 关联的料车编号
        /// </summary>
        public string CartNo { get; set; } = string.Empty;

        /// <summary>
        /// 物料编码
        /// </summary>
        public string GoodsCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料的唯一追溯码(ReelId)，支持一物一码精确定位
        /// </summary>
        public string ReelId { get; set; } = string.Empty;

        /// <summary>
        /// 装载数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 扫码装车的时间
        /// </summary>
        public DateTime LoadTime { get; set; } = DateTime.Now;
    }
}