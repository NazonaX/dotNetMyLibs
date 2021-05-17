using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapAndSimulation.Mission;

namespace MapAndSimulation.Map
{
    public class Particle
    {
        private static double InertiaElement = 1;
        private static double RateElementP = 2;
        private static double RateElementG = 2;

        private static object Global_Lock = new object();
        public static AGV[] Global_Best_AGVs = null;
        public static double Global_Best_Score = double.MaxValue;
        public static int[,] Global_Best_Attribution = null;
        private AGV[] P_Best_AGVs = null;
        private int[,] P_Best_Attribution = null;
        private double P_Best_Score = 0;

        private List<Mission.Mission> Missions = null;
        private Mission.AGV[] AGVs = null;
        private int NumOfAGVs = 0;
        private static int RandomPostionMAxTimes = 20;
        private static double ElementCounter = 1;

        private int[,] attribution;
        private int[] size;
        private double evaluatedTime;
        
        public int[,] Attribution { get => attribution; }
        public int[] Size { get => size; }
        public double EvaluatedTime { get => evaluatedTime; }

        /// <summary>
        /// New and Initialize a particle
        /// </summary>
        /// <param name="numOfAgvs"></param>
        /// <param name="missionList"></param>
        public Particle(int numOfAgvs, List<Mission.Mission> missionList, Map map)
        {
            Missions = missionList;
            this.NumOfAGVs = numOfAgvs;
            AGVs = new AGV[numOfAgvs];
            for(int i =0; i < numOfAgvs; i++)
            {
                AGVs[i] = new AGV();
            }
            size = new int[] { numOfAgvs, missionList.Count };
            attribution = new int[numOfAgvs, missionList.Count];
            for(int i=0;i<missionList.Count; i++)
            {
                //All random initialize
                double[] random = new double[numOfAgvs];
                for (int j = 0; j < numOfAgvs; j++)
                    random[j] = Utils.Randomer.GetRandom();
                bool[] randomSet = Utils.Randomer.GetRandomOneHot(random);
                for (int j = 0; j < numOfAgvs; j++)
                    attribution[j, i] = randomSet[j] ? 1 : 0;
            }
            SetMissionsToAGVs();
            EvaluateTimeCost(map);
            P_Best_AGVs = AGVs;
            P_Best_Score = evaluatedTime;
            //need deep copy
            P_Best_Attribution = new int[size[0], size[1]];
            Array.Copy(attribution, 0, P_Best_Attribution, 0, size[0] * size[1]);

            if (P_Best_Score < Particle.Global_Best_Score)
            {
                Particle.Global_Best_Score = P_Best_Score;
                Particle.Global_Best_AGVs = P_Best_AGVs;
                Particle.Global_Best_Attribution = P_Best_Attribution;
            }
            //PrintAGVStatus();
        }

        public void EvaluateIter(object mapobj)
        {
            Map map = (Map)mapobj;
            //use particle alg with attributes
            //new all AGVs
            AGVs = new AGV[NumOfAGVs];
            for (int i = 0; i < NumOfAGVs; i++)
            {
                AGVs[i] = new AGV();
            }
            for(int i = 0; i < Missions.Count; i++)
            {
                double[] newAttri = new double[NumOfAGVs];
                lock (Global_Lock)
                {
                    for (int j = 0; j < NumOfAGVs; j++)
                    {
                        newAttri[j] = attribution[j, i] * InertiaElement
                            + RateElementP * Utils.Randomer.GetRandomMOPO() * (this.P_Best_Attribution[j, i] - attribution[j, i])
                            + RateElementG * Utils.Randomer.GetRandomMOPO() * (Particle.Global_Best_Attribution[j, i] - attribution[j, i]);
                    }
                }
                bool[] selected = Utils.Randomer.GetRandomOneHot(newAttri);
                for(int j = 0; j < NumOfAGVs; j++)
                {
                    attribution[j, i] = selected[j] ? 1 : 0;
                }
            }
            //evaluate
            SetMissionsToAGVs();
            EvaluateTimeCost(map);
            if(evaluatedTime < P_Best_Score)
            {
                P_Best_AGVs = AGVs;
                P_Best_Score = evaluatedTime;
                P_Best_Attribution = new int[size[0], size[1]];
                Array.Copy(attribution, 0, P_Best_Attribution, 0, size[0] * size[1]);
            }
            //need lock when may modify the global paras
            lock (Global_Lock)
            {
                if (evaluatedTime < Particle.Global_Best_Score)
                {
                    Particle.Global_Best_Score = P_Best_Score;
                    Particle.Global_Best_AGVs = P_Best_AGVs;
                    Particle.Global_Best_Attribution = P_Best_Attribution;
                }
            }
        }

        private void PrintAGVStatus()
        {
            int counter = 1;
            StringBuilder sb = new StringBuilder();
            foreach(AGV a in AGVs)
            {
                sb.Append("AGV No." + counter + "'s mission situations...\n");
                foreach(Mission.Mission m in a.Missions)
                {
                    sb.Append(m.Name);
                    sb.Append("---->from: " + m.FromPosition.ToString() + "\n");
                    sb.Append("---->to: " + m.TargetPosition.ToString() + "\n");
                    sb.Append("---->Taking main path: " + m.MainPathToTake + "\n");
                    sb.Append("---->Taking elevator: " + m.ElevatorToTake + "\n");
                    sb.Append("---->WaitCost: " + m.WaitedTime + "\n");
                    sb.Append("---->Cost: " + m.CostTime + "\n");
                }
                counter++;
            }
            sb.Append("The Total EvaluateTimeCost is: " + evaluatedTime);
            Utils.Logger.WriteMsgAndLog(sb.ToString());
        }

        private void EvaluateTimeCost(Map map)
        {
            double timeCost = 0;
            double element = ElementCounter;
            while (true)
            {
                Mission.Mission[] missionsForNow = new Mission.Mission[NumOfAGVs];
                double[] costs = new double[NumOfAGVs];
                //calculate cost until uncompleted missions
                for(int i = 0; i < NumOfAGVs; i++)
                {
                    bool found = false;
                    costs[i] = 0;
                    for(int j = 0; j < AGVs[i].Missions.Count; j++)
                    {
                        if (AGVs[i].Missions[j].Status != Mission.Mission.MissionStatus.COMPLETED)
                        {
                            missionsForNow[i] = AGVs[i].Missions[j];
                            found = true;
                            break;
                        }
                        costs[i] += AGVs[i].Missions[j].CostTime + AGVs[i].Missions[j].WaitedTime;
                    }
                    if (!found)
                        missionsForNow[i] = null;
                }
                //evaluate un completed missions
                for(int i = 0; i< NumOfAGVs; i++)
                {
                    //all uncompleted missions
                    if (missionsForNow[i] == null)
                        continue;
                    if(missionsForNow[i].CostTime == -1)
                    {
                        //uncalculated, need to random a route and do the calculation
                        //mission type and available
                        if(missionsForNow[i].Type == Mission.Mission.MissionType.TAKE_OUT)
                        {
                            //int[] nearestMainPath = map.NearestMainPath(missionsForNow[i].TargetPosition);
                            int[] nearestMainPath = missionsForNow[i].NearestMainPath;
                            bool available = map.CheckForAvalibleForNow(nearestMainPath,
                                missionsForNow[i].TargetPosition);
                            if (available)
                            {
                                //calculate the cost
                                //select a nearestPath
                                //select a random elevator
                                int path = GetARandomPath(nearestMainPath);
                                missionsForNow[i].Direction = path - missionsForNow[i].TargetPosition.Row > 0 ? 1 : -1;
                                missionsForNow[i].MainPathToTake = path;
                                //int path = map.CheckForAvalibleForNowByOneMainPath(
                                //    nearestMainPath[0], missionsForNow[i].TargetPosition) ?
                                //    nearestMainPath[0] : nearestMainPath[1];
                                double[] eleDistance = new double[map.Elevators.Length];
                                //the probability is higher if the elevator's distance is lower
                                for (int k = 0; k < map.Elevators.Length; k++)
                                    eleDistance[k] = 1.0 / (Math.Abs(map.Elevators[k] - missionsForNow[i].TargetPosition.Column));
                                bool[] selectedElevator = Utils.Randomer.GetRandomOneHot(eleDistance);
                                int elevator = 0;
                                for (int k = 0; k < map.Elevators.Length; k++)
                                    if (selectedElevator[k])
                                        elevator = map.Elevators[k];
                                missionsForNow[i].ElevatorToTake = elevator;
                                double costBack = (missionsForNow[i].TargetPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                    + Math.Abs(elevator - missionsForNow[i].TargetPosition.Column) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                    + Math.Abs(path - missionsForNow[i].TargetPosition.Row) * map.LengthGapOfRow / AGV.SpeedCross;
                                missionsForNow[i].CostTime = costBack;
                                //calculate another cost by the AGV's now position -go
                                //and add mission to the certain position of the missiongList
                                double costGo = 0;
                                List<Mission.Mission> prepareMissions = new List<Mission.Mission>();
                                if (AGVs[i].NowPosition.Equals(new Position(-1, -1, -1)))
                                {
                                    missionsForNow[i].CostTime += costBack;
                                }
                                else
                                {
                                    //calculate the cost by different rows
                                    //here need to get down to layer 1 and change to the correct row
                                    //ignore the AGV's speed on the ground
                                    int[] nowNearestPath = map.NearestMainPath(AGVs[i].NowPosition);
                                    int nowPath = GetARandomPath(nowNearestPath);
                                    for (int k = 0; k < map.Elevators.Length; k++)
                                        eleDistance[k] = 1.0 / (Math.Abs(map.Elevators[k] - AGVs[i].NowPosition.Column));
                                    bool[] selectedElevator2 = Utils.Randomer.GetRandomOneHot(eleDistance);
                                    int elevator2 = 0;
                                    for (int k = 0; k < map.Elevators.Length; k++)
                                        if (selectedElevator2[k])
                                            elevator2 = map.Elevators[k];
                                    if (nowPath != path)
                                    {
                                        costGo = Math.Abs(AGVs[i].NowPosition.Row - nowPath) * map.LengthGapOfRow / AGV.SpeedCross
                                            + Math.Abs(AGVs[i].NowPosition.Column - elevator) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                            + (AGVs[i].NowPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater;
                                        Mission.Mission preMission = new Mission.Mission();
                                        preMission.CostTime = costGo;
                                        preMission.Name = missionsForNow[i].Name + "-Back_To_Ground";
                                        preMission.FromPosition = AGVs[i].NowPosition.Copy();
                                        preMission.TargetPosition = new Position(-1, -1, -1);
                                        preMission.MainPathToTake = nowPath;
                                        preMission.ElevatorToTake = elevator2;
                                        preMission.WaitedTime = missionsForNow[i].WaitedTime;
                                        missionsForNow[i].WaitedTime = 0;
                                        prepareMissions.Add(preMission);
                                        preMission = new Mission.Mission();
                                        preMission.CostTime = costBack;
                                        preMission.Name = missionsForNow[i].Name + "-Go_To_The_Target_Position";
                                        preMission.FromPosition = new Position(-1, -1, -1);
                                        preMission.TargetPosition = missionsForNow[i].TargetPosition.Copy();
                                        preMission.MainPathToTake = path;
                                        preMission.ElevatorToTake = elevator;
                                        prepareMissions.Add(preMission);
                                    }
                                    else
                                    {
                                        if (AGVs[i].NowPosition.Layer != missionsForNow[i].TargetPosition.Layer)
                                            costGo = Math.Abs(AGVs[i].NowPosition.Row - nowPath) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(AGVs[i].NowPosition.Layer - missionsForNow[i].TargetPosition.Layer) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                + Math.Abs(AGVs[i].NowPosition.Column - elevator) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].TargetPosition.Column - elevator) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].TargetPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross;
                                        else
                                            costGo = Math.Abs(AGVs[i].NowPosition.Row - nowPath) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(missionsForNow[i].TargetPosition.Column - AGVs[i].NowPosition.Column) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].TargetPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross;
                                        Mission.Mission preMission = new Mission.Mission();
                                        preMission.CostTime = costGo;
                                        preMission.Name = missionsForNow[i].Name + "-Go_To_The_Target_Position";
                                        preMission.Type = Mission.Mission.MissionType.NONE;
                                        preMission.FromPosition = AGVs[i].NowPosition.Copy();
                                        preMission.TargetPosition = missionsForNow[i].TargetPosition.Copy();
                                        preMission.MainPathToTake = nowPath;
                                        preMission.ElevatorToTake = elevator;
                                        preMission.WaitedTime = missionsForNow[i].WaitedTime;
                                        missionsForNow[i].WaitedTime = 0;
                                        prepareMissions.Add(preMission);
                                    }
                                }
                                //add pre missions to the AGV's misison list
                                int index = 0;
                                for(int k = 0; k < AGVs[i].Missions.Count; k++)
                                    if(AGVs[i].Missions[k] == missionsForNow[i])
                                    {
                                        index = k;
                                        break;
                                    }
                                for (int k = 0; k < prepareMissions.Count; k++)
                                    AGVs[i].Missions.Insert(index + k, prepareMissions[k]);
                            }
                            else
                            {
                                //set wait time
                                missionsForNow[i].WaitedTime = timeCost - costs[i];
                            }
                        }
                        else if(missionsForNow[i].Type == Mission.Mission.MissionType.SET_IN)
                        {
                            //select a random position which will not be an obstacle of the missions
                            //break down to the elevator
                            //break down to the main path

                        }
                        else if(missionsForNow[i].Type == Mission.Mission.MissionType.MOVE)
                        {
                            //int[] nearestMainPath = map.NearestMainPath(missionsForNow[i].FromPosition);
                            int[] nearestMainPath = missionsForNow[i].NearestMainPath;
                            bool available = map.CheckForAvalibleForNow(nearestMainPath,
                                missionsForNow[i].FromPosition);
                            if (available)
                            {
                                //select a main path
                                int path = GetARandomPath(nearestMainPath);
                                missionsForNow[i].Direction = path - missionsForNow[i].FromPosition.Row > 0 ? 1 : -1;
                                missionsForNow[i].MainPathToTake = path;
                                //select an elevator
                                double[] eleDistance = new double[map.Elevators.Length];
                                for (int k = 0; k < map.Elevators.Length; k++)
                                    eleDistance[k] = 1.0 / (Math.Abs(map.Elevators[k] - missionsForNow[i].FromPosition.Column));
                                bool[] selectedElevator = Utils.Randomer.GetRandomOneHot(eleDistance);
                                int elevator = 0;
                                for (int k = 0; k < map.Elevators.Length; k++)
                                    if (selectedElevator[k])
                                        elevator = map.Elevators[k];
                                missionsForNow[i].ElevatorToTake = elevator;
                                //select a position
                                Position randomPosition = null;
                                for(int k = 0; k < RandomPostionMAxTimes; k++)
                                {
                                    int layer = (int)Utils.Randomer.GetRandomByMinMax(1, map.Size[0] + 1);
                                    int row = (int)Utils.Randomer.GetRandomByMinMax(1, map.Size[1] + 1);
                                    int column = (int)Utils.Randomer.GetRandomByMinMax(1, map.Size[2] + 1);
                                    //check for if being an obstacle
                                    Position p = new Position(layer, row, column);
                                    if (map.GetStatusAt(p) != 0)
                                        continue;
                                    int[] randomPath = map.NearestMainPath(p);
                                    if (!map.CheckForAvalibleForNow(randomPath, p))
                                        continue;
                                    bool isObstacle = false;
                                    foreach(int randomP in randomPath)
                                    {
                                        if (randomP == -1)
                                            continue;
                                        int randomDirection = randomP - p.Row > 0 ? 1 : -1;
                                        //obstacle if any side is a obstacle
                                        isObstacle = isObstacle || CheckForObstacle(p, randomDirection, map);
                                    }
                                    if (!isObstacle)
                                    {
                                        //calculate a random probability to decide whether to use the random position
                                        double randomDistance = Math.Abs(p.Layer - missionsForNow[i].FromPosition.Layer) * map.LengthHeightOfLayer
                                            + Math.Abs(p.Row - missionsForNow[i].FromPosition.Row) * map.LengthGapOfRow
                                            + Math.Abs(p.Column - missionsForNow[i].FromPosition.Column) * map.LengthGapOfColumn;
                                        double mapSize = (map.Size[0] - 1) * map.LengthHeightOfLayer
                                            + (map.Size[1] - 1) * map.LengthGapOfRow
                                            + (map.Size[2] - 1) * map.LengthGapOfColumn;
                                        if (Utils.Randomer.GetRandom() <= (mapSize - randomDistance) / mapSize)
                                        {
                                            randomPosition = p;
                                        }
                                    }
                                    else
                                        continue;
                                }
                                if(randomPosition == null)
                                    missionsForNow[i].WaitedTime = timeCost - costs[i];
                                else
                                {
                                    //calculate distance
                                    missionsForNow[i].TargetPosition = randomPosition;
                                    missionsForNow[i].TargetGood = map.GetGoodAt(missionsForNow[i].FromPosition);
                                    List<Mission.Mission> prepareMissions = new List<Mission.Mission>();
                                    int[] targetPath = map.NearestMainPath(missionsForNow[i].TargetPosition);
                                    int selectedTargetPath = GetARandomPath(targetPath);
                                    
                                    if(AGVs[i].NowPosition.Equals(new Position(-1, -1, -1)))
                                    {
                                        Mission.Mission premission = new Mission.Mission();
                                        premission.Name = missionsForNow[i].Name + "-Go_To_From";
                                        premission.ElevatorToTake = elevator;
                                        premission.MainPathToTake = path;
                                        premission.WaitedTime = missionsForNow[i].WaitedTime;
                                        missionsForNow[i].WaitedTime = 0;
                                        premission.FromPosition = new Position(-1, -1, -1);
                                        premission.TargetPosition = missionsForNow[i].FromPosition.Copy();
                                        premission.CostTime = (premission.TargetPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                            + Math.Abs(premission.TargetPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                            + Math.Abs(premission.TargetPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath;
                                        prepareMissions.Add(premission);
                                    }
                                    else
                                    {
                                        int nowMainPath = GetARandomPath(map.NearestMainPath(AGVs[i].NowPosition));
                                        if (path == nowMainPath)
                                        {
                                            Mission.Mission premission = new Mission.Mission();
                                            premission.Name = missionsForNow[i].Name + "-Go_To_From";
                                            premission.ElevatorToTake = elevator;
                                            premission.MainPathToTake = path;
                                            premission.WaitedTime = missionsForNow[i].WaitedTime;
                                            missionsForNow[i].WaitedTime = 0;
                                            premission.FromPosition = AGVs[i].NowPosition.Copy();
                                            premission.TargetPosition = missionsForNow[i].TargetPosition.Copy();
                                            if(premission.FromPosition.Layer != premission.TargetPosition.Layer)
                                                premission.CostTime = Math.Abs(premission.FromPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                    + Math.Abs(premission.FromPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                    + Math.Abs(premission.FromPosition.Layer - premission.TargetPosition.Layer) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                    + Math.Abs(premission.TargetPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                    + Math.Abs(premission.TargetPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross;
                                            else
                                                premission.CostTime = Math.Abs(premission.FromPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                    + Math.Abs(premission.FromPosition.Column - premission.TargetPosition.Column) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                    + Math.Abs(premission.TargetPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross;
                                            prepareMissions.Add(premission);
                                        }
                                        else
                                        {
                                            Mission.Mission premission = new Mission.Mission();
                                            premission.Name = missionsForNow[i].Name + "-Back_To_Ground";
                                            premission.FromPosition = AGVs[i].NowPosition.Copy();
                                            premission.TargetPosition = new Position(-1, -1, -1);
                                            premission.ElevatorToTake = elevator;
                                            premission.MainPathToTake = nowMainPath;
                                            premission.WaitedTime = missionsForNow[i].WaitedTime;
                                            missionsForNow[i].WaitedTime = 0;
                                            premission.CostTime = (premission.FromPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                + Math.Abs(premission.FromPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(premission.FromPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath;
                                            prepareMissions.Add(premission);
                                            premission = new Mission.Mission();
                                            premission.Name = missionsForNow[i].Name + "-Go_To_From";
                                            premission.FromPosition = new Position(-1, -1, -1);
                                            premission.TargetPosition = missionsForNow[i].FromPosition.Copy();
                                            premission.MainPathToTake = path;
                                            premission.ElevatorToTake = elevator;
                                            premission.CostTime = (premission.TargetPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                + Math.Abs(premission.TargetPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(premission.TargetPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath;
                                            prepareMissions.Add(premission);
                                        }
                                    }

                                    if(selectedTargetPath == path)
                                    {
                                        if (missionsForNow[i].FromPosition.Layer == missionsForNow[i].TargetPosition.Layer)
                                            missionsForNow[i].CostTime = Math.Abs(missionsForNow[i].FromPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(missionsForNow[i].FromPosition.Column - missionsForNow[i].TargetPosition.Column) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].TargetPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross;
                                        else
                                            missionsForNow[i].CostTime = Math.Abs(missionsForNow[i].FromPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(missionsForNow[i].FromPosition.Column - missionsForNow[i].ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].TargetPosition.Row - path) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(missionsForNow[i].TargetPosition.Column - missionsForNow[i].ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath
                                                + Math.Abs(missionsForNow[i].FromPosition.Layer - missionsForNow[i].TargetPosition.Layer) * map.LengthHeightOfLayer / AGV.SpeedElevater;
                                    }
                                    else
                                    {
                                        Mission.Mission premission = new Mission.Mission();
                                        premission.Name = missionsForNow[i].Name + "-ING_Back_To_Ground";
                                        premission.FromPosition = missionsForNow[i].FromPosition.Copy();
                                        premission.TargetPosition = new Position(-1, -1, -1);
                                        premission.MainPathToTake = path;
                                        premission.ElevatorToTake = elevator;
                                        premission.CostTime = (premission.FromPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                + Math.Abs(premission.FromPosition.Row - premission.MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(premission.FromPosition.Column - premission.ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath;
                                        prepareMissions.Add(premission);
                                        //rewrite the missionForNow[i]'s from position to (-1,-1,-1)
                                        //calculate the cost of time of the final mission
                                        missionsForNow[i].Name = missionsForNow[i].Name + "-FINISHING_Go_To_Target";
                                        missionsForNow[i].MainPathToTake = selectedTargetPath;
                                        missionsForNow[i].ElevatorToTake = elevator;
                                        missionsForNow[i].CostTime = (missionsForNow[i].TargetPosition.Layer - 1) * map.LengthHeightOfLayer / AGV.SpeedElevater
                                                + Math.Abs(missionsForNow[i].TargetPosition.Row - missionsForNow[i].MainPathToTake) * map.LengthGapOfRow / AGV.SpeedCross
                                                + Math.Abs(missionsForNow[i].TargetPosition.Column - missionsForNow[i].ElevatorToTake) * map.LengthGapOfColumn / AGV.SpeedMainPath;
                                    }
                                    //add pre missions to the AGV's misison list
                                    int index = 0;
                                    for (int k = 0; k < AGVs[i].Missions.Count; k++)
                                        if (AGVs[i].Missions[k] == missionsForNow[i])
                                        {
                                            index = k;
                                            break;
                                        }
                                    for (int k = 0; k < prepareMissions.Count; k++)
                                        AGVs[i].Missions.Insert(index + k, prepareMissions[k]);
                                }
                            }
                            else
                            {
                                missionsForNow[i].WaitedTime = timeCost - costs[i];
                            }
                            //System.Diagnostics.Debug.WriteLine(missionsForNow[i].FromPosition.ToString());
                        }
                    }

                    else if (costs[i] + missionsForNow[i].CostTime + missionsForNow[i].WaitedTime <= timeCost)
                    {
                        //mark mission as completed
                        //remove mission from the map if TAKE_OUT
                        missionsForNow[i].Status = Mission.Mission.MissionStatus.COMPLETED;
                        if(missionsForNow[i].Type == Mission.Mission.MissionType.TAKE_OUT)
                        {
                            map.SetStatusAt(0, missionsForNow[i].TargetPosition);
                            map.SetGoodAt(new Good(), missionsForNow[i].TargetPosition);
                            AGVs[i].NowPosition = new Position(-1, -1, -1);
                        }
                        else if(missionsForNow[i].Type == Mission.Mission.MissionType.SET_IN)
                        {

                        }
                        else if(missionsForNow[i].Type == Mission.Mission.MissionType.MOVE)
                        {
                            if(missionsForNow[i].TargetPosition.Equals(new Position(-1, -1, -1)))
                            {
                                map.SetStatusAt(0, missionsForNow[i].FromPosition);
                            }
                            map.SetStatusAt(0, missionsForNow[i].FromPosition);
                            map.SetGoodAt(new Good(), missionsForNow[i].FromPosition);
                            map.SetStatusAt(1, missionsForNow[i].TargetPosition);
                            map.SetGoodAt(missionsForNow[i].TargetGood, missionsForNow[i].TargetPosition);
                            AGVs[i].NowPosition = missionsForNow[i].TargetPosition.Copy();
                        }
                        else if(missionsForNow[i].Type == Mission.Mission.MissionType.NONE)
                        {
                            //normal move
                            AGVs[i].NowPosition = missionsForNow[i].TargetPosition.Copy();
                        }
                    }
                }
                bool finished = true;
                //is all missions are completed
                for(int i =0; i < NumOfAGVs; i++)
                    if(AGVs[i].Missions.Count > 0)
                        finished = finished && (AGVs[i].Missions[AGVs[i].Missions.Count - 1].Status == Mission.Mission.MissionStatus.COMPLETED);
                if (finished)
                    break;
                timeCost += element;
            }
            evaluatedTime = timeCost;
        }


        private int GetARandomPath(int[] targetPath)
        {
            int selectedTargetPath = 0;
            bool[] targetSelect;
            if (targetPath.Contains(-1))
                targetSelect = Utils.Randomer.GetRandomOneHot(targetPath);
            else
                targetSelect = Utils.Randomer.GetRandomOneHot(new int[] { 1, 1 });
            for (int k = 0; k < targetSelect.Length; k++)
                if (targetSelect[k])
                    selectedTargetPath = targetPath[k];
            return selectedTargetPath;
        }

        private bool CheckForObstacle(Position p, int randomDirection, Map map)
        {
            int searchDirection = -1 * randomDirection;
            bool isObstacle = false;
            int row = p.Row + searchDirection;
            while (row >0 && row <= map.Size[1] && !map.MainPaths.Contains(row))
            {
                Position pt = new Position(p.Layer, row, p.Column);
                foreach(AGV a in AGVs)
                {
                    //direction = 0 means could be taken out by any directions
                    //direciont = randomDirection means need to be taken out cross the postion p
                    isObstacle = isObstacle || a.Missions.Where(m => m.Status != Mission.Mission.MissionStatus.COMPLETED
                                                    && (m.FromPosition.Equals(pt) || m.TargetPosition.Equals(pt))
                                                    && ((m.Direction != 0 && m.Direction == randomDirection)
                                                    || m.Direction == 0)).ToList().Count != 0;
                }
                row = row + searchDirection;
            }
            return isObstacle;
        }

        /// <summary>
        /// get the min element as a time cost counter
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        private double GetMinElement(double v1, double v2, double v3)
        {
            //double element = 1;
            //while(v1 % element != 0 || v2 % element != 0 || v3 % element != 0)
            //{
            //    element /= 10;
            //    if (element < 0.1)
            //        break;
            //}
            //return element;
            return 1;
        }

        /// <summary>
        /// to set mission to certain agvs
        /// calculate the mission cost except for the WAIT mission between agvs
        /// </summary>
        /// <param name="map"></param>
        private void SetMissionsToAGVs()
        {
            //the missions can be done definitly in order
            for(int j = 0; j < size[1]; j++)
            {
                for(int i = 0; i < size[0]; i++)
                {
                    if (attribution[i, j] == 1)
                    {
                        Mission.Mission m = Missions[j].Copy();
                        AGVs[i].Missions.Add(m);
                    }
                }
            }
        }
    }
}
