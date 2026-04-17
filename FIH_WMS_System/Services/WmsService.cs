using FIH_WMS_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Dapper;

using FIH_WMS_System.Utils; // 【新增】引入DbHelper

namespace FIH_WMS_System.Services
{
    public class WmsService
    {
        // 数据库连接
        //private string connStr = "Server=LAPTOP-9IABD0I1;Database=FIH_WMS_DB;User Id=sa;Password=123456;TrustServerCertificate=True;";
        // 数据库连接 (使用 . 代表本机 localhost，相对路径！)
        private string connStr = "Server=.;Database=FIH_WMS_DB;User Id=sa;Password=123456;TrustServerCertificate=True;";

        // ==========================================
        // 1. 入库操作 (升级版：支持批次和单据)
        // ==========================================
        // 为了不让现有的 UI 报错，给 batchNo 和 orderNo 加了默认值
        public bool InStock(string goodsCode, int qty, string locCode, string batchNo = "", string orderNo = "AUTO-IN-001")
        {
            if (qty <= 0) return false;

            // 如果没传批次号，系统自动拿今天日期生成一个，例如 BATCH-20260318
            if (string.IsNullOrEmpty(batchNo))
            {
                batchNo = "BATCH-" + DateTime.Now.ToString("yyyyMMdd");
            }

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. 检查同一个库位、同一个物料、同一个批次是不是已经有记录了
                        var stockId = db.ExecuteScalar<int?>(
                            "SELECT Id FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode AND BatchNo = @batch",
                            new { gCode = goodsCode, lCode = locCode, batch = batchNo }, transaction);

                        if (stockId.HasValue)
                        {
                            // 存在同批次，累加数量
                            db.Execute("UPDATE Stock SET Qty = Qty + @qty WHERE Id = @id",
                                new { qty = qty, id = stockId.Value }, transaction);
                        }
                        else
                        {
                            // 不存在，新增一条库存记录（插入批次号）
                            db.Execute(@"INSERT INTO Stock (GoodsCode, LocationCode, Qty, BatchNo, InStockTime) 
                                         VALUES (@gCode, @lCode, @qty, @batch, GETDATE())",
                                new { gCode = goodsCode, lCode = locCode, qty = qty, batch = batchNo }, transaction);

                            // 更新库位状态为被占用
                            db.Execute("UPDATE Location SET IsUsed = 1 WHERE Code = @lCode",
                                new { lCode = locCode }, transaction);
                        }

                        // 2. 记一笔入库流水账 (记录上单据号和批次号)
                        //要求，// 把原本写死的 'admin' 换成参数 @opName
                        //db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                        //             VALUES (0, @order, @gCode, @lCode, @qty, @batch, GETDATE(), 'admin')",
                        db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                     VALUES (0, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                     new { 
                                         order = orderNo, 
                                         gCode = goodsCode, 
                                         lCode = locCode, 
                                         qty = qty, 
                                         batch = batchNo,
                                         // 👇 在这里把全局变量里的名字传给数据库！
                                         opName = FIH_WMS_System.Program.CurrentUsername
                                     }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /*        // ==========================================
                // 2. 出库操作 (升级版：初步的先进先出 FIFO 逻辑)
                // ==========================================
                public bool OutStock(string goodsCode, int qty, string locCode, string orderNo = "AUTO-OUT-001")
                {
                    if (qty <= 0) return false;

                    using (var db = new SqlConnection(connStr))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            try
                            {
                                // 1. 查出这个库位上，这个物料的所有批次库存，并且【按入库时间从早到晚排序】（这就是先进先出的核心！）
                                var stocks = db.Query<Stock>(
                                    "SELECT Id, Qty, BatchNo FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode ORDER BY InStockTime ASC",
                                    new { gCode = goodsCode, lCode = locCode }, transaction).ToList();

                                int totalQty = stocks.Sum(s => s.Qty);
                                if (totalQty < qty) return false; // 总库存不够，直接拒绝

                                int needToDeduct = qty; // 还需要扣减的数量

                                // 2. 循环扣减，老批次先扣，扣完再扣新批次
                                foreach (var stock in stocks)
                                {
                                    if (needToDeduct <= 0) break;

                                    int deductQty = Math.Min(stock.Qty, needToDeduct); // 这次要扣多少
                                    needToDeduct -= deductQty;

                                    if (stock.Qty == deductQty)
                                    {
                                        // 这个批次扣光了，删除记录
                                        db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = stock.Id }, transaction);
                                    }
                                    else
                                    {
                                        // 没扣光，更新剩余数量
                                        db.Execute("UPDATE Stock SET Qty = Qty - @qty WHERE Id = @id", new { qty = deductQty, id = stock.Id }, transaction);
                                    }

                                    // 3. 记一笔出库流水，标记扣的是哪个批次
                                    db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                                 VALUES (1, @order, @gCode, @lCode, @qty, @batch, GETDATE(), 'admin')",
                                                 new { order = orderNo, gCode = goodsCode, lCode = locCode, qty = deductQty, batch = stock.BatchNo }, transaction);
                                }

                                // 如果这个库位完全空了，把状态改回未占用
                                var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = locCode }, transaction);
                                if (remain == 0)
                                {
                                    db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = locCode }, transaction);
                                }

                                transaction.Commit();
                                return true;
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                }*/


        // ==========================================
        // 2. 出库操作 (WMS + WCS 融合版：出库自动呼叫 AGV)
        // ==========================================
        public bool OutStock(string goodsCode, int qty, string locCode, string orderNo = "AUTO-OUT-001")
        {
            if (qty <= 0) return false;

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var stocks = db.Query<Stock>(
                            "SELECT Id, Qty, BatchNo FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode ORDER BY InStockTime ASC",
                            new { gCode = goodsCode, lCode = locCode }, transaction).ToList();

                        int totalQty = stocks.Sum(s => s.Qty);
                        if (totalQty < qty) return false;

                        int needToDeduct = qty;

                        foreach (var stock in stocks)
                        {
                            if (needToDeduct <= 0) break;

                            int deductQty = Math.Min(stock.Qty, needToDeduct);
                            needToDeduct -= deductQty;

                            if (stock.Qty == deductQty)
                                db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = stock.Id }, transaction);
                            else
                                db.Execute("UPDATE Stock SET Qty = Qty - @qty WHERE Id = @id", new { qty = deductQty, id = stock.Id }, transaction);

                            // 记流水
                            db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                         VALUES (1, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                         new { 
                                             order = orderNo, 
                                             gCode = goodsCode, 
                                             lCode = locCode, 
                                             qty = deductQty, 
                                             batch = stock.BatchNo,
                                             // 👇 把全局变量里的名字传给数据库！
                                             opName = FIH_WMS_System.Program.CurrentUsername
                                         }, transaction);
                        }

                        var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = locCode }, transaction);
                        if (remain == 0) db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = locCode }, transaction);

                        // 👇 【WCS核心：召唤 AGV 小车！】 👇
                        // 库存扣完后，生成一个 AGV 任务指令发给硬件层
                        string agvTaskNo = "AGV-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        db.Execute(@"INSERT INTO AgvTask (TaskNo, TaskType, Status, GoodsCode, Qty, FromLocation, ToLocation, CreateTime) 
                                     VALUES (@tNo, 1, 0, @gCode, @qty, @fromLoc, '产线接驳口1', GETDATE())",
                                     new { tNo = agvTaskNo, gCode = goodsCode, qty = qty, fromLoc = locCode }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 3. 查询库存 (升级版：带出批次等新信息)
        // ==========================================
        public List<Stock> GetStockList()
        {
            using (var db = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT s.Id, s.ReelId, s.Qty, s.BatchNo, s.ProduceDate, s.InStockTime,
                           g.Id, g.Code, g.Name, g.Spec, g.Price, g.Category,
                           l.Id, l.Code, l.Area, l.IsUsed
                    FROM Stock s
                    INNER JOIN Goods g ON s.GoodsCode = g.Code
                    INNER JOIN Location l ON s.LocationCode = l.Code";

                var list = db.Query<Stock, Goods, Location, Stock>(
                    sql,
                    (stock, goods, location) =>
                    {
                        stock.Goods = goods;
                        stock.Location = location;
                        return stock;
                    },
                    splitOn: "Id,Id"
                ).ToList();

                return list;
            }
        }

        // ==========================================
        // 4. 查询流水 (升级版：带出单据号、批次等新信息)
        // ==========================================
        public List<StockRecord> GetRecords()
        {
            using (var db = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT r.Id, r.ReelId, r.RecordType as Type, r.OrderNo, r.BatchNo, r.Qty, r.OperateTime, r.Operator,
                           g.Id, g.Code, g.Name, g.Spec, g.Price, g.Category,
                           l.Id, l.Code, l.Area, l.IsUsed
                    FROM StockRecord r
                    INNER JOIN Goods g ON r.GoodsCode = g.Code
                    INNER JOIN Location l ON r.LocationCode = l.Code
                    ORDER BY r.OperateTime DESC";

                var list = db.Query<StockRecord, Goods, Location, StockRecord>(
                    sql,
                    (record, goods, location) =>
                    {
                        record.Goods = goods;
                        record.Location = location;
                        return record;
                    },
                    splitOn: "Id,Id"
                ).ToList();

                return list;
            }
        }



        // ==========================================
        // 5. 智能分配库位算法 (入库规则引擎)
        //入库规则包括：按未满库位入库（减少库位物料碎片）以及按空库位入库（不与原库位物料混合存储）
        // ==========================================
        //(接入全新入库规则引擎)3月20
        //加了 strategy 参数，并给了默认值 SameMaterialMerge (未满库位优先)。
        // 这样以前的 UI 代码直接调用 GetRecommendLocation("G001") 也不会报错。
        // ==========================================
        //松耦合设计：未将复杂的判断写在 WmsService 里，而是抽离到了 InboundRuleEngine 中。
        //以后如果有“我要加一个新的入库规则”，只需要修改 InboundRuleEngine.cs 即可。
        //WmsService 的代码几乎不用动，此为高内聚低耦合
        // ==========================================
        //public string GetRecommendLocation(string goodsCode)
        //public string GetRecommendLocation(string goodsCode, InboundStrategy strategy = InboundStrategy.SameMaterialMerge)
        /*        public string GetRecommendLocation(string goodsCode, int inQty, InboundStrategy strategy, int agvX = 0, int agvY = 0)
                {

                    *//*            using (var db = new SqlConnection(connStr))
                                {
                                    // (1)按未满库位入库（减少碎片）
                                    // 先去库存表里找找，有没有哪个库位已经放了这个物料了？如果有，我们就推荐跟它放在一起。
                                    string existLoc = db.ExecuteScalar<string>(
                                        "SELECT TOP 1 LocationCode FROM Stock WHERE GoodsCode = @gCode",
                                        new { gCode = goodsCode });

                                    if (!string.IsNullOrEmpty(existLoc))
                                    {
                                        return existLoc; // 找到了！直接推荐合并存放
                                    }

                                    // (2)按空库位入库（不混合存储）
                                    // 如果这是一个全新的物料，就去 Location 表里找一个完全没人用的空库位 (IsUsed = 0)
                                    string emptyLoc = db.ExecuteScalar<string>( "SELECT TOP 1 Code FROM Location WHERE IsUsed = 0");

                                    if (!string.IsNullOrEmpty(emptyLoc))
                                    {
                                        return emptyLoc; // 找到了一个空闲的新家！
                                    }

                                    // (3)如果都没找到，说明整个仓库连一个空位都没有了，爆仓了！
                                    return string.Empty;
                                }*//*

                    // 如果用户选择了“人工指定”，系统就不用费劲推荐了
                    if (strategy == InboundStrategy.Manual)
                    {
                        return string.Empty;
                    }

                    using (var db = new SqlConnection(connStr))
                    {
                        // 1. 收集情报：查出当前仓库里所有的库位 (给大脑提供地图)
                        var allLocations = db.Query<Location>("SELECT * FROM Location").ToList();

                        // 2. 收集情报：查出当前仓库里所有非空的库存 (给大脑提供当前战况)
                        // 只查 Qty > 0 的，如果 Qty 是 0 说明那个库存记录是空的
                        var currentStocks = db.Query<Stock>("SELECT * FROM Stock WHERE Qty > 0").ToList();

                        // 3. 召唤大脑：实例化我们的入库规则引擎
                        var engine = new InboundRuleEngine();

                        // 4. 开始计算：把物料、地图、战况、策略 喂给引擎
                        //Location recommendedLoc = engine.RecommendLocation(goodsCode, allLocations, currentStocks, strategy);
                        Location recommendedLoc = engine.RecommendLocation(goodsCode, inQty, agvX, agvY, allLocations, currentStocks, strategy);

                        // 5. 返回结果：如果引擎算出来了，就返回库位编码；否则返回空字符串表示“没找到合适位置”
                        if (recommendedLoc != null)
                        {
                            return recommendedLoc.Code;
                        }
                        else
                        {
                            return string.Empty; // 比如仓库全满的时候
                        }
                    }



                }*/
        /// <summary>
        /// 提供给外部 UI 调用的智能推荐入库库位接口
        /// </summary>
        public string GetRecommendLocation(string goodsCode, int inQty, InboundStrategy strategy, int agvX = 0, int agvY = 0)
        {
            // 1. 如果用户选择了“人工指定”，系统就不用连数据库了
            if (strategy == InboundStrategy.Manual)
            {
                return string.Empty;
            }

            try
            {
                // 2. 利用 DbHelper 直接从数据库抓取真实货架和库存数据
                List<Location> allLocations = DbHelper.Db.Queryable<Location>().ToList();
                List<Stock> currentStocks = DbHelper.Db.Queryable<Stock>().Where(s => s.Qty > 0).ToList();

                // 3. 召唤大脑：实例化入库规则引擎
                InboundRuleEngine engine = new InboundRuleEngine();

                // 4. 开始计算分配
                Location recommendedLoc = engine.RecommendLocation(goodsCode, inQty, agvX, agvY, allLocations, currentStocks, strategy);

                if (recommendedLoc != null)
                    return recommendedLoc.Code;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("数据库连接失败：" + ex.Message, "系统错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return string.Empty;
            }
        }




        // ==========================================
        // 6. 库位移库/合并 
        //如果是在真实的仓库中，经常会出现“碎片化”的库存（比如 A 货架剩 10 个，B 货架剩 5 个）。为了腾出空间，我需要把货物从一个库位搬到另一个库位。
        // ==========================================
        public bool MoveStock(string goodsCode, string fromLoc, string toLoc, int qty)
        {
            if (qty <= 0 || fromLoc == toLoc) return false;

            // 自动生成一个移库专属的单据号
            string orderNo = "MOVE-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. 检查源库位有没有这个货，数量够不够？(还是按先进先出挑选批次移动)
                        var sourceStocks = db.Query<Stock>(
                            "SELECT Id, Qty, BatchNo FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode ORDER BY InStockTime ASC",
                            new { gCode = goodsCode, lCode = fromLoc }, transaction).ToList();

                        if (sourceStocks.Sum(s => s.Qty) < qty) return false; // 源库位库存不足

                        int needToMove = qty;

                        // 2. 开始搬砖 (从旧库位扣除，加到新库位)
                        foreach (var stock in sourceStocks)
                        {
                            if (needToMove <= 0) break;

                            int moveQty = Math.Min(stock.Qty, needToMove);
                            needToMove -= moveQty;

                            // 【动作 A：从源库位扣减】
                            if (stock.Qty == moveQty)
                                db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = stock.Id }, transaction);
                            else
                                db.Execute("UPDATE Stock SET Qty = Qty - @qty WHERE Id = @id", new { qty = moveQty, id = stock.Id }, transaction);

                            // 【动作 B：往目标库位增加】(必须保持批次号不变，因为是原封不动搬过去的)
                            var destStockId = db.ExecuteScalar<int?>(
                                "SELECT Id FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode AND BatchNo = @batch",
                                new { gCode = goodsCode, lCode = toLoc, batch = stock.BatchNo }, transaction);

                            if (destStockId.HasValue)
                            {
                                db.Execute("UPDATE Stock SET Qty = Qty + @qty WHERE Id = @id", new { qty = moveQty, id = destStockId.Value }, transaction);
                            }
                            else
                            {
                                db.Execute(@"INSERT INTO Stock (GoodsCode, LocationCode, Qty, BatchNo, InStockTime) 
                                             VALUES (@gCode, @lCode, @qty, @batch, GETDATE())",
                                    new { gCode = goodsCode, lCode = toLoc, qty = moveQty, batch = stock.BatchNo }, transaction);
                                db.Execute("UPDATE Location SET IsUsed = 1 WHERE Code = @lCode", new { lCode = toLoc }, transaction);
                            }

                            // 【记录两笔流水账】一出一进，便于追溯
                            // 1. 记录源库位出库
                            db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                         VALUES (1, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                         new { 
                                             order = orderNo, 
                                             gCode = goodsCode, 
                                             lCode = fromLoc, 
                                             qty = moveQty, 
                                             batch = stock.BatchNo,
                                             // 👇 把全局变量里的名字传给数据库！
                                             opName = FIH_WMS_System.Program.CurrentUsername
                                         }, transaction);
                            // 2. 记录目标库位入库
                            db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                         VALUES (0, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                         new { 
                                             order = orderNo, 
                                             gCode = goodsCode, 
                                             lCode = toLoc, 
                                             qty = moveQty, 
                                             batch = stock.BatchNo,
                                             // 👇 把全局变量里的名字传给数据库！
                                             opName = FIH_WMS_System.Program.CurrentUsername
                                         }, transaction);
                        }

                        // 3. 收尾清理：如果源库位空了，释放它
                        var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = fromLoc }, transaction);
                        if (remain == 0)
                        {
                            db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = fromLoc }, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 7. 库存盘点与盈亏调整
        //仓库在实际运营中，经常会出现“电脑上显示有100个，去货架上一数只有98个”的情况（可能因为损耗、错发等）。盘点的作用就是：以实物数量为准，强制修正系统数量，并自动记一笔“盘盈（多出来了）”或“盘亏（少了）”的流水账 。
        // ==========================================
        public bool AdjustStock(string goodsCode, string locCode, string batchNo, int physicalQty)
        {
            if (physicalQty < 0) return false; // 实物数量不能是负数

            // 自动生成一个盘点专属单号
            string orderNo = "CHECK-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. 精准锁定那一条库存记录
                        var stock = db.QueryFirstOrDefault<Stock>(
                            "SELECT Id, Qty FROM Stock WHERE GoodsCode = @gCode AND LocationCode = @lCode AND BatchNo = @batch",
                            new { gCode = goodsCode, lCode = locCode, batch = batchNo }, transaction);

                        if (stock == null) return false; // 没找到对应库存，无法盘点

                        // 2. 计算差异 (实物数量 - 系统数量)
                        int diff = physicalQty - stock.Qty;
                        if (diff == 0) return true; // 账实相符，完美！不用做任何修改

                        // 3. 修改系统库存为实物数量
                        if (physicalQty == 0)
                        {
                            // 如果实物盘点为0，说明货没了，直接删掉记录
                            db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = stock.Id }, transaction);

                            // 检查库位是不是彻底空了
                            var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = locCode }, transaction);
                            if (remain == 0) db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = locCode }, transaction);
                        }
                        else
                        {
                            // 还没空，直接更新为真实的实物数量
                            db.Execute("UPDATE Stock SET Qty = @qty WHERE Id = @id", new { qty = physicalQty, id = stock.Id }, transaction);
                        }

                        // 4. 记录盘盈/盘亏流水账
                        // 如果 diff > 0 (盘盈，多出来了)，记作入库(0)；如果 diff < 0 (盘亏，少了)，记作出库(1)
                        int recordType = diff > 0 ? 0 : 1;
                        int changeQty = Math.Abs(diff); // 流水记录里只记变动的绝对值

                        db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                     VALUES (@rType, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                     new { 
                                         rType = recordType, 
                                         order = orderNo, 
                                         gCode = goodsCode, 
                                         lCode = locCode, 
                                         qty = changeQty, 
                                         batch = batchNo,
                                         // 👇 在这里把全局变量里的名字传给数据库！
                                         opName = FIH_WMS_System.Program.CurrentUsername
                                     }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 8. 查询 AGV 任务列表
        // ==========================================
        public List<AgvTask> GetAgvTasks()
        {
            using (var db = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT t.*, g.Id, g.Code, g.Name, g.Spec 
                    FROM AgvTask t
                    INNER JOIN Goods g ON t.GoodsCode = g.Code
                    ORDER BY t.CreateTime DESC";

                var list = db.Query<AgvTask, Goods, AgvTask>(
                    sql,
                    (task, goods) => { task.Goods = goods; return task; },
                    splitOn: "Id"
                ).ToList();

                return list;
            }
        }


        // ==========================================
        // 9. 模拟 AGV 小车完成任务反馈
        // ==========================================
        //public bool CompleteAgvTask(int taskId)
        //{
        //    using (var db = new SqlConnection(connStr))
        //    {
        //        // 将状态更新为 3 (已完成)，并打上完成时间戳
        //        int rows = db.Execute(@"
        //            UPDATE AgvTask 
        //            SET Status = 3, FinishTime = GETDATE() 
        //            WHERE Id = @id AND Status != 3",
        //            new { id = taskId });

        //        return rows > 0; // 如果受影响的行数大于0，说明更新成功
        //    }
        //}

        // ==========================================
        // 9. 模拟 AGV 小车完成任务反馈 (闭环升级)
        // ==========================================
        public bool CompleteAgvTask(int taskId)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. 查出当前正在跑的任务信息
                        var task = Dapper.SqlMapper.QueryFirstOrDefault<AgvTask>(db,
                            "SELECT * FROM AgvTask WHERE Id = @id", new { id = taskId }, transaction);

                        if (task == null || task.Status == 3) return false;

                        // 2. 更新原任务为已完成
                        Dapper.SqlMapper.Execute(db, @"
                            UPDATE AgvTask SET Status = 3, FinishTime = GETDATE() WHERE Id = @id",
                            new { id = taskId }, transaction);

                        // 3. 记录小车到达终点的日志
                        Dapper.SqlMapper.Execute(db, "INSERT INTO AgvLog (TaskNo, Action, Location, LogTime) VALUES (@t, @a, @l, GETDATE())",
                            new { t = task.TaskNo, a = "✅ 任务完成，卸载货物", l = task.ToLocation }, transaction);

                        // 4. 【核心智能联动】：如果这不是一个回充任务，那么送完货后，立刻派它去充电桩或待命区！
                        // 规定 TaskType: 0入库, 1出库, 2移库, 3自动回充
                        if (task.TaskType != 3)
                        {
                            string newChargeTaskNo = "AGV-CHG-" + DateTime.Now.ToString("HHmmss");

                            Dapper.SqlMapper.Execute(db, @"
                                INSERT INTO AgvTask (TaskNo, TaskType, Status, GoodsCode, Qty, FromLocation, ToLocation, CreateTime) 
                                VALUES (@tNo, 3, 0, '无(空载)', 0, @fromLoc, 'C区-充电接驳站', GETDATE())",
                                new { tNo = newChargeTaskNo, fromLoc = task.ToLocation }, transaction);

                            // 记录回程指令日志
                            Dapper.SqlMapper.Execute(db, "INSERT INTO AgvLog (TaskNo, Action, Location, LogTime) VALUES (@t, @a, @l, GETDATE())",
                                new { t = newChargeTaskNo, a = "🔋 触发空闲自动回充指令", l = task.ToLocation }, transaction);
                        }
                        else
                        {
                            // 如果它本身就是回充任务，说明它现在已经到达充电桩了
                            Dapper.SqlMapper.Execute(db, "INSERT INTO AgvLog (TaskNo, Action, Location, LogTime) VALUES (@t, @a, @l, GETDATE())",
                                new { t = task.TaskNo, a = "⚡ 到达充电站，开始充电", l = task.ToLocation }, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        // ==========================================
        // 供外部主动记录 AGV 日志的通用接口
        // ==========================================
        public void AddAgvLog(string taskNo, string action, string location)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                Dapper.SqlMapper.Execute(db, "INSERT INTO AgvLog (TaskNo, Action, Location, LogTime) VALUES (@t, @a, @l, GETDATE())",
                    new { t = taskNo, a = action, l = location });
            }
        }


        // ==========================================
        // 10. 获取图表统计数据 (各物料总库存)
        // ==========================================
        public Dictionary<string, int> GetStockChartData()
        {
            using (var db = new SqlConnection(connStr))
            {
                // 使用 SQL 的 GROUP BY 把同一个物料在不同库位的数量加起来 (SUM)
                string sql = @"
                    SELECT g.Name, SUM(s.Qty) as TotalQty 
                    FROM Stock s 
                    INNER JOIN Goods g ON s.GoodsCode = g.Code 
                    GROUP BY g.Name";

                // 执行查询，并转换成 字典(Dictionary) 格式，方便前端画图 (Key:物料名, Value:总数量)
                var result = db.Query(sql)
                               .ToDictionary(row => (string)row.Name, row => (int)row.TotalQty);
                return result;
            }
        }


        // ==========================================
        // 11. 获取图表统计数据 (库位使用率)
        // ==========================================
        public Dictionary<string, int> GetLocationUsage()
        {
            using (var db = new SqlConnection(connStr))
            {
                // 用 SQL 统计已占用和未占用的数量
                string sql = "SELECT IsUsed, COUNT(1) as Cnt FROM Location GROUP BY IsUsed";
                var list = db.Query(sql).ToList();

                // 准备一个字典来装结果
                var result = new Dictionary<string, int>();
                result.Add("已占用", 0);
                result.Add("空闲中", 0);

                // 把数据库查到的结果翻译过来
                foreach (var item in list)
                {
                    if ((bool)item.IsUsed)
                        result["已占用"] = (int)item.Cnt;
                    else
                        result["空闲中"] = (int)item.Cnt;
                }

                return result;
            }
        }


        // ==========================================
        // 12. 2D 库位地图数据源 (可视化屏)
        //服务层增加“扫描仓库”的大脑
        //我们需要一次性把所有货架、货架上有没有东西、有什么东西全部查出来。
        // ==========================================
        public List<dynamic> GetLocationMapData()
        {
            using (var db = new SqlConnection(connStr))
            {
                // 用 LEFT JOIN 把库位表、库存表、物料表连起来
                // 这样既能查到空闲的货架，也能查到放了什么货

                // 使用 GROUP BY 和 SUM，确保每个库位只返回唯一的一行汇总数据
                string sql = @"
                    SELECT l.Code, l.IsUsed, l.Status,
                           MAX(ISNULL(g.Name, '')) as GoodsName, 
                           SUM(ISNULL(s.Qty, 0)) as Qty
                    FROM Location l
                    LEFT JOIN Stock s ON l.Code = s.LocationCode
                    LEFT JOIN Goods g ON s.GoodsCode = g.Code
                    GROUP BY l.Code, l.IsUsed, l.Status
                    ORDER BY l.Code";

                // Dapper 支持直接返回 dynamic 动态类型，适合混合数据
                return db.Query(sql).ToList();
            }
        }


        // ==========================================
        // 13. 系统登录校验
        // ==========================================
        public Models.User? Login(string username, string password)
        {
            using (var db = new SqlConnection(connStr))
            {
                // 去数据库查有没有这个账号和密码组合
                string sql = "SELECT * FROM Users WHERE Username = @u AND Password = @p";
                var user = db.QueryFirstOrDefault<Models.User>(sql, new { u = username, p = password });
                return user; // 如果密码错了或没这人，会返回 null
            }
        }



        // ==========================================
        // 获取智能出库拣货建议 (只查询，不扣减，目前用于UI显示)
        // ==========================================
        public List<Stock> GetOutboundPickAdvice(string goodsCode, int requiredQty, OutboundStrategy strategy = OutboundStrategy.FIFO)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 1. 获取所有非空库存
                //var currentStocks = db.Query<Stock>("SELECT * FROM Stock WHERE Qty > 0").ToList();



                //(为智能大脑提供保质期数据)

                //核心修复：不能只查 Stock 表，必须连表查出 Goods 的 ShelfLifeDays！
                string sql = @"
                    SELECT s.*, g.Id, g.Code, g.Name, g.ShelfLifeDays 
                    FROM Stock s 
                    INNER JOIN Goods g ON s.GoodsCode = g.Code 
                    WHERE s.Qty > 0";

                // Dapper 强类型一对多映射
                var currentStocks = db.Query<Stock, Goods, Stock>(sql, (stock, goods) =>
                {
                    stock.Goods = goods;
                    return stock;
                }, splitOn: "Id").ToList();


                // 升级：若是 ReelId 扫码精准出库，直接拦截
                if (strategy == OutboundStrategy.ByReelId)
                {
                    // 此时传进来的 goodsCode 其实是那一盘料独一无二的 ReelId
                    var exactStock = currentStocks.FirstOrDefault(s => s.ReelId == goodsCode);
                    if (exactStock == null || (exactStock.Qty - exactStock.FrozenQty) < requiredQty)
                        return new List<Stock>(); // 没找到或者数量不够

                    // 若找到，无视所有规则，直接返回这盘料
                    return new List<Stock> {
                        new Stock {
                            Id = exactStock.Id, LocationCode = exactStock.LocationCode,
                            BatchNo = exactStock.BatchNo, Qty = requiredQty
                        }
                    };
                }





                // 2. 召唤出库大脑进行优先级排序
                var engine = new OutboundRuleEngine();
                var sortedStocks = engine.RecommendOutboundStocks(goodsCode, currentStocks, strategy);

                // 3. 按照所需数量，模拟拣货拼凑过程
                var pickList = new List<Stock>();
                int remainQty = requiredQty;

                foreach (var stock in sortedStocks)
                {
                    if (remainQty <= 0) break; // 已经凑够了，停止寻找

                    int availableQty = stock.Qty - stock.FrozenQty;
                    if (availableQty <= 0) continue;

                    // 计算这个库位本次需要拿走多少
                    int pickQty = Math.Min(availableQty, remainQty);

                    // 克隆一个 Stock 对象，把 Qty 替换成【建议拿走的数量】
                    pickList.Add(new Stock
                    {
                        Id = stock.Id,
                        LocationCode = stock.LocationCode,
                        BatchNo = stock.BatchNo,
                        Qty = pickQty,
                        InStockTime = stock.InStockTime
                    });

                    remainQty -= pickQty;
                }

                return pickList;
            }
        }

        // ==========================================
        // 智能出库执行 (WMS + WCS 融合版升级)
        // ==========================================
        public bool SmartOutStock(string goodsCode, int qty, string orderNo = "AUTO-OUT-001", OutboundStrategy strategy = OutboundStrategy.FIFO)
        {
            if (qty <= 0) return false;




            // 修复：若是扫 ReelId 进来的，goodsCode 其实是长串追溯码。
            // 需要把它拆开，提取出真实的物料编码 (比如 G001)，防止错存数据库！
            string realGoodsCode = goodsCode;
            if (strategy == OutboundStrategy.ByReelId && goodsCode.Contains("-"))
            {
                var parts = goodsCode.Split('-');
                if (parts.Length >= 3) realGoodsCode = parts[2]; // ReelId 第3段就是物料编码
            }




            // 1. 先调用上面的建议方法，看库存到底够不够
            var pickAdvice = GetOutboundPickAdvice(goodsCode, qty, strategy);
            int totalAvailable = pickAdvice.Sum(p => p.Qty);

            if (totalAvailable < qty)
            {
                return false; // 整个仓库的可用库存加起来都不够！
            }

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 2. 遍历拣货建议列表，按计划挨个扣除
                        foreach (var pick in pickAdvice)
                        {
                            // 查出现有真实库存
                            var stockInDb = db.QueryFirstOrDefault<Stock>("SELECT Id, Qty FROM Stock WHERE Id = @id", new { id = pick.Id }, transaction);
                            if (stockInDb == null) throw new Exception("库存数据异常");

                            if (stockInDb.Qty == pick.Qty)
                                db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = pick.Id }, transaction);
                            else
                                db.Execute("UPDATE Stock SET Qty = Qty - @pickQty WHERE Id = @id", new { pickQty = pick.Qty, id = pick.Id }, transaction);

                            // 记流水账
                            db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                         VALUES (1, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                         new
                                         {
                                             order = orderNo,
                                             //gCode = goodsCode,
                                             gCode = realGoodsCode,
                                             lCode = pick.LocationCode,
                                             qty = pick.Qty,
                                             batch = pick.BatchNo,
                                             opName = FIH_WMS_System.Program.CurrentUsername
                                         }, transaction);

                            // 检查库位是否彻底空了，空了就释放掉 (IsUsed = 0)
                            var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = pick.LocationCode }, transaction);
                            if (remain == 0) db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = pick.LocationCode }, transaction);

                            // 【联动 AGV】呼叫小车去对应的库位拿货！
                            string agvTaskNo = "AGV-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + pick.Id;
                            db.Execute(@"INSERT INTO AgvTask (TaskNo, TaskType, Status, GoodsCode, Qty, FromLocation, ToLocation, CreateTime) 
                                         VALUES (@tNo, 1, 0, @gCode, @qty, @fromLoc, '产线接驳口1', GETDATE())",
                                         //new { tNo = agvTaskNo, gCode = goodsCode, qty = pick.Qty, fromLoc = pick.LocationCode }, transaction);
                                         new { tNo = agvTaskNo, gCode = realGoodsCode, qty = pick.Qty, fromLoc = pick.LocationCode }, transaction);//// ✅ 改成 gCode = realGoodsCode
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // 如果中间任何一步报错（比如断网），全部撤销，保护数据！
                        return false;
                    }
                }
            }
        }



        // ==========================================
        // 智能制造：产线追加需求单 (不良/损耗追加出库)
        // ==========================================
        public bool ExecuteAdditionalOutbound(string workOrderNo, string goodsCode, int qty, OutboundStrategy strategy = OutboundStrategy.FIFO)
        {
            if (qty <= 0) return false;

            // 1. 拦截防呆：先看看仓库里这玩意儿还够不够？
            var pickAdvice = GetOutboundPickAdvice(goodsCode, qty, strategy);
            int totalAvailable = pickAdvice.Sum(p => p.Qty);

            if (totalAvailable < qty)
            {
                return false; // 连追加的量都不够了，直接驳回
            }

            // 2. 智能生成追加单号 (前缀 ADD 代表追加单)
            string addOrderNo = string.IsNullOrEmpty(workOrderNo)
                ? "ADD-" + DateTime.Now.ToString("yyyyMMddHHmmss")
                : "ADD-" + workOrderNo + "-" + DateTime.Now.ToString("HHmmss");

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 3. 往【单据管理中心】写入宏观单据留底
                        Dapper.SqlMapper.Execute(db, "INSERT INTO WmsOrder (OrderNo, OrderType, Status, CreateTime) VALUES (@o, 1, 2, GETDATE())",
                                   new { o = addOrderNo }, transaction);

                        Dapper.SqlMapper.Execute(db, "INSERT INTO WmsOrderDetail (OrderNo, GoodsCode, PlanQty, ActualQty, Status) VALUES (@o, @g, @q, @q, 2)",
                                   new { o = addOrderNo, g = goodsCode, q = qty }, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            // 4. 完美复用底层智能出库引擎：扣减真实库存、记流水账、并呼叫 AGV 给你送过去！
            return SmartOutStock(goodsCode, qty, addOrderNo, strategy);
        }




        // ==========================================
        // 智能盘点 - 1. 根据条件生成盘点清单
        // ==========================================
        public List<StockCountItem> GenerateCountList(int countType, string keyword)
        {
            //// countType 规则约定: 0=按单一库位盘点, 1=按指定物料盘点, 2=全仓盲盘 (对应文档的多方式盘点要求)
            //// 0=按单一库位, 1=按指定物料, 2=按物料分类盘点, 3=按品牌盘点（更新）
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 基础 SQL：关联查询库存和物料名称
                string sql = @"
                    SELECT s.Id as StockId, s.GoodsCode, g.Name as GoodsName, 
                           s.LocationCode, s.BatchNo, s.Qty as SystemQty, 
                           s.Qty as PhysicalQty -- 初始让实盘等于账面
                    FROM Stock s
                    INNER JOIN Goods g ON s.GoodsCode = g.Code
                    WHERE s.Qty > 0";

                // 动态拼接查询条件
                //核心扩展：增加分类和品牌判定逻辑
                if (countType == 0 && !string.IsNullOrEmpty(keyword))
                {
                    sql += " AND s.LocationCode = @kw";
                }
                else if (countType == 1 && !string.IsNullOrEmpty(keyword))
                {
                    sql += " AND s.GoodsCode = @kw";
                }
                else if (countType == 2 && !string.IsNullOrEmpty(keyword))
                {
                    //  对应 PDF 需求：物料分类盘点 
                    sql += " AND g.Category = @kw";
                }
                else if (countType == 3 && !string.IsNullOrEmpty(keyword))
                {
                    //  对应 PDF 需求：物料品牌盘点 
                    sql += " AND g.Brand = @kw";
                }

                // 按照库位和物料排序，方便工人按顺序去数
                sql += " ORDER BY s.LocationCode, s.GoodsCode";

                return db.Query<StockCountItem>(sql, new { kw = keyword }).ToList();
            }
        }

        // ==========================================
        // 智能盘点 - 2. 批量提交盘点差异并一键平账
        // ==========================================
        public bool SubmitBatchCountResult(List<StockCountItem> countList)
        {
            // 过滤出真正产生了差异的记录（如果实盘和账面一样，就不浪费数据库性能去更新了）
            var changedItems = countList.Where(x => x.Difference != 0).ToList();
            if (changedItems.Count == 0) return true; // 全都账实相符，直接返回成功

            string orderNo = "CHECK-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var db = new SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in changedItems)
                        {
                            // 1. 更新库存表
                            if (item.PhysicalQty == 0)
                            {
                                // 盘点发现货完全没了，删除记录
                                db.Execute("DELETE FROM Stock WHERE Id = @id", new { id = item.StockId }, transaction);

                                // 检查库位是否彻底空了，释放库位
                                var remain = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Stock WHERE LocationCode = @lCode", new { lCode = item.LocationCode }, transaction);
                                if (remain == 0) db.Execute("UPDATE Location SET IsUsed = 0 WHERE Code = @lCode", new { lCode = item.LocationCode }, transaction);
                            }
                            else
                            {
                                // 直接将库存更新为真实的物理数量
                                db.Execute("UPDATE Stock SET Qty = @qty WHERE Id = @id", new { qty = item.PhysicalQty, id = item.StockId }, transaction);
                            }

                            // 2. 记一笔盘盈/盘亏流水账
                            int recordType = item.Difference > 0 ? 0 : 1; // 差异>0算入库(盘盈)，<0算出库(盘亏)
                            int changeQty = Math.Abs(item.Difference);    // 流水里只记变动的绝对值

                            db.Execute(@"INSERT INTO StockRecord (RecordType, OrderNo, GoodsCode, LocationCode, Qty, BatchNo, OperateTime, Operator) 
                                         VALUES (@rType, @order, @gCode, @lCode, @qty, @batch, GETDATE(), @opName)",
                                         new
                                         {
                                             rType = recordType,
                                             order = orderNo,
                                             gCode = item.GoodsCode,
                                             lCode = item.LocationCode,
                                             qty = changeQty,
                                             batch = item.BatchNo,
                                             opName = FIH_WMS_System.Program.CurrentUsername
                                         }, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 智能引擎：全仓扫描，生成库位合并(碎片整理)建议
        // ==========================================
        public List<ConsolidationAdvice> GetConsolidationAdvice()
        {
            using (var db = new SqlConnection(connStr))
            {
                var adviceList = new List<ConsolidationAdvice>();

                // 1. 找出所有“身首异处”的物料 (同一种物料，被分散存放在了 > 1 个库位里)
                string sqlFindFragmented = @"
                    SELECT GoodsCode 
                    FROM Stock 
                    WHERE Qty > 0 
                    GROUP BY GoodsCode 
                    HAVING COUNT(DISTINCT LocationCode) > 1";

                var fragmentedGoodsCodes = db.Query<string>(sqlFindFragmented).ToList();

                // 2. 针对每一个分散的物料，计算最佳合并路径
                foreach (string gCode in fragmentedGoodsCodes)
                {
                    // 查出该物料在所有库位的情况，并【按数量从大到小排序】
                    string sqlGetStocks = @"
                        SELECT s.LocationCode, s.Qty, g.Name as GoodsName 
                        FROM Stock s 
                        INNER JOIN Goods g ON s.GoodsCode = g.Code 
                        WHERE s.GoodsCode = @code AND s.Qty > 0 
                        ORDER BY s.Qty DESC";

                    // 使用 dynamic 动态接收连表查询的结果
                    var stocks = db.Query(sqlGetStocks, new { code = gCode }).ToList();

                    if (stocks.Count > 1)
                    {
                        // 策略：把数量最多的那个库位作为“大本营” (ToLocation)
                        var targetStock = stocks.First();

                        // 把其他数量较少的库位 (碎片)，全部建议搬到大本营去
                        for (int i = 1; i < stocks.Count; i++)
                        {
                            var sourceStock = stocks[i];

                            adviceList.Add(new ConsolidationAdvice
                            {
                                GoodsCode = gCode,
                                GoodsName = targetStock.GoodsName,
                                FromLocation = sourceStock.LocationCode,
                                ToLocation = targetStock.LocationCode,
                                MoveQty = (int)sourceStock.Qty
                            });
                        }
                    }
                }

                return adviceList;
            }
        }


        // ==========================================
        // 智能制造：1. 分析 BOM 齐套性
        // ==========================================
        public List<BOMRequirement> AnalyzeBOM(string parentCode, int produceQty)
        {
            using (var db = new SqlConnection(connStr))
            {
                // 这段 SQL 非常强大：它不仅根据生产数量算出了总需求，还同时去查了真实可用库存
                string sql = @"
                    SELECT 
                        b.ChildGoodsCode, 
                        g.Name as ChildGoodsName, 
                        CAST(b.RequiredQty * @qty AS INT) as RequiredTotalQty,
                        ISNULL((SELECT SUM(Qty - FrozenQty) FROM Stock WHERE GoodsCode = b.ChildGoodsCode), 0) as CurrentAvailableQty
                    FROM ProductBOM b
                    LEFT JOIN Goods g ON b.ChildGoodsCode = g.Code
                    WHERE b.ParentGoodsCode = @pCode";

                return db.Query<BOMRequirement>(sql, new { qty = produceQty, pCode = parentCode }).ToList();
            }
        }

        // ==========================================
        // 智能制造：2. 一键执行 BOM 全套出库
        // ==========================================
        public bool ExecuteBOMOutbound(List<BOMRequirement> requirements, string orderNo, OutboundStrategy strategy = OutboundStrategy.FIFO)
        {
            // 遍历所有物料需求，依次调用我们之前写好的智能出库核心 SmartOutStock！
            foreach (var req in requirements)
            {
                if (!req.IsEnough) return false; // 安全卡控：只要有一个物料不够，绝对不执行

                // 复用智能出库逻辑，自动拼单、扣库存、呼叫 AGV
                bool ok = SmartOutStock(req.ChildGoodsCode, req.RequiredTotalQty, orderNo, strategy);

                if (!ok) return false; // 如果某个物料出库异常，立即中止
            }
            return true;
        }


        // ==========================================
        // 智能制造：贴标入库 (自动生成并记录 ReelId)
        // ==========================================
        //参数增加 produceDate，选填
        //// 1. 在参数列表最后增加 relatedOrderNo
        public string InStockWithLabel(string goodsCode, int qty, string locationCode, DateTime? produceDate = null, string relatedOrderNo = "")
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 1. 物料安检，同时把该物料的详细信息（品牌、分类）查出来，用于生成条码！
                var goodsInfo = db.QueryFirstOrDefault<Goods>("SELECT * FROM Goods WHERE Code = @g", new { g = goodsCode });
                if (goodsInfo == null) return "ERROR_GOODS";

                int locCount = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Location WHERE Code = @l", new { l = locationCode });
                if (locCount == 0) return "ERROR_LOCATION";

                // 核心：自定义编码规则引擎
                // 规则设定为：[品牌前缀(取前2位)]-[分类前缀(取前2位)]-[物料编码]-[年月日]-[4位流水号]
                string brandPrefix = string.IsNullOrEmpty(goodsInfo.Brand) ? "XX" : (goodsInfo.Brand.Length >= 2 ? goodsInfo.Brand.Substring(0, 2).ToUpper() : goodsInfo.Brand.ToUpper());
                string catPrefix = string.IsNullOrEmpty(goodsInfo.Category) ? "XX" : (goodsInfo.Category.Length >= 2 ? goodsInfo.Category.Substring(0, 2).ToUpper() : goodsInfo.Category.ToUpper());

                // 生成最终符合工业溯源标准的 ReelId
                string reelId = $"{brandPrefix}-{catPrefix}-{goodsCode}-{DateTime.Now.ToString("yyyyMMdd")}-{new Random().Next(1000, 9999)}";

                string batchNo = "BATCH-" + DateTime.Now.ToString("yyyyMMdd");

                string checkSql = "SELECT Id FROM Stock WHERE GoodsCode = @g AND LocationCode = @l";



                //(入库时记录生产日期)
                //如果工人没填生产日期，系统默认算作今天
                DateTime pd = produceDate ?? DateTime.Now.Date;

                var stockId = db.QueryFirstOrDefault<int?>("SELECT Id FROM Stock WHERE GoodsCode = @g AND LocationCode = @l", new { g = goodsCode, l = locationCode });

                if (stockId != null)
                {
                    // 更新库存时，刷新 ProduceDate
                    db.Execute("UPDATE Stock SET Qty = Qty + @q, ReelId = @r, BatchNo = @b, InStockTime = GETDATE(), ProduceDate = @pd WHERE Id = @id",
                        new { q = qty, r = reelId, b = batchNo, pd = pd, id = stockId });
                }
                else
                {
                    // 新增库存时，插入 ProduceDate
                    db.Execute("INSERT INTO Stock (GoodsCode, LocationCode, Qty, BatchNo, ReelId, InStockTime, FrozenQty, ProduceDate) VALUES (@g, @l, @q, @b, @r, GETDATE(), 0, @pd)",
                        new { g = goodsCode, l = locationCode, q = qty, b = batchNo, r = reelId, pd = pd });
                }



                //string orderNo = "AUTO-IN-" + DateTime.Now.ToString("HHmmss");
                //2. 修改：如果有传关联单号，就用它；没有才生成 AUTO-IN
                string orderNo = string.IsNullOrEmpty(relatedOrderNo) ? "AUTO-IN-" + DateTime.Now.ToString("HHmmss") : relatedOrderNo;



                //db.Execute("INSERT INTO StockRecord (OrderNo, RecordType, GoodsCode, LocationCode, Qty, BatchNo, ReelId, OperateTime, Operator) VALUES (@o, 0, @g, @l, @q, @b, @r, GETDATE(), @op)",
                //    new { o = orderNo, g = goodsCode, l = locationCode, q = qty, b = batchNo, r = reelId, op = FIH_WMS_System.Program.CurrentUsername });

                //db.Execute("UPDATE Location SET IsUsed = 1 WHERE Code = @l", new { l = locationCode });
                // ... (记流水账和更新Location逻辑保持不变，确保流水账里用到上面的 orderNo) ...
                db.Execute("INSERT INTO StockRecord (OrderNo, RecordType, GoodsCode, LocationCode, Qty, BatchNo, ReelId, OperateTime, Operator) VALUES (@o, 0, @g, @l, @q, @b, @r, GETDATE(), @op)",
                    new { o = orderNo, g = goodsCode, l = locationCode, q = qty, b = batchNo, r = reelId, op = FIH_WMS_System.Program.CurrentUsername });
                db.Execute("UPDATE Location SET IsUsed = 1 WHERE Code = @l", new { l = locationCode });



                //3. 核心新增：联动闭环！反写更新采购单状态
                if (!string.IsNullOrEmpty(relatedOrderNo))
                {
                    // ① 累加明细表的实际收货数量
                    db.Execute("UPDATE WmsOrderDetail SET ActualQty = ActualQty + @q WHERE OrderNo = @o AND GoodsCode = @g", new { q = qty, o = relatedOrderNo, g = goodsCode });

                    // ② 判定该条明细是否收齐 (0待处理, 1部分完成, 2已完成)
                    db.Execute("UPDATE WmsOrderDetail SET Status = CASE WHEN ActualQty >= PlanQty THEN 2 ELSE 1 END WHERE OrderNo = @o AND GoodsCode = @g", new { o = relatedOrderNo, g = goodsCode });

                    // ③ 检查整个主订单的所有明细是不是都收齐了？
                    int pendingCount = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM WmsOrderDetail WHERE OrderNo = @o AND Status < 2", new { o = relatedOrderNo });
                    if (pendingCount == 0)
                    {
                        db.Execute("UPDATE WmsOrder SET Status = 2 WHERE OrderNo = @o", new { o = relatedOrderNo }); // 全部收齐，主单结案！
                    }
                    else
                    {
                        db.Execute("UPDATE WmsOrder SET Status = 1 WHERE OrderNo = @o", new { o = relatedOrderNo }); // 还有欠料，主单变更为“执行中”
                    }
                }



                // 4. 核心闭环：生成 AGV 入库搬运任务！(起点是收货贴标区，终点是分配好的智能库位)
                string agvTaskNo = "AGV-IN-" + DateTime.Now.ToString("HHmmss") + "-" + new Random().Next(10, 99);

                db.Execute(@"INSERT INTO AgvTask (TaskNo, TaskType, Status, GoodsCode, Qty, FromLocation, ToLocation, CreateTime) 
                             VALUES (@tNo, 0, 0, @gCode, @qty, '收货接驳区(贴标站)', @toLoc, GETDATE())",
                             new { tNo = agvTaskNo, gCode = goodsCode, qty = qty, toLoc = locationCode });



                return reelId;
            }
        }


        // ==========================================
        // 基础数据管理：获取所有物料档案
        // ==========================================

        //错误：WinForms 遇上 Dapper dynamic 的底层大坑
        /*        public List<dynamic> GetAllGoods()
                {
                    using (var db = new SqlConnection(connStr))
                    {
                        // 按最新的倒序排列，方便刚添加完就能看到
                        //return db.Query("SELECT Code as 物料编码, Name as 物料名称, Spec as 规格, Category as 分类 FROM Goods ORDER BY Id DESC").ToList();

                        // 👈 SQL 查询中加入了 Brand 字段
                        return db.Query("SELECT Code as 物料编码, Name as 物料名称, Brand as 品牌, Spec as 规格, Category as 分类, SafeQty as 安全库存线 FROM Goods ORDER BY Id DESC").ToList();
                    }
                }*/

        // ==========================================
        // 基础数据管理：获取所有物料档案 (新增展示品牌)
        // ==========================================
        public System.Data.DataTable GetAllGoods() // 此返回值从 List<dynamic> 改为 System.Data.DataTable
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();

                // 使用 ExecuteReader 获取底层数据流，并直接装载进 DataTable
                // 如此WinForms 的表格就能完美识别列名，不会重复拼接了

                //(基础档案展示带上保质期)
                // 新增：SQL中增加 ShelfLifeDays
                var reader = db.ExecuteReader("SELECT Code as 物料编码, Name as 物料名称, Brand as 品牌, Spec as 规格, Category as 分类, SafeQty as 安全库存线, ShelfLifeDays as 保质期天数 FROM Goods ORDER BY Id DESC");
                dt.Load(reader);

                return dt;
            }
        }

        // ==========================================
        // 基础数据管理：新增物料档案 (支持录入品牌)
        // ==========================================
        // 参数中增加 shelfLifeDays(建档时写入保质期)
        public bool AddNewGoods(string code, string name, string spec, string category, string brand, int shelfLifeDays)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 先查一下，防止编码重复录入
                int count = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Goods WHERE Code = @c", new { c = code });
                if (count > 0) return false; // 如果已经存在，直接返回失败

                //(建档时写入保质期)
                // 写入新的物料档案 (增加了 Brand, SafeQty 默认给0)
                //插入语句加上 ShelfLifeDays
                db.Execute("INSERT INTO Goods (Code, Name, Spec, Category, Brand, SafeQty, ShelfLifeDays) VALUES (@c, @n, @s, @cat, @b, 0, @shelf)",
                    new { c = code, n = name, s = spec, cat = category, b = brand, shelf = shelfLifeDays });

                //记录日志
                AddOperationLog("基础数据", $"新增了物料档案：【{name}】(编码:{code})");

                return true;
            }
        }


        // ==========================================
        // 逆向物流：产线返料退回 (自动合并碎片 + 呼叫AGV + 订单追溯)
        // ==========================================
        // 新增第三个参数 relatedOrderNo，默认为空
        public string SmartReturnMaterial(string goodsCode, int qty, string relatedOrderNo = "")
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 1. 安检：校验物料是否存在
                int goodsCount = Dapper.SqlMapper.QueryFirstOrDefault<int>(db, "SELECT COUNT(1) FROM Goods WHERE Code = @g", new { g = goodsCode });
                if (goodsCount == 0) return "ERROR_GOODS";

                // 2. 智能算法：找库位。优先找【同类物料且未满的碎片库位】进行合并！
                string targetLoc = Dapper.SqlMapper.QueryFirstOrDefault<string>(db,
                    "SELECT TOP 1 LocationCode FROM Stock WHERE GoodsCode = @g ORDER BY Qty ASC", new { g = goodsCode });

                // 如果没找到同类物料，找一个完全空闲的新库位
                if (string.IsNullOrEmpty(targetLoc))
                {
                    targetLoc = Dapper.SqlMapper.QueryFirstOrDefault<string>(db,
                        "SELECT TOP 1 Code FROM Location WHERE IsUsed = 0 AND Status = 0 ORDER BY Code ASC");
                }

                if (string.IsNullOrEmpty(targetLoc)) return "ERROR_FULL"; // 仓库满了

                // 3. 执行入账
                string reelId = $"{goodsCode}-RET-{DateTime.Now.ToString("yyyyMMdd")}-{new Random().Next(1000, 9999)}";



                //string batchNo = "RETURN-" + DateTime.Now.ToString("yyyyMMdd"); // 特殊的退料批次前缀
                //核心追溯：如果填写了关联工单，这批退料的批次号直接打上工单烙印
                string batchNo = string.IsNullOrEmpty(relatedOrderNo) ? "RETURN-" + DateTime.Now.ToString("yyyyMMdd") : "RET-" + relatedOrderNo;



                var stockId = Dapper.SqlMapper.QueryFirstOrDefault<int?>(db, "SELECT Id FROM Stock WHERE GoodsCode = @g AND LocationCode = @l", new { g = goodsCode, l = targetLoc });

                if (stockId != null)
                {
                    Dapper.SqlMapper.Execute(db, "UPDATE Stock SET Qty = Qty + @q, ReelId = @r, BatchNo = @b, InStockTime = GETDATE() WHERE Id = @id", new { q = qty, r = reelId, b = batchNo, id = stockId });
                }
                else
                {
                    Dapper.SqlMapper.Execute(db, "INSERT INTO Stock (GoodsCode, LocationCode, Qty, BatchNo, ReelId, InStockTime, FrozenQty) VALUES (@g, @l, @q, @b, @r, GETDATE(), 0)",
                        new { g = goodsCode, l = targetLoc, q = qty, b = batchNo, r = reelId });
                    Dapper.SqlMapper.Execute(db, "UPDATE Location SET IsUsed = 1 WHERE Code = @l", new { l = targetLoc });
                }

                // 4. 写流水账 (记录为入库 RecordType=0)
                //string orderNo = "RETURN-" + DateTime.Now.ToString("HHmmss");
                //流水账的单据号也变更为工单号相关
                string orderNo = string.IsNullOrEmpty(relatedOrderNo) ? "RETURN-" + DateTime.Now.ToString("HHmmss") : "RET-" + relatedOrderNo;

                Dapper.SqlMapper.Execute(db, "INSERT INTO StockRecord (OrderNo, RecordType, GoodsCode, LocationCode, Qty, BatchNo, ReelId, OperateTime, Operator) VALUES (@o, 0, @g, @l, @q, @b, @r, GETDATE(), @op)",
                    new { o = orderNo, g = goodsCode, l = targetLoc, q = qty, b = batchNo, r = reelId, op = FIH_WMS_System.Program.CurrentUsername });


                // 5. 核心：生成 AGV 回收任务！(起点是产线，终点是算出来的智能库位)
                string taskNo = "AGV-RET-" + DateTime.Now.ToString("HHmmss");
                Dapper.SqlMapper.Execute(db, "INSERT INTO AgvTask (TaskNo, TaskType, Status, GoodsCode, Qty, FromLocation, ToLocation, CreateTime) VALUES (@tn, 0, 0, @g, @q, '产线接驳口(退料)', @toLoc, GETDATE())",
                    new { tn = taskNo, g = goodsCode, q = qty, toLoc = targetLoc });

                return targetLoc; // 返回算出来的库位名，给界面弹窗用
            }
        }


        // ==========================================
        // 基础数据管理：批量导入物料档案
        // ==========================================
        public (int successCount, int failCount) BatchAddGoods(List<Models.Goods> goodsList)
        {
            int successCount = 0;
            int failCount = 0; // 记录有多少条因为编码重复被跳过

            using (var db = new SqlConnection(connStr))
            {
                foreach (var g in goodsList)
                {
                    // 查重：防止导入的 Excel 里有已经建过档的物料
                    int count = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Goods WHERE Code = @c", new { c = g.Code });

                    if (count == 0)
                    {
                        // 没见过的新物料，直接插入！
                        db.Execute("INSERT INTO Goods (Code, Name, Spec, Category) VALUES (@c, @n, @s, @cat)",
                            new { c = g.Code, n = g.Name, s = g.Spec, cat = g.Category });
                        successCount++;
                    }
                    else
                    {
                        failCount++; // 遇到重复的老物料，跳过并计数
                    }
                }
            }
            return (successCount, failCount); // 返回战报
        }


        // ==========================================
        // 库位管理：切换库位状态 (0=正常, 1=锁定停用)
        // ==========================================
        public void ToggleLocationStatus(string locCode, int newStatus)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Execute("UPDATE Location SET Status = @s WHERE Code = @c", new { s = newStatus, c = locCode });

                // 记录日志
                string statusName = newStatus == 1 ? "锁定停用" : "解除锁定(恢复正常)";
                AddOperationLog("库位管理", $"将库位【{locCode}】的状态修改为：{statusName}");
            }
        }


        // ==========================================
        // 核心防呆：BOM 齐套性短板检查 (木桶效应)
        // ==========================================
        public string CheckBOMShortage(string parentGoodsCode, int produceQty)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 1. 展开 BOM 树：查出造这个产品都需要哪些零件，各需要多少个？
                string bomSql = "SELECT ChildGoodsCode, RequiredQty FROM ProductBOM WHERE ParentGoodsCode = @p";
                var bomList = db.Query(bomSql, new { p = parentGoodsCode }).ToList();

                // 如果这个物料根本没有 BOM（说明它就是个普通零件，不是组装成品），直接放行
                if (bomList.Count == 0) return "";

                System.Text.StringBuilder shortageMsg = new System.Text.StringBuilder();

                // 2. 遍历每一个子零件，去仓库里翻账本算总账
                foreach (var item in bomList)
                {
                    string childCode = item.ChildGoodsCode;
                    // 计算出产线总共需要的数量 (单机用量 * 生产台数)
                    int requiredTotal = (int)(item.RequiredQty * produceQty);

                    // 查出当前仓库里，这个子零件【所有货架加起来的总可用库存】
                    int availableQty = db.QueryFirstOrDefault<int>(
                        "SELECT ISNULL(SUM(Qty), 0) FROM Stock WHERE GoodsCode = @c", new { c = childCode });

                    // 3. 木桶理论：只要可用库存小于总需求，立刻记入“黑名单”！
                    if (availableQty < requiredTotal)
                    {
                        // 顺手查一下这个零件叫什么名字，方便报错时显示信息
                        string goodsName = db.QueryFirstOrDefault<string>(
                            "SELECT Name FROM Goods WHERE Code = @c", new { c = childCode });

                        int gap = requiredTotal - availableQty; // 计算缺口
                        shortageMsg.AppendLine($"📦 【{goodsName}】(编码:{childCode})：需 {requiredTotal} 个，库存仅 {availableQty} 个，缺口 {gap} 个！");
                    }
                }

                // 返回最终的缺料黑名单。如果长度为0，说明齐套，顺利过关！
                return shortageMsg.ToString();
            }
        }


        // ==========================================
        // 智能制造：3. 波次运算 (Wave Picking) - 多工单物料合并引擎
        // ==========================================
        public List<BOMRequirement> AnalyzeWaveBOM(Dictionary<string, int> waveOrders)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var combinedList = new List<BOMRequirement>();

                // 1. 挨个展开波次里每一个工单的 BOM 需求
                foreach (var order in waveOrders)
                {
                    var singleRequirements = AnalyzeBOM(order.Key, order.Value);
                    combinedList.AddRange(singleRequirements);
                }

                // 2. 波次核心算法：同类项合并！(把不同工单里相同的零件需求加在一起)
                var waveResult = combinedList
                    .GroupBy(x => new { x.ChildGoodsCode, x.ChildGoodsName })
                    .Select(g => new BOMRequirement
                    {
                        ChildGoodsCode = g.Key.ChildGoodsCode,
                        ChildGoodsName = g.Key.ChildGoodsName,
                        // 需求量：把所有工单对这个零件的需求累加
                        RequiredTotalQty = g.Sum(x => x.RequiredTotalQty),
                        // 可用库存：无论几个工单，查出来的全仓可用库存是一样的，取第一条即可
                        CurrentAvailableQty = g.First().CurrentAvailableQty
                        // 防呆重算：合并后需求变大了，必须重新判断库存还够不够！
                        //IsEnough = g.First().CurrentAvailableQty >= g.Sum(x => x.RequiredTotalQty)
                    })
                    .ToList();

                return waveResult;
            }
        }


        // ==========================================
        // 智能预警：查询所有低于安全库存的物料
        // ==========================================
        public List<LowStockItem> GetLowStockWarnings() // 👈 这里换成了具体的类
        {
            using (var db = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT 
                        g.Code AS GoodsCode, 
                        g.Name AS GoodsName, 
                        g.SafeQty AS SafeQty, 
                        ISNULL(SUM(s.Qty - s.FrozenQty), 0) AS CurrentTotalQty
                    FROM Goods g
                    LEFT JOIN Stock s ON g.Code = s.GoodsCode
                    GROUP BY g.Code, g.Name, g.SafeQty
                    HAVING ISNULL(SUM(s.Qty - s.FrozenQty), 0) < g.SafeQty AND g.SafeQty > 0";

                // 👈 Dapper 强类型映射
                return db.Query<LowStockItem>(sql).ToList();
            }
        }

        // ==========================================
        // 智能预警：一键自动生成采购补货单
        // ==========================================
        public bool AutoGeneratePurchaseOrder()
        {
            var shortageList = GetLowStockWarnings();
            if (shortageList.Count == 0) return false;

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string orderNo = "PUR-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        db.Execute("INSERT INTO WmsOrder (OrderNo, OrderType, Status, CreateTime) VALUES (@o, 0, 0, GETDATE())",
                                   new { o = orderNo }, transaction);

                        foreach (var item in shortageList)
                        {
                            // 👈 因为有了强类型，这里不再需要 (int) 和 (string) 强制转换了，代码更安全！
                            int safeQty = item.SafeQty;
                            int currentQty = item.CurrentTotalQty;
                            int needQty = safeQty - currentQty;

                            if (needQty <= 0) continue;

                            db.Execute("INSERT INTO WmsOrderDetail (OrderNo, GoodsCode, PlanQty, ActualQty, Status) VALUES (@o, @g, @q, 0, 0)",
                                new { o = orderNo, g = item.GoodsCode, q = needQty }, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }



        // ==========================================
        // 智能预警：根据 BOM 缺口一键生成采购单
        // ==========================================
        public bool GeneratePurchaseOrderByBOM(List<BOMRequirement> requirements)
        {
            // 1. 过滤出真正“缺料”的项，计算缺口数量
            var shortageList = requirements.Where(r => !r.IsEnough).ToList();
            if (shortageList.Count == 0) return false; // 根本不缺料，不用采购

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 2. 生成一个带 BOM 标识的采购单号
                        string orderNo = "PUR-BOM-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                        // 插入主单据 (OrderType = 0 表示入库/采购)
                        db.Execute("INSERT INTO WmsOrder (OrderNo, OrderType, Status, CreateTime) VALUES (@o, 0, 0, GETDATE())",
                                   new { o = orderNo }, transaction);

                        // 3. 遍历缺料清单，生成明细
                        foreach (var item in shortageList)
                        {
                            // 计算精准的缺口数量：总需求 - 当前可用
                            int needQty = item.RequiredTotalQty - item.CurrentAvailableQty;

                            if (needQty <= 0) continue; // 防呆

                            db.Execute("INSERT INTO WmsOrderDetail (OrderNo, GoodsCode, PlanQty, ActualQty, Status) VALUES (@o, @g, @q, 0, 0)",
                                new { o = orderNo, g = item.ChildGoodsCode, q = needQty }, transaction);
                        }

                        // 4. 记录系统操作日志
                        db.Execute("INSERT INTO SysOperationLog (Username, ActionType, Description, OperateTime) VALUES (@u, '智能采购', @d, GETDATE())",
                            new { u = Program.CurrentUsername, d = $"根据BOM齐套性分析，自动下发采购单：{orderNo}" }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 宏观单据流转：获取主单据列表 (WmsOrder)
        // ==========================================
        public System.Data.DataTable GetAllOrders()
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();
                string sql = @"
                    SELECT 
                        OrderNo AS 单据编号, 
                        CASE OrderType WHEN 0 THEN N'📥 入库单/采购补单' ELSE N'📤 出库单/生产领料单' END AS 单据类型, 
                        CASE Status WHEN 0 THEN N'⏳ 待处理' WHEN 1 THEN N'🚀 执行中' ELSE N'✅ 已完成' END AS 单据状态,
                        CreateTime AS 创建时间 
                    FROM WmsOrder 
                    ORDER BY CreateTime DESC";

                var reader = db.ExecuteReader(sql);
                dt.Load(reader);
                return dt;
            }
        }

        // ==========================================
        // 宏观单据流转：获取单据物料明细 (WmsOrderDetail)
        // ==========================================
        public System.Data.DataTable GetOrderDetails(string orderNo)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();
                string sql = @"
                    SELECT 
                        d.GoodsCode AS 物料编码, 
                        g.Name AS 物料名称, 
                        g.Brand AS 品牌,
                        d.PlanQty AS 计划数量, 
                        d.ActualQty AS 实际完成数量, 
                        CASE d.Status WHEN 0 THEN '待处理' WHEN 1 THEN '部分完成' ELSE '已完成' END AS 明细状态 
                    FROM WmsOrderDetail d 
                    LEFT JOIN Goods g ON d.GoodsCode = g.Code 
                    WHERE d.OrderNo = @o";

                var reader = db.ExecuteReader(sql, new { o = orderNo });
                dt.Load(reader);
                return dt;
            }
        }


        // ==========================================
        // 用户权限管理：获取所有账号
        // ==========================================
        public System.Data.DataTable GetAllUsers()
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();
                // 密码是隐私，不能在表格里直接展示出来！
                var reader = db.ExecuteReader("SELECT Id AS 用户编号, Username AS 登录账号, Role AS 角色权限 FROM Users ORDER BY Id");
                dt.Load(reader);
                return dt;
            }
        }

        // ==========================================
        // 用户权限管理：新增账号
        // ==========================================
        public bool AddUser(string username, string password, string role)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 检查账号是否已存在
                int count = db.QueryFirstOrDefault<int>("SELECT COUNT(1) FROM Users WHERE Username = @u", new { u = username });
                if (count > 0) return false;

                db.Execute("INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)",
                    new { u = username, p = password, r = role });

                //记录日志
                AddOperationLog("权限管理", $"新增了系统账号：【{username}】，角色为：{role}");

                return true;
            }
        }

        // ==========================================
        // 用户权限管理：重置密码
        // ==========================================
        public void ResetPassword(int userId, string newPassword)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Execute("UPDATE Users SET Password = @p WHERE Id = @id", new { p = newPassword, id = userId });

                //记录日志
                AddOperationLog("权限管理", $"将用户编号为 【{userId}】 的密码重置为初始密码");
            }
        }

        // ==========================================
        // 用户权限管理：删除账号
        // ==========================================
        public void DeleteUser(int userId)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Execute("DELETE FROM Users WHERE Id = @id", new { id = userId });

                //记录日志
                AddOperationLog("权限管理", $"永久删除了用户编号为 【{userId}】 的账号");
            }
        }


        // ==========================================
        // 库存管理：冻结 / 解除冻结
        // ==========================================
        public bool ToggleFreezeStock(int stockId, bool isFreeze)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                if (isFreeze)
                {
                    // 🔒 全数冻结（把可用数量全部变为冻结状态）
                    db.Execute("UPDATE Stock SET FrozenQty = Qty WHERE Id = @id", new { id = stockId });
                }
                else
                {
                    // 🔓 解除冻结（冻结数量清零）
                    db.Execute("UPDATE Stock SET FrozenQty = 0 WHERE Id = @id", new { id = stockId });
                }

                // 记录日志
                string actionName = isFreeze ? "全数冻结" : "解除冻结";
                AddOperationLog("库存管理", $"{actionName}了系统库存ID：【{stockId}】");

                return true;
            }
        }


        // ==========================================
        // 业务流转：获取待收货的采购入库单
        // ==========================================
        public List<string> GetPendingInboundOrders()
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 查找订单类型为入库(0)，且状态不是已完成(< 2)的单号
                return db.Query<string>("SELECT OrderNo FROM WmsOrder WHERE OrderType = 0 AND Status < 2 ORDER BY CreateTime DESC").ToList();
            }
        }

        // 业务流转：根据采购单号，智能带出下一条缺料明细
        public Models.WmsOrderDetail GetNextPendingDetail(string orderNo)
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 找出这个订单下，还没收齐货的第一条物料
                return db.QueryFirstOrDefault<Models.WmsOrderDetail>("SELECT * FROM WmsOrderDetail WHERE OrderNo = @o AND Status < 2", new { o = orderNo });
            }
        }


        // ==========================================
        // 14. 获取图表统计数据 (近7天出入库流量趋势)
        // ==========================================
        public List<dynamic> Get7DaysTraffic()
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                // 核心逻辑：使用 CONVERT 截取日期部分(砍掉时分秒)，按天分组，统计过去 7 天每天的进出总量
                string sql = @"
            SELECT 
                CONVERT(varchar(10), OperateTime, 120) as DateStr,
                RecordType,
                SUM(Qty) as TotalQty
            FROM StockRecord
            WHERE OperateTime >= CAST(DATEADD(day, -6, GETDATE()) AS DATE)
            GROUP BY CONVERT(varchar(10), OperateTime, 120), RecordType
            ORDER BY DateStr";

                // 直接用 Dapper 返回 dynamic 列表，前台拆解
                return Dapper.SqlMapper.Query(db, sql).ToList();
            }
        }


        // ==========================================
        // 审计功能：记录系统操作日志
        // ==========================================
        public void AddOperationLog(string actionType, string description)
        {
            // 如果还没登录（比如登录失败的尝试），或者拿不到用户名，就记作 System 或 Unknown
            string currentUser = string.IsNullOrEmpty(Program.CurrentUsername) ? "System" : Program.CurrentUsername;

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                string sql = @"INSERT INTO SysOperationLog (Username, ActionType, Description, OperateTime) 
                               VALUES (@u, @a, @d, GETDATE())";

                Dapper.SqlMapper.Execute(db, sql, new
                {
                    u = currentUser,
                    a = actionType,
                    d = description
                });
            }
        }


        // ==========================================
        // 审计功能：获取所有系统操作日志
        // ==========================================
        public System.Data.DataTable GetOperationLogs()
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();
                // 按时间倒序，最新的操作排在最上面
                string sql = @"
                    SELECT 
                        Id AS 日志编号, 
                        Username AS 操作人账号, 
                        ActionType AS 操作模块, 
                        Description AS 详细操作记录, 
                        OperateTime AS 操作时间 
                    FROM SysOperationLog 
                    ORDER BY OperateTime DESC";

                var reader = Dapper.SqlMapper.ExecuteReader(db, sql);
                dt.Load(reader);
                return dt;
            }
        }


        // ==========================================
        // 获取 AGV 运行轨迹日志
        // 可以按任务单号过滤，也可以查询全部
        // ==========================================
        public System.Data.DataTable GetAgvLogs(string taskNo = "")
        {
            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                var dt = new System.Data.DataTable();
                string sql = @"
            SELECT 
                Id AS 序号, 
                TaskNo AS 任务单号, 
                Action AS 动作描述, 
                Location AS 当前位置, 
                LogTime AS 记录时间 
            FROM AgvLog ";

                if (!string.IsNullOrEmpty(taskNo))
                {
                    sql += " WHERE TaskNo = @t ";
                }

                sql += " ORDER BY LogTime DESC";

                var reader = Dapper.SqlMapper.ExecuteReader(db, sql, new { t = taskNo });
                dt.Load(reader);
                return dt;
            }
        }


        // ==========================================
        // 宏观单据流转：手工建单 (手工录入采购入库单)
        // ==========================================
        public bool CreateManualPurchaseOrder(List<Models.WmsOrderDetail> details)
        {
            if (details == null || details.Count == 0) return false;

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. 生成带有 MAN (Manual) 标识的采购单号
                        string orderNo = "PUR-MAN-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                        // 2. 插入主单据 (OrderType = 0 表示入库单/采购单)
                        Dapper.SqlMapper.Execute(db,
                            "INSERT INTO WmsOrder (OrderNo, OrderType, Status, CreateTime) VALUES (@o, 0, 0, GETDATE())",
                            new { o = orderNo }, transaction);

                        // 3. 循环插入所有物料明细
                        foreach (var item in details)
                        {
                            Dapper.SqlMapper.Execute(db,
                                "INSERT INTO WmsOrderDetail (OrderNo, GoodsCode, PlanQty, ActualQty, Status) VALUES (@o, @g, @q, 0, 0)",
                                new { o = orderNo, g = item.GoodsCode, q = item.PlanQty }, transaction);
                        }

                        // 4. 记录系统审计日志
                        Dapper.SqlMapper.Execute(db,
                            "INSERT INTO SysOperationLog (Username, ActionType, Description, OperateTime) VALUES (@u, '单据管理', @d, GETDATE())",
                            new { u = Program.CurrentUsername, d = $"手工创建了采购入库单：{orderNo}" }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // ==========================================
        // 宏观单据流转：入库波次合并 (Consolidation)
        // ==========================================
        public bool ConsolidateInboundOrders(List<string> orderNos, string waveNo)
        {
            if (orderNos == null || orderNos.Count == 0 || string.IsNullOrEmpty(waveNo)) return false;

            using (var db = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 将选中的所有入库单的 WaveNo 更新为指定值
                        foreach (var orderNo in orderNos)
                        {
                            Dapper.SqlMapper.Execute(db,
                                "UPDATE WmsOrder SET WaveNo = @w WHERE OrderNo = @o",
                                new { w = waveNo, o = orderNo }, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }



    }
}