using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models.Classes
{
    [Serializable]
    public class Map
    {
        private static string FileName = "map.nax";
        private static string GoodsTypesFileName = "gt.nax";
        //layout
        public static int Padding = 7;
        public static int Width = 14;

        private int _rackCount = -1;
        private int _columnCount = -1;
        private int _layerCount = -1;
        private MapItem[,,] _mapItems = null;
        public double GapAlongRack { get; set; }
        public double GapAlongCloumn { get; set; }
        public double GapBetweenLayers { get; set; }
        public double PSMaxSpeed { get; set; }
        public double PSAcceleration { get; set; }
        public double PSDeceleration { get; set; }
        public double CSMaxSpeed { get; set; }
        public double CSAcceleration { get; set; }
        public double CSDeceleration { get; set; }
        public double LMaxSpeed { get; set; }
        public double LAcceleration { get; set; }
        public double LDeceleration { get; set; }

        public Map(int layerCount, int rackCount, int columnCount)
        {
            InitialByXYZ(layerCount, rackCount, columnCount);
        }
        public Map() { }
        public void SetMapFromScratch(int layerCount, int rackCount, int columnCount)
        {
            InitialByXYZ(layerCount, rackCount, columnCount);
        }
        /// <summary>
        /// initial the map by layer, rackcount and columncount
        /// </summary>
        /// <param name="layerCount"></param>
        /// <param name="rackCount"></param>
        /// <param name="columnCount"></param>
        private void InitialByXYZ(int layerCount, int rackCount, int columnCount)
        {
            RackCount = rackCount;
            ColumnCount = columnCount;
            LayerCount = layerCount;

            _mapItems = new MapItem[LayerCount, RackCount, ColumnCount];
            ReadGoodsTypesFile();//read goods types first for initial map items
            for (int i = 0; i < LayerCount; i++)
            {
                for (int j = 0; j < RackCount; j++)
                {
                    for (int m = 0; m < ColumnCount; m++)
                    {
                        _mapItems[i, j, m] = new MapItem(Width, Width,
                            j * (Width + Padding) + Padding,
                            m * (Width + Padding) + Padding,
                            MapItem.ItemTypes.NONE,
                            i, j, m);
                        //initial the available types of each map item
                        for (int k = 0; k < GoodsTypes.Count; k++)
                            _mapItems[i, j, m].AvailableGoodTypes.Add(GoodsTypes[k], false);
                    }
                }
            }
        }

        private static void ReadGoodsTypesFile()
        {
            if (!IOOps.IsFileExisted(GoodsTypesFileName))
                return;
            List<string> ls = IOOps.ClassRead(GoodsTypesFileName) as List<string>;
            if (ls != null)
            {
                Goods.GoodsTypes = ls;
            }
        }

        public int RackCount
        {
            get
            {
                return _rackCount;
            }
            set
            {
                _rackCount = value;
            }
        }
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
            }
        }
        public int LayerCount
        {
            get { return _layerCount; }
            set
            {
                _layerCount = value;
            }
        }
        public MapItem[,,] MapItems
        {
            get { return _mapItems; }
        }
        public MapItem this[int layer, int rack, int column]
        {
            get
            {
                if (layer >= 0 && layer < this.LayerCount
                    && rack >= 0 && rack < this.RackCount
                    && column >= 0 && column < this.ColumnCount)
                    return MapItems[layer, rack, column];
                else
                    throw new IndexOutOfRangeException("layer:" + layer + "-rack:" + rack + "-column:" + column + " is out of boundary..."
                        + "the total layer-rack-column is: " + LayerCount + "-" + RackCount + "-" + column);
            }
            set
            {
                throw new NotImplementedException("Map--MapItem--Set");
            }
        }
        public List<string> GoodsTypes
        {
            get { return Goods.GoodsTypes; }
        }
        /// <summary>
        /// deep copy of the map existance
        /// </summary>
        /// <returns>a map obj or null</returns>
        public Map Copy()
        {
            return IOOps.CopyMemory(this) as Map;
        }
        /// <summary>
        /// serialize and write the map existance to the file
        /// </summary>
        public void SaveToFile()
        {
            IOOps.ClassWrite(FileName, this);
        }
        public void SaveGoodsTypesToFile()
        {
            IOOps.ClassWrite(GoodsTypesFileName, Goods.GoodsTypes);
        }
        /// <summary>
        /// to read the map object from the file
        /// </summary>
        /// <returns>an existed map</returns>
        public static Map ReadFromFile()
        {
            ReadGoodsTypesFile();
            Map map = IOOps.ClassRead(FileName) as Map;
            if (map == null)
            {
                map = new Map();
                IOOps.DeleteFile(FileName);
                throw new NullReferenceException("No Map Existance when read from file..." + FileName);
            }
            return map;
        }
        /// <summary>
        /// check for the file if it is existed
        /// </summary>
        /// <returns></returns>
        public static bool CheckForFile()
        {
            return IOOps.IsFileExisted(FileName);
        }
        public bool IsEmpty()
        {
            return LayerCount == -1 && RackCount == -1 && ColumnCount == -1;
        }
        /// <summary>
        /// add a type for GoodsTypes
        /// </summary>
        /// <param name="type"></param>
        public void AddGoodsTypes(string type)
        {
            Goods.AddGoodsTypes(type);
            //Add the new goods types to all map items' available dictionary
            for(int i = 0; i < LayerCount; i++)
            {
                for(int j = 0; j < RackCount; j++)
                {
                    for(int k = 0; k < ColumnCount; k++)
                    {
                        MapItems[i, j, k].AvailableGoodTypes.Add(type, false);
                    }
                }
            }
        }
        public void DeleteGoodsTypes(string type)
        {
            Goods.DeleteGoodsTypes(type);
            //delete the certain good type of each available dictionary
            for (int i = 0; i < LayerCount; i++)
            {
                for (int j = 0; j < RackCount; j++)
                {
                    for (int k = 0; k < ColumnCount; k++)
                    {
                        MapItems[i, j, k].AvailableGoodTypes.Remove(type);
                        if (!MapItems[i, j, k].HasAvailableGoodTypes() &&
                            (MapItems[i, j, k].Type == MapItem.ItemTypes.EMPTY_STORAGE || MapItems[i, j, k].Type == MapItem.ItemTypes.FULL_STORAGE))
                        {
                            //TODO:NazonaX -->for now is just clear
                            //need to do some other reasonable actions
                            MapItems[i, j, k].Type = MapItem.ItemTypes.NONE;
                            MapItems[i, j, k].Good.Clear();
                        }
                    }
                }
            }
        }
        public void ModifyGoodsTypes(string original, string target)
        {
            Goods.ModifyGoodsTypes(original, target);
            //modify the certain good type to another type
            //delete and add
            for (int i = 0; i < LayerCount; i++)
            {
                for (int j = 0; j < RackCount; j++)
                {
                    for (int k = 0; k < ColumnCount; k++)
                    {
                        bool value = MapItems[i, j, k].AvailableGoodTypes[original];
                        MapItems[i, j, k].AvailableGoodTypes.Remove(original);
                        MapItems[i, j, k].AvailableGoodTypes.Add(target, value);
                    }
                }
            }
        }

        //used for reset all Map Items' AvailableGoodsTypes to all false
        public void ResetAllMapItemsAvailableGoodsTypes()
        {
            for (int i = 0; i < LayerCount; i++)
                for (int j = 0; j < RackCount; j++)
                    for (int k = 0; k < ColumnCount; k++)
                        MapItems[i, j, k].ResetAvailableGoodTypes();
        }


    }
}
