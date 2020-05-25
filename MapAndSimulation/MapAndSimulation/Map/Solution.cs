using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MapAndSimulation.Mission;

namespace MapAndSimulation.Map
{
    /// <summary>
    /// used to solve the problem of AGV with a Map
    /// </summary>
    public class Solution
    {
        public static int MAX_RANDOM_MISSION = 10;
        public static int NUM_OF_ITER = 50;
        public static int PARTICLE_NUM = 20;
        public static bool DEBUG_MODE = false;

        private List<Mission.Mission> missionToDo = new List<Mission.Mission>();
        private AGV[] agvs = null;
        private double costTime = double.MaxValue;
        private int AGVNum = 0;
        private List<List<Mission.Mission>> TakeOutMissionsMatrix = new List<List<Mission.Mission>>();
        private List<Mission.Mission> SetInMissions = new List<Mission.Mission>();

        public List<Mission.Mission> MissionToDo { get => missionToDo; set => missionToDo = value; }
        public AGV[] AGVs { get => agvs; }
        public double CostTime { get => costTime; set => costTime = value; }

        /// <summary>
        /// used to initialize the AGVs
        /// </summary>
        /// <param name="NumOfAGVs"></param>
        public Solution(int NumOfAGVs)
        {
            AGVNum = NumOfAGVs;
        }

        /// <summary>
        /// evaluate a solution with new and existed missions
        /// with a map and now agvs
        /// </summary>
        /// <param name="map"></param>
        /// <param name="missionList"></param>
        /// <param name="agvs"></param>
        /// <returns>detailed list of AGVS</returns>
        public async Task<AGV[]> Solve(Map map,
            List<Mission.Mission> missionList)
        {
            //1.pick up all the goods' position of the missions
            foreach(Mission.Mission m in missionList)
            {
                foreach (Layer layer in map.Layers)
                {
                    foreach (Rack r in layer.Values)
                    {
                        for (int i = 0; i < r.Values.Count; i++)
                        {
                            if (r.Goods[i].OrderNumber == m.TargetGood.OrderNumber)
                            {
                                Mission.Mission tmp = new Mission.Mission();
                                tmp.Name = "mission_" + (missionToDo.Count + 1);
                                tmp.TargetGood.GoodName = r.Goods[i].GoodName;
                                tmp.TargetGood.OrderNumber = r.Goods[i].OrderNumber;
                                tmp.TargetGood.Specification = r.Goods[i].Specification;
                                tmp.TargetPosition = new Position(r.LayerNum, r.RowNum, i + 1);
                                tmp.Type = m.Type;
                                missionToDo.Add(tmp);
                                //Utils.Logger.WriteMsgAndLog("Picking..." + r.LayerNum + "-" +
                                //    "-" + r.RowNum + "-" + (i + 1) + "--" + m.TargetGood.OrderNumber);
                            }
                        }
                    }
                }
            }
            Utils.Logger.WriteMsgAndLog("All missions count: " + missionToDo.Count);
            //map.WriteMapToFile("map.tmp");
            //2.easy sort of the detailed missions
            DoSortMethod(map.Copy());
            //3.start the particle swarm optimization
            await Task.Run(() => DoAlg(map.Copy()));
            //4.Delete the tmp file
            //Map.DeleteMap("map.tmp");
            return AGVs;
        }

        /// <summary>
        /// Do algrithm with missionToDo
        /// the result will be stored in AGVs
        /// </summary>
        private void DoAlg(Map map)
        {
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < MAX_RANDOM_MISSION; i++)
            {
                //↓mix set in missions randomly in to take out missions by each iterator of MAX_RANDOM_MISSION
                List<Mission.Mission> enumMissionList = RandomMission(map, i);
                if(DEBUG_MODE)
                {
                    Utils.Logger.WriteMsgAndLog("FINAL-RANDOM-TASK: " + enumMissionList.Count);
                    foreach (Mission.Mission m in enumMissionList)
                        Utils.Logger.WriteMsgAndLog(m.Name + ": "
                            + m.FromPosition.ToString()
                            + "<--->" + m.TargetPosition.ToString());
                }
                //Initialize the particles by number of PARTICLE_NUM
                Particle.Global_Best_AGVs = null;
                Particle.Global_Best_Score = double.MaxValue;
                Particle.Global_Best_Attribution = null;
                Particle[] particles = new Particle[PARTICLE_NUM];
                for (int j = 0; j < PARTICLE_NUM; j++)
                    particles[j] = new Particle(AGVNum, 
                        enumMissionList,
                        map.Copy());
                //run iter of each random missions
                //multi thread here
                sw.Restart();
                for (int j = 0; j < NUM_OF_ITER; j++)
                {
                    ////almost 6s all for
                    //for (int k = 0; k < PARTICLE_NUM; k++)
                    //{
                    //    //almost 8ms each EvaluateIter
                    //    particles[k].EvaluateIter(map.Copy());
                    //}

                    //same result as below
                    //Task[] tasks = new Task[PARTICLE_NUM];
                    //for (int k = 0; k < PARTICLE_NUM; k++)
                    //{
                    //    int pk = k;
                    //    Task t = new Task(() => particles[pk].EvaluateIter(map.Copy()));
                    //    tasks[k] = t;
                    //    t.Start();
                    //}
                    //Task.WaitAll(tasks);

                    //almost 3.4s all for
                    Parallel.For(0, PARTICLE_NUM, k =>
                    {
                        particles[k].EvaluateIter(map.Copy());
                    });
                }
                sw.Stop();
                if(Particle.Global_Best_Score < this.costTime)
                {
                    this.costTime = Particle.Global_Best_Score;
                    this.agvs = Particle.Global_Best_AGVs;
                }
                Utils.Logger.WriteMsgAndLog("The " + i + " iter --" + Particle.Global_Best_Score + "---" + sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Random Mission list in some random order
        /// And analysis the obstacle missions to be addition misisons
        /// </summary>
        /// <returns>a deep copy of the mission list, maybe extended</returns>
        private List<Mission.Mission> RandomMission(Map map, int iter)
        {
            //here should randomly mix the missions in by TakeOutMissionsMatrix's certain order
            //TODO::::Need some random algrithms with priority
            List<Mission.Mission> TakeOutMissions = new List<Mission.Mission>();
            int matrixCounter = 0;
            foreach (List<Mission.Mission> lst in TakeOutMissionsMatrix)
            {
                for(int i = 0; i < lst.Count; i++)
                {
                    TakeOutMissions.Insert(RandomIndex(iter, matrixCounter + i + 1), lst[i]);
                }
                matrixCounter += lst.Count;
            }
            List<Mission.Mission> ml = new List<Mission.Mission>();
            //take out missions
            foreach (Mission.Mission m in TakeOutMissions)
            {
                if (m.Status != Mission.Mission.MissionStatus.BEEING_STUCK)
                    ml.Add(m.Copy());
                else
                {
                    //two directions
                    //m.GoodCount[0]-↑
                    //m.GoodCount[1]-↓
                    //here randoms for global iter as MAX_RANDOM_MISSION
                    int direction = 0;
                    if (m.GoodsCount[0] != -1 && m.GoodsCount[1] != -1)
                        direction = Utils.Randomer.GetRandomOneHot(m.GoodsCount)[0] ? 1 : -1;
                    else
                        direction = Utils.Randomer.GetRandomOneHot(m.GoodsCount)[0] ? -1 : 1;
                    int row = m.TargetPosition.Row + direction;
                    List<Mission.Mission> obms = EvaluateObstacleMissions(map.MainPaths,
                        m,
                        direction,
                        row,
                        ml,
                        map,
                        TakeOutMissions);
                    Mission.Mission tmp = m.Copy();
                    tmp.Direction = direction;
                    if(ml.Where(t => t.TargetPosition.Equals(tmp.TargetPosition)).ToList().Count == 0)
                        obms.Add(tmp);
                    ml.AddRange(obms);
                }
            }
            //set in missions
            foreach (Mission.Mission m in SetInMissions)
            {
                ml.Insert(RandomIndex(iter, matrixCounter++), m.Copy());
            }

            //to exchange the obstacle missions TargetPosition and FromPosition
            for(int i = 0; i < ml.Count; i++)
                if(ml[i].Type == Mission.Mission.MissionType.MOVE)
                {
                    Position t = ml[i].FromPosition;
                    ml[i].FromPosition = ml[i].TargetPosition;
                    ml[i].TargetPosition = t;
                }
            return ml;
        }

        /// <summary>
        /// get a random index by iter and maxIndex
        /// </summary>
        /// <param name="iter">control the vibration</param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private int RandomIndex(int iter, int maxIndex)
        {
            int randomIndex = Utils.Randomer.GetRandomInt(maxIndex);
            if (maxIndex == 1)
                return 0;
            else
            {
                //TODO::::iter vibration here
                
            }
            return randomIndex;
        }

        private List<Mission.Mission> EvaluateObstacleMissions(int[] mainPaths, Mission.Mission m,
            int direction, 
            int row,
            List<Mission.Mission> ml,
            Map map, 
            List<Mission.Mission> TakeOutMissions)
        {
            List<Mission.Mission> res = new List<Mission.Mission>();
            if(mainPaths.Contains(row))
                return res;
            res.AddRange(EvaluateObstacleMissions(mainPaths, m, direction, row + direction, ml, map, TakeOutMissions));
            Position p = new Position(m.TargetPosition.Layer, row, m.TargetPosition.Column);
            //mission list for obstacle evaluattion
            if(ml.Where(t =>t.TargetPosition != null 
                && t.TargetPosition.Equals(p)).ToList().Count == 0)
            {
                //mission list for all take out missions
                if(TakeOutMissions.Where(t => t.TargetPosition.Equals(p)).ToList().Count == 0 
                    && map.GetStatusAt(p) == 1)
                {
                    Mission.Mission obstacle = new Mission.Mission();
                    obstacle.Name = "Obstacle";
                    obstacle.TargetGood = map.GetGoodAt(p);
                    obstacle.TargetPosition = p;
                    obstacle.Type = Mission.Mission.MissionType.MOVE;
                    obstacle.Direction = direction;
                    obstacle.NearestMainPath = map.NearestMainPath(p);
                    obstacle.GoodsCount = map.GetGoodCountAtBothSides(obstacle.NearestMainPath, p);
                    res.Add(obstacle);
                }
                else if(map.GetStatusAt(p) == 1)
                {
                    Mission.Mission tmp = TakeOutMissions.Find(t => t.TargetPosition.Equals(p)).Copy();
                    tmp.Direction = direction;
                    tmp.Name = tmp.Name + "_Obstacle";
                    res.Add(tmp);
                }
            }
            //do nothing if the can mission list contains the position for now
            return res;
        }

        /// <summary>
        /// Sort with missionToDo
        /// </summary>
        private void DoSortMethod(Map map)
        {
            List<Mission.Mission> TOMissions = missionToDo
                .Where(m => m.Type == Mission.Mission.MissionType.TAKE_OUT)
                .ToList();
            TakeOutMissionsMatrix = SortTakeOutMissions(map, TOMissions);
            SetInMissions = missionToDo
                .Where(m => m.Type == Mission.Mission.MissionType.SET_IN)
                .ToList();
        }

        private List<List<Mission.Mission>> SortTakeOutMissions(Map map,
            List<Mission.Mission> missions)
        {
            List<Mission.Mission> res = new List<Mission.Mission>();
            List<List<Mission.Mission>> missionMatrix = new List<List<Mission.Mission>>();
            //the missionMatrix is in order by which mission should be done first or later
            while (missions.Count > 0)
            {
                List<Mission.Mission> tmpMissionList = new List<Mission.Mission>();
                foreach (Mission.Mission m in missions)
                {
                    int[] nearestPath = map.NearestMainPath(m.TargetPosition);
                    //chechk for the reachable paths
                    //if could reach with no obstacles the add in tmpMissionList
                    //and change the map status at certain position
                    if (map.CheckForAvalibleForNow(nearestPath, m.TargetPosition))
                    {
                        int[] goodsCount = map.GetGoodCountAtBothSides(nearestPath, m.TargetPosition);
                        m.GoodsCount = goodsCount;
                        m.NearestMainPath = nearestPath;
                        if (missionMatrix.Count > 0)
                            m.Status = Mission.Mission.MissionStatus.BEEING_STUCK;
                        tmpMissionList.Add(m);
                    }
                    //no need to think about NeedTodoBefore, cuz if in order to take
                    //each good could be token away
                }
                if (tmpMissionList.Count == 0 && missions.Count > 0)
                {
                    Utils.Logger.WriteMsgAndLog(missions.Count + " missions are obstacled...");
                    for(int i = 0; i < missions.Count; i++)
                    {
                        Mission.Mission m = missions[i];
                        int[] nearestPath = map.NearestMainPath(m.TargetPosition);
                        //check for the number of goods in the two side of the two path
                        //random a number to decide which way to pick 
                        int[] goodsCount = map.GetGoodCountAtBothSides(nearestPath, m.TargetPosition);
                        m.GoodsCount = goodsCount;
                        m.NearestMainPath = nearestPath;
                        m.Status = Mission.Mission.MissionStatus.BEEING_STUCK;
                        tmpMissionList.Add(m);
                    }
                }
                //delete the selected mission in missions
                //delete the map status and goods
                foreach (Mission.Mission m in tmpMissionList)
                {
                    missions.Remove(m);
                    map.SetGoodAt(new Good(), m.TargetPosition);
                    map.SetStatusAt(0, m.TargetPosition);
                }
                //if tmpMissionList's count is 0 and missions is not empty
                //means the missions are obstacled
                //need to move the obstacles first
                if (tmpMissionList.Count != 0)
                    missionMatrix.Add(tmpMissionList);
            }
            return missionMatrix;
            //foreach(List<Mission.Mission> lst in missionMatrix)
            //{
            //    res.AddRange(lst);
            //}
            //return res;
        }
    }
}
