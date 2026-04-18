using System;
using SqlSugar;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 移动料车主表
    /// </summary>
    [SugarTable("MobileCart")]
    public class MobileCart
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 料车编号 (如: CART-001)
        /// </summary>
        public string CartNo { get; set; } = string.Empty;

        /// <summary>
        /// 料车状态：0=空闲, 1=装载中, 2=满载待命, 3=运输中
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 料车当前所在物理位置
        /// </summary>
        public string CurrentLocation { get; set; } = string.Empty;
    }
}