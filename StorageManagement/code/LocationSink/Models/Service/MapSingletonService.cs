using Models.Entity;
using Models.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Models.Service
{
    public class MapSingletonService: IMapSingletonService
    {
        #region private property
        private static MapSingletonService _singleton = null;
        private Repository.IMapDictionaryService mapDictionaryService = new Repository.MapDictionaryService();
        private Repository.IMapItemsService mapItemsService = null;
        private Repository.IGoodsService goodsService = null;
        private Repository.IZonesService zonesService = null;
        private Repository.ITypesService typesService = null;
        private Repository.ISpecialConnectionService specialConnectionService = null;
        private Repository.ICargoWaysService cargoWaysService = null;
        private Repository.ICargoWaysLockService cargoWaysLockService = null;
        public Repository.IRailsService railsService = null;
        private IMapLogicsService mapLogicsService = null;
        //private IMapAlgorithmService algorithmService = null;


        private Entity.Map _map = null;//may be null
        private Entity.Types _default_storage = null;
        #endregion

        #region public property

        public static IMapSingletonService Instance
        {
            get
            {
                if (_singleton == null)
                    _singleton = new MapSingletonService();
                return _singleton;
            }
        }
        public Entity.Map Map
        {
            get { return _map; }
            private set { _map = value; }
        }
        public Repository.IGoodsService GoodsService
        {
            get { return goodsService; }
            private set { goodsService = value; }
        }
        public Repository.IMapItemsService MapItemsService
        {
            get { return mapItemsService; }
            private set { mapItemsService = value; }
        }
        public Repository.IZonesService ZonesService
        {
            get { return zonesService; }
            private set { zonesService = value; }
        }
        public Repository.ITypesService TypesService
        {
            get { return typesService; }
            private set { typesService = value; }
        }
        public Repository.IMapDictionaryService MapDictionaryService
        {
            get { return mapDictionaryService; }
            private set { mapDictionaryService = value; }
        }
        public Repository.ISpecialConnectionService SpecialConnectionService
        {
            get { return specialConnectionService; }
            private set { specialConnectionService = value; }
        }
        public Repository.ICargoWaysService CargoWaysService
        {
            get { return cargoWaysService; }
            set { cargoWaysService = value; }
        }
        public Repository.ICargoWaysLockService CargoWaysLockService
        {
            get { return cargoWaysLockService; }
            set { cargoWaysLockService = value; }
        }
        public Repository.IRailsService RailsService
        {
            get { return railsService; }
            set { railsService = value; }
        }
        public IMapLogicsService MapLogicsService
        {
            get { return mapLogicsService; }
            set { this.mapLogicsService = value; }
        }
        //public IMapAlgorithmService MapAlgorithmService
        //{
        //    get { return algorithmService; }
        //    set { algorithmService = value; }
        //}
        #endregion


        #region methods for IMapSingletonService
        private MapSingletonService() {
            InitialMapInfos();
        }
        public IMapItemsService GetMapItemsService()
        {
            return this.MapItemsService;
        }
        public ITypesService GetTypesService()
        {
            return this.TypesService;
        }
        public IZonesService GetZonesService()
        {
            return this.ZonesService;
        }
        public IGoodsService GetGoodsService()
        {
            return this.GoodsService;
        }
        public ISpecialConnectionService GetSpecialConnectionService()
        {
            return this.SpecialConnectionService;
        }
        public ICargoWaysService GetCargoWaysService()
        {
            return CargoWaysService;
        }
        public ICargoWaysLockService GetCargoWaysLockService()
        {
            return CargoWaysLockService;
        }
        public IRailsService GetRailsService()
        {
            return RailsService;
        }
        public IMapLogicsService GetMapLogicsService()
        {
            return MapLogicsService;
        }
        //public IMapAlgorithmService GetMapAlgorithmService()
        //{
        //    return MapAlgorithmService;
        //}

        private void InitialMapInfos()
        {
            Map = MapDictionaryService.GetMap();
            if(Map != null)
            {
                //initial the services, for Map as one reference
                //should do load in this certain sequence
                this.MapDictionaryService = new Repository.MapDictionaryService();
                InitialServices(Map);
                this.TypesService.LoadTypes();
                this.ZonesService.LoadZones();
                //we get cargoways first here, for the mapitems and cargolocks should take a reference of the cargoway
                this.CargoWaysService.LoadCargoWays();
                this.CargoWaysLockService.LoadCargoWaysLocks();
                this.MapItemsService.LoadMapItems();
                this.GoodsService.LoadGoods();
                this.RailsService.LoadRails();

                this.SpecialConnectionService.LoadSpecialConnections();
                this._default_storage = Map.Types.SingleOrDefault(t => t.Name == ItemTypesString.ITEM_TYPE_DEFAULT_STORAGE);
            }
        }
        /// <summary>
        /// use this method only when recreate a map or the map id the database is null
        /// </summary>
        /// <param name="MapName"></param>
        /// <param name="layers"></param>
        /// <param name="racks"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public Entity.Map CreateNewMap(string MapName, int layers, int racks, int columns)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                Map = new Entity.Map();
                Map.MapName = MapName;
                Map.RackCount = racks;
                Map.LayerCount = layers;
                Map.ColumnCount = columns;
                //first save map initial messages
                this.MapDictionaryService = new Repository.MapDictionaryService();
                this.MapDictionaryService.InsertMap(Map);
                //Reset Zones' static color
                Zone.ResetRrandomColor();
                //initial all services as the map reference for one
                InitialServices(Map);
                //create general types list
                List<Entity.Types> generalTypes = TypesService.LoadTypes();
                if (generalTypes.Count == 0)//if there has no types in the database, create new and save from code
                {
                    generalTypes = GetGeneralTypes();
                    this.TypesService.InsertTypes(generalTypes);
                }
                //ALERT!! must be constrainted here as the same name of all the GENERAL_TYPES, or will has logical error
                //ALERT!! you can modify and make contraint from "public class ItemTypesString" in this singleton class
                Entity.Types tt = Map.Types.Single(t => t.Name == ItemTypesString.ITEM_TYPE_DEFAULT_STORAGE);
                this._default_storage = tt;
                //then set mapitems, default type Id is zt's Type Id
                List<Entity.MapItems> additionMapItemList = new List<MapItems>();
                for (int i = 0; i < layers; i++)
                {
                    for (int j = 0; j < racks; j++)
                    {
                        for (int k = 0; k < columns; k++)
                        {
                            Entity.MapItems tmp = new Entity.MapItems();
                            DAL.MapItems DAL_tmp = new DAL.MapItems()
                            {
                                MapItemLayer = i,
                                MapItemRack = j,
                                MapItemColumn = k,
                                TypeId = tt.Id,
                                ZoneId = 0,
                                CargowayId = 0,
                                Status = (int)MapItems.MapItemStatus.STATUS_NOT_STORAGE
                            };
                            tmp.DAL_SetMapItem(DAL_tmp);
                            additionMapItemList.Add(tmp);
                        }
                    }
                }
                MapItemsService.InsertMapItems(additionMapItemList);
                scope.Complete();
            }
            return Map;
        }

        /// <summary>
        /// maybe no use of this method after rebuild from bottom to top
        /// </summary>
        public void SaveMap()
        {
            //just update all from all of the services
            this.MapDictionaryService.UpdateMap(this.Map);
            this.MapItemsService.UpdateAllMapItems();
            this.GoodsService.UpdateGoods();
            this.TypesService.UpdateTypes();
            this.ZonesService.UpdateZones();
            this.SpecialConnectionService.UpdateSpecialConnections();
            this.CargoWaysService.UpdateCargoWays();
            this.CargoWaysLockService.UpdateCargoWaysLocks();
            this.RailsService.UpdateRails();
        }

        /// <summary>
        /// delelte all infos including zones, goods and types
        /// </summary>
        public void DeleteMap()
        {
            using(TransactionScope scope = new TransactionScope())
            {
                //delete all in the database
                this.CargoWaysLockService.DeleteCargoWaysLocks(new List<Entity.CargoWaysLock>(this.Map.CargoWaysLocks));
                this.CargoWaysService.DeleteAllCargoWays();
                this.GoodsService.DeleteAllGoods();
                this.ZonesService.DeleteZones(new List<Entity.Zone>(this.Map.Zones));
                this.SpecialConnectionService.DeleteSpecialConnections(new List<SpecialConnection>(Map.SpecialConnections));
                this.MapItemsService.DeleteAllMapItems();
                this.TypesService.DeleteTypes(new List<Entity.Types>(this.Map.Types));
                this.MapDictionaryService.DeleteMap(this.Map);
                ResetServices();
                //set Map to null
                Map = null;
                scope.Complete();
            }
        }

        public Entity.Map GetMap()
        {
            return Map;
        }

        /// <summary>
        /// reload all infos from database
        /// </summary>
        /// <returns></returns>
        public Entity.Map RefreshMap()
        {
            InitialMapInfos();
            return Map;
        }

        #region some other methods for map operations

        /// <summary>
        /// the method is just used for initialize the based zone infos
        /// </summary>
        private List<Entity.Types> GetGeneralTypes()
        {
            List<Entity.Types> res = new List<Types>();
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_NONE, Color = ItemColors.NONE });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_RAIL, Color = ItemColors.RAIL });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_INPUT_POINT, Color = ItemColors.INPUT_POINT });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_OUTPUT_POINT, Color = ItemColors.OUTPUT_POINT });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_UNAVAILABLE, Color = ItemColors.UNAVAILABLE });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_LIFTER, Color = ItemColors.LIFTER });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_DEFAULT_STORAGE, Color = ItemColors.EMPTY_STORAGE });
            res.Add(new Types() { Name = ItemTypesString.ITEM_TYPE_STORAGE_RAIL, Color = ItemColors.STORAGE_RAIL });
            return res;
        }
        private void InitialServices(Entity.Map map)
        {
            this.MapItemsService = new Repository.MapItemService(map);
            this.GoodsService = new Repository.GoodsService(map);
            this.ZonesService = new Repository.ZonesService(map);
            this.TypesService = new Repository.TypesService(map);
            this.SpecialConnectionService = new Repository.SpecialConnectionService(map);
            this.CargoWaysService = new Repository.CargoWaysService(map);
            this.CargoWaysLockService = new Repository.CargoWaysLockService(map);
            this.RailsService = new Repository.RailsService(map);

            this.MapLogicsService = new MapLogicsService(map);
            //this.MapAlgorithmService = new MapAlgorithmService(map);
        }
        private void ResetServices()
        {
            this.MapDictionaryService = null;
            this.MapItemsService = null;
            this.TypesService = null;
            this.ZonesService = null;
            this.GoodsService = null;
            this.SpecialConnectionService = null;
            this.CargoWaysService = null;
            this.CargoWaysLockService = null;
            this.RailsService = null;

            this.MapLogicsService = null;
            //this.MapAlgorithmService = null;
        }
        /// <summary>
        /// Get color of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetColor(MapItems item)
        {
            if (item.TypeId == 0)
            {
                return ItemColors.UNAVAILABLE;
            }
            else
            {
                if (_default_storage == null)
                    throw new Exception("At Singleton.GetColor....Can not find DEFAULT_STORAGE Type, pelase check whether the database and code are the same.");

                if (item.TypeId != _default_storage.Id)//not a storage
                    return Map.Types.Single(t => t.Id == item.TypeId).Color;
                else if (item.ZoneId != 0)//default storage but has no zone
                    return Map.Zones.Single(z => z.Id == item.ZoneId).Color;
                else//storage with zone
                    return _default_storage.Color;
            }
        }
        /// <summary>
        /// to make sure if an item is a storage grid
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if is; false if not</returns>
        public bool IsStorage(MapItems item)
        {
            if (_default_storage == null)
                throw new Exception("At Singleton.IsStorage....Can not find DEFAULT_STORAGE Type, pelase check whether the database and code are the same.");
            return item.TypeId == _default_storage.Id;
        }
        public bool HasGood(MapItems item)
        {
            bool has = _map.Goods.Any(g => g.MapItemsId == item.MapItemID);
            return has;
        }
        public MapItems.MapItemStatus GetMapItemStatus(MapItems item)
        {
            if (item.TypeId != Type_GetStorageId())
                return MapItems.MapItemStatus.STATUS_NOT_STORAGE;
            if (_map.Goods.Any(g => g.MapItemsId == item.MapItemID))
                return MapItems.MapItemStatus.STATUS_FULL;
            else if (_map.CargoWaysLocks.Any(cwl => cwl.CargoWayId == item.CargowayId))
                return MapItems.MapItemStatus.STATUS_LOCK;
            else
                return MapItems.MapItemStatus.STATUS_EMPTY;
        }

        private int FindTypeId(string NAME)
        {
            return _map.Types.Single(t => t.Name == NAME).Id;
        }
        public int Type_GetInputId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_INPUT_POINT);
        }
        public int Type_GetOutputId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_OUTPUT_POINT);
        }
        public int Type_GetRailId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_RAIL);
        }
        public int Type_GetStorageId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_DEFAULT_STORAGE);
        }
        public int Type_GetLifterId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_LIFTER);
        }
        public int Type_GetNoneId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_NONE);
        }
        public int Type_GetUnavailableId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_UNAVAILABLE);
        }
        public int Type_GetStorageRailId()
        {
            return FindTypeId(ItemTypesString.ITEM_TYPE_STORAGE_RAIL);
        }
        #endregion

        #endregion

        #region public definations

        public HashSet<string> BaseDefine = new HashSet<string>()
        {
            ItemTypesString.ITEM_TYPE_INPUT_POINT,
            ItemTypesString.ITEM_TYPE_NONE,
            ItemTypesString.ITEM_TYPE_OUTPUT_POINT,
            ItemTypesString.ITEM_TYPE_RAIL,
            ItemTypesString.ITEM_TYPE_UNAVAILABLE,
            ItemTypesString.ITEM_TYPE_LIFTER,
            ItemTypesString.ITEM_TYPE_STORAGE_RAIL
        };
        public class ItemTypesString
        {
            public static string ITEM_TYPE_NONE = "None";
            public static string ITEM_TYPE_RAIL = "Rail";
            public static string ITEM_TYPE_STORAGE_RAIL = "Storage-Rail";
            public static string ITEM_TYPE_INPUT_POINT = "InPoint";
            public static string ITEM_TYPE_OUTPUT_POINT = "Outpoint";
            public static string ITEM_TYPE_UNAVAILABLE = "Unavailable";
            public static string ITEM_TYPE_LIFTER = "Lifter";
            public static string ITEM_TYPE_DEFAULT_STORAGE = "Storage";
        }
        public class ItemColors
        {
            public static string NONE = "#000000";//black
            public static string RAIL = "#acacac";//gray
            public static string STORAGE_RAIL = "#c2acac";//gray-adsuki
            public static string INPUT_POINT = "#e71d36";//bright red
            public static string OUTPUT_POINT = "#3C38F5";//bright blue
            public static string LIFTER = "#A258B4";//bright purple
            public static string UNAVAILABLE = "#ffffff";//white
            public static string EMPTY_STORAGE = "#50514f";//black gray
            public static string FULL_STORAGE = "#ff9f1c";//bright orange
            public static string PARENT_SHUTTLE_UNCARRY = "#e5989b";//gray pink
            public static string PARENT_SHUTTLE_CARRIED = "#b5838d";//grayer pink
            public static string CHILD_SHUTTLE_UNCARRY = "#9bc1bc";//gray green
            public static string CHILD_SHUTTLE_CARRIED = "#5ca4a9";//grayer green
        }
        #endregion


       

    }
}
