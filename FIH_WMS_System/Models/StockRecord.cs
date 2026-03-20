using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIH_WMS_System.Models
{
    /// <summary>
    /// 出入库记录
    /// </summary>
    public class StockRecord
    {
        /*
        public int Id { get; set; }
        public RecordType Type { get; set; }
        public Goods? Goods { get; set; }
        public Location? Location { get; set; }
        public int Qty { get; set; }
        public DateTime OperateTime { get; set; }
        public string Operator { get; set; } = "admin";*/

        public int Id { get; set; }
        public string OrderNo { get; set; } = string.Empty; // 【新增】关联的单据号，表明这笔流水是因为执行哪个订单产生的
        public RecordType Type { get; set; }                // 0:入库, 1:出库 (对应RecordType枚举)

        public Goods? Goods { get; set; }                   //商品编码
        public Location? Location { get; set; }             //库位编码

        public int Qty { get; set; }                        //变动数量
        public string BatchNo { get; set; } = string.Empty; // 【新增】批次号，记录具体动了哪个批次的货

        public DateTime OperateTime { get; set; }           //操作时间，默认当前系统时间
        public string Operator { get; set; } = "admin";     //操作人，默认admin，实际使用中可以改为当前登录用户
    }
}
