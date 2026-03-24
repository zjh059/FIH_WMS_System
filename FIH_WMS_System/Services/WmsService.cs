using FIH_WMS_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Dapper;




namespace FIH_WMS_System.Services
{
    public class WmsService
    {
        // 数据库连接
        private string connStr = "Server=LAPTOP-9IABD0I1;Database=FIH_WMS_DB;User Id=sa;Password=123456;TrustServerCertificate=True;";

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
                        // 1. 检查同一个库位、同一个商品、同一个批次是不是已经有记录了
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
                                // 1. 查出这个库位上，这个商品的所有批次库存，并且【按入库时间从早到晚排序】（这就是先进先出的核心！）
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
                        // 库存扣完后，我们生成一个 AGV 任务指令发给硬件层
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
                    SELECT s.Id, s.Qty, s.BatchNo, s.ProduceDate, s.InStockTime,
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
                    SELECT r.Id, r.RecordType as Type, r.OrderNo, r.BatchNo, r.Qty, r.OperateTime, r.Operator,
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
        public string GetRecommendLocation(string goodsCode, InboundStrategy strategy = InboundStrategy.SameMaterialMerge)
        {

/*            using (var db = new SqlConnection(connStr))
            {
                // (1)按未满库位入库（减少碎片）
                // 先去库存表里找找，有没有哪个库位已经放了这个商品了？如果有，我们就推荐跟它放在一起。
                string existLoc = db.ExecuteScalar<string>(
                    "SELECT TOP 1 LocationCode FROM Stock WHERE GoodsCode = @gCode",
                    new { gCode = goodsCode });

                if (!string.IsNullOrEmpty(existLoc))
                {
                    return existLoc; // 找到了！直接推荐合并存放
                }

                // (2)按空库位入库（不混合存储）
                // 如果这是一个全新的商品，就去 Location 表里找一个完全没人用的空库位 (IsUsed = 0)
                string emptyLoc = db.ExecuteScalar<string>( "SELECT TOP 1 Code FROM Location WHERE IsUsed = 0");

                if (!string.IsNullOrEmpty(emptyLoc))
                {
                    return emptyLoc; // 找到了一个空闲的新家！
                }

                // (3)如果都没找到，说明整个仓库连一个空位都没有了，爆仓了！
                return string.Empty;
            }*/

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
                Location recommendedLoc = engine.RecommendLocation(goodsCode, allLocations, currentStocks, strategy);

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
        public bool CompleteAgvTask(int taskId)
        {
            using (var db = new SqlConnection(connStr))
            {
                // 将状态更新为 3 (已完成)，并打上完成时间戳
                int rows = db.Execute(@"
                    UPDATE AgvTask 
                    SET Status = 3, FinishTime = GETDATE() 
                    WHERE Id = @id AND Status != 3",
                    new { id = taskId });

                return rows > 0; // 如果受影响的行数大于0，说明更新成功
            }
        }


        // ==========================================
        // 10. 获取图表统计数据 (各商品总库存)
        // ==========================================
        public Dictionary<string, int> GetStockChartData()
        {
            using (var db = new SqlConnection(connStr))
            {
                // 使用 SQL 的 GROUP BY 把同一个商品在不同库位的数量加起来 (SUM)
                string sql = @"
                    SELECT g.Name, SUM(s.Qty) as TotalQty 
                    FROM Stock s 
                    INNER JOIN Goods g ON s.GoodsCode = g.Code 
                    GROUP BY g.Name";

                // 执行查询，并转换成 字典(Dictionary) 格式，方便前端画图 (Key:商品名, Value:总数量)
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
                // 用 LEFT JOIN 把库位表、库存表、商品表连起来
                // 这样既能查到空闲的货架，也能查到放了什么货

                // 使用 GROUP BY 和 SUM，确保每个库位只返回唯一的一行汇总数据
                string sql = @"
                    SELECT l.Code, l.IsUsed, 
                           MAX(ISNULL(g.Name, '')) as GoodsName, 
                           SUM(ISNULL(s.Qty, 0)) as Qty
                    FROM Location l
                    LEFT JOIN Stock s ON l.Code = s.LocationCode
                    LEFT JOIN Goods g ON s.GoodsCode = g.Code
                    GROUP BY l.Code, l.IsUsed
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
            using (var db = new SqlConnection(connStr))
            {
                // 1. 获取所有非空库存
                var currentStocks = db.Query<Stock>("SELECT * FROM Stock WHERE Qty > 0").ToList();

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
                                             gCode = goodsCode,
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
                                         new { tNo = agvTaskNo, gCode = goodsCode, qty = pick.Qty, fromLoc = pick.LocationCode }, transaction);
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
        // 智能盘点 - 1. 根据条件生成盘点清单
        // ==========================================
        public List<StockCountItem> GenerateCountList(int countType, string keyword)
        {
            // countType 规则约定: 0=按单一库位盘点, 1=按指定物料盘点, 2=全仓盲盘 (对应文档的多方式盘点要求)
            using (var db = new SqlConnection(connStr))
            {
                // 基础 SQL：关联查询库存和商品名称
                string sql = @"
                    SELECT s.Id as StockId, s.GoodsCode, g.Name as GoodsName, 
                           s.LocationCode, s.BatchNo, s.Qty as SystemQty, 
                           s.Qty as PhysicalQty -- 初始让实盘等于账面
                    FROM Stock s
                    INNER JOIN Goods g ON s.GoodsCode = g.Code
                    WHERE s.Qty > 0";

                // 动态拼接查询条件
                if (countType == 0 && !string.IsNullOrEmpty(keyword))
                {
                    sql += " AND s.LocationCode = @kw";
                }
                else if (countType == 1 && !string.IsNullOrEmpty(keyword))
                {
                    sql += " AND s.GoodsCode = @kw";
                }

                // 按照库位和商品排序，方便工人按顺序去数
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



    }
}