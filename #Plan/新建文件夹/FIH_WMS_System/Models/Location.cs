using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 库位信息表
    /// </summary>
    public class Location
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // 库位编码 A-01-01
        public string Area { get; set; } = string.Empty; // 库区
        public bool IsUsed { get; set; } = false;        // 是否占用
    }

}
