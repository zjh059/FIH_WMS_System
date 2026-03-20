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
        public string GetRecommendLocation(string goodsCode)
        {
            using (var db = new SqlConnection(connStr))
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



    }
}