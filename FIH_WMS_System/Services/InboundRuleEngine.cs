using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using FIH_WMS_System.Models;

namespace FIH_WMS_System.Services
{
    /// <summary>
    /// 入库规则策略枚举InboundStrategy
    /// 对应 IWMCS系统功能介绍 文档中的入库规格设定
    /// </summary>
    public enum InboundStrategy
    {
        /// <summary>
        /// 1. 直接人工指定 (引擎不分配，由人工扫码决定)
        /// </summary>
        Manual = 1,

        /// <summary>
        /// 2. 按未满库位入库 (优先找已经存有相同物料的库位，合并存放，减少库位碎片)
        /// </summary>
        SameMaterialMerge = 2,

        /// <summary>
        /// 3. 按空库位入库 (优先找一个纯空的库位，不与其他物料混放)
        /// </summary>
        EmptyLocationFirst = 3,

        /// <summary>
        /// 4. 按就近库位原则入库 (根据库位编码顺序分配，减少AGV或人工走动时间)
        /// </summary>
        NearestFirst = 4
    }

    /// <summary>
    /// 智能入库规则引擎
    /// 负责为即将入库的物料推荐最佳库位
    /// </summary>
    public class InboundRuleEngine
    {
        /// <summary>
        /// 核心方法：智能推荐入库库位
        /// </summary>
        /// <param name="goodsCode">要入库的物料编码</param>
        /// <param name="allLocations">系统中所有的库位列表</param>
        /// <param name="currentStocks">系统中当前的实时库存列表</param>
        /// <param name="strategy">选择的分配策略</param>
        /// <returns>推荐的库位对象，如果没有可用库位则返回 null</returns>
        public Location RecommendLocation(string goodsCode, List<Location> allLocations, List<Stock> currentStocks, InboundStrategy strategy)
        {
            // 第一步：过滤出所有“状态正常”的库位 (Status == 0 表示正常空闲/可用，排除被锁定或停用的库位)
            // 兼容原有 IsUsed 字段的逻辑
            var validLocations = allLocations.Where(loc => loc.Status == 0).ToList();

            if (validLocations.Count == 0) return null; // 仓库没有可用库位了

            switch (strategy)
            {
                case InboundStrategy.SameMaterialMerge:
                    return GetSameMaterialMergeLocation(goodsCode, validLocations, currentStocks)
                           ?? GetEmptyLocation(validLocations, currentStocks); // 如果找不到同类物料库位，就退化为找空库位

                case InboundStrategy.EmptyLocationFirst:
                    return GetEmptyLocation(validLocations, currentStocks);

                case InboundStrategy.NearestFirst:
                    return GetNearestLocation(validLocations, currentStocks);

                case InboundStrategy.Manual:
                default:
                    return null; // 人工指定时不进行系统推荐
            }
        }

        /// <summary>
        /// 策略 A：找含有相同物料的库位合并 (减少碎片)
        /// </summary>
        private Location GetSameMaterialMergeLocation(string goodsCode, List<Location> validLocations, List<Stock> currentStocks)
        {
            // 找出存放了该物料的所有库存记录
            var existingStocks = currentStocks.Where(s => s.GoodsCode == goodsCode && s.Qty > 0).ToList();

            // 提取这些库存所在的库位编码
            var occupiedLocationCodes = existingStocks.Select(s => s.LocationCode).Distinct().ToList();

            // 在有效库位中，找到这些已经被该物料占用的库位
            var mergeTarget = validLocations.FirstOrDefault(loc => occupiedLocationCodes.Contains(loc.Code));
            return mergeTarget;
        }

        /// <summary>
        /// 策略 B：找一个完全为空的库位
        /// </summary>
        private Location GetEmptyLocation(List<Location> validLocations, List<Stock> currentStocks)
        {
            // 提取所有有库存的库位编码 (不管是哪种物料，只要数量大于0就算被占用)
            var occupiedLocationCodes = currentStocks.Where(s => s.Qty > 0).Select(s => s.LocationCode).Distinct().ToList();

            // 在有效库位中，排除掉被占用的，返回第一个纯空的库位
            // (这里可以配合 IsUsed 字段，为了严谨，我们从库存反查真实空库位)
            var emptyLocation = validLocations.FirstOrDefault(loc => !occupiedLocationCodes.Contains(loc.Code));
            return emptyLocation;
        }

        /// <summary>
        /// 策略 C：按就近原则入库
        /// 假设：库位编码(如 A-01-01)按字母数字顺序排序，越靠前的离入口越近
        /// </summary>
        private Location GetNearestLocation(List<Location> validLocations, List<Stock> currentStocks)
        {
            // 先找出所有的纯空库位
            var occupiedLocationCodes = currentStocks.Where(s => s.Qty > 0).Select(s => s.LocationCode).Distinct().ToList();
            var emptyLocations = validLocations.Where(loc => !occupiedLocationCodes.Contains(loc.Code)).ToList();

            // 按编码升序排列 (比如 A-01-01 优先于 B-01-01)，取最前面的一个
            var nearestLocation = emptyLocations.OrderBy(loc => loc.Code).FirstOrDefault();
            return nearestLocation;
        }
    }



}