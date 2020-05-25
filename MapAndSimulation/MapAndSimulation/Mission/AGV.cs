using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Mission
{
    public class AGV
    {
        public enum Direction {UNWORK, ROW_L_R, ROW_R_L, COL_U_D, COL_D_U}
        public enum Status { TAKING_GOOD, EMPTY}

        private List<Mission> missions = new List<Mission>();
        public static double SpeedMainPath = 5;
        public static double SpeedCross = 2;
        public static double SpeedElevater = 7;

        private Position nowPosition = new Position(-1, -1, -1);
        private Status nowStatus = Status.EMPTY;
        private Direction nowDirection = Direction.UNWORK;

        public List<Mission> Missions { get => missions; set => missions = value; }
        public Position NowPosition { get => nowPosition; set => nowPosition = value; }
        public Status NowStatus { get => nowStatus; set => nowStatus = value; }
        public Direction NowDirection { get => nowDirection; set => nowDirection = value; }
    }
}
