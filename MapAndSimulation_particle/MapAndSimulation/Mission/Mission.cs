using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MapAndSimulation.Map;

namespace MapAndSimulation.Mission
{
    public class Mission
    {
        public enum MissionType{ NONE, TAKE_OUT, SET_IN, SET_BUFFER, MOVE }
        public enum MissionStatus { NONE, BEEING_STUCK, COMPLETED}
        //↑TAKE_OUT-this mission type means that the mission is to set goods out of rack
        //SET_IN-to get goods into the rack
        //BEEING_STUCK-the good is stuck when taking out
        //SET_BUFFER-all racks are storaged, need some place else to take goods out or set in
        //NEED_MOVE-need to move some goods that are not inculuded in missions to complete the missions
        private string name = "null";//the name of mission
        private MissionType type = MissionType.NONE;
        private MissionStatus status = MissionStatus.NONE;

        //the location of mission start and target
        private Position fromPosition = new Position(-1, -1, -1);
        private Position targetPosition = new Position(-1, -1, -1);
        private Map.Good targetGood = new Good();
        private int elevatorToTake = 0;
        private int direction = 0;
        private int mainPathToTake = 0;
        private double priority = 0;
        private Mission needTodoBefore = null;
        private double costTime = -1;
        private double waitedTime = 0;

        //the nearest main path and the goods count on the two side of main path
        //used to calculate randomly
        private int[] nearestMainPath;
        private int[] goodsCount;


        public string Name { get => name; set => name = value; }
        public MissionType Type { get => type; set => type = value; }
        public Position FromPosition { get => fromPosition; set => fromPosition = value; }
        public Position TargetPosition { get => targetPosition; set => targetPosition = value; }
        public Good TargetGood { get => targetGood; set => targetGood = value; }
        public double Priority { get => priority; set => priority = value; }
        public Mission NeedTodoBefore { get => needTodoBefore; set => needTodoBefore = value; }
        public MissionStatus Status { get => status; set => status = value; }
        public int[] NearestMainPath { get => nearestMainPath; set => nearestMainPath = value; }
        public int[] GoodsCount { get => goodsCount; set => goodsCount = value; }
        public double CostTime { get => costTime; set => costTime = value; }
        public int Direction { get => direction; set => direction = value; }
        public double WaitedTime { get => waitedTime; set => waitedTime = value; }
        public int ElevatorToTake { get => elevatorToTake; set => elevatorToTake = value; }
        public int MainPathToTake { get => mainPathToTake; set => mainPathToTake = value; }

        public Mission Copy()
        {
            Mission mission = new Mission();
            mission.name = this.name;
            mission.type = this.type;
            mission.status = this.status;

            mission.direction = this.direction;
            mission.mainPathToTake = this.mainPathToTake;
            mission.elevatorToTake = this.elevatorToTake;
            if(this.fromPosition != null)
                mission.fromPosition = this.fromPosition.Copy();
            if(this.targetPosition != null)
                mission.targetPosition = this.targetPosition.Copy();
            if(this.targetGood != null)
                mission.targetGood = this.targetGood.Copy();
            mission.priority = this.priority;
            if(this.needTodoBefore != null)
                mission.needTodoBefore = mission.needTodoBefore.Copy();
            if (this.nearestMainPath != null)
            {
                mission.nearestMainPath = new int[this.nearestMainPath.Length];
                this.nearestMainPath.CopyTo(mission.nearestMainPath, 0);
            }
            if(this.goodsCount != null)
            {
                mission.goodsCount = new int[this.goodsCount.Length];
                this.goodsCount.CopyTo(mission.goodsCount, 0);
            }

            mission.costTime = this.costTime;
            mission.waitedTime = this.waitedTime;

            return mission;
        }
    }
}