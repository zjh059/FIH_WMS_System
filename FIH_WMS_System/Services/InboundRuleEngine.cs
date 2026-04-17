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
        NearestFirst = 4,

        UsageFrequency = 5, // 新增：按使用频率入库 (智能冷热分区)


        ByCategory = 6, // 按物料分类、规格、品牌等分类入库 (同类集中)
        ByWave = 7      // 按波次入库 (预留给后续波次入库的高级应用)
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
        /// <summary>
        /// 核心方法：智能推荐入库库位
        /// </summary>
        //  增加 inQty, agvX, agvY 三个参数
        // 在最后增加 DateTime? produceDate = null
        public Location RecommendLocation(string goodsCode, int inQty, int agvX, int agvY, List<Location> allLocations, List<Stock> currentStocks, InboundStrategy strategy, DateTime? produceDate = null)
        {
            // 第一步：过滤出所有“状态正常”的库位 (Status == 0 表示正常空闲/可用，排除被锁定或停用的库位)
            // 兼容原有 IsUsed 字段的逻辑
            var validLocations = allLocations.Where(loc => loc.Status == 0).ToList();

            if (validLocations.Count == 0) return null; // 仓库没有可用库位了

            switch (strategy)
            {
                case InboundStrategy.SameMaterialMerge:
                    // 【修改此处】：传入 inQty
                    //return GetSameMaterialMergeLocation(goodsCode, validLocations, currentStocks)
                    return GetSameMaterialMergeLocation(goodsCode, inQty, validLocations, currentStocks)
                           ?? GetEmptyLocation(validLocations, currentStocks); // 如果找不到同类物料库位，就退化为找空库位

                case InboundStrategy.EmptyLocationFirst:
                    return GetEmptyLocation(validLocations, currentStocks);

                case InboundStrategy.NearestFirst:
                    // 【修改此处】：传入 agvX, agvY
                    //return GetNearestLocation(validLocations, currentStocks);
                    return GetNearestLocation(agvX, agvY, validLocations, currentStocks);

                //冷热分区策略：根据物料的使用频率（出库频率）来决定入库位置，常用的放在更靠近出口的位置，减少后续出库时间
                case InboundStrategy.UsageFrequency:
                    return GetLocationByUsageFrequency(goodsCode, agvX, agvY, validLocations, currentStocks);



                //新增：按物料分类、规格等特征集中存放的策略分支
                // 如果同类物料的货架都满了，就找一个纯空的新库位开辟新领地
                case InboundStrategy.ByCategory:
                    //return GetLocationByCategory(goodsCode, inQty, validLocations, currentStocks);
                    //替换为多维度匹配方法
                    //return GetLocationByMultiDimensions(goodsCode, inQty, validLocations, currentStocks);
                    //把 produceDate 传给多维度匹配方法
                    return GetLocationByMultiDimensions(goodsCode, inQty, validLocations, currentStocks, produceDate);

                //  新增波次入库方法
                case InboundStrategy.ByWave:
                    return GetLocationByWave(goodsCode, validLocations, currentStocks);



                case InboundStrategy.Manual:
                default:
                    return null; // 人工指定时不进行系统推荐
            }
        }

        /// <summary>
        /// 策略 A：找含有相同物料的库位合并 (减少碎片)
        /// </summary>
        private Location GetSameMaterialMergeLocation(string goodsCode, int inQty, List<Location> validLocations, List<Stock> currentStocks) //增加 inQty 参数
        {
            // 找出存放了该物料的所有库存记录
            var existingStocks = currentStocks.Where(s => s.GoodsCode == goodsCode && s.Qty > 0).ToList();

            // 提取这些库存所在的库位编码
            var occupiedLocationCodes = existingStocks.Select(s => s.LocationCode).Distinct().ToList();

            // 在有效库位中，找到这些已经被该物料占用的库位
            //var mergeTarget = validLocations.FirstOrDefault(loc => occupiedLocationCodes.Contains(loc.Code));
            // 【修改此处】：增加防爆仓判断 -> 该库位当前总存量 + 本次放入的数量 <= 最大容量
            var mergeTarget = validLocations.FirstOrDefault(loc =>
                occupiedLocationCodes.Contains(loc.Code) &&
                (currentStocks.Where(s => s.LocationCode == loc.Code).Sum(s => s.Qty) + inQty) <= loc.MaxCapacity);
            return mergeTarget;
        }

        /// <summary>
        /// 策略 B：找一个完全为空的库位
        /// </summary>
        /// 增加防爆仓逻辑
        private Location GetEmptyLocation(List<Location> validLocations, List<Stock> currentStocks)
        {
            // 提取所有有库存的库位编码 (不管是哪种物料，只要数量大于0就算被占用)
            var occupiedLocationCodes = currentStocks.Where(s => s.Qty > 0).Select(s => s.LocationCode).Distinct().ToList();

            // 在有效库位中，排除掉被占用的，返回第一个纯空的库位
            // (这里可以配合 IsUsed 字段，严谨从库存反查真实空库位)
            var emptyLocation = validLocations.FirstOrDefault(loc => !occupiedLocationCodes.Contains(loc.Code));
            return emptyLocation;
        }

        /// <summary>
        /// 策略 C：按就近原则入库
        /// 假设：库位编码(如 A-01-01)按字母数字顺序排序，越靠前的离入口越近
        /// </summary>
        /// 升级为真实的2D坐标距离
        /// 增加 agvX, agvY 参数
        private Location GetNearestLocation(int agvX, int agvY, List<Location> validLocations, List<Stock> currentStocks)
        {
            // 先找出所有的纯空库位
            var occupiedLocationCodes = currentStocks.Where(s => s.Qty > 0).Select(s => s.LocationCode).Distinct().ToList();
            var emptyLocations = validLocations.Where(loc => !occupiedLocationCodes.Contains(loc.Code)).ToList();

            // 按编码升序排列 (比如 A-01-01 优先于 B-01-01)，取最前面的一个
            //var nearestLocation = emptyLocations.OrderBy(loc => loc.Code).FirstOrDefault();
            // 【修改此处】：利用勾股定理计算目标库位(PosX, PosY)与AGV当前坐标的直线距离平方，取最小的一个
            var nearestLocation = emptyLocations
                .OrderBy(loc => Math.Pow(loc.PosX - agvX, 2) + Math.Pow(loc.PosY - agvY, 2))
                .FirstOrDefault();
            return nearestLocation;
        }

        /// <summary>
        /// 策略 D：按使用频率 (冷热物料智能分区) 分配库位
        /// 高频物料优先放门口，低频物料发配到仓库最深处
        /// </summary>
        private Location GetLocationByUsageFrequency(string goodsCode, int agvX, int agvY, List<Location> validLocations, List<Stock> currentStocks)
        {
            // 1. 大数据分析：去流水账表里查该物料最近 30 天的出入库总次数 (定义为“热度”)
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            // 利用全局 DbHelper 直接统计该物料的操作频次
            int usageCount = FIH_WMS_System.Utils.DbHelper.Db.Queryable<StockRecord>()
                .Where(r => r.GoodsCode == goodsCode && r.OperateTime >= thirtyDaysAgo)
                .Count();

            // 2. 找出所有纯空的库位作为候选目标
            var occupiedLocationCodes = currentStocks.Where(s => s.Qty > 0).Select(s => s.LocationCode).Distinct().ToList();
            var emptyLocations = validLocations.Where(loc => !occupiedLocationCodes.Contains(loc.Code)).ToList();

            if (emptyLocations.Count == 0) return null; // 仓库没有纯空位了

            // 3. 智能判断：以 30天内 10 次为界限划分冷热物料
            if (usageCount > 10)
            {
                // 🔥 热物料 (高频)：按距离【升序】排列，取离起点 (agvX, agvY) 【最近】的空库位
                return emptyLocations
                    .OrderBy(loc => Math.Pow(loc.PosX - agvX, 2) + Math.Pow(loc.PosY - agvY, 2))
                    .FirstOrDefault();
            }
            else
            {
                // 🧊 冷物料 (低频)：按距离【降序】排列，把它发配到离起点【最远】的深处角落
                return emptyLocations
                    .OrderByDescending(loc => Math.Pow(loc.PosX - agvX, 2) + Math.Pow(loc.PosY - agvY, 2))
                    .FirstOrDefault();
            }
        }

        ///// <summary>
        ///// 策略 E：按物料分类集中存放 (同类分区策略)
        ///// 核心逻辑：检索待入库物料所属分类，优先将其分配至已存放同类物料的非满载库位；若无，则分配至空库位。
        ///// </summary>
        ///// <param name="goodsCode">待入库的物料编码</param>
        ///// <param name="inQty">本次计划入库数量</param>
        ///// <param name="validLocations">当前状态正常的可用库位集合</param>
        ///// <param name="currentStocks">当前系统的实时库存快照</param>
        ///// <returns>符合同类集中原则的目标库位，若无合适库位则返回首个空库位</returns>
        //private Location GetLocationByCategory(string goodsCode, int inQty, List<Location> validLocations, List<Stock> currentStocks)
        //{
        //    // 1. 获取当前待入库物料的完整档案信息 (提取分类特征)
        //    var currentGoods = FIH_WMS_System.Utils.DbHelper.Db.Queryable<Goods>().Where(g => g.Code == goodsCode).First();

        //    // 容错处理：若该物料未维护分类信息，则直接降级为“寻找空库位”策略
        //    if (currentGoods == null || string.IsNullOrEmpty(currentGoods.Category))
        //    {
        //        return GetEmptyLocation(validLocations, currentStocks);
        //    }

        //    // 2. 检索系统中所有隶属于同一分类的物料编码集合
        //    var sameCategoryGoodsCodes = FIH_WMS_System.Utils.DbHelper.Db.Queryable<Goods>()
        //        .Where(g => g.Category == currentGoods.Category)
        //        .Select(g => g.Code)
        //        .ToList();

        //    // 3. 筛选当前库存中，存放了这些同类物料的有效库存记录
        //    var categoryStocks = currentStocks.Where(s => sameCategoryGoodsCodes.Contains(s.GoodsCode) && s.Qty > 0).ToList();

        //    // 提取同类物料目前占用的所有库位编码 (去重)
        //    var occupiedLocationCodes = categoryStocks.Select(s => s.LocationCode).Distinct().ToList();

        //    // 4. 在状态正常的可用库位中寻找目标：
        //    // 条件A：必须是同类物料所在的库位 (聚集效应)
        //    // 条件B：该库位的当前总存量 + 本次入库量 <= 库位最大容量 (防爆仓校验)
        //    var targetLocation = validLocations.FirstOrDefault(loc =>
        //        occupiedLocationCodes.Contains(loc.Code) &&
        //        (currentStocks.Where(s => s.LocationCode == loc.Code).Sum(s => s.Qty) + inQty) <= loc.MaxCapacity);

        //    // 5. 最终决策：若找到了满足条件的同类库位，则返回之；若同类库位均已满载，则开辟一个新的空库位
        //    return targetLocation ?? GetEmptyLocation(validLocations, currentStocks);
        //}


        /// <summary>
        /// 策略 E (完全版)：多维度特征识别集中存放 (同类分区策略)
        /// 对应文档：通过物料reelId识别，按物料分类、规格、品牌、用途、有效期等分类入库
        /// </summary>
        /// 修改方法签名，增加 DateTime? produceDate 参数
        private Location GetLocationByMultiDimensions(string goodsCode, int inQty, List<Location> validLocations, List<Stock> currentStocks, DateTime? produceDate = null)
        {
            // 1. 获取当前待入库物料的完整档案信息
            var currentGoods = FIH_WMS_System.Utils.DbHelper.Db.Queryable<Goods>().Where(g => g.Code == goodsCode).First();

            if (currentGoods == null) return GetEmptyLocation(validLocations, currentStocks);


            // 新增核心：最高优先级匹配
            // 如果用户传了生产日期，优先找：完全相同物料 且 生产日期完全相同 的库位！(防止不同日期的物料混放)
            if (produceDate.HasValue)
            {
                var sameBatchStocks = currentStocks.Where(s =>
                    s.GoodsCode == goodsCode &&
                    s.ProduceDate == produceDate.Value.Date &&
                    s.Qty > 0).ToList();

                var sameBatchLocCodes = sameBatchStocks.Select(s => s.LocationCode).Distinct().ToList();

                var bestLoc = validLocations.FirstOrDefault(loc =>
                    sameBatchLocCodes.Contains(loc.Code) &&
                    (currentStocks.Where(s => s.LocationCode == loc.Code).Sum(s => s.Qty) + inQty) <= loc.MaxCapacity);

                // 如果找到了完美匹配同批次(同生产日期)的库位，直接返回！
                if (bestLoc != null) return bestLoc;
            }




            // 2. 核心：多维度相似度匹配！不仅仅看分类，还看品牌、规格、保质期、用途
            // 只要满足其中任意一个核心维度相同（且不为空），我们就认为它们是“同类特征物料”，可以放在一起
            var similarGoodsCodes = FIH_WMS_System.Utils.DbHelper.Db.Queryable<Goods>()
                .Where(g =>
                    (g.Category == currentGoods.Category && !string.IsNullOrEmpty(currentGoods.Category)) ||
                    (g.Brand == currentGoods.Brand && !string.IsNullOrEmpty(currentGoods.Brand)) ||
                    (g.Spec == currentGoods.Spec && !string.IsNullOrEmpty(currentGoods.Spec)) ||
                    (g.Usage == currentGoods.Usage && !string.IsNullOrEmpty(currentGoods.Usage)) || // 👈 新增：把“用途”加入同类项判定！
                    (g.ShelfLifeDays == currentGoods.ShelfLifeDays && currentGoods.ShelfLifeDays > 0)
                )
                .Select(g => g.Code)
                .ToList();

            // 如果连一个相似特征的都没有，退化为找空库位
            if (similarGoodsCodes.Count == 0) return GetEmptyLocation(validLocations, currentStocks);

            // 3. 筛选当前库存中，存放了这些“相似物料”的有效记录，提取它们目前占用的库位
            var similarStocks = currentStocks.Where(s => similarGoodsCodes.Contains(s.GoodsCode) && s.Qty > 0).ToList();
            var occupiedLocationCodes = similarStocks.Select(s => s.LocationCode).Distinct().ToList();

            // 4. 在状态正常的可用库位中寻找目标：
            // 条件A：必须是相似物料所在的库位 (多维度聚集效应)
            // 条件B：该库位的当前总存量 + 本次入库量 <= 库位最大容量 (防爆仓校验)
            var targetLocation = validLocations.FirstOrDefault(loc =>
                occupiedLocationCodes.Contains(loc.Code) &&
                (currentStocks.Where(s => s.LocationCode == loc.Code).Sum(s => s.Qty) + inQty) <= loc.MaxCapacity);

            // 5. 最终决策：若找到了满足条件的同类库位，则返回；若均已满载或无相似物料，则开辟空库位
            return targetLocation ?? GetEmptyLocation(validLocations, currentStocks);
        }



        /// <summary>
        /// 策略 F：按波次入库 (集中区域存放)
        /// 对应文档：按波次入库。将属于同一个采购单/波次单的物料，集中存放在相近的空库位，方便后续一波端出库。
        /// </summary>
        private Location GetLocationByWave(string goodsCode, List<Location> validLocations, List<Stock> currentStocks)
        {
            // 1. 查找当前物料所属的未完成入库单
            var orderInfo = FIH_WMS_System.Utils.DbHelper.Db.Queryable<WmsOrder>()
                .LeftJoin<WmsOrderDetail>((o, d) => o.OrderNo == d.OrderNo)
                .Where((o, d) => d.GoodsCode == goodsCode && d.Status < 2)
                .Select((o, d) => new { o.OrderNo, o.WaveNo })
                .First();

            if (orderInfo == null) return GetEmptyLocation(validLocations, currentStocks);

            // 2. 【核心升级】：跨单据聚合波次物料
            List<string> waveGoodsCodes;
            if (!string.IsNullOrEmpty(orderInfo.WaveNo))
            {
                // 如果有大波次编号，找出该波次下所有采购单的所有物料
                waveGoodsCodes = FIH_WMS_System.Utils.DbHelper.Db.Queryable<WmsOrderDetail>()
                    .Where(d => FIH_WMS_System.Utils.DbHelper.Db.Queryable<WmsOrder>()
                        .Where(o => o.WaveNo == orderInfo.WaveNo)
                        .Select(o => o.OrderNo).ToList().Contains(d.OrderNo))
                    .Select(d => d.GoodsCode)
                    .Distinct().ToList();
            }
            else
            {
                // 否则，维持原状：仅查找当前单据内的物料
                waveGoodsCodes = FIH_WMS_System.Utils.DbHelper.Db.Queryable<WmsOrderDetail>()
                    .Where(d => d.OrderNo == orderInfo.OrderNo)
                    .Select(d => d.GoodsCode)
                    .ToList();
            }

            // 3. 寻找这些“波次兄弟”物料目前在仓库中的分布区域
            var waveStocks = currentStocks.Where(s => waveGoodsCodes.Contains(s.GoodsCode) && s.Qty > 0).ToList();
            var emptyLocations = validLocations.Where(loc => !currentStocks.Select(s => s.LocationCode).Contains(loc.Code)).ToList();

            if (waveStocks.Count > 0)
            {
                // 获取第一个兄弟物料所在的区域（例如 A区）
                var siblingLoc = waveStocks.First().LocationCode;
                var targetArea = validLocations.Where(l => l.Code == siblingLoc).Select(l => l.Area).FirstOrDefault();

                // 优先在同一个区域寻找空位，实现“跨单据、同波次、近距离存储”
                var targetLoc = emptyLocations.FirstOrDefault(l => l.Area == targetArea);
                if (targetLoc != null) return targetLoc;
            }

            return GetEmptyLocation(validLocations, currentStocks);
        }



    }

}