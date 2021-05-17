using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service
{
    /// <summary>
    /// this class is just used and called by LogicsService class.
    /// Or we can move all the methods to LogicsService class and delete this Algorithm interface and class.
    /// </summary>
    internal class MapAlgorithmService: IMapAlgorithmService
    {


        /// <summary>
        /// 计算封装CargoWays，对CargoWays.AvgTimeCost赋值。
        /// 区别于CargoWays.Left/Right-In/Out-Points, 这个封装的AvgTimeCost计算更为细节的单货道平均时间花费，每一个栅格都会被计算。
        /// 扫描每一个合法栅格，Rail-Storage-StorageRail。
        /// </summary>
        /// <param name="_map">全局唯一的地图引用</param>
        public void WrapCargoWaysWithDetailAvgCost(Models.Entity.Map _map)
        {
            int None = MapSingletonService.Instance.Type_GetNoneId();//get Type-None Id
            int Input = MapSingletonService.Instance.Type_GetInputId();//get Type-Input Id
            int Output = MapSingletonService.Instance.Type_GetOutputId();//get Type-Output Id
            int Lifter = MapSingletonService.Instance.Type_GetLifterId();//get Type-Lifter Id
            int Rail = MapSingletonService.Instance.Type_GetRailId();//get Type-Rail Id
            int Unavailable = MapSingletonService.Instance.Type_GetUnavailableId();//get Type-Unavailable Id
            int StorageRail = MapSingletonService.Instance.Type_GetStorageRailId();//get Type-StorageRail Id
            int Storage = MapSingletonService.Instance.Type_GetStorageId();//get Type-Storage Id
            //使用上述Type-Ids为了避免多处多次调用获取函数，导致重复计算
            //下述三个threshold是通过地图设置的各个参数计算的时间花费公式的选择阈值
            double threshold_bt_layers = this.CalculateThreshold(_map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration);
            double threshold_bt_rails = this.CalculateThreshold(_map.PSMaxSpeed, _map.PSAcceleration, _map.PSDeceleration);
            double threshold_bt_cargopaths = this.CalculateThreshold(_map.CSMaxSpeed, _map.CSAcceleration, _map.CSDeceleration);
            //Utils.IOOps.DeleteFile(Utils.Logger.LogPath);
            //设置一个临时矩阵用于存放mapitems，多出边界方面扫描
            Entity.MapItems[,,] caltmp = new Entity.MapItems[_map.LayerCount + 1, _map.RackCount + 2, _map.ColumnCount + 2];
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
            //用于存储计算结果
            //-1代表入库点，0代表出库点，划分出来是因为最后需要对平均入库、出库时间作加法
            Dictionary<int, Dictionary<int, double[,]>> tempMatrix = new Dictionary<int, Dictionary<int, double[,]>>();
            tempMatrix.Add(-1, new Dictionary<int, double[,]>());
            tempMatrix.Add(0, new Dictionary<int, double[,]>());
            for(int i = 0; i < _map.LayerCount; i++)
            {
                tempMatrix[-1].Add(i, new double[_map.RackCount, _map.ColumnCount]);
                tempMatrix[0].Add(i, new double[_map.RackCount, _map.ColumnCount]);
            }
            Dictionary<Entity.MapItems, int> tempCounter = new Dictionary<Entity.MapItems, int>();
            List<Entity.MapItems> spTempRecorder = new List<Entity.MapItems>();
            Dictionary<Entity.MapItems, double> spValueRecorder = new Dictionary<Entity.MapItems, double>();
            //所有input points直接到达的Rail
            List<Entity.SpecialConnection> frominputList = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Rail).ToList();
            foreach(Entity.SpecialConnection sc in frominputList)
            {
                if (!tempCounter.ContainsKey(sc.MapItemToEntity))
                {
                    spTempRecorder.Add(sc.MapItemToEntity);
                    spValueRecorder.Add(sc.MapItemToEntity, sc.TimeCost);
                    tempCounter.Add(sc.MapItemToEntity, 1);
                }
                else
                {
                    spValueRecorder[sc.MapItemToEntity] = (spValueRecorder[sc.MapItemToEntity] * tempCounter[sc.MapItemToEntity] + sc.TimeCost) / (tempCounter[sc.MapItemToEntity] + 1);
                    tempCounter[sc.MapItemToEntity] = tempCounter[sc.MapItemToEntity] + 1;
                }
            }
            //input -- lifter -- rail
            List<Entity.SpecialConnection> fromInputLifter = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Input && sc.MapItemToEntity.TypeId == Lifter).ToList();
            foreach(Entity.SpecialConnection sc in fromInputLifter)
            {
                List<Entity.SpecialConnection> fromInputLifterToRail = _map.SpecialConnections.Where(sct => sct.MapItemFromEntity.TypeId == Lifter 
                                                                                                        && sct.MapItemToEntity.TypeId == Rail
                                                                                                        && sct.MapItemFromEntity.Layer != sc.MapItemToEntity.Layer
                                                                                                        && sct.MapItemFromEntity.Rack == sc.MapItemToEntity.Rack
                                                                                                        && sct.MapItemFromEntity.Column == sc.MapItemToEntity.Column).ToList();
                foreach(Entity.SpecialConnection ltr in fromInputLifterToRail)
                {
                    if (!tempCounter.ContainsKey(ltr.MapItemToEntity))
                    {
                        spTempRecorder.Add(ltr.MapItemToEntity);
                        spValueRecorder.Add(ltr.MapItemToEntity, sc.TimeCost + ltr.TimeCost + CalculateTime(threshold_bt_layers, 
                            ltr.MapItemFromEntity.Layer * _map.GapBetweenLayers, _map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration));
                        tempCounter.Add(ltr.MapItemToEntity, 1);
                    }
                    else
                    {
                        spValueRecorder[ltr.MapItemToEntity] = (spValueRecorder[ltr.MapItemToEntity] * tempCounter[ltr.MapItemToEntity] +
                            sc.TimeCost + ltr.TimeCost + CalculateTime(threshold_bt_layers, ltr.MapItemFromEntity.Layer * _map.GapBetweenLayers, _map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration)
                            ) / (tempCounter[ltr.MapItemToEntity] + 1);
                        tempCounter[ltr.MapItemToEntity] = tempCounter[ltr.MapItemToEntity] + 1;
                    }
                }
            }
            ScanTheLayerWithStartGids(caltmp, tempMatrix[-1], tempCounter, spTempRecorder, spValueRecorder, _map,
                threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
            List<Entity.SpecialConnection> fromOutputList = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Rail).ToList();
            //for outputpoint
            spTempRecorder.Clear();
            spValueRecorder.Clear();
            tempCounter.Clear();
            foreach (Entity.SpecialConnection sc in fromOutputList)
            {
                if (!tempCounter.ContainsKey(sc.MapItemToEntity))
                {
                    spTempRecorder.Add(sc.MapItemToEntity);
                    spValueRecorder.Add(sc.MapItemToEntity, sc.TimeCost);
                    tempCounter.Add(sc.MapItemToEntity, 1);
                }
                else
                {
                    spValueRecorder[sc.MapItemToEntity] = (spValueRecorder[sc.MapItemToEntity] * tempCounter[sc.MapItemToEntity] + sc.TimeCost) / (tempCounter[sc.MapItemToEntity] + 1);
                    tempCounter[sc.MapItemToEntity] = tempCounter[sc.MapItemToEntity] + 1;
                }
            }
            //output -- lifter -- rail
            List<Entity.SpecialConnection> fromOutputLifter = _map.SpecialConnections.Where(sc => sc.MapItemFromEntity.TypeId == Output && sc.MapItemToEntity.TypeId == Lifter).ToList();
            foreach (Entity.SpecialConnection sc in fromOutputLifter)
            {
                List<Entity.SpecialConnection> fromOutputLifterToRail = _map.SpecialConnections.Where(sct => sct.MapItemFromEntity.TypeId == Lifter
                                                                                                        && sct.MapItemToEntity.TypeId == Rail
                                                                                                        && sct.MapItemFromEntity.Layer != sc.MapItemToEntity.Layer
                                                                                                        && sct.MapItemFromEntity.Rack == sc.MapItemToEntity.Rack
                                                                                                        && sct.MapItemFromEntity.Column == sc.MapItemToEntity.Column).ToList();
                foreach (Entity.SpecialConnection ltr in fromOutputLifterToRail)
                {
                    if (!tempCounter.ContainsKey(ltr.MapItemToEntity))
                    {
                        spTempRecorder.Add(ltr.MapItemToEntity);
                        spValueRecorder.Add(ltr.MapItemToEntity, sc.TimeCost + ltr.TimeCost + CalculateTime(threshold_bt_layers,
                            ltr.MapItemFromEntity.Layer * _map.GapBetweenLayers, _map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration));
                        tempCounter.Add(ltr.MapItemToEntity, 1);
                    }
                    else
                    {
                        spValueRecorder[ltr.MapItemToEntity] = (spValueRecorder[ltr.MapItemToEntity] * tempCounter[ltr.MapItemToEntity] +
                            sc.TimeCost + ltr.TimeCost + CalculateTime(threshold_bt_layers, ltr.MapItemFromEntity.Layer * _map.GapBetweenLayers, _map.LMaxSpeed, _map.LAcceleration, _map.LDeceleration)
                            ) / (tempCounter[ltr.MapItemToEntity] + 1);
                        tempCounter[ltr.MapItemToEntity] = tempCounter[ltr.MapItemToEntity] + 1;
                    }
                }
            }
            ScanTheLayerWithStartGids(caltmp, tempMatrix[0], tempCounter, spTempRecorder, spValueRecorder, _map,
                threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
            //赋值
            foreach(CargoWays newone in _map.CargoWays)
            {
                double point1Cost = 0;
                if (newone.LeftIsRail)
                {
                    double inputcost = tempMatrix[-1][newone.LayerAt][newone.RackAt, newone.LeftRailColumn];
                    double outputcost = tempMatrix[0][newone.LayerAt][newone.RackAt, newone.LeftRailColumn];
                    if (inputcost != 0 && outputcost != 0)
                        point1Cost = inputcost + outputcost;
                }
                double point2Cost = 0;
                if (newone.RightIsRail)
                {
                    double inputcost = tempMatrix[-1][newone.LayerAt][newone.RackAt, newone.RightRailColumn];
                    double outputcost = tempMatrix[0][newone.LayerAt][newone.RackAt, newone.RightRailColumn];
                    if (inputcost != 0 && outputcost != 0)
                        point2Cost = inputcost + outputcost;
                }
                int counter = 0;
                counter = point1Cost == 0 ? counter : counter + 1;
                counter = point2Cost == 0 ? counter : counter + 1;
                if (counter != 0)
                    newone.AvgTimeCost = (point1Cost + point2Cost) / counter;
                //Utils.Logger.WriteMsgAndLog(newone.CargoWayNumber + " -->" + newone.LeftRailColumn + ":" + newone.LeftIsRail
                //    + " -->" + newone.RightRailColumn + ":" + newone.RightIsRail + " -->Zone:" + newone.ZoneAt
                //    + " -->Col:" + newone.RackAt + " -->Layer:" + newone.LayerAt + " --->C:  " + newone.AvgTimeCost);
            }

        }

        /// <summary>
        /// 扫描所有合法起始点
        /// </summary>
        /// <param name="caltmp">扩展的栅格集</param>
        /// <param name="tempMatrix">总结果集，result</param>
        /// <param name="tempCounter">保存以计算过的mapitem以及其次数</param>
        /// <param name="spTempRecorder">合法起始点列表</param>
        /// <param name="spValueRecorder">到达合法起始点的时间花费列表</param>
        /// <param name="_map">地图</param>
        /// <param name="threshold_bt_cargopaths">货到轨道距离阈值</param>
        /// <param name="threshold_bt_rails">轨道距离阈值</param>
        /// <param name="StorageRail">货道轨道ID</param>
        /// <param name="Storage">货位ID</param>
        /// <param name="Rail">轨道ID</param>
        private void ScanTheLayerWithStartGids(MapItems[,,] caltmp, Dictionary<int, double[,]> tempMatrix,
            Dictionary<MapItems, int> tempCounter, List<MapItems> spTempRecorder, Dictionary<MapItems, double> spValueRecorder, Map _map,
            double threshold_bt_rails, double threshold_bt_cargopaths, int StorageRail, int Storage, int Rail)
        {
            tempCounter.Clear();
            Queue<Entity.MapItems> queue = new Queue<MapItems>();
            foreach(Entity.MapItems item in spTempRecorder)
            {
                queue.Enqueue(item);
                double[,] oneceTemp = new double[_map.RackCount, _map.ColumnCount];
                oneceTemp[item.Rack, item.Column] = spValueRecorder[item];
                while(queue.Count != 0)
                {
                    Entity.MapItems fromItem = queue.Dequeue();
                    int caltmpI = fromItem.Rack + 1;
                    int caltmpJ = fromItem.Column + 1;
                    //left, right, front, beneath
                    ScanOneDirection(caltmp, fromItem, caltmp[fromItem.Layer, caltmpI - 1, caltmpJ], -1, 0, oneceTemp, queue, _map,
                        threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
                    ScanOneDirection(caltmp, fromItem, caltmp[fromItem.Layer, caltmpI + 1, caltmpJ], 1, 0, oneceTemp, queue, _map,
                        threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
                    ScanOneDirection(caltmp, fromItem, caltmp[fromItem.Layer, caltmpI, caltmpJ - 1], 0, -1, oneceTemp, queue, _map,
                        threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
                    ScanOneDirection(caltmp, fromItem, caltmp[fromItem.Layer, caltmpI, caltmpJ + 1], 0, 1, oneceTemp, queue, _map,
                        threshold_bt_rails, threshold_bt_cargopaths, StorageRail, Storage, Rail);
                }
                //add oneceTemp to TEmpMatrix[tempMatrixLayer]
                for(int i = 0; i < _map.RackCount; i++)
                {
                    for(int j = 0; j < _map.ColumnCount; j++)
                    {
                        Entity.MapItems additionItem = caltmp[item.Layer, i + 1, j + 1];
                        if (tempCounter.ContainsKey(additionItem))
                        {
                            tempMatrix[item.Layer][i, j] = (tempMatrix[item.Layer][i, j] * tempCounter[additionItem] + oneceTemp[i, j]) / (tempCounter[additionItem] + 1);
                            tempCounter[additionItem] = tempCounter[additionItem] + 1;
                        }
                        else
                        {
                            tempMatrix[item.Layer][i, j] = oneceTemp[i, j];
                            tempCounter.Add(additionItem, 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 扫描一个方向
        /// </summary>
        /// <param name="caltmp">栅格集</param>
        /// <param name="fromItem">单次扫描的起始点</param>
        /// <param name="toItem">单次扫描的重点</param>
        /// <param name="rack">rack移动基数</param>
        /// <param name="column">column移动基数</param>
        /// <param name="oneceTemp">单次入口扫描的本地结果集</param>
        /// <param name="queue">缓存队列，存储下一次的单次扫描起始点</param>
        /// <param name="_map"></param>
        /// <param name="threshold_bt_cargopaths">货到轨道距离阈值</param>
        /// <param name="threshold_bt_rails">轨道距离阈值</param>
        /// <param name="StorageRail">货道轨道ID</param>
        /// <param name="Storage">货位ID</param>
        /// <param name="Rail">轨道ID</param>
        private void ScanOneDirection(MapItems[,,] caltmp, MapItems fromItem, MapItems toItem, int rack, int column, double[,] oneceTemp, Queue<Entity.MapItems> queue,
            Models.Entity.Map _map,
            double threshold_bt_rails, double threshold_bt_cargopaths, int StorageRail, int Storage, int Rail)
        {
            if (oneceTemp[toItem.Rack, toItem.Column] != 0)
            {
                return;
            }
            int orig = toItem.TypeId;
            while(toItem.TypeId == orig)
            {
                double maxSpeed = 0;
                double acc = 0;
                double dec = 0;
                double distance = 0;
                double threshold = 0;
                if (fromItem.TypeId == Rail && toItem.TypeId == Rail)
                {
                    maxSpeed = _map.PSMaxSpeed;
                    acc = _map.PSAcceleration;
                    dec = _map.PSDeceleration;
                    threshold = threshold_bt_rails;
                    //as we dont know which direction, but we can know that the scan is on one direction and the grids are straightly on the same line
                    distance = Math.Abs(fromItem.Rack - toItem.Rack) * _map.GapAlongCloumn + Math.Abs(fromItem.Column - toItem.Column) * _map.GapAlongRack;
                }
                else if ((fromItem.TypeId == StorageRail || fromItem.TypeId == Storage) && toItem.TypeId == Rail
                    || fromItem.TypeId == Rail && (toItem.TypeId == StorageRail || toItem.TypeId == Storage))//we assume storage and storageRail as the same for now
                {
                    maxSpeed = _map.CSMaxSpeed;
                    acc = _map.CSAcceleration;
                    dec = _map.CSDeceleration;
                    threshold = threshold_bt_cargopaths;
                    distance = Math.Abs(fromItem.Rack - toItem.Rack) * _map.GapAlongCloumn + Math.Abs(fromItem.Column - toItem.Column) * _map.GapAlongRack;
                }
                else
                {
                    break;
                }
                oneceTemp[toItem.Rack, toItem.Column] = CalculateTime(threshold, distance, maxSpeed, acc, dec) + oneceTemp[fromItem.Rack, fromItem.Column];
                //Utils.Logger.WriteMsgAndLog(fromItem.Layer + ", " + fromItem.Rack + ", " + fromItem.Column + " --> "
                //    + toItem.Layer + ", " + toItem.Rack + ", " + toItem.Column + " -- > " + oneceTemp[toItem.Rack, toItem.Column]);
                queue.Enqueue(toItem);
                toItem = caltmp[fromItem.Layer, toItem.Rack + rack + 1, toItem.Column + column + 1];
            }
            
        }

        /// <summary>
        /// 检查一个lifter是否垂直贯通到底部，实际没用上
        /// </summary>
        /// <param name="special"></param>
        /// <param name="_map"></param>
        /// <returns></returns>
        private bool CheckStraightLifter(Entity.MapItems special, Models.Entity.Map _map)
        {
            for (int i = 0; i < special.Layer; i++)
            {
                if (_map.SpecialMapItems.SingleOrDefault(sm => sm.Layer == i && sm.Rack == special.Rack && sm.Column == sm.Column) == null)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 对外提供时间花费的计算，主要提供LogicsService使用。
        /// 通过已知的最高速度、恒定加速度、恒定减速度和距离，算出时间花费。
        /// </summary>
        /// <param name="threshold">公式变换使用阈值，该阈值可以直接通过最高速度、恒定加速度和恒定减速度来计算，此处传入是为了在外围控制重复计算。</param>
        /// <param name="distance">行驶的距离</param>
        /// <param name="max_speed">最大速度</param>
        /// <param name="acc">恒定加速度</param>
        /// <param name="dec">恒定减速度</param>
        /// <returns>时间花费</returns>
        public double CalculateTime(double threshold, double distance, double max_speed, double acc, double dec)
        {
            if (distance <= threshold)
                return Math.Sqrt(2 * distance * acc / ((acc + dec) * dec))
                + Math.Sqrt(2 * distance * dec / ((acc + dec) * acc));
            else
                return distance / max_speed + max_speed / (2 * acc) + max_speed / (2 * dec);
        }
        /// <summary>
        /// 通过传入的最大速度、恒定加速度和恒定减速度计算一个距离阈值，该阈值用于控制时间花费计算的公式选择。
        /// </summary>
        /// <param name="max_speed">最大速度</param>
        /// <param name="acc">恒定加速度</param>
        /// <param name="dec">恒定减速度</param>
        /// <returns></returns>
        public double CalculateThreshold(double max_speed, double acc, double dec)
        {
            return 0.5 * max_speed * (max_speed / acc + max_speed / dec);
        }

    }
}
