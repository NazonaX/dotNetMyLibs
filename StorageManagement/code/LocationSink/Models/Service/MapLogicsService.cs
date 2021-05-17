using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Models.Entity;

namespace Models.Service
{
    /// <summary>
    /// need to be notified that we define one cargoway just has one zone, or for now will cause some calculating exceptions
    /// </summary>
    internal class MapLogicsService: IMapLogicsService
    {
        private Entity.Map _map = null;
        private Indexer _indexer = null;
        //这个两个Unit用于计算简单时间花费
        double LifterCostUnit = 7;
        double SrCostUnit = 1;
        double RCostUnit = 1;

        //各个用于计算罚分的系数
        //层系数，该系数用于控制换层策略
        double LayerRatio = 10;
        //入库点系数，该系数用于控制入库点到货道的简单时间花费，也即基本货道筛选策略
        double InputRatio = 10;
        //填充系数，该系数用于控制优先填充策略
        double FillRatio = 10;
        //混乱系数，该系数用于控制选择货道所含物品种类的策略
        double ChaosRatio = 10;
        //多重入库点系数，该系数用于计算单货道对于多个入库点的选择策略
        double MultiInRatio = 10;

        public MapLogicsService(Entity.Map map)
        {
            this._map = map;
        }


        #region Methods
        /// <summary>
        /// 根据当前栅格地图设置，扫描并初始化货道信息。
        /// 初始化直接在_map中，记得需要保存到数据库。
        /// 调用会签需要先删除原有数据库的CargoWays，CaogoWaysLock和Goods，然后调用，然后保存到数据库，然后更新MapItems的CargoWaysId。
        /// </summary>
        /// <returns>计算得出的货道列表</returns>
        public List<CargoWays> InitializeCargoPaths()
        {
            //Utils.IOOps.DeleteFile(Utils.Logger.LogPath);
            Entity.MapItems[,,] caltmp = new Entity.MapItems[_map.LayerCount + 1, _map.RackCount + 2, _map.ColumnCount + 2];
            //find the None Zone Id
            int None = MapSingletonService.Instance.Type_GetNoneId();
            int Input = MapSingletonService.Instance.Type_GetInputId();
            int Output = MapSingletonService.Instance.Type_GetOutputId();
            int Lifter = MapSingletonService.Instance.Type_GetLifterId();
            int Rail = MapSingletonService.Instance.Type_GetRailId();
            int Unavailable = MapSingletonService.Instance.Type_GetUnavailableId();
            int StorageRail = MapSingletonService.Instance.Type_GetStorageRailId();
            int Storage = MapSingletonService.Instance.Type_GetStorageId();
            //initial the tmp map, for the tmp map has two more dimesions as bounds
            for (int i = 0; i < _map.LayerCount + 1; i++)
            {
                for (int j = 0; j < _map.RackCount + 2; j++)
                {
                    for (int k = 0; k < _map.ColumnCount + 2; k++)
                    {
                        if (i == _map.LayerCount || j == 0 || j == _map.RackCount + 1
                            || k == 0 || k == _map.ColumnCount + 1)
                        {
                            caltmp[i, j, k] = new Entity.MapItems()
                            {
                                TypeId = None
                            };
                        }
                        else
                        {
                            caltmp[i, j, k] = _map[i, j - 1, k - 1];
                        }
                    }
                }
            }
            //scan each
            List<CargoWays> additionCargoways = new List<CargoWays>();
            for (int i = 0; i < _map.LayerCount; i++)
            {
                //we scan each row of certain columns
                for (int j = 0; j < _map.RackCount + 2; j++)
                {
                    for (int k = 0; k < _map.ColumnCount + 2; k++)
                    {
                        if (caltmp[i, j, k].TypeId == Storage)
                        {
                            Entity.CargoWays newone = new CargoWays();
                            //TODO:: need to set cargoway number here as a code string stands for layer-zone-rack infos
                            newone.LeftRailColumn = k - 2;//-1 is for back to the pre grid; another -1 is for from caltmp to mapitems
                            newone.LeftIsRail = caltmp[i, j, k - 1].TypeId == Rail ? true : false;
                            while (k < _map.ColumnCount + 2 && caltmp[i, j, k].TypeId == Storage)
                            {
                                k++;
                            };
                            newone.RightRailColumn = k - 1;
                            newone.RightIsRail = caltmp[i, j, k].TypeId == Rail ? true : false;
                            newone.ZoneAt = caltmp[i, j, k - 1].ZoneId;//assign the last mapitem scaned
                            newone.LayerAt = i;
                            newone.RackAt = j - 1;
                            newone.CargoWayNumber = string.Format("{0:X2}{1:X2}{2:X2}", newone.LayerAt, newone.RightRailColumn, newone.RackAt);//256*256*256种情况容量
                            additionCargoways.Add(newone);
                        }
                    }
                }
            }
            return additionCargoways;
        }
        /// <summary>
        /// 根据当前的栅格地图设置，扫描并初始化轨道信息。
        /// </summary>
        /// <returns>计算得出的轨道列表</returns>
        public List<Rails> InitializeRails()
        {
            int Rail = MapSingletonService.Instance.Type_GetRailId();
            List<Models.Entity.Rails> newRails = new List<Rails>();
            for(int i = 0; i < _map.LayerCount; i++)
            {
                for(int k = 0; k < _map.ColumnCount; k++)
                {
                    bool isRail = true;
                    for(int j = 0; j < _map.RackCount; j++)
                    {
                        if(_map[i,j,k].TypeId != Rail)
                        {
                            isRail = false;
                            break;
                        }
                    }
                    if (isRail)
                    {
                        Models.Entity.Rails tmp = new Rails();
                        tmp.RailColumn = k;
                        tmp.RailLayer = i;
                        tmp.RailNumber = string.Format("{0:X2}{1:X2}", i, k);
                        newRails.Add(tmp);
                    }
                }
            }
            return newRails;
        }
        /// <summary>
        /// 构建货道索引，包含了初始化计算Units、初始化扫描空白货道、初始化计算势能、初始化计算简单花费、装载货物以及装载货道锁。
        /// </summary>
        public void InitializeIndexer()
        {
            //Utils.IOOps.DeleteFile(Utils.Logger.LogPath);
            ComputeRatiosAndUnits();//as const, compute the ratios and units using _map's basic infos
            InitializeBlankCargoWays();//as const, initialize bottom and top 1~2
            InitializePotentialEnergy();//as const, initialize potential energy
            InitializeWithIOReachable();//as const, initialize Input/Output_OutputPoint 1~2
            InitializeWithGoods();//as variable, load goods
            InitializeWithLocks();//as variable, load cargoway locks
            //we need a class to organize the indexes here
            //designe the class as an inner class
            //WrapCargoWaysWithAvgDetailedCost();//wrap with detailed time cost
            _indexer = new Indexer();
            foreach (Entity.CargoWays cw in _map.CargoWays)
            {
                _indexer.Insert(cw);
            }
            //used to log out for indexer message
            //_indexer.PlainToLog();
        }
        /// <summary>
        /// 该方法用于输入一个货物订单、入库点和区域来获取货道锁定信息。
        /// 该方法获取到的货道锁定信息，并未保存以及同步当前内存中的货道、索引状态，需要调用ConfirmAndSetCargoWaysLocks来保存。
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="inPoint">指定入库点列表，可以为空代表所有入库点可选</param>
        /// <param name="destArea">指定区域，可以为空代表所有区域可选</param>
        /// <returns></returns>
        private List<Entity.CargoWaysLock> GetLockLocations(LogicsOrder order, List<Entity.MapItems> inPoint = null, List<int> destArea = null)
        {
            //Utils.IOOps.DeleteFile(Utils.Logger.LogPath);
            if (destArea == null)
                destArea = new List<int>();
            if (inPoint == null)
                inPoint = new List<MapItems>();
            //filter the filteredByGoodsAndZones by inPoints
            //we dont filter count and layers here, cuz the score computation contains the filter in hidden
            //通过传入的LogicsOrder中货物的信息和目标区域，初步筛选出所有的 数量-货道列表 
            Dictionary<int, List<CargoWays>> filteredByGoodsAndZones = _indexer.FilterGoodsAndZones(order, destArea);
            //平整上述 数量-货道列表 使该键值对字典组成一个列表。此处组成的列表，
            //由于经过了货物和区域的初步筛选，因此就算是双头通的货道两端拥有不同的可用数量，也仍旧符合筛选条件，只需要控制在此处不重复添加即可
            SortedList<string, CargoWays> plainFilterGoodsAndZones = Indexer.FilterByInPoint(filteredByGoodsAndZones, inPoint);
            //to do the recursion, we need to record the result set in res,
            //a list to record and trace back the cargoways and cargowaysLock
            //List<CargoWays> tmpSelectedCargoways = new List<CargoWays>();
            //List<CargoWaysLock> tmpSelectedCargowaysLock = new List<CargoWaysLock>();
            //组织封装一个计算用内部类，用于保存全局最优解
            BoxedOrderForCompute bofc = new BoxedOrderForCompute()
            {
                InPoint = inPoint,
                MinSumScore = double.MaxValue,
                Order = order,
                ResCargoways = new List<CargoWays>(),
                ResCargowaysLock = new List<CargoWaysLock>()
            };
            //_indexer.PlainToLog();
            //DoRecursionByGetLockLocation(bofc, plainFilterGoodsAndZones, tmpSelectedCargoways, tmpSelectedCargowaysLock, 0, order.Units.Count);
            //执行动态规划算法
            DynamicMethodByGetLockLocation(bofc, plainFilterGoodsAndZones);
            //ConfirmAndSetCargoWaysLocks(bofc.ResCargowaysLock, order);
            //_indexer.PlainToLog();
            return bofc.MinSumScore == double.MaxValue ? new List<CargoWaysLock>() : bofc.ResCargowaysLock;
        }
        /// <summary>
        /// 确认并保存货道锁定信息
        /// </summary>
        /// <param name="additionList"></param>
        /// <param name="order"></param>
        public void ConfirmAndSetCargoWaysLocks(List<LockLocations> additionList, LogicsOrder order)
        {
            List<Entity.CargoWaysLock> transform = new List<CargoWaysLock>();
            foreach(LockLocations ll in additionList)
            {
                Entity.CargoWaysLock cw = new CargoWaysLock(){
                    CargoWayId = ll.CargoWayId,
                    InPointMapItemId = ll.InPointId,
                    LockEnd = ll.LockEnd - 1,
                    LockStart = ll.LockStart - 1,
                    RailColumn = ll.RailColumn - 1,
                    RackAt = ll.Rack - 1,
                    LayerAt = ll.Layer - 1
                };
                transform.Add(cw);
            }
            using(TransactionScope scope = new TransactionScope())
            {
                //save Locks for id
                MapSingletonService.Instance.GetCargoWaysLockService().InsertCargoWaysLocks(transform);
                int indexUnits = 0;
                List<Entity.Goods> additionGoodsList = new List<Goods>();
                foreach (CargoWaysLock cwl in transform)
                {
                    for(int i = 0; i < Math.Abs(cwl.LockStart - cwl.LockEnd) + 1; i++)
                    {
                        Entity.Goods tmp = new Goods();
                        tmp.BarCode = order.BarCode;
                        tmp.Batch = order.GoodBatch;
                        tmp.CargoWayLockId = cwl.Id;
                        tmp.Count = order.Units[indexUnits++];
                        tmp.MapItemsId = 0;
                        tmp.Model = order.GoodModel;
                        tmp.Name = order.GoodName;
                        tmp.ProductId = order.ProductId;
                        additionGoodsList.Add(tmp);
                    }
                    //update the memory of indexer
                    _indexer.Update(cwl, _map.CargoWays.Single(cw => cw.Id == cwl.CargoWayId), order.GoodModel + "_" + order.GoodBatch);
                }
                MapSingletonService.Instance.GetGoodsService().InsertGoods(additionGoodsList);
                MapSingletonService.Instance.GetMapItemsService().UpdateAllMapItems();
                scope.Complete();
            }
        }
        /// <summary>
        /// 对货道赋值以精细计算的平均周转率。
        /// </summary>
        public void WrapCargoWaysWithAvgDetailedCost()
        {
            IMapAlgorithmService mapAlgorithmService = new MapAlgorithmService();
            mapAlgorithmService.WrapCargoWaysWithDetailAvgCost(_map);
        }
        /// <summary>
        /// 封装的方法
        /// </summary>
        /// <param name="order"></param>
        /// <param name="inPoint"></param>
        /// <param name="destArea"></param>
        /// <returns></returns>
        public List<LockLocations> GetLockLocations(LogicsOrder order,Entity.MapItems inPoint, List<int> destArea = null)
        {
            List<MapItems> ins = new List<MapItems>();
            ins.Add(inPoint);
            List<Entity.CargoWaysLock> res = GetLockLocations(order, ins, destArea);
            List<LockLocations> returnRes = new List<LockLocations>();
            foreach(CargoWaysLock cw in res)
            {
                LockLocations tmp = new LockLocations();
                tmp.CargoWayId = cw.CargoWayId;
                tmp.Layer = cw.LayerAt + 1;
                tmp.Rack = cw.RackAt + 1;
                tmp.InPointId = cw.InPointMapItemId;
                tmp.LockEnd = cw.LockEnd + 1;
                tmp.LockStart = cw.LockStart + 1;
                tmp.RailColumn = cw.RailColumn + 1;
                returnRes.Add(tmp);
            }
            return returnRes;
        }


        #region private methods

        #region InitializeIndexer
        /// <summary>
        /// 计算势能
        /// </summary>
        private void InitializePotentialEnergy()
        {
            int[,,] potentialEnergy = new int[_map.LayerCount, _map.RackCount, _map.ColumnCount];
            int Input = MapSingletonService.Instance.Type_GetInputId();
            int Output = MapSingletonService.Instance.Type_GetOutputId();
            int Lifter = MapSingletonService.Instance.Type_GetLifterId();
            int Rail = MapSingletonService.Instance.Type_GetRailId();
            List<SpecialConnection> inputconns = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Rail).ToList();
            List<SpecialConnection> outputconns = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Rail).ToList();
            List<SpecialConnection> il = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Lifter).ToList();
            foreach(SpecialConnection sc in il)
            {
                List<SpecialConnection> fromil = _map.SpecialConnections.Where(s => s.MapItemFromEntity.Rack == sc.MapItemToEntity.Rack
                                                                                && s.MapItemFromEntity.Column == sc.MapItemToEntity.Column
                                                                                && s.MapItemFromEntity.Layer != sc.MapItemToEntity.Layer
                                                                                && s.MapItemToEntity.TypeId == Rail
                                                                                && s.MapItemFromEntity.TypeId == Lifter).ToList();
                inputconns.AddRange(fromil);//may be the same reference and be added more than onece, but that is the true result for calculating potential energy
            }
            List<SpecialConnection> ol = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Lifter).ToList();
            foreach(SpecialConnection sc in ol)
            {
                List<SpecialConnection> fromol = _map.SpecialConnections.Where(s => s.MapItemFromEntity.Rack == sc.MapItemToEntity.Rack
                                                                                && s.MapItemFromEntity.Column == sc.MapItemToEntity.Column
                                                                                && s.MapItemFromEntity.Layer != sc.MapItemToEntity.Layer
                                                                                && s.MapItemToEntity.TypeId == Rail
                                                                                && s.MapItemFromEntity.TypeId == Lifter).ToList();
                outputconns.AddRange(fromol);
            }
            //initial the potential energy map
            for(int i = 0; i < _map.LayerCount; i++)
            {
                for(int j = 0; j < _map.RackCount; j++)
                {
                    for(int k = 0; k < _map.ColumnCount; k++)
                    {
                        potentialEnergy[i, j, k] = int.MinValue;
                    }
                }
            }
            //for io
            ColorFromTargetByInitializePotentialEnergy(inputconns, 1, potentialEnergy, Input, Output, Lifter, Rail);
            ColorFromTargetByInitializePotentialEnergy(outputconns, -1, potentialEnergy, Input, Output, Lifter, Rail);
            //load potential energy to cargoways
            foreach (CargoWays cw in _map.CargoWays)
            {
                cw.LeftPotentialEnergy = cw.LeftIsRail ? potentialEnergy[cw.LayerAt, cw.RackAt, cw.LeftRailColumn] : int.MinValue;
                cw.RightPotentialEnergy = cw.RightIsRail ? potentialEnergy[cw.LayerAt, cw.RackAt, cw.RightRailColumn] : int.MinValue;
                //Utils.Logger.WriteMsgAndLog(cw.LayerAt + "--" + cw.RackAt + "--" + cw.LeftRailColumn + "--" + cw.RightRailColumn + "--" + cw.LeftPotentialEnergy + "--" + cw.RightPotentialEnergy);
            }
        }
        /// <summary>
        /// 计算势能-扫描单层
        /// </summary>
        /// <param name="target">list of SpecialConnection, we use "To"</param>
        /// <param name="score">potentialEnergy score, 1->input, -1->output</param>
        /// <param name="potentialEnergy">result matrix</param>
        /// <param name="Input">Type</param>
        /// <param name="Output">Type</param>
        /// <param name="Lifter">Type</param>
        /// <param name="Rail">Type</param>
        private void ColorFromTargetByInitializePotentialEnergy(List<SpecialConnection> target, int score, int[,,] potentialEnergy, int Input, int Output, int Lifter, int Rail)
        {
            foreach (SpecialConnection sc in target)
            {
                MapItems tmi = sc.MapItemToEntity;
                HashSet<MapItems> hasCal = new HashSet<MapItems>();
                Queue<MapItems> toCal = new Queue<MapItems>();
                potentialEnergy[tmi.Layer, tmi.Rack, tmi.Column] = potentialEnergy[tmi.Layer, tmi.Rack, tmi.Column] == int.MinValue ? score : (potentialEnergy[tmi.Layer, tmi.Rack, tmi.Column] + score);
                hasCal.Add(tmi);
                toCal.Enqueue(tmi);
                while (toCal.Count > 0)
                {
                    MapItems tmp = toCal.Dequeue();
                    //left, right, up and down to scan one direction line
                    ScanByDirectionByInitializePotentialEnergy(potentialEnergy, tmp, -1, 0, hasCal, toCal, Rail, score);
                    ScanByDirectionByInitializePotentialEnergy(potentialEnergy, tmp, 0, 1, hasCal, toCal, Rail, score);
                    ScanByDirectionByInitializePotentialEnergy(potentialEnergy, tmp, 1, 0, hasCal, toCal, Rail, score);
                    ScanByDirectionByInitializePotentialEnergy(potentialEnergy, tmp, 0, -1, hasCal, toCal, Rail, score);
                }
            }
        }
        /// <summary>
        /// 计算势能-扫描单向
        /// </summary>
        /// <param name="potentialEnergy">result</param>
        /// <param name="tmp">starting mapitem</param>
        /// <param name="x">direction row movement</param>
        /// <param name="y">direction column movement</param>
        /// <param name="hasCal">hashset to restore the mapitems which have been done</param>
        /// <param name="toCal">queue to resotre next mapitems</param>
        /// <param name="Rail">Type</param>
        /// <param name="score">potentialEnergy score</param>
        private void ScanByDirectionByInitializePotentialEnergy(int[,,] potentialEnergy, MapItems tmp, int x, int y, HashSet<MapItems> hasCal, Queue<MapItems> toCal, int Rail, int score)
        {
            int nowZ = tmp.Layer;
            int nowX = tmp.Rack + x ;
            int nowY = tmp.Column + y;
            while (nowX >= 0 && nowX < _map.RackCount && nowY >= 0 && nowY < _map.ColumnCount)
            {
                tmp = _map[nowZ, nowX, nowY];
                if (tmp.TypeId == Rail && !hasCal.Contains(tmp))
                {
                    potentialEnergy[nowZ, nowX, nowY] = potentialEnergy[nowZ, nowX, nowY] == int.MinValue ? score : (potentialEnergy[nowZ, nowX, nowY] + score);
                    hasCal.Add(tmp);
                    toCal.Enqueue(tmp);
                    nowX += x;
                    nowY += y;
                }
                else
                    break;
            }

        }

        /// <summary>
        /// 空货道赋值，主要赋值top和bottom
        /// </summary>
        private void InitializeBlankCargoWays()
        {
            //notice that oupupoint1 ~ 2 is from lower row to greater row
            for (int i = _map.CargoWays.Count - 1; i >= 0; i--)
            {
                //initial four dicts
                _map.CargoWays[i].LeftRailInPoints = new Dictionary<MapItems, double>();
                _map.CargoWays[i].RightRailInPoints = new Dictionary<MapItems, double>();
                _map.CargoWays[i].LeftRailOutPoints = new Dictionary<MapItems, double>();
                _map.CargoWays[i].RightRailOutPoints = new Dictionary<MapItems, double>();
                _map.CargoWays[i].GoodHas = new HashSet<string>();
                _map.CargoWays[i].LeftPotentialEnergy = int.MinValue;
                _map.CargoWays[i].RightPotentialEnergy = int.MinValue;
                //use two bottom top 1~2 as the splited ones
                if (_map.CargoWays[i].LeftIsRail)
                {
                    _map.CargoWays[i].TopLeft = _map.CargoWays[i].LeftRailColumn + 1;
                    _map.CargoWays[i].BottomLeft = _map.CargoWays[i].RightRailColumn - 1;
                }
                if (_map.CargoWays[i].RightIsRail)
                {
                    _map.CargoWays[i].TopRight = _map.CargoWays[i].RightRailColumn - 1;
                    _map.CargoWays[i].BottomRight = _map.CargoWays[i].LeftRailColumn + 1;
                }
                //make sure the unavailable side's count is 0
                if (!_map.CargoWays[i].LeftIsRail)
                {
                    _map.CargoWays[i].TopLeft = -1;
                    _map.CargoWays[i].BottomLeft= -2;
                }
                if (!_map.CargoWays[i].RightIsRail)
                {
                    _map.CargoWays[i].TopRight = -1;
                    _map.CargoWays[i].BottomRight = 0;
                }
            }
        }
        /// <summary>
        /// 根据当前地图的goods设置货道的bottom
        /// </summary>
        private void InitializeWithGoods()
        {
            foreach (Entity.Goods g in _map.Goods)
            {
                //MapitemId == 0 means the goods have locked cargoway but not be assigned with a mapitem yet
                if (g.MapItemsId == 0)
                    continue;
                Entity.MapItems certainMapItem = _map.FastFinder[g.MapItemsId];
                Entity.CargoWays targetCargoway = _map.CargoWays
                    .SingleOrDefault(c => c.LayerAt == certainMapItem.Layer && c.RackAt == certainMapItem.Rack && certainMapItem.Column > c.LeftRailColumn && certainMapItem.Column < c.RightRailColumn);
                //set all 'can' directions of bottoms to initialize as available goods stack
                if (targetCargoway.LeftIsRail && certainMapItem.Column <= targetCargoway.BottomLeft)
                {
                    targetCargoway.BottomLeft = certainMapItem.Column - 1;
                }
                if (targetCargoway.RightIsRail && certainMapItem.Column >= targetCargoway.BottomRight)
                {
                    targetCargoway.BottomRight = certainMapItem.Column + 1;
                }
                //set goods to cargoway's HasGoods
                targetCargoway.GoodHas.Add(g.Model_Batch);
            }
        }
        /// <summary>
        /// 根据货道锁定信息设置货道的bottom
        /// </summary>
        private void InitializeWithLocks()
        {
            foreach (Entity.CargoWaysLock l in _map.CargoWaysLocks)
            {
                //need to add good infos to cargoways
                List<Goods> goods = _map.Goods.Where(g => g.CargoWayLockId == l.Id).ToList();
                //initialize by locks
                Entity.CargoWays targetCargoway = _map.CargoWays.SingleOrDefault(cw => cw.Id == l.CargoWayId);
                foreach (Goods g in goods)
                    targetCargoway.GoodHas.Add(g.Model_Batch);
                if (targetCargoway == null)
                {
                    System.Diagnostics.Debug.WriteLine("Can not find CargoWayNumber: " + l.CargoWayId + ", AtMapLogicsService::InitializeGoods.");
                    //throw new NullReferenceException("Can not find CargoWayNumber: " + l.CargoWayNumber + ", AtMapLogicsService::InitializeGoods.");
                }
                if (targetCargoway.LeftIsRail && l.LockStart == targetCargoway.BottomLeft)
                {
                    targetCargoway.BottomLeft = l.LockEnd - 1;
                }
                else if (targetCargoway.RightIsRail && l.LockStart == targetCargoway.BottomRight)
                {
                    targetCargoway.BottomRight = l.LockEnd + 1;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("AtMapLogicsService::InitializeGoods doesnt match: " + l.CargoWayId);
                    //throw new Exception("AtMapLogicsService::InitializeGoods doesnt match: " + l.CargoWayNumber);
                }
            }
        }
        /// <summary>
        /// 计算并设置简单花费，存储于Left/Right-In/Out-Points
        /// </summary>
        private void InitializeWithIOReachable()
        {
            int Input = MapSingletonService.Instance.Type_GetInputId();
            int Output = MapSingletonService.Instance.Type_GetOutputId();
            int Lifter = MapSingletonService.Instance.Type_GetLifterId();
            int Rail = MapSingletonService.Instance.Type_GetRailId();
            int StorageRail = MapSingletonService.Instance.Type_GetStorageRailId();
            Dictionary<MapItems, List<MapItems>> inputStart = new Dictionary<MapItems, List<MapItems>>();
            Dictionary<MapItems, List<MapItems>> outputStart = new Dictionary<MapItems, List<MapItems>>();
            Dictionary<MapItems, List<double>> inputStartCost = new Dictionary<MapItems, List<double>>();
            Dictionary<MapItems, List<double>> outputStartCost = new Dictionary<MapItems, List<double>>();
            List<SpecialConnection> inputconns = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Rail).ToList();
            List<SpecialConnection> outputconns = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Rail).ToList();
            List<SpecialConnection> il = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Lifter).ToList();
            List<SpecialConnection> ol = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Lifter).ToList();
            //from inputpoint to rail special connection
            foreach (SpecialConnection sc in inputconns)
            {
                if (!inputStart.ContainsKey(sc.MapItemFromEntity))
                {
                    inputStart.Add(sc.MapItemFromEntity, new List<MapItems>());
                    inputStartCost.Add(sc.MapItemFromEntity, new List<double>());
                }
                inputStart[sc.MapItemFromEntity].Add(sc.MapItemToEntity);
                inputStartCost[sc.MapItemFromEntity].Add(sc.TimeCost);
            }
            //from outputpoint to rail special connection
            foreach(SpecialConnection sc in outputconns)
            {
                if (!outputStart.ContainsKey(sc.MapItemFromEntity))
                {
                    outputStart.Add(sc.MapItemFromEntity, new List<MapItems>());
                    outputStartCost.Add(sc.MapItemFromEntity, new List<double>());
                }
                outputStart[sc.MapItemFromEntity].Add(sc.MapItemToEntity);
                outputStartCost[sc.MapItemFromEntity].Add(sc.TimeCost);
            }
            //from inputpoint to lifter special connection, here we need to search for lifter-rail special connections
            foreach(SpecialConnection sc in il)
            {
                MapItems inputpoint = sc.MapItemFromEntity;
                MapItems lifter0 = sc.MapItemToEntity;
                List<SpecialConnection> fromlifter = _map.SpecialConnections.Where(flsc => flsc.MapItemFromEntity.Layer != lifter0.Layer
                                                                                    && flsc.MapItemFromEntity.Rack == lifter0.Rack
                                                                                    && flsc.MapItemFromEntity.Column == lifter0.Column
                                                                                    && flsc.MapItemFromEntity.TypeId == Lifter
                                                                                    && flsc.MapItemToEntity.TypeId == Rail).ToList();
                foreach(SpecialConnection target in fromlifter)
                {
                    if (!inputStart.ContainsKey(inputpoint))
                    {
                        inputStart.Add(inputpoint, new List<MapItems>());
                        inputStartCost.Add(inputpoint, new List<double>());
                    }
                    inputStart[inputpoint].Add(target.MapItemToEntity);
                    inputStartCost[inputpoint].Add(sc.TimeCost + Math.Abs(target.MapItemToEntity.Layer - inputpoint.Layer) * LifterCostUnit + target.TimeCost);
                }
            }
            //from outputpoint to lifter special connection, the same as before
            foreach(SpecialConnection sc in ol)
            {
                MapItems outputpoint = sc.MapItemFromEntity;
                MapItems lifter0 = sc.MapItemToEntity;
                List<SpecialConnection> fromlifter = _map.SpecialConnections.Where(flsc => flsc.MapItemFromEntity.Layer != lifter0.Layer
                                                                                    && flsc.MapItemFromEntity.Rack == lifter0.Rack
                                                                                    && flsc.MapItemFromEntity.Column == lifter0.Column
                                                                                    && flsc.MapItemFromEntity.TypeId == Lifter
                                                                                    && flsc.MapItemToEntity.TypeId == Rail).ToList();
                foreach (SpecialConnection target in fromlifter)
                {
                    if (!outputStart.ContainsKey(outputpoint))
                    {
                        outputStart.Add(outputpoint, new List<MapItems>());
                        outputStartCost.Add(outputpoint, new List<double>());
                    }
                    outputStart[outputpoint].Add(target.MapItemToEntity);
                    outputStartCost[outputpoint].Add(sc.TimeCost + Math.Abs(target.MapItemToEntity.Layer - outputpoint.Layer) * LifterCostUnit + target.TimeCost);
                }
            }
            //scan all the input to target list
            foreach(KeyValuePair<MapItems, List<MapItems>> kv in inputStart)
            {
                Dictionary<MapItems, int> hasCal = new Dictionary<MapItems, int>();
                double[,,] costTmp = new double[_map.LayerCount, _map.RackCount, _map.ColumnCount];
                List<double> startCost = inputStartCost[kv.Key];
                for(int i = 0; i < kv.Value.Count; i++)
                {
                    ColorFromTargetByInitializeWithIOReachable(kv.Key, kv.Value[i], startCost[i], SrCostUnit,  Rail, StorageRail, true, hasCal, costTmp);
                }
            }
            //scan all the output to target list
            foreach(KeyValuePair<MapItems, List<MapItems>> kv in outputStart)
            {
                Dictionary<MapItems, int> hasCal = new Dictionary<MapItems, int>();
                double[,,] costTmp = new double[_map.LayerCount, _map.RackCount, _map.ColumnCount];
                List<double> startCost = outputStartCost[kv.Key];
                for(int i = 0; i < kv.Value.Count; i++)
                {
                    ColorFromTargetByInitializeWithIOReachable(kv.Key, kv.Value[i], startCost[i], SrCostUnit, Rail, StorageRail, false, hasCal, costTmp);
                }
            }
            System.Diagnostics.Debug.WriteLine("Initial Reachable Done..");
        }
        /// <summary>
        /// 计算简单花费-扫描单层
        /// 使用Units进行计算
        /// </summary>
        /// <param name="key">IO MapItme</param>
        /// <param name="target">from io to target</param>
        /// <param name="startScore">the time cost from io to target</param>
        /// <param name="srCostUnit">Time Cost Unit of StorageRail</param>
        /// <param name="Rail">Type</param>
        /// <param name="StorageRail">Type</param>
        /// <param name="fromInput">to differ io</param>
        /// <param name="hasCal">Dictionary to resotre the mapitems that has been done and the times</param>
        /// <param name="costTmp">local time cost</param>
        private void ColorFromTargetByInitializeWithIOReachable(MapItems key, MapItems target,
            double startScore, double srCostUnit, int Rail, int StorageRail, bool fromInput, 
            Dictionary<MapItems, int> hasCal, double[,,] costTmp)
        {
            double[,] localCostTmp = new double[_map.RackCount, _map.ColumnCount];
            //the target here must be a rail grid
            double startCost = startScore;
            localCostTmp[target.Rack, target.Column] = startCost;
            if(!hasCal.ContainsKey(target))
                hasCal.Add(target, 0);
            costTmp[target.Layer, target.Rack, target.Column] = (costTmp[target.Layer, target.Rack, target.Column] * hasCal[target] + startCost) / (hasCal[target] + 1);
            hasCal[target] = hasCal[target] + 1;
            //load IODicts for first
            List<CargoWays> op1 = _map.CargoWays.Where(cw => cw.LeftIsRail && cw.LeftRailColumn == target.Column && cw.LayerAt == target.Layer && cw.RackAt == target.Rack).ToList();
            List<CargoWays> op2 = _map.CargoWays.Where(cw => cw.RightIsRail && cw.RightRailColumn == target.Column && cw.LayerAt == target.Layer && cw.RackAt == target.Rack).ToList();
            LoadIODictsByInitializeWithIOReachable(op1, key, fromInput, true, costTmp[target.Layer, target.Rack, target.Column]);
            LoadIODictsByInitializeWithIOReachable(op2, key, fromInput, false, costTmp[target.Layer, target.Rack, target.Column]);
            Queue<MapItems> queue = new Queue<MapItems>();
            queue.Enqueue(target);
            HashSet<MapItems> tmpHas = new HashSet<MapItems>();
            tmpHas.Add(target);
            while(queue.Count > 0)
            {
                MapItems nowitem = queue.Dequeue();
                ScanByDirectionByInitializeWithIOReachable(key, nowitem, tmpHas, hasCal, queue, localCostTmp, costTmp, srCostUnit, Rail, StorageRail, fromInput, -1, 0);
                ScanByDirectionByInitializeWithIOReachable(key, nowitem, tmpHas, hasCal, queue, localCostTmp, costTmp, srCostUnit, Rail, StorageRail, fromInput, 1, 0);
                ScanByDirectionByInitializeWithIOReachable(key, nowitem, tmpHas, hasCal, queue, localCostTmp, costTmp, srCostUnit, Rail, StorageRail, fromInput, 0, -1);
                ScanByDirectionByInitializeWithIOReachable(key, nowitem, tmpHas, hasCal, queue, localCostTmp, costTmp, srCostUnit, Rail, StorageRail, fromInput, 0, 1);
            }
            //System.Diagnostics.Debug.WriteLine("");
        }
        /// <summary>
        /// set simple time cost to CargoWays
        /// </summary>
        /// <param name="cwl"></param>
        /// <param name="key"></param>
        /// <param name="fromInput"></param>
        /// <param name="isOp1"></param>
        /// <param name="cost"></param>
        private void LoadIODictsByInitializeWithIOReachable(List<CargoWays> cwl, MapItems key, bool fromInput, bool isOp1, double cost)
        {
            foreach (CargoWays cw in cwl)
            {
                if (fromInput)
                {
                    //set to input dicts
                    if (isOp1)
                    {
                        if (!cw.LeftRailInPoints.ContainsKey(key))
                            cw.LeftRailInPoints.Add(key, cost);
                        else
                            cw.LeftRailInPoints[key] = cost;
                    }
                    else
                    {
                        if (!cw.RightRailInPoints.ContainsKey(key))
                            cw.RightRailInPoints.Add(key, cost);
                        else
                            cw.RightRailInPoints[key] = cost;
                    }
                }
                else
                {
                    //set to output dicts
                    if (isOp1)
                    {
                        if (!cw.LeftRailOutPoints.ContainsKey(key))
                            cw.LeftRailOutPoints.Add(key, cost);
                        else
                            cw.LeftRailOutPoints[key] = cost;
                    }
                    else
                    {
                        if (!cw.RightRailOutPoints.ContainsKey(key))
                            cw.RightRailOutPoints.Add(key, cost);
                        else
                            cw.RightRailOutPoints[key] = cost;
                    }
                }
            }
        }
        /// <summary>
        /// 计算简单花费-扫描单向
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nowitem"></param>
        /// <param name="tmpHas"></param>
        /// <param name="hasCal"></param>
        /// <param name="queue"></param>
        /// <param name="localCostTmp"></param>
        /// <param name="costTmp"></param>
        /// <param name="srCostUnit"></param>
        /// <param name="Rail"></param>
        /// <param name="StorageRail"></param>
        /// <param name="fromInput"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ScanByDirectionByInitializeWithIOReachable(MapItems key, MapItems nowitem, HashSet<MapItems> tmpHas,
            Dictionary<MapItems, int> hasCal, Queue<MapItems> queue, double[,] localCostTmp,
            double[,,] costTmp, double srCostUnit, int Rail, int StorageRail, bool fromInput, int x, int y)
        {
            double fromCost = localCostTmp[nowitem.Rack, nowitem.Column];
            int nowX = nowitem.Rack + x;
            int nowY = nowitem.Column + y;
            int nowZ = nowitem.Layer;
            //we need the first TypeId to make sure the scan could consume continuious the same type
            if (nowX < 0 || nowX >= _map.RackCount || nowY < 0 || nowY >= _map.ColumnCount)
                return;
            MapItems tmp = _map[nowZ, nowX, nowY];
            int originalType = tmp.TypeId;
            //StorageRail must be along with Column
            if (originalType == StorageRail && Math.Abs(y) == 0)
                return;
            while (nowX >= 0 && nowX < _map.RackCount && nowY >= 0 && nowY < _map.ColumnCount)
            {
                tmp = _map[nowZ, nowX, nowY];
                if (tmp.TypeId != Rail && tmp.TypeId != StorageRail)
                    break;
                if (tmp.TypeId == originalType && !tmpHas.Contains(tmp))
                {
                    if (tmp.TypeId == StorageRail)
                    {
                        fromCost += srCostUnit;
                        localCostTmp[tmp.Rack, tmp.Column] = fromCost;
                    }
                    else
                    {
                        //if (!hasCal.ContainsKey(tmp))
                        //    hasCal.Add(tmp, 0);
                        //costTmp[tmp.Layer, tmp.Rack, tmp.Column] = (costTmp[tmp.Layer, tmp.Rack, tmp.Column] * hasCal[tmp] + fromCost) / (hasCal[tmp] + 1);
                        //hasCal[tmp] = hasCal[tmp] + 1;
                        fromCost += RCostUnit;
                        costTmp[tmp.Layer, tmp.Rack, tmp.Column] = fromCost;
                        localCostTmp[tmp.Rack, tmp.Column] = fromCost;
                        List<CargoWays> op1 = _map.CargoWays.Where(cw => cw.LeftIsRail && cw.LeftRailColumn == tmp.Column && cw.LayerAt == tmp.Layer && cw.RackAt == tmp.Rack).ToList();
                        List<CargoWays> op2 = _map.CargoWays.Where(cw => cw.RightIsRail && cw.RightRailColumn == tmp.Column && cw.LayerAt == tmp.Layer && cw.RackAt == tmp.Rack).ToList();
                        LoadIODictsByInitializeWithIOReachable(op1, key, fromInput, true, costTmp[tmp.Layer, tmp.Rack, tmp.Column]);
                        LoadIODictsByInitializeWithIOReachable(op2, key, fromInput, false, costTmp[tmp.Layer, tmp.Rack, tmp.Column]);
                    }
                    tmpHas.Add(tmp);
                    queue.Enqueue(tmp);
                    nowX += x;
                    nowY += y;
                }
                else
                    break;
            }
        }

        #endregion


        #region GetLocationLock

        //private void DoRecursionByGetLockLocation(BoxedOrderForCompute bofc, List<CargoWays> filterdCargoways, 
        //    List<CargoWays> tmpSelectedCargoways, List<CargoWaysLock> tmpSelectedCargowaysLock, double tmpSumScore,
        //    int count)
        //{
        //    if(count == 0)
        //    {
        //        if (bofc.MinSumScore > tmpSumScore)
        //        {
        //            bofc.ResCargoways.Clear();
        //            bofc.ResCargowaysLock.Clear();
        //            bofc.ResCargoways.AddRange(tmpSelectedCargoways);
        //            bofc.ResCargowaysLock.AddRange(tmpSelectedCargowaysLock);
        //            bofc.MinSumScore = tmpSumScore;
        //            //Utils.Logger.WriteMsgAndLog("Setting " + bofc.Order.Units.Count + " units goods...");
        //            //for (int i = 0; i < bofc.ResCargowaysLock.Count; i++)
        //            //{
        //            //    Utils.Logger.WriteMsgAndLog("Taking " + bofc.ResCargowaysLock[i].CargoWayId + " cargoway, lock at layer: " + bofc.ResCargoways[i].LayerAt + ", RackAt: " + bofc.ResCargoways[i].RackAt
        //            //        + "\r\n--------------> from " + bofc.ResCargowaysLock[i].LockStart + " to " + bofc.ResCargowaysLock[i].LockEnd
        //            //        + " with in point " + bofc.ResCargowaysLock[i].InPointMapItemId + ". At outputpoint " + bofc.ResCargowaysLock[i].OutputPoint);
        //            //}
        //            //Utils.Logger.WriteMsgAndLog("--------------------------------------------------------");
        //        }
        //    }
        //    else
        //    {
        //        foreach(CargoWays tmpAdditionCW in filterdCargoways)
        //        {

        //            if (bofc.InPoint == null && tmpAdditionCW.AvailableCountRight == 0 && tmpAdditionCW.AvailableCountLeft == 0)
        //            {
        //                continue;
        //            }
        //            else if (bofc.InPoint != null)
        //            {
        //                int available = 2;
        //                if (!tmpAdditionCW.LeftIsRail || tmpAdditionCW.LeftRailInPoints.ContainsKey(bofc.InPoint) && tmpAdditionCW.AvailableCountLeft == 0 || !tmpAdditionCW.LeftRailInPoints.ContainsKey(bofc.InPoint))
        //                    available--;
        //                if (!tmpAdditionCW.RightIsRail || tmpAdditionCW.RightRailInPoints.ContainsKey(bofc.InPoint) && tmpAdditionCW.AvailableCountRight == 0 || !tmpAdditionCW.RightRailInPoints.ContainsKey(bofc.InPoint))
        //                    available--;
        //                if (available == 0)
        //                    continue;
        //            }
        //            //at least one of the outputpoint is availabe
        //            //outputpoint1
        //            if (tmpAdditionCW.LeftIsRail && (bofc.InPoint == null || tmpAdditionCW.LeftRailInPoints.ContainsKey(bofc.InPoint)) && tmpAdditionCW.AvailableCountLeft > 0)
        //            {
        //                CargoWaysLock tmpLock = new CargoWaysLock();
        //                //we choose the lowest cost one of inPoint if the inPoint is null
        //                tmpLock.InPointMapItemId = TakeCertainMapItemsId(bofc.InPoint, tmpAdditionCW, 1);
        //                if (tmpLock.InPointMapItemId != 0)
        //                {
        //                    tmpLock.CargoWayId = tmpAdditionCW.Id;//to set cargoway number
        //                    tmpLock.OutputPoint = tmpAdditionCW.LeftRailColumn;
        //                    tmpLock.LockStart = tmpAdditionCW.BottomLeft;
        //                    if (count > tmpAdditionCW.AvailableCountLeft)
        //                    {
        //                        tmpLock.LockEnd = tmpAdditionCW.TopLeft;
        //                    }
        //                    else
        //                    {
        //                        tmpLock.LockEnd = tmpAdditionCW.BottomLeft - count + 1;//notice the left and right
        //                    }
        //                    int consumeCount = Math.Abs(tmpLock.LockEnd - tmpLock.LockStart) + 1;
        //                    count -= consumeCount;
        //                    tmpAdditionCW.BottomLeft = tmpAdditionCW.BottomLeft - consumeCount;//set cargoways status, so that the available could be 0
        //                    //set another bottom2 if double can and blank
        //                    int bottom2Record = tmpAdditionCW.BottomRight;
        //                    if(tmpAdditionCW.LeftIsRail && tmpAdditionCW.RightIsRail && tmpAdditionCW.IsBlank)
        //                    {
        //                        tmpAdditionCW.BottomRight = tmpAdditionCW.TopRight + 1;
        //                    }
        //                    double thisScore = EstimateScoreByGetLockLocation(bofc, tmpLock.InPointMapItemId, tmpSelectedCargoways, tmpAdditionCW, tmpLock, bofc.Order.GoodModel + "_" + bofc.Order.GoodBatch, 1);
        //                    //add to recurse
        //                    tmpSumScore += thisScore;
        //                    tmpSelectedCargoways.Add(tmpAdditionCW);
        //                    tmpSelectedCargowaysLock.Add(tmpLock);
        //                    DoRecursionByGetLockLocation(bofc, filterdCargoways, tmpSelectedCargoways, tmpSelectedCargowaysLock, tmpSumScore, count);
        //                    //trace back to the before status
        //                    tmpSumScore -= thisScore;
        //                    tmpSelectedCargoways.RemoveAt(tmpSelectedCargoways.Count - 1);
        //                    tmpSelectedCargowaysLock.RemoveAt(tmpSelectedCargowaysLock.Count - 1);
        //                    tmpAdditionCW.BottomLeft = tmpAdditionCW.BottomLeft + consumeCount;
        //                    tmpAdditionCW.BottomRight = bottom2Record;
        //                    count += consumeCount;
        //                }
        //            }
        //            //outputpoint2
        //            if (tmpAdditionCW.RightIsRail && (bofc.InPoint == null || tmpAdditionCW.RightRailInPoints.ContainsKey(bofc.InPoint)) && tmpAdditionCW.AvailableCountRight > 0)
        //            {
        //                CargoWaysLock tmpLock = new CargoWaysLock();
        //                //we choose the lowest cost one of inPoint if the inPoint is null
        //                tmpLock.InPointMapItemId = TakeCertainMapItemsId(bofc.InPoint, tmpAdditionCW, 2);
        //                if (tmpLock.InPointMapItemId != 0)
        //                {
        //                    tmpLock.CargoWayId = tmpAdditionCW.Id;
        //                    tmpLock.OutputPoint = tmpAdditionCW.RightRailColumn;
        //                    tmpLock.LockStart = tmpAdditionCW.BottomRight;
        //                    if (count > tmpAdditionCW.AvailableCountRight)
        //                    {
        //                        tmpLock.LockEnd = tmpAdditionCW.TopRight;
        //                    }
        //                    else
        //                    {
        //                        tmpLock.LockEnd = tmpAdditionCW.BottomRight + count - 1;//notice the left and right
        //                    }
        //                    int consumeCount = Math.Abs(tmpLock.LockEnd - tmpLock.LockStart) + 1;
        //                    count -= consumeCount;
        //                    tmpAdditionCW.BottomRight = tmpAdditionCW.BottomRight + consumeCount;//set cargoways status, so that the available could be 0
        //                    int BottomLeftRecord = tmpAdditionCW.BottomLeft;
        //                    if (tmpAdditionCW.LeftIsRail && tmpAdditionCW.RightIsRail && tmpAdditionCW.IsBlank)
        //                    {
        //                        tmpAdditionCW.BottomLeft = tmpAdditionCW.TopLeft - 1;
        //                    }
        //                    double thisScore = EstimateScoreByGetLockLocation(bofc, tmpLock.InPointMapItemId, tmpSelectedCargoways, tmpAdditionCW, tmpLock, bofc.Order.GoodModel + "_" + bofc.Order.GoodBatch, 2);
        //                    //add to recurse
        //                    tmpSumScore += thisScore;
        //                    tmpSelectedCargoways.Add(tmpAdditionCW);
        //                    tmpSelectedCargowaysLock.Add(tmpLock);
        //                    DoRecursionByGetLockLocation(bofc, filterdCargoways, tmpSelectedCargoways, tmpSelectedCargowaysLock, tmpSumScore, count);
        //                    //trace back to the before status
        //                    tmpSumScore -= thisScore;
        //                    tmpSelectedCargoways.RemoveAt(tmpSelectedCargoways.Count - 1);
        //                    tmpSelectedCargowaysLock.RemoveAt(tmpSelectedCargowaysLock.Count - 1);
        //                    tmpAdditionCW.BottomRight = tmpAdditionCW.BottomRight - consumeCount;
        //                    tmpAdditionCW.BottomLeft = BottomLeftRecord;
        //                    count += consumeCount;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 计算罚分，根据传入的入库点、已选择的货道、已选入库点使用次数、已选货道锁定信息、货物标识以及货道的左右出口，对单货道的选择进行罚分估值。
        /// 采用多维欧氏零距作为最终罚分，由于需要系数的参与比重，因此不采用马氏距离。
        /// </summary>
        /// <param name="bofc"></param>
        /// <param name="inPoint"></param>
        /// <param name="tmpSelectedCargoways"></param>
        /// <param name="inCountDict"></param>
        /// <param name="tmpAdditionCW"></param>
        /// <param name="locker"></param>
        /// <param name="good"></param>
        /// <param name="outputpoint"></param>
        /// <returns></returns>
        private double EstimateScoreByGetLockLocation(BoxedOrderForCompute bofc, MapItems inPoint, List<CargoWays> tmpSelectedCargoways, 
            Dictionary<MapItems, int> inCountDict,
            CargoWays tmpAdditionCW, CargoWaysLock locker,
            string good, int outputpoint)
        {
            double score = 0;
            int goodsLayerCount = tmpSelectedCargoways.Where(cw => cw.LayerAt == tmpAdditionCW.LayerAt && !cw.GoodHas.Contains(good)).ToList().Count
                + _map.CargoWays.Where(cw => cw.LayerAt == tmpAdditionCW.LayerAt && cw.GoodHas.Contains(good)).ToList().Count;
            double layerScore = bofc.Order.DifferLayersFirst ? (goodsLayerCount * LayerRatio) : 0;
            double ioScore = EstimateIOScore(tmpAdditionCW, outputpoint, inPoint, inCountDict);
            if (tmpAdditionCW.GoodHas.Contains(good))
            {
                layerScore = 0;
                ioScore = 0;
            }
            double fillScore = (outputpoint == 1 ? (locker.LockEnd - tmpAdditionCW.TopLeft) : (tmpAdditionCW.TopRight - locker.LockEnd)) * FillRatio;
            fillScore = tmpAdditionCW.IsBlank ? 
                (fillScore + FillRatio * (outputpoint == 1 ? tmpAdditionCW.AvailableCountLeft : tmpAdditionCW.AvailableCountRight)) 
                : fillScore;//新货道使填充罚分加上自身货道长度的填充罚分，给已有货物货道让位；
            double chaosScore = (tmpAdditionCW.GoodHas.Contains(good) ? (tmpAdditionCW.GoodHas.Count - 1) : tmpAdditionCW.GoodHas.Count) * ChaosRatio;
            score = Math.Sqrt(Math.Pow(layerScore, 2) + Math.Pow(ioScore, 2) + Math.Pow(fillScore, 2) + Math.Pow(chaosScore, 2));
            return score;
        }
        /// <summary>
        /// 单个用于估值入库点的罚分;
        ///  注意如果启用multiUnScore则会考虑货道扫描的顺序，影响最终结果。
        ///  好在目前只需考虑单个入库点
        /// </summary>
        /// <param name="tmpAdditionCW"></param>
        /// <param name="outputpoint"></param>
        /// <param name="inPoint"></param>
        /// <param name="inCountDict"></param>
        /// <returns></returns>
        private double EstimateIOScore(CargoWays tmpAdditionCW, int outputpoint, MapItems inPoint, Dictionary<MapItems, int> inCountDict)
        {
            double inpointScore = tmpAdditionCW.AvgTimeCost;
            inpointScore = inpointScore == 0 ? (outputpoint == 1 ? tmpAdditionCW.LeftRailInPoints[inPoint] : tmpAdditionCW.RightRailInPoints[inPoint]) * InputRatio : inpointScore * InputRatio;
            double multiInScore = inCountDict.ContainsKey(inPoint) ? (MultiInRatio * inCountDict[inPoint]) : 0;
            multiInScore = 0;
            return Math.Sqrt(Math.Pow(inpointScore, 2) + Math.Pow(multiInScore, 2));
        }
        /// <summary>
        /// 根据传入的货道和可选入库点列表，选取一个入库点罚分最小的入库点
        /// </summary>
        /// <param name="tmpAdditionCW"></param>
        /// <param name="outputpoint"></param>
        /// <param name="inPoint"></param>
        /// <param name="inCountDict"></param>
        /// <returns></returns>
        private MapItems TakeCertainIOPoint(CargoWays tmpAdditionCW, int outputpoint, List<MapItems> inPoint, Dictionary<MapItems, int> inCountDict)
        {
            double ioScore = EstimateIOScore(tmpAdditionCW, outputpoint, inPoint[0], inCountDict);
            MapItems target = inPoint[0];
            for(int i = 1; i < inPoint.Count; i++)
            {
                double tmpScore = EstimateIOScore(tmpAdditionCW, outputpoint, inPoint[i], inCountDict);
                if(tmpScore < ioScore)
                {
                    ioScore = tmpScore;
                    target = inPoint[i];
                }
            }
            return target;
        }
        /// <summary>
        /// 根据传入的货道、左右出口和入库点，过滤并获取入库点ID
        /// </summary>
        /// <param name="inPoint"></param>
        /// <param name="cw"></param>
        /// <param name="outPutPoint"></param>
        /// <returns></returns>
        private int TakeCertainMapItemsId(MapItems inPoint, CargoWays cw, int outPutPoint)
        {
            int min = 0;
            //first to see if the cargoway is blank and two dict get the same simple score of the same inpoint
            if(cw.IsBlank)
            {
                if(inPoint != null)
                {
                    if (cw.LeftRailInPoints.ContainsKey(inPoint) && cw.RightRailInPoints.ContainsKey(inPoint))
                    {
                        if (cw.LeftRailInPoints[inPoint] == cw.RightRailInPoints[inPoint])
                        {
                            //cross select with the same potentialEnergy and the same simple cost
                            if (Math.Abs(cw.RackAt - outPutPoint + 1) % 2 != 1)
                                min = 0;
                            else
                            {
                                min = inPoint.MapItemID;
                            }
                        }
                        else
                        {
                            //select the lower one
                            if (cw.LeftRailInPoints[inPoint] > cw.RightRailInPoints[inPoint])
                                min = outPutPoint == 2 ? inPoint.MapItemID : 0;
                            else
                                min = outPutPoint == 1 ? inPoint.MapItemID : 0;
                        }
                    }
                    else
                    {
                        if (cw.LeftRailInPoints.ContainsKey(inPoint) && outPutPoint == 1
                            || cw.RightRailInPoints.ContainsKey(inPoint) && outPutPoint == 2)
                            min = inPoint.MapItemID;
                        else
                            min = 0;
                    }
                }
                else
                {
                    if(cw.LeftIsRail && cw.RightIsRail)
                    {
                        if(cw.LeftPotentialEnergy == cw.RightPotentialEnergy)
                        {
                            if (Math.Abs(cw.RackAt - outPutPoint + 1) % 2 != 1)
                                min = 0;
                            else
                            {
                                Dictionary<MapItems, double> dict = outPutPoint == 1 ? cw.LeftRailInPoints : cw.RightRailInPoints;
                                min = TakeLowestInDictionary(dict);
                            }
                        }
                        else
                        {
                            if (cw.LeftPotentialEnergy > cw.RightPotentialEnergy && outPutPoint == 1
                                || cw.RightPotentialEnergy > cw.LeftPotentialEnergy && outPutPoint == 2)
                            {
                                Dictionary<MapItems, double> dict = outPutPoint == 1 ? cw.LeftRailInPoints : cw.RightRailInPoints;
                                min = TakeLowestInDictionary(dict);
                            }
                            else
                                min = 0;
                        }
                    }
                    else
                    {
                        if (cw.LeftIsRail && outPutPoint == 1
                            || cw.RightIsRail && outPutPoint == 2)
                        {
                            Dictionary<MapItems, double> dict = outPutPoint == 1 ? cw.LeftRailInPoints : cw.RightRailInPoints;
                            min = TakeLowestInDictionary(dict);
                        }
                        else
                            min = 0;
                    }
                }
                //min = ((cw.RackAt - outPutPoint + 1) % 2) == 1 ? inPoint.MapItemID : 0;
            }
            else
            {
                //if inPoint is null then we choose the inputPoint of certain OutPutPoint with lowest simple cost
                if (inPoint == null)
                {
                    if (cw.LeftIsRail && outPutPoint == 1
                        || cw.RightIsRail && outPutPoint == 2)
                    {
                        Dictionary<MapItems, double> dict = outPutPoint == 1 ? cw.LeftRailInPoints : cw.RightRailInPoints;
                        min = TakeLowestInDictionary(dict);
                    }
                    else
                        min = 0;
                }
                else
                {
                    Dictionary<MapItems, double> dict = outPutPoint == 1 ? cw.LeftRailInPoints : cw.RightRailInPoints;
                    min = dict.ContainsKey(inPoint) ? inPoint.MapItemID : 0;
                }
            }
            return min;
        }
        /// <summary>
        /// 获取可选入库点字典中，简单花费最小的MapItem Id
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public int TakeLowestInDictionary(Dictionary<MapItems, double> dict)
        {
            int min = 0;
            double cost = double.MaxValue;
            foreach (KeyValuePair<MapItems, double> kv in dict)
            {
                if (cost > kv.Value)
                {
                    cost = kv.Value;
                    min = kv.Key.MapItemID;
                }
            }
            return min;
        }
        /// <summary>
        /// 使用地图的基本属性计算Units的值以及各个系数的值
        /// </summary>
        private void ComputeRatiosAndUnits()
        {
            //the two units should be related with time
            //and the four ratios should be related with the two cost units
            //so is there any relation from time units to InPointRatio?
            // --> the time units decides the simple cost, and then we multiple the simple cost by inpointRatio
            IMapAlgorithmService mapAlgorithmService = new MapAlgorithmService();
            double threshold_bt_layers = mapAlgorithmService.CalculateThreshold(_map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration);
            double threshold_bt_cargopaths = mapAlgorithmService.CalculateThreshold(_map.CSMaxSpeed, _map.CSAcceleration, _map.CSDeceleration);
            double threshold_bt_rails = mapAlgorithmService.CalculateThreshold(_map.PSMaxSpeed, _map.PSAcceleration, _map.PSDeceleration);
            List<int> cargoPath = new List<int>();
            foreach(CargoWays cw in _map.CargoWays)
            {
                cargoPath.Add(cw.TotalCount);
            }
            double avgLayersTimeCostByUnit = 0;
            double avgStorageTimeCostByUnit = 0;
            int avgStorageCount = 0;

            for(int i = 1; i < _map.LayerCount; i++)
            {
                avgLayersTimeCostByUnit += mapAlgorithmService.CalculateTime(threshold_bt_layers, i * _map.GapBetweenLayers, _map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration);
            }
            avgLayersTimeCostByUnit /= _map.LayerCount;
            foreach(int count in cargoPath)
            {
                avgStorageCount += count;
                avgStorageTimeCostByUnit += mapAlgorithmService.CalculateTime(threshold_bt_cargopaths, count * _map.GapAlongCloumn, _map.CSMaxSpeed, _map.CSAcceleration, _map.CSDeceleration);
            }
            avgStorageTimeCostByUnit /= cargoPath.Count;
            avgStorageCount /= cargoPath.Count;
            //average cost of each layer
            LifterCostUnit = avgLayersTimeCostByUnit;
            //average cost of each storage rail
            SrCostUnit = avgStorageTimeCostByUnit / avgStorageCount;
            RCostUnit = mapAlgorithmService.CalculateTime(threshold_bt_rails, _map.GapAlongRack * _map.RackCount, _map.PSMaxSpeed, _map.PSAcceleration, _map.PSDeceleration) / _map.RackCount;
            //decide and set four ratios
            // we control the  ratio of input cost, cuz here we use the two units to set ratios, and the simple cost or cost is depend on the units
            //so we choose to set inputRadio 1 here, also we can control change the weight to control the result we want
            InputRatio = 1;
            LayerRatio = (avgLayersTimeCostByUnit + avgStorageTimeCostByUnit + RCostUnit) * 5;//differ layers first
            FillRatio = (avgLayersTimeCostByUnit + avgStorageTimeCostByUnit + RCostUnit) * 5;//fill cargoway fisrt
            ChaosRatio = (avgLayersTimeCostByUnit + avgStorageTimeCostByUnit + RCostUnit) * 5;//as low as possible of the count of cargoways' HasGoods
            MultiInRatio = (avgLayersTimeCostByUnit + avgStorageTimeCostByUnit + RCostUnit) * 5;//as to split io points of using
        }
        /// <summary>
        /// 动态规划计算获得货道锁定信息
        /// </summary>
        /// <param name="bofc">计算使用信息以及结果保存的实体</param>
        /// <param name="plainFilterGoodsAndZones">计算对象列表</param>
        private void DynamicMethodByGetLockLocation(BoxedOrderForCompute bofc, SortedList<string, CargoWays> plainFilterGoodsAndZones)
        {
            if (bofc.Order.Units.Count == 0)
                return;
            //用于展开传入的货道，因为需要确定货道的使用点是左端还是右端；以及双头通的货道有可能会被扩展成两个引用
            //doublePlainCargowaysList用于存储扩展的货道引用
            //doublePlainOutputList用于存储doublePlainCargowaysList对应项的出口轨道位置，也即是使用左端还是右端
            //doublePlainInPoint用于存储doublePlainCargowaysList对应项的入库点的id
            List<CargoWays> doublePlainCargowaysList = new List<CargoWays>();
            List<int> doublePlainOutputList = new List<int>();
            List<List<MapItems>> doublePlainInPoint = new List<List<MapItems>>();
            //plain the cargoways, for double can with no blank cargoways can split as two rows of datas
            //here each row of data can be selected as one cargoway
            HashSet<CargoWays> has = new HashSet<CargoWays>();
            FilterCargowaysByDouble(doublePlainCargowaysList, doublePlainInPoint, doublePlainOutputList, has, plainFilterGoodsAndZones.ToList(), bofc);
            FilterCargowaysByDouble(doublePlainCargowaysList, doublePlainInPoint, doublePlainOutputList, has, plainFilterGoodsAndZones.Reverse().ToList(), bofc);
            //若列表扩展-筛选之后的数量为0，不需计算，直接不可用
            if (doublePlainCargowaysList.Count == 0)
                return;
            //all data saved in bofc
            //we need tri-two to save temp datas
            //所有的Tmp用于保存上一次迭代的计算结果和状态
            //所有Now用于保存当前迭代项的计算结果保存
            //score用于保存分数
            //selected用于保存已选择的货道状态
            //lock用于保存锁定信息的状态
            //takingCount用于保存已选取的品托数量
            //in用于保存已选择的入库点，以及相应的数量
            double[] scoresTmp = new double[bofc.Order.Units.Count + 1];
            double[] scoresNow = new double[bofc.Order.Units.Count + 1];
            List<CargoWays>[] selectedTmp = new List<CargoWays>[bofc.Order.Units.Count + 1];
            List<CargoWays>[] selectedNow = new List<CargoWays>[bofc.Order.Units.Count + 1];
            List<CargoWaysLock>[] lockTmp = new List<CargoWaysLock>[bofc.Order.Units.Count + 1];
            List<CargoWaysLock>[] lockNow = new List<CargoWaysLock>[bofc.Order.Units.Count + 1];
            Dictionary<MapItems, int>[] inTmp = new Dictionary<MapItems, int>[bofc.Order.Units.Count + 1];
            Dictionary<MapItems, int>[] inNow = new Dictionary<MapItems, int>[bofc.Order.Units.Count + 1];
            int[] takingCountTmp = new int[bofc.Order.Units.Count + 1];
            int[] takingCountNow = new int[bofc.Order.Units.Count + 1];
            //for debug
            //doublePlainCargowaysList.Reverse();
            //doublePlainInPoint.Reverse();
            //doublePlainOutputList.Reverse();
            //初始化第一列，直接使用裸计算和裸状态赋予
            for (int i = 0; i < bofc.Order.Units.Count + 1; i++)
            {
                //此处的i表示扫描到第0根货道时，共装入i托货物时的状态
                //此处对于所有的数组进行初始化
                selectedNow[i] = new List<CargoWays>();
                selectedTmp[i] = new List<CargoWays>();
                lockTmp[i] = new List<CargoWaysLock>();
                lockNow[i] = new List<CargoWaysLock>();
                inTmp[i] = new Dictionary<MapItems, int>();
                inNow[i] = new Dictionary<MapItems, int>();
                //calculate the first line straightly to tmp
                if (i > 0)
                {
                    //对第0根货道装入i托货物
                    int availableCount = doublePlainOutputList[0] == 1 ? doublePlainCargowaysList[0].AvailableCountLeft : doublePlainCargowaysList[0].AvailableCountRight;
                    //若第0根货道可以容纳i托货物，状态对应的初始状态
                    if (availableCount >= i)
                    {
                        //take the lowest input point
                        MapItems certainIOPoint = TakeCertainIOPoint(doublePlainCargowaysList[0], doublePlainOutputList[0], doublePlainInPoint[0], inTmp[i]);
                        //enough storage, clculate new value
                        CargoWaysLock cl = new CargoWaysLock();
                        cl.CargoWayId = doublePlainCargowaysList[0].Id;
                        cl.InPointMapItemId = certainIOPoint.MapItemID;
                        cl.LockStart = doublePlainOutputList[0] == 1 ? doublePlainCargowaysList[0].BottomLeft : doublePlainCargowaysList[0].BottomRight;
                        cl.LockEnd = doublePlainOutputList[0] == 1 ? (doublePlainCargowaysList[0].BottomLeft - i + 1) : (doublePlainCargowaysList[0].BottomRight + i - 1);
                        cl.RailColumn = doublePlainOutputList[0] == 1 ? doublePlainCargowaysList[0].LeftRailColumn : doublePlainCargowaysList[0].RightRailColumn;
                        cl.LayerAt = doublePlainCargowaysList[0].LayerAt;
                        cl.RackAt = doublePlainCargowaysList[0].RackAt;
                        lockTmp[i].Add(cl);
                        takingCountTmp[i] = i;
                        scoresTmp[i] = EstimateScoreByGetLockLocation(bofc, certainIOPoint, selectedTmp[i], inTmp[i], doublePlainCargowaysList[0], cl, bofc.Order.GoodModel + "_" + bofc.Order.GoodBatch, doublePlainOutputList[0]);
                        selectedTmp[i].Add(doublePlainCargowaysList[0]);
                        inTmp[i].Add(certainIOPoint, 1);
                    }
                    else
                    {
                        //not enough, use the previous one
                        //第0根货道无法容纳i托货物，则延续前一状态，表示当共装入i托货物时，第0根货道可装载availableCount托货物
                        scoresTmp[i] = scoresTmp[i - 1];
                        takingCountTmp[i] = takingCountTmp[i - 1];
                        selectedTmp[i].AddRange(selectedTmp[i - 1]);
                        lockTmp[i].AddRange(lockTmp[i - 1]);
                    }

                    ////debug
                    //if (bofc.Order.Units.Count == 41)
                    //{
                    //    StringBuilder sb = new StringBuilder();
                    //    sb.Append("Taking ").Append(i).Append(" units of ")
                    //        .Append(doublePlainCargowaysList[0].LayerAt).Append("-").Append(doublePlainCargowaysList[0].RackAt).Append("-")
                    //        .Append(doublePlainCargowaysList[0].LeftRailColumn).Append("\r\n");
                    //    foreach (CargoWaysLock ll in lockTmp[i])
                    //    {
                    //        sb.Append("--->").Append(ll.LayerAt).Append("-").Append(ll.RackAt).Append("-").Append(ll.RailColumn)
                    //            .Append(": ").Append(ll.LockStart).Append(" -- ").Append(ll.LockEnd).Append("\r\n");
                    //    }
                    //    sb.Append("\r\n");
                    //    sb.Append("EScore: ").Append(scoresTmp[i]).Append("\r\n");
                    //    sb.Append("-------------------------------------\r\n");
                    //    Utils.Logger.WriteMsgAndLog(sb.ToString());
                    //}
                }
            }
            //System.Diagnostics.Debug.WriteLine("first line initial complete...");
            //开始进行动态规划的计算，详细公式和流程可见“设计构想思路”
            //从第1根货道开始计算，此时第0根货道已经初始化作为前一次迭代状态
            for (int i = 1; i < doublePlainCargowaysList.Count; i++)
            {
                //同样先获取当前扫描货道的最大可容纳数量availableCount
                int availableCount = doublePlainOutputList[i] == 1 ? doublePlainCargowaysList[i].AvailableCountLeft : doublePlainCargowaysList[i].AvailableCountRight;
                for (int j = 1; j < bofc.Order.Units.Count + 1; j++)
                {
                    //must be greater than or eaquals to 0, this is to make sure we can taking enough units
                    int startK = j - takingCountTmp[j];
                    scoresNow[j] = double.MaxValue;
                    //对于第i跟货道，若全局共装入j托货物时，扫描底i根货道从startK托开始一直到j托或者直到装不下时的所有情况
                    //此处的startK根据takingCount计算，假设每根货道都可以容纳j托货物，那么takingCount[j]=j
                    //而出现货道无法容纳的情况，takingCount[j] < j，此时的startK表示：在扫描完上一根货道后的，对于全局共需装入j托货物，还需要startK才能装入全部的品托货物
                    //另外由于每根货道都是从 全局0托到全局bofc.Order.Units.Count + 1托 ，因此startK在每一次j层的循环总会拥有0起始，于是迭代到后面，假设startK的起始大于了availableCount，也拥有可延续的前一状态
                    for (int k = startK; k <= availableCount && k <= j; k++)
                    {
                        //k here means availableCount, the cargoway can take k units
                        //k表示第i跟货道在全局取j托货物时，存入k托货物
                        if (k == 0)
                        {
                            //take pre level j - k
                            //存入0托货物即为不选该货道，不纳入计算（若计算会得出一个值，而此处应当为0）
                            //0状态存入的是前一状态下全局存入j托货物时的最优解，已然保存，直接赋值
                            scoresNow[j] = scoresTmp[j - k];
                            lockNow[j].Clear();
                            lockNow[j].AddRange(lockTmp[j - k]);
                            selectedNow[j].Clear();
                            selectedNow[j].AddRange(selectedTmp[j - k]);
                            takingCountNow[j] = j;
                            inNow[j].Clear();
                            foreach (KeyValuePair<MapItems, int> kv in inTmp[j])
                                inNow[j].Add(kv.Key, kv.Value);
                        }
                        else
                        {
                            MapItems certainIOPoint = TakeCertainIOPoint(doublePlainCargowaysList[i], doublePlainOutputList[i], doublePlainInPoint[i], inTmp[j]);
                            //生成装入第i跟货道在全局入j托货物时，装入该货道k托时的锁定实体，备用
                            CargoWaysLock cl = new CargoWaysLock();
                            cl.CargoWayId = doublePlainCargowaysList[i].Id;
                            cl.InPointMapItemId = certainIOPoint.MapItemID;
                            cl.LockStart = doublePlainOutputList[i] == 1 ? doublePlainCargowaysList[i].BottomLeft : doublePlainCargowaysList[i].BottomRight;
                            cl.LockEnd = doublePlainOutputList[i] == 1 ? (doublePlainCargowaysList[i].BottomLeft - k + 1) : (doublePlainCargowaysList[i].BottomRight + k - 1);
                            cl.RailColumn = doublePlainOutputList[i] == 1 ? doublePlainCargowaysList[i].LeftRailColumn : doublePlainCargowaysList[i].RightRailColumn;
                            cl.LayerAt = doublePlainCargowaysList[i].LayerAt;
                            cl.RackAt = doublePlainCargowaysList[i].RackAt;
                            //计算分数，为前一状态入j-k托时的最优解分数加上第i跟货道入k托时的分数
                            double escore = EstimateScoreByGetLockLocation(bofc, certainIOPoint, selectedTmp[j - k], inTmp[j - k], doublePlainCargowaysList[i], cl, bofc.Order.GoodModel + "_" + bofc.Order.GoodBatch, doublePlainOutputList[i]);
                            double localScore = scoresTmp[j - k] + escore;

                            ////debug
                            //if (bofc.Order.Units.Count == 41)
                            //{
                            //    StringBuilder sb = new StringBuilder();
                            //    sb.Append("Taking ").Append(k).Append(" units of ")
                            //        .Append(doublePlainCargowaysList[i].LayerAt).Append("-").Append(doublePlainCargowaysList[i].RackAt).Append("-")
                            //        .Append(doublePlainCargowaysList[i].LeftRailColumn).Append("\r\n");
                            //    sb.Append("j=").Append(j).Append("; ").Append(" Tmp Score=").Append(scoresTmp[j]).Append("\r\n");
                            //    foreach (CargoWaysLock ll in lockTmp[j])
                            //    {
                            //        sb.Append("--->").Append(ll.LayerAt).Append("-").Append(ll.RackAt).Append("-").Append(ll.RailColumn)
                            //            .Append(": ").Append(ll.LockStart).Append(" -- ").Append(ll.LockEnd).Append("\r\n");
                            //    }
                            //    sb.Append("\r\n");
                            //    sb.Append("EScore: ").Append(escore).Append(" ,taking ").Append(k).Append("units. ").Append(j - k).Append("ScoresTmp: ").Append(scoresTmp[j - k])
                            //        .Append(". Total score= ").Append(localScore).Append("\r\n");
                            //    foreach (CargoWaysLock ll in lockTmp[j - k])
                            //    {
                            //        sb.Append("====>").Append(ll.LayerAt).Append("-").Append(ll.RackAt).Append("-").Append(ll.RailColumn)
                            //            .Append(": ").Append(ll.LockStart).Append(" -- ").Append(ll.LockEnd).Append("\r\n");
                            //    }
                            //    sb.Append("-------------------------------------\r\n");
                            //    Utils.Logger.WriteMsgAndLog(sb.ToString());
                            //}

                            //若分数更低，则表示更优，更新当前状态
                            if (localScore < scoresNow[j])
                            {
                                //replace the status
                                scoresNow[j] = localScore;
                                lockNow[j].Clear();
                                lockNow[j].AddRange(lockTmp[j - k]);
                                lockNow[j].Add(cl);
                                selectedNow[j].Clear();
                                selectedNow[j].AddRange(selectedTmp[j - k]);
                                selectedNow[j].Add(doublePlainCargowaysList[i]);
                                takingCountNow[j] = j;
                                inNow[j].Clear();
                                foreach (KeyValuePair<MapItems, int> kv in inTmp[j])
                                    inNow[j].Add(kv.Key, kv.Value);
                                if (inNow[j].ContainsKey(certainIOPoint))
                                    inNow[j][certainIOPoint] = inNow[j][certainIOPoint] + 1;
                                else
                                    inNow[j].Add(certainIOPoint, 1);

                                //if (bofc.Order.Units.Count == 41)
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    sb.Append("Overwritting...").Append("\r\n");
                                //    foreach (CargoWaysLock ll in lockNow[j])
                                //    {
                                //        sb.Append("->").Append(ll.LayerAt).Append("-").Append(ll.RackAt).Append("-").Append(ll.RailColumn)
                                //            .Append(": ").Append(ll.LockStart).Append(" -- ").Append(ll.LockEnd).Append("\r\n");
                                //    }
                                //    sb.Append("================================================\r\n");
                                //    Utils.Logger.WriteMsgAndLog(sb.ToString());
                                //}
                            }
                        }
                    }
                    //表示第i跟应当从入startK托货物开始，但是可用数量不足，使用当前状态的前一状态，也即第i跟货道最大装入的情况。此时的全局入库品托数j肯定是无法被满足的，也即takingCountNow[j] < j
                    if (startK > availableCount)
                    {
                        //no enough units, set previous status
                        scoresNow[j] = scoresNow[j - 1];
                        lockNow[j].Clear();
                        lockNow[j].AddRange(lockNow[j - 1]);
                        selectedNow[j].Clear();
                        selectedNow[j].AddRange(selectedNow[j - 1]);
                        takingCountNow[j] = takingCountNow[j - 1];
                        inNow[j].Clear();
                        foreach (KeyValuePair<MapItems, int> kv in inTmp[j])
                            inNow[j].Add(kv.Key, kv.Value);
                    }
                }
                //rewrite now to tmp
                //交换Tmp和Now，把Now作为前一状态Tmp，并对Now清零，赋予0状态
                double[] sctmp = scoresTmp;
                scoresTmp = scoresNow;
                scoresNow = sctmp;
                List<CargoWays>[] cwtp = selectedTmp;
                selectedTmp = selectedNow;
                selectedNow = cwtp;
                List<CargoWaysLock>[] cwltp = lockTmp;
                lockTmp = lockNow;
                lockNow = cwltp;
                int[] ttp = takingCountNow;
                takingCountNow = takingCountTmp;
                takingCountTmp = ttp;
                Dictionary<MapItems, int>[] intmp = inTmp;
                inTmp = inNow;
                inNow = intmp;
                //set now 0 position to 0 status
                scoresNow[0] = 0;
                selectedNow[0].Clear();
                lockNow[0].Clear();
                takingCountNow[0] = 0;
                inNow[0].Clear();
            }
            //set results
            //计算完毕，此时如果takingCount[bofc.Order.Units.Count]不满足bofc.Order.Units.Count的话，表示所有货道使用的情况下，均无法装入bofc.Order.Units.Count托货物
            //若可装入，则直接赋予扫描完毕的结果
            if (takingCountTmp[bofc.Order.Units.Count] == bofc.Order.Units.Count)
            {
                bofc.MinSumScore = scoresTmp[bofc.Order.Units.Count];
                bofc.ResCargoways = selectedTmp[bofc.Order.Units.Count];
                bofc.ResCargowaysLock = lockTmp[bofc.Order.Units.Count];
            }
        }
        /// <summary>
        /// 过滤&、扩展双通、减少备选货道数量函数，结果存储在三个double参数中。
        /// 也根据入库点的行号筛选货道。
        /// </summary>
        /// <param name="doublePlainCargowaysList"></param>
        /// <param name="doublePlainInPoint"></param>
        /// <param name="doublePlainOutputList"></param>
        /// <param name="has"></param>
        /// <param name="counter"></param>
        /// <param name="plainFilterGoodsAndZones"></param>
        private void FilterCargowaysByDouble(List<CargoWays> doublePlainCargowaysList, List<List<MapItems>> doublePlainInPoint, List<int> doublePlainOutputList,
            HashSet<CargoWays> has, List<KeyValuePair<string, CargoWays>> plainFilterGoodsAndZones,
            BoxedOrderForCompute bofc)
        {
            string locationCoder = "";
            int unitsCounter = 0;
            List<CargoWays> toDoList = new List<CargoWays>();
            List<CargoWays> tmpList = new List<CargoWays>();
            List<CargoWays> hasGoodsList = new List<CargoWays>();
            int[] direction = new int[bofc.InPoint.Count];
            bool directionDone = false;
            plainFilterGoodsAndZones.Add(
                new KeyValuePair<string, CargoWays>("BLANK",
                    new CargoWays() {
                        LayerAt = -1,
                        RightRailColumn = -1
                        }));
            foreach (KeyValuePair<string, CargoWays> kv in plainFilterGoodsAndZones)
            {
                CargoWays cw = kv.Value;
                //层-右列表示当前轨道
                string cwCoder = string.Format("{0:X2}{1:X2}", cw.LayerAt, cw.RightRailColumn);
                if (cw.GoodHas.Contains(bofc.Order.Model_Batch))
                {
                    hasGoodsList.Add(cw);
                    continue;
                }
                //同层同物理区顺序或者逆序选择一定数量的货道，缩减计算规模
                if (locationCoder != cwCoder)
                {
                    bool addition = false;
                    unitsCounter = bofc.Order.Units.Count;
                    locationCoder = cwCoder;
                    for(int i = 0; i < bofc.InPoint.Count; i++)
                    {
                        //1代表顺位可行；0代表只有1行，无法判别，但是加入计算影响程度较轻
                        if (direction[i] == 1 || direction[i] == 0){
                            addition = true;
                        }
                        direction[i] = 0;
                    }
                    if (addition){
                        toDoList.AddRange(tmpList);
                    }
                    directionDone = false;
                    tmpList.Clear();
                }
                else if(unitsCounter <= 0)
                {
                    continue;
                }
                tmpList.Add(cw);
                unitsCounter -= TakeCertainMapItemsId(null, cw, 1) != 0 ? cw.AvailableCountLeft : 0;
                unitsCounter -= TakeCertainMapItemsId(null, cw, 2) != 0 ? cw.AvailableCountRight : 0;
                if (!directionDone && tmpList.Count == 2)
                {
                    for(int i = 0; i < bofc.InPoint.Count; i++)
                    {
                        int gap1 = Math.Abs(tmpList[0].RackAt - bofc.InPoint[i].Rack);
                        int gap2 = Math.Abs(tmpList[1].RackAt - bofc.InPoint[i].Rack);
                        if(gap1 < gap2)
                        {
                            direction[i] = 1;
                        }
                        else
                        {
                            direction[i] = -1;
                        }
                    }
                    directionDone = true;
                }
            }
            hasGoodsList.AddRange(toDoList);
            toDoList = hasGoodsList;
            foreach(CargoWays cw in toDoList)
            {
                //考虑到传入的入库点可能是null值，此处使用一个入库点筛选函数，详细可见“设计构想思路”文档
                //函数返回0则表示对于当前货道选择左端或者右端的不可用，因此保存所有非零项，即为所有货道以及其对应的入库点和左端还是右端的扩展列表
                //一个货道的引用可能会出现两次在列表中，因此取名为double_**
                //think about that we got a list of inPoint here, and we loop the list for a list of mapitemId with not 0 value
                List<MapItems> itemList1 = new List<MapItems>();
                List<MapItems> itemList2 = new List<MapItems>();
                if (bofc.InPoint.Count != 0)
                {
                    //we use null to decide which side should be taken in case of double throught empty cargoway
                    //with null we use potentialEnergy to decide
                    bool decideSide1 = TakeCertainMapItemsId(null, cw, 1) != 0;
                    bool decideSide2 = TakeCertainMapItemsId(null, cw, 2) != 0;
                    foreach (MapItems mi in bofc.InPoint)
                    {
                        int mapitemId1 = decideSide1 ? TakeCertainMapItemsId(mi, cw, 1) : 0;
                        int mapitemId2 = decideSide2 ? TakeCertainMapItemsId(mi, cw, 2) : 0;
                        if (mapitemId1 != 0 && cw.AvailableCountLeft != 0)
                            itemList1.Add(mi);
                        if (mapitemId2 != 0 && cw.AvailableCountRight != 0)
                            itemList2.Add(mi);
                    }
                }
                else
                {
                    int mapitemId1 = TakeCertainMapItemsId(null, cw, 1);
                    int mapitemId2 = TakeCertainMapItemsId(null, cw, 2);
                    if (mapitemId1 != 0)
                        itemList1.AddRange(cw.LeftRailInPoints.Keys.ToList());
                    if (mapitemId2 != 0)
                        itemList2.AddRange(cw.RightRailInPoints.Keys.ToList());
                }
                if (itemList1.Count != 0 && !has.Contains(cw) && cw.AvailableCountLeft != 0)
                {
                    doublePlainCargowaysList.Add(cw);
                    doublePlainOutputList.Add(1);
                    doublePlainInPoint.Add(itemList1);
                }
                if (itemList2.Count != 0 && !has.Contains(cw) && cw.AvailableCountRight != 0)
                {
                    doublePlainCargowaysList.Add(cw);
                    doublePlainOutputList.Add(2);
                    doublePlainInPoint.Add(itemList2);
                }
                has.Add(cw);
            }
        }


        #endregion

        #endregion

        #endregion


        #region inner class
        private class Indexer
        {
            //model & batch -> zone -> available count -> cargoway
            private Dictionary<string, Dictionary<int, Dictionary<int, List<CargoWays>>>> _triIndexer = null;
            private string mb_none = "";//for no goods
            //private string mb_all = "all";//for no goods filter
            //private int zone_all = -1;//for no zone filter

            public Indexer()
            {
                this._triIndexer = new Dictionary<string, Dictionary<int, Dictionary<int, List<CargoWays>>>>();
            }

            public void Insert(Entity.CargoWays cargoway)
            {
                //first to look if the cargoway is available
                int available = 2;
                if (!cargoway.LeftIsRail || cargoway.LeftRailInPoints.Count == 0 && cargoway.LeftRailOutPoints.Count == 0)
                    available--;
                if (!cargoway.RightIsRail || cargoway.RightRailInPoints.Count == 0 && cargoway.RightRailOutPoints.Count == 0)
                    available--;
                if (available == 0)
                    return;
                //load into indexer
                //no goods
                if(cargoway.GoodHas.Count == 0)
                {
                    if (!_triIndexer.ContainsKey(mb_none))
                        _triIndexer.Add(mb_none, new Dictionary<int, Dictionary<int, List<CargoWays>>>());
                    IndexerBuildByZone(_triIndexer[mb_none], cargoway);
                }
                else//has goods
                {
                    //usually shoudl be one element when the cargoway is dismixable
                    foreach (string good in cargoway.GoodHas)
                    {
                        if (!_triIndexer.ContainsKey(good))
                            _triIndexer.Add(good, new Dictionary<int, Dictionary<int, List<CargoWays>>>());
                        IndexerBuildByZone(_triIndexer[good], cargoway);
                    }
                }
            }
            public void Delete(CargoWays cargoway)
            {
                if (cargoway.GoodHas.Count == 0 && _triIndexer.ContainsKey(mb_none))
                {
                    DeleteByZones(cargoway, _triIndexer[mb_none]);
                    if (_triIndexer[mb_none].Count == 0)
                        _triIndexer.Remove(mb_none);
                }
                else
                {
                    foreach (string good in cargoway.GoodHas)
                    {
                        if (_triIndexer.ContainsKey(good))
                        {
                            DeleteByZones(cargoway, _triIndexer[good]);
                            if (_triIndexer[good].Count == 0)
                                _triIndexer.Remove(good);
                        }
                    }
                }
            }
            private void DeleteByZones(CargoWays cargoway, Dictionary<int, Dictionary<int, List<CargoWays>>> dictionary)
            {
                if (dictionary.ContainsKey(0))
                {
                    DeleteByCount(cargoway, dictionary[0]);
                    if (dictionary[0].Count == 0)
                        dictionary.Remove(0);
                }
                if (dictionary.ContainsKey(cargoway.ZoneAt))
                {
                    DeleteByCount(cargoway, dictionary[cargoway.ZoneAt]);
                    if (dictionary[cargoway.ZoneAt].Count == 0)
                        dictionary.Remove(cargoway.ZoneAt);
                }
            }
            private void DeleteByCount(CargoWays cargoway, Dictionary<int, List<CargoWays>> dictionary)
            {
                if (dictionary.ContainsKey(cargoway.AvailableCountLeft))
                {
                    dictionary[cargoway.AvailableCountLeft].Remove(cargoway);
                    if (dictionary[cargoway.AvailableCountLeft].Count == 0)
                        dictionary.Remove(cargoway.AvailableCountLeft);
                }
                if (dictionary.ContainsKey(cargoway.AvailableCountRight))
                {
                    dictionary[cargoway.AvailableCountRight].Remove(cargoway);
                    if (dictionary[cargoway.AvailableCountRight].Count == 0)
                        dictionary.Remove(cargoway.AvailableCountRight);
                }
            }

            public void Update(Entity.CargoWaysLock locker, CargoWays target, string good)
            {
                //first to delete the certain position of the target in the indexer
                this.Delete(target);
                //set new status
                if (target.LeftIsRail && target.RightIsRail && target.IsBlank)
                {
                    if (locker.RailColumn == target.LeftRailColumn)
                    {
                        target.BottomLeft = locker.LockEnd - 1;
                        target.BottomRight = locker.LockStart + 1;
                    }
                    else if (locker.RailColumn == target.RightRailColumn)
                    {
                        target.BottomRight = locker.LockEnd + 1;
                        target.BottomLeft = locker.LockStart - 1;
                    }
                }
                else
                {
                    if (locker.RailColumn == target.LeftRailColumn)
                    {
                        target.BottomLeft = locker.LockEnd - 1;
                    }
                    else if (locker.RailColumn == target.RightRailColumn)
                    {
                        target.BottomRight = locker.LockEnd + 1;
                    }
                }
                target.GoodHas.Add(good);
                //insert into indexer
                this.Insert(target);
            }
            //build and add dictionary with cargoways' zone
            private void IndexerBuildByZone(Dictionary<int, Dictionary<int, List<CargoWays>>> dictionary, CargoWays cargoway)
            {
                //certain zone number
                int zone = cargoway.ZoneAt;
                if (!dictionary.ContainsKey(zone))
                    dictionary.Add(zone, new Dictionary<int, List<CargoWays>>());
                //double directions may be two reference in the indexer
                //need to be notified
                if (cargoway.LeftIsRail)
                {
                    if (!dictionary[zone].ContainsKey(cargoway.AvailableCountLeft))
                        dictionary[zone].Add(cargoway.AvailableCountLeft, new List<CargoWays>());
                    //here to check if the 
                    if (!dictionary[zone][cargoway.AvailableCountLeft].Contains(cargoway))
                        dictionary[zone][cargoway.AvailableCountLeft].Add(cargoway);
                }
                if (cargoway.RightIsRail)
                {
                    if (!dictionary[zone].ContainsKey(cargoway.AvailableCountRight))
                        dictionary[zone].Add(cargoway.AvailableCountRight, new List<CargoWays>());
                    //may be the same entity, so be careful
                    if(!dictionary[zone][cargoway.AvailableCountRight].Contains(cargoway))
                        dictionary[zone][cargoway.AvailableCountRight].Add(cargoway);
                }
            }

            /// <summary>
            /// return a temp built up dictionary of int -> cargoway, filtered with goods and zones;
            /// </summary>
            /// <param name="model_batch">the model_batch filtered with mixable, if the order is mixable, then select all cargoways; or just select has and blank cargoways</param>
            /// <param name="zones">the zones filter the cargoways, if the zones count is 0, then select all cargoways; or just select the cargoways with certain zones</param>
            /// <returns></returns>
            public Dictionary<int, List<CargoWays>> FilterGoodsAndZones(LogicsOrder order, List<int> zones)
            {
                Dictionary<int, List<CargoWays>> tempCargoways = new Dictionary<int, List<CargoWays>>();
                string model_batch = order.GoodModel + "_" + order.GoodBatch;
                foreach(KeyValuePair<string, Dictionary<int, Dictionary<int, List<CargoWays>>>> kvGoods in _triIndexer)
                {
                    if(order.Mixable || kvGoods.Key == mb_none || kvGoods.Key == model_batch)
                    {
                        foreach(KeyValuePair<int, Dictionary<int, List<CargoWays>>> kvZones in kvGoods.Value)
                        {
                            if(zones.Count == 0 || zones.Any(zone => zone == kvZones.Key))
                            {
                                foreach(KeyValuePair<int, List<CargoWays>> kvCount in kvZones.Value)
                                {
                                    if (!tempCargoways.ContainsKey(kvCount.Key))
                                        tempCargoways.Add(kvCount.Key, new List<CargoWays>());
                                    //by boxing, there is no possibility for existing the same entity in the same count key
                                    tempCargoways[kvCount.Key].AddRange(kvCount.Value);
                                }
                            }
                        }
                    }
                }
                return tempCargoways;
            }

            internal void PlainToLog()
            {
                foreach(KeyValuePair<string, Dictionary<int, Dictionary<int, List<CargoWays>>>> kvp in _triIndexer)
                {
                    Utils.Logger.WriteMsgAndLog("-->" + kvp.Key);
                    foreach(KeyValuePair<int, Dictionary<int, List<CargoWays>>> kvp2 in kvp.Value)
                    {
                        Utils.Logger.WriteMsgAndLog("    --> Zone: " + kvp2.Key);
                        foreach(KeyValuePair<int, List<CargoWays>> kvp3 in kvp2.Value)
                        {
                            Utils.Logger.WriteMsgAndLog("        --> Count: " + kvp3.Key);
                            foreach(CargoWays cw in kvp3.Value)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(cw.Id + "--" + cw.ZoneAt + "--(" + cw.LayerAt + ", " + cw.RackAt + ") -- (" + cw.LeftRailColumn + "--" + cw.RightRailColumn + ")\r\n");
                                sb.Append("detailed Cost: ").Append(cw.AvgTimeCost).Append(". potential1: ").Append(cw.LeftPotentialEnergy).Append(" -- potential2: ").Append(cw.RightPotentialEnergy);
                                sb.Append("\r\n");
                                foreach(KeyValuePair<MapItems, double> kv4 in cw.LeftRailInPoints)
                                {
                                    sb.Append("------i1------>").Append("(")
                                        .Append(kv4.Key.Layer).Append(", ").Append(kv4.Key.Rack).Append(", ").Append(kv4.Key.Column).Append(") --> ")
                                        .Append(kv4.Value).Append("\r\n");
                                }
                                foreach (KeyValuePair<MapItems, double> kv4 in cw.RightRailInPoints)
                                {
                                    sb.Append("-------i2----->").Append("(")
                                        .Append(kv4.Key.Layer).Append(", ").Append(kv4.Key.Rack).Append(", ").Append(kv4.Key.Column).Append(") --> ")
                                        .Append(kv4.Value).Append("\r\n");
                                }
                                foreach (KeyValuePair<MapItems, double> kv4 in cw.LeftRailOutPoints)
                                {
                                    sb.Append("-------o1----->").Append("(")
                                        .Append(kv4.Key.Layer).Append(", ").Append(kv4.Key.Rack).Append(", ").Append(kv4.Key.Column).Append(") --> ")
                                        .Append(kv4.Value).Append("\r\n");
                                }
                                foreach (KeyValuePair<MapItems, double> kv4 in cw.RightRailOutPoints)
                                {
                                    sb.Append("--------o2---->").Append("(")
                                        .Append(kv4.Key.Layer).Append(", ").Append(kv4.Key.Rack).Append(", ").Append(kv4.Key.Column).Append(") --> ")
                                        .Append(kv4.Value).Append("\r\n");
                                }
                                Utils.Logger.WriteMsgAndLog(sb.ToString());
                                
                            }
                        }
                    }
                }
            }

            internal static SortedList<string, CargoWays> FilterByInPoint(Dictionary<int, List<CargoWays>> filteredByGoods, List<MapItems> inPoint)
            {
                SortedList<string, CargoWays> res = new SortedList<string, CargoWays>();
                HashSet<CargoWays> has = new HashSet<CargoWays>();
                foreach(KeyValuePair<int, List<CargoWays>> kv in filteredByGoods)
                {
                    //to filter inPoints
                    List<CargoWays> filteredInPoints = kv.Value;
                    if (inPoint.Count != 0)
                    {
                        //get if the cargoway got intersection with inPoints
                        //for (int i = filteredInPoints.Count - 1; i >= 0; i--)
                        //{
                        //    System.Diagnostics.Debug.WriteLine(filteredInPoints[i].LeftRailInPoints.Keys.ToList().Intersect(inPoint).ToList().Count
                        //        + " -- " + filteredInPoints[i].RightRailInPoints.Keys.ToList().Intersect(inPoint).ToList().Count);
                        //    //the intersection can work with the same reference in global

                        //}
                        filteredInPoints = filteredInPoints.Where(cw => cw.LeftRailInPoints.Keys.ToList().Intersect(inPoint).ToList().Count > 0
                                                                    || cw.RightRailInPoints.Keys.ToList().Intersect(inPoint).ToList().Count > 0).ToList();
                    }
                    foreach(CargoWays cw in filteredInPoints)
                    {
                        if (has.Contains(cw))
                            continue;
                        res.Add(cw.CargoWayNumber, cw);
                        has.Add(cw);
                    }
                }
                return res;
            }
        }

        private class BoxedOrderForCompute
        {
            public LogicsOrder Order;
            public double MinSumScore;
            public List<CargoWays> ResCargoways;
            public List<CargoWaysLock> ResCargowaysLock;
            public List<Entity.MapItems> InPoint;
        }

        #endregion
    }
}
