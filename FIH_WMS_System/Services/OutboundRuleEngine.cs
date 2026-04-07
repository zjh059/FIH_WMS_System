using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIH_WMS_System.Models;

namespace FIH_WMS_System.Services
{
    /// <summary>
    /// 出库规则策略枚举
    /// 对应 IWMCS系统功能介绍 文档中的出库规格设定
    /// </summary>
    public enum OutboundStrategy
    {
        Manual = 1,                 // 直接人工指定库位出库
        FIFO = 2,                   // 采用先进先出原则出库 (First In First Out)
        LIFO = 3,                   // 采用后进先出原则出库 (Last In First Out)
        NearestFirst = 4,           // 采用就近原则出库

        LeastQuantityFirst = 5,     // 存量最少优先 (优先清空物料极少的零星碎片库位)
        MostQuantityFirst = 6       // 存量充足优先 (集中大批量出库，减少搬运次数)
    }

    /// <summary>
    /// 智能出库规则引擎
    /// </summary>
    public class OutboundRuleEngine
    {
        /// <summary>
        /// 智能排序出库库存推荐
        /// </summary>
        /// <param name="goodsCode">要出库的物料编码</param>
        /// <param name="currentStocks">当前系统中的所有可用库存</param>
        /// <param name="strategy">出库策略</param>
        /// <returns>按照优先级排好序的库存列表 (排在越前面的越应该先出库)</returns>
        

        //public List<Stock> RecommendOutboundStocks(string goodsCode, List<Stock> currentStocks, OutboundStrategy strategy)
        //加入了可选参数 allLocations(库位列表) 以及 targetX, targetY (产线 / 交接点坐标)
        public List<Stock> RecommendOutboundStocks(string goodsCode, List<Stock> currentStocks, OutboundStrategy strategy, List<Location> allLocations = null, int targetX = 0, int targetY = 0)
        {
            // 1. 先过滤出含有该物料，且实际可用数量大于0的库存 (Qty - FrozenQty 是真实可用的)
            var availableStocks = currentStocks
                .Where(s => s.GoodsCode == goodsCode && (s.Qty - s.FrozenQty) > 0)
                .ToList();

            if (availableStocks.Count == 0) return new List<Stock>(); // 没货了

            // 2. 根据不同策略进行排序推荐
            switch (strategy)
            {
                case OutboundStrategy.FIFO:
                    // 先进先出：入库时间越早的，排在越前面
                    //return availableStocks.OrderBy(s => s.InStockTime).ToList();
                    // 先进先出：入库时间越早的排前面。如果时间一样，优先出数量少的（清空碎片货架）
                    //return availableStocks.OrderBy(s => s.InStockTime).ThenBy(s => s.Qty).ToList();
                    //优先看生产日期(FEFO防过期机制)，没生产日期的再看入库时间。
                    return availableStocks.OrderBy(s => s.ProduceDate ?? s.InStockTime).ThenBy(s => s.Qty).ToList();

                case OutboundStrategy.LIFO:
                    // 后进先出：入库时间越晚的，排在越前面
                    return availableStocks.OrderByDescending(s => s.InStockTime).ToList();

                case OutboundStrategy.NearestFirst:
                    // 就近原则：假设库位编码越小离出口越近 (比如 A-01 优先于 B-01)
                    //return availableStocks.OrderBy(s => s.LocationCode).ToList();
                    // 就近原则：如果提供了库位坐标，则计算距离，距离近的排前面；否则按库位编码排序
                    //如果有传入库位表，计算物理真实坐标；否则退化为原来的按字符串编码排序
                    if (allLocations != null && allLocations.Any())
                    {
                        return availableStocks
                            .Join(allLocations, s => s.LocationCode, l => l.Code, (s, l) => new { Stock = s, Loc = l })
                            .OrderBy(x => Math.Pow(x.Loc.PosX - targetX, 2) + Math.Pow(x.Loc.PosY - targetY, 2))
                            .Select(x => x.Stock)
                            .ToList();
                    }
                    return availableStocks.OrderBy(s => s.LocationCode).ToList();



                case OutboundStrategy.LeastQuantityFirst:
                    //按数量从小到大排序。数量越少排越前，优先把快空了的货架搬空
                    return availableStocks.OrderBy(s => s.Qty).ThenBy(s => s.InStockTime).ToList();

                case OutboundStrategy.MostQuantityFirst:
                    //按数量从大到小排序。数量越多排越前，适合一次性大批量出库
                    return availableStocks.OrderByDescending(s => s.Qty).ToList();



                case OutboundStrategy.Manual://人工排序
                default:
                    // 人工指定时不进行特定智能排序，保持原样
                    return availableStocks;
            }
        }



    }
}