using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class MapItems
    {
        #region private property
        private DAL.MapItems _mapItem = new DAL.MapItems();
        
        #endregion

        public MapItems()
        {
            this.Status = MapItemStatus.STATUS_NOT_STORAGE;
        }

        #region public property
        public int MapItemID
        {
            get { return _mapItem.Id; }
            private set { _mapItem.Id = value; }
        }
        public int Layer
        {
            get { return _mapItem.MapItemLayer; }
            set { _mapItem.MapItemLayer = value; }
        }
        public int Rack
        {
            get { return _mapItem.MapItemRack; }
            set { _mapItem.MapItemRack = value; }
        }
        public int Column
        {
            get { return _mapItem.MapItemColumn; }
            set { _mapItem.MapItemColumn = value; }
        }
        public int CargowayId
        {
            get { return _mapItem.CargowayId; }
            set { _mapItem.CargowayId = value; }
        }
        public int ZoneId
        {
            get { return _mapItem.ZoneId; }
            set { _mapItem.ZoneId = value; }
        }
        public int TypeId
        {
            get { return _mapItem.TypeId; }
            set { _mapItem.TypeId = value; }
        }
        public MapItemStatus Status
        {
            get { return (MapItemStatus)_mapItem.Status; }
            set { _mapItem.Status = (int)value; }
        }
        #endregion

        #region method
        public void DAL_SetMapItem(DAL.MapItems mapitem)
        {
            this._mapItem = mapitem;
        }
        public DAL.MapItems DAL_GetMapItem()
        {
            return _mapItem;
        }

        #endregion

        public enum MapItemStatus
        {
            STATUS_EMPTY, STATUS_FULL, STATUS_LOCK, STATUS_NOT_STORAGE
        }


    }
}
