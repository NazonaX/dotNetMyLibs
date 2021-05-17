using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class CargoWays
    {
        private DAL.CargoWays _cargoway = new DAL.CargoWays();

        //bottom and top restore row number, which is mapitem
        public int BottomLeft { get; set; }
        public int TopLeft { get; set; }
        public int AvailableCountLeft
        {
            get { return BottomLeft - TopLeft + 1; }
        }
        public int BottomRight { get; set; }
        public int TopRight { get; set; }
        public int AvailableCountRight
        {
            get { return TopRight - BottomRight + 1; }
        }
        public int TotalCount
        {
            get { return Math.Abs(LeftRailColumn - RightRailColumn) - 1; }
        }
        public double AvgTimeCost { get; set; }//just for test


        private HashSet<string> _goodHas = new HashSet<string>();
        public HashSet<string> GoodHas
        {
            get { return _goodHas; }
            set { _goodHas = value; }
        }
        public bool IsBlank
        {
            get
            {
                if(LeftIsRail && RightIsRail)
                {
                    return TotalCount == AvailableCountLeft && TotalCount == AvailableCountRight;
                }
                if (LeftIsRail)
                {
                    return TotalCount == AvailableCountLeft;
                }
                if (RightIsRail)
                {
                    return TotalCount == AvailableCountRight;
                }
                return false;
            }
        }
        //we use potential energy to decide which bottom should be the bottom at very first, so does no business to the four dicts
        public int LeftPotentialEnergy { get; set; }
        public int RightPotentialEnergy { get; set; }
        //following four dictionary are used for reachable and obstruction for each OutPoint to input and output
        //we use the four dicts to filter the io points, and we can also use simple cost to calculate the score
        public Dictionary<MapItems, double> LeftRailInPoints { get; set; }
        public Dictionary<MapItems, double> LeftRailOutPoints { get; set; }
        public Dictionary<MapItems, double> RightRailInPoints { get; set; }
        public Dictionary<MapItems, double> RightRailOutPoints { get; set; }


        //outputpoint restore row number, which is not a mapitem
        public int LeftRailColumn//LeftRailColumn <--> database also need to be modified
        {
            get { return _cargoway.LeftRailColumn; }
            set { _cargoway.LeftRailColumn = value; }
        }
        public int RightRailColumn
        {
            get { return _cargoway.RightRailColumn; }
            set { _cargoway.RightRailColumn = value; }
        }
        public bool LeftIsRail//LeftIsRail
        {
            get { return _cargoway.LeftIsRail; }
            set { _cargoway.LeftIsRail = value; }
        }
        public bool RightIsRail
        {
            get { return _cargoway.RightIsRail; }
            set { _cargoway.RightIsRail = value; }
        }
        public string CargoWayNumber// --> string, according to layer, rack
        {
            get { return _cargoway.CargoWayNumber; }
            set { _cargoway.CargoWayNumber = value; }
        }
        public int ZoneAt
        {
            get { return _cargoway.ZoneAt; }
            set { _cargoway.ZoneAt = value; }
        }
        public int LayerAt
        {
            get { return _cargoway.LayerAt; }
            set { _cargoway.LayerAt = value; }
        }
        public int RackAt//RowAt
        {
            get { return _cargoway.ColAt; }
            set { _cargoway.ColAt = value; }
        }
        public int Id { get { return _cargoway.Id; } }

        #region methods
        public void DAL_SetCargoWay(DAL.CargoWays cargoway)
        {
            this._cargoway = cargoway;
        }
        public DAL.CargoWays DAL_GetCargoWay()
        {
            return this._cargoway;
        }
        #endregion
    }
}
