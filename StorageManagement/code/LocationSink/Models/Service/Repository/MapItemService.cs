using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Models.Service.Repository
{
    internal class MapItemService : IMapItemsService
    {
        private Entity.Map _map = null;
        private List<DAL.MapItems> _DAL_MapItemList = null;
        private List<DAL.MapItems> _DAL_SpecialMapItemList = null;

        public MapItemService(Entity.Map map)
        {
            this._map = map;
            this._DAL_MapItemList = new List<DAL.MapItems>();
            this._DAL_SpecialMapItemList = new List<DAL.MapItems>();
        }

        /// <summary>
        /// for load mapitems, as the map instance is just one reference in all repository services
        /// must be done after CargoWays Loading
        /// </summary>
        /// <returns></returns>
        public List<Entity.MapItems> LoadMapItems()
        {
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            List<DAL.MapItems> glist = mapItemDA.GetMapItems();
            List<Entity.MapItems> resList = new List<Entity.MapItems>();
            foreach (DAL.MapItems mi in glist)
            {
                Entity.MapItems tmp = new Entity.MapItems();
                tmp.DAL_SetMapItem(mi);
                resList.Add(tmp);
            }
            _map.MapItems = resList;
            //clear first for new lists
            _DAL_MapItemList.Clear();
            _DAL_SpecialMapItemList.Clear();
            //add to _DAL_List
            _DAL_MapItemList = glist;
            //for map to to the flatten operation and 
            _map.FlattenMapItems(_DAL_MapItemList, _DAL_SpecialMapItemList);
            return resList;
        }
        public void InsertMapItems(List<Entity.MapItems> MapItemList)
        {
            List<DAL.MapItems> addition = new List<DAL.MapItems>();
            foreach(Entity.MapItems i in MapItemList)
            {
                addition.Add(i.DAL_GetMapItem());
            }
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.InsertMapItems(addition);
            _DAL_MapItemList.AddRange(addition);
            _map.MapItems.AddRange(MapItemList);
        }
        /// <summary>
        /// for special mapitem list deleting items as O(n^2)
        /// </summary>
        /// <param name="MapItemList"></param>
        public void DeleteSpecialMapItems(List<Entity.MapItems> MapItemList)
        {
            List<DAL.MapItems> todel = new List<DAL.MapItems>();
            foreach (Entity.MapItems i in MapItemList)
            {
                todel.Add(i.DAL_GetMapItem());
            }
            //delete database
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.DeleteMapItem(todel);
            //delete memory
            foreach (Entity.MapItems i in MapItemList)
            {
                _map.SpecialMapItems.Remove(i);
                _map.FastFinder.Remove(i.MapItemID);
                _DAL_SpecialMapItemList.Remove(i.DAL_GetMapItem());
            }
        }
        /// <summary>
        /// for special mapitem list insert
        /// </summary>
        /// <param name="MapItemList"></param>
        public void InserSpecialtMapItems(List<Entity.MapItems> MapItemList)
        {
            List<DAL.MapItems> insert = new List<DAL.MapItems>();
            foreach (Entity.MapItems mi in MapItemList)
            {
                insert.Add(mi.DAL_GetMapItem());
            }
            //insert into databse
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.InsertMapItems(insert);
            //insert into memory
            _map.SpecialMapItems.AddRange(MapItemList);
            _DAL_SpecialMapItemList.AddRange(insert);
            foreach(Entity.MapItems mi in MapItemList)
            {
                _map.FastFinder.Add(mi.MapItemID, mi);
            }
        }
        /// <summary>
        /// for special mapitem list update
        /// </summary>
        public void UpdateSpecialMapItems()
        {
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.UpdateMapItem(_DAL_SpecialMapItemList);
        }
        /// <summary>
        /// for deleting all mapitems
        /// </summary>
        public void DeleteAllMapItems()
        {
            //delete database
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.DeleteMapItem(_DAL_MapItemList);
            mapItemDA.DeleteMapItem(_DAL_SpecialMapItemList);
            //delete memory
            _map.MapItems = new List<Entity.MapItems>();
            _map.SpecialMapItems = new List<Entity.MapItems>();
            _map.FastFinder.Clear();
        }
        //TODO::update the real cargowaynumber
        public void UpdateAllMapItems()
        {
            foreach (Models.Entity.MapItems mi in _map.MapItems)
                mi.Status = Models.Service.MapSingletonService.Instance.GetMapItemStatus(mi);
            DAL.MapItemDA.IMapItemDA mapItemDA = new DAL.MapItemDA.MapItemDAO();
            mapItemDA.UpdateMapItem(_DAL_MapItemList);
            mapItemDA.UpdateMapItem(_DAL_SpecialMapItemList);
        }
    }
}
