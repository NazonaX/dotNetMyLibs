using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class Map
    {
        #region static public
        public static string KEY_MAP_NAME = "MapName";
        public static string KEY_LAYER_COUNT = "LayerCount";
        public static string KEY_RACK_COUNT = "RackCount";
        public static string KEY_COLUMN_COUNT = "ColumnCount";
        public static string KEY_GAP_ALOMG_RACK = "GapAlongRack";
        public static string KEY_GAP_ALONG_COLUMN = "GapAlongColumn";
        public static string KEY_GAP_BETWEEN_LAYERS = "GapBetweenLayers";
        public static string KEY_PS_MAXSPEED = "PSManSpeed";
        public static string KEY_PS_ACCELERATION = "PSAcceleration";
        public static string KEY_PS_DECELERATION = "PSDeceleration";
        public static string KEY_CS_MAXSPEED = "CSManSpeed";
        public static string KEY_CS_ACCELERATION = "CSAcceleration";
        public static string KEY_CS_DECELERATION = "CSDeceleration";
        public static string KEY_L_MAXSPEED = "LManSpeed";
        public static string KEY_L_ACCELERATION = "LAcceleration";
        public static string KEY_L_DECELERATION = "LDeceleration";
        #endregion


        #region public properties
        public string MapName;
        public int LayerCount;
        public int RackCount;
        public int ColumnCount;
        public double GapAlongRack;
        public double GapAlongCloumn;
        public double GapBetweenLayers;
        //Parent Shuttle
        public double PSMaxSpeed;
        public double PSAcceleration;
        public double PSDeceleration;
        //Child Shuttle
        public double CSMaxSpeed;
        public double CSAcceleration;
        public double CSDeceleration;
        //Lifter
        public double LMaxSpeed;
        public double LAcceleration;
        public double LDeceleration;
        
        public List<MapItems> MapItems
        {
            get { return _mapItemList; }
            set { _mapItemList = value; }
        }
        public List<Goods> Goods
        {
            get { return _goodList; }
            set { _goodList = value; }
        }
        public List<Zone> Zones
        {
            get { return _zoneList; }
            set { _zoneList = value; }
        }
        public List<Types> Types
        {
            get { return _typesList; }
            set { _typesList = value; }
        }
        public List<MapItems> SpecialMapItems
        {
            get { return _specialMapItemList; }
            set { _specialMapItemList = value; }
        }
        public List<SpecialConnection> SpecialConnections
        {
            get { return _specialConnectionList; }
            set { _specialConnectionList = value; }
        }
        public Dictionary<int, MapItems> FastFinder
        {
            get { return _fastFinder; }
            set { _fastFinder = value; }
        }
        public List<CargoWays> CargoWays
        {
            get { return _cargowaysList; }
            set { _cargowaysList = value; }
        }
        public List<CargoWaysLock> CargoWaysLocks
        {
            get { return _cargowaysLocksList; }
            set { _cargowaysLocksList = value; }
        }
        public List<Rails> Rails
        {
            get { return _rails; }
            set { _rails = value; }
        }

        public MapItems this[int i, int j, int k]
        {
            get
            {
                if(_mapFlatten != null)
                {
                    return _mapFlatten[i, j, k];
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                if(_mapFlatten != null)
                {
                    _mapFlatten[i, j, k] = value;
                }
            }
        }
        #endregion

        #region private properties
        private MapItems[,,] _mapFlatten = null;
        private List<MapItems> _mapItemList = new List<MapItems>();
        private List<Goods> _goodList = new List<Goods>();
        private List<Zone> _zoneList = new List<Zone>();
        private List<Types> _typesList = new List<Types>();
        private List<MapItems> _specialMapItemList = new List<MapItems>();
        private List<SpecialConnection> _specialConnectionList = new List<SpecialConnection>();
        private Dictionary<int, MapItems> _fastFinder = new Dictionary<int, MapItems>();//used to fast find mapitem with mapitemId
        private List<CargoWays> _cargowaysList = new List<CargoWays>();
        private List<CargoWaysLock> _cargowaysLocksList = new List<CargoWaysLock>();
        private List<Rails> _rails = new List<Rails>();

        #endregion

        #region Methods
        public Map()
        {

        }
        public void FlattenMapItems(List<DAL.MapItems>  _DAL_MApITems, List<DAL.MapItems> _DAL_SpecialMapItems)
        {
            if(_mapItemList.Count == 0)
            {
                return;
            }
            else
            {
                _mapFlatten = new MapItems[this.LayerCount, this.RackCount, this.ColumnCount];
                for(int i = _mapItemList.Count - 1; i >= 0; i--)
                {
                    FastFinder.Add(_mapItemList[i].MapItemID, _mapItemList[i]);
                    if (_mapItemList[i].Rack < 0 || _mapItemList[i].Rack >= this.RackCount 
                        || _mapItemList[i].Column < 0 || _mapItemList[i].Column >= this.ColumnCount)
                    {
                        SpecialMapItems.Add(_mapItemList[i]);
                        _DAL_SpecialMapItems.Add(_mapItemList[i].DAL_GetMapItem());
                        //for they are loead in the same sequence
                        //so here we take them as the same sequence to delete
                        _mapItemList.RemoveAt(i);
                        _DAL_MApITems.RemoveAt(i);
                    }
                    else
                        this[_mapItemList[i].Layer, _mapItemList[i].Rack, _mapItemList[i].Column] = _mapItemList[i];
                }
            }
        }

        #endregion


    }
}
