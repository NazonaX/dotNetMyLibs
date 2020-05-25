using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Classes
{
    [Serializable]
    public class MapItem
    {
        #region for view
        public static class ItemColors
        {
            public static string NONE = "#000000";//black
            public static string EMPTY_STORAGE = "#50514f";//black gray
            public static string FULL_STORAGE = "#ff9f1c";//bright orange
            public static string RAIL = "#acacac";//gray
            public static string INPUT_POINT = "#e71d36";//bright red
            public static string OUTPUT_POINT = "#5c5ff2";//bright blue
            public static string PARENT_SHUTTLE_UNCARRY = "#e5989b";//gray pink
            public static string PARENT_SHUTTLE_CARRIED = "#b5838d";//grayer pink
            public static string CHILD_SHUTTLE_UNCARRY = "#9bc1bc";//gray green
            public static string CHILD_SHUTTLE_CARRIED = "#5ca4a9";//grayer green
            public static string UNAVAILABLE = "#ffffff";//white
        }
        public enum ItemColorTypes
        {
            NONE, EMPTY_STORAGE, FULL_STORAGE, RAIL, INPUT_POINT, OUTPUT_POINT,
            PARENT_SHUTTLE_UNCARRY, PARENT_SHUTTLE_CARRIED, CHILD_SHUTTLE_UNCARRY, CHILD_SHUTTLE_CARRIED,
            UNAVAILABLE
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int TopPad { get; set; }
        public int LeftPad { get; set; }
        private ItemColorTypes ColorType = ItemColorTypes.NONE;
        public string Color {
            get {
                if (ColorType == ItemColorTypes.NONE)
                    return ItemColors.NONE;
                else if (ColorType == ItemColorTypes.INPUT_POINT)
                    return ItemColors.INPUT_POINT;
                else if (ColorType == ItemColorTypes.OUTPUT_POINT)
                    return ItemColors.OUTPUT_POINT;
                else if (ColorType == ItemColorTypes.RAIL)
                    return ItemColors.RAIL;
                else if (ColorType == ItemColorTypes.CHILD_SHUTTLE_CARRIED)
                    return ItemColors.CHILD_SHUTTLE_CARRIED;
                else if (ColorType == ItemColorTypes.CHILD_SHUTTLE_UNCARRY)
                    return ItemColors.CHILD_SHUTTLE_UNCARRY;
                else if (ColorType == ItemColorTypes.EMPTY_STORAGE)
                    return ItemColors.EMPTY_STORAGE;
                else if (ColorType == ItemColorTypes.FULL_STORAGE)
                    return ItemColors.FULL_STORAGE;
                else if (ColorType == ItemColorTypes.PARENT_SHUTTLE_CARRIED)
                    return ItemColors.PARENT_SHUTTLE_CARRIED;
                else if (ColorType == ItemColorTypes.PARENT_SHUTTLE_UNCARRY)
                    return ItemColors.PARENT_SHUTTLE_UNCARRY;
                else
                    return ItemColors.UNAVAILABLE;
            }
        }
        public Position Location { get; private set; }

        public MapItem(int w, int h, int tp, int lp, ItemTypes itemType, int layer, int rack, int column)
        {
            this.Width = w;
            this.Height = h;
            this.TopPad = tp;
            this.LeftPad = lp;
            this.Type = itemType;
            Location = new Position(layer, rack, column);
            InitialStroageInfo();
        }
        public MapItem()
        {
            //default excluding Map Class
            //not use this 
            //should initial in Map Class
            this.Width = 20;
            this.Height = 20;
            this.TopPad = 10;
            this.LeftPad = 10;
            this.Type = ItemTypes.NONE;
            Location = new Position();
            InitialStroageInfo();
        }

        #endregion


        #region storage properties
        public enum ItemTypes
        {
            NONE, EMPTY_STORAGE, FULL_STORAGE, RAIL, INPUT_POINT, OUTPUT_POINT,
            PARENT_SHUTTLE_UNCARRY, PARENT_SHUTTLE_CARRIED, CHILD_SHUTTLE_UNCARRY, CHILD_SHUTTLE_CARRIED,
            UNAVAILABLE
        }
        private ItemTypes _itemType = ItemTypes.NONE;
        public ItemTypes Type { get { return _itemType; }
            set {
                _itemType = value;
                switch (_itemType)
                {
                    case ItemTypes.NONE :
                        ColorType = ItemColorTypes.NONE;
                        break;
                    case ItemTypes.INPUT_POINT:
                        ColorType = ItemColorTypes.INPUT_POINT;
                        break;
                    case ItemTypes.OUTPUT_POINT:
                        ColorType = ItemColorTypes.OUTPUT_POINT;
                        break;
                    case ItemTypes.EMPTY_STORAGE:
                        ColorType = ItemColorTypes.EMPTY_STORAGE;
                        break;
                    case ItemTypes.FULL_STORAGE:
                        ColorType = ItemColorTypes.FULL_STORAGE;
                        break;
                    case ItemTypes.PARENT_SHUTTLE_UNCARRY:
                        ColorType = ItemColorTypes.PARENT_SHUTTLE_UNCARRY;
                        break;
                    case ItemTypes.PARENT_SHUTTLE_CARRIED:
                        ColorType = ItemColorTypes.PARENT_SHUTTLE_CARRIED;
                        break;
                    case ItemTypes.RAIL:
                        ColorType = ItemColorTypes.RAIL;
                        break;
                    case ItemTypes.CHILD_SHUTTLE_UNCARRY:
                        ColorType = ItemColorTypes.CHILD_SHUTTLE_UNCARRY;
                        break;
                    case ItemTypes.CHILD_SHUTTLE_CARRIED:
                        ColorType = ItemColorTypes.CHILD_SHUTTLE_CARRIED;
                        break;
                    case ItemTypes.UNAVAILABLE:
                        ColorType = ItemColorTypes.UNAVAILABLE;
                        break;
                }
            } }
        //Goods information
        public Goods Good { get; set; }
        public Dictionary<string, bool> AvailableGoodTypes { get; set; }//used for quick search of goods' types

        private void InitialStroageInfo()
        {
            this.Good = new Goods();
            AvailableGoodTypes = new Dictionary<string, bool>();
        }

        public void ResetAvailableGoodTypes()
        {
            //To Reset all goodstypes to false
            AvailableGoodTypes.Clear();
            for(int i= 0; i< Goods.GoodsTypes.Count; i++)
            {
                AvailableGoodTypes.Add(Goods.GoodsTypes[i], false);
            }
        }
        /// <summary>
        /// to make sure if the current map item's available good types is empty
        /// </summary>
        /// <returns></returns>
        public bool HasAvailableGoodTypes()
        {
            bool res = false;
            for (int i = 0; i < Goods.GoodsTypes.Count; i++)
            {
                res = res || AvailableGoodTypes[Goods.GoodsTypes[i]];
            }
            return res;
        }

        #endregion

    }
}
