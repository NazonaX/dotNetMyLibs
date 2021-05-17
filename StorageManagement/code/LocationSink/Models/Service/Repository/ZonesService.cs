using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    internal class ZonesService: IZonesService
    {
        private Entity.Map _map = null;
        private List<DAL.Zone> _DAL_Zone = null;


        public ZonesService(Entity.Map map)
        {
            this._map = map;
            _DAL_Zone = new List<DAL.Zone>();
        }

        public List<Entity.Zone> LoadZones()
        {
            DAL.ZoneDA.IZoneDA zoneDA = new DAL.ZoneDA.ZoneDAO();
            List<DAL.Zone> zlist = zoneDA.GetZone();
            List<Entity.Zone> resList = new List<Entity.Zone>();
            foreach (DAL.Zone z in zlist)
            {
                z.Name = z.Name.Trim();
                Entity.Zone ztmp = new Entity.Zone();
                ztmp.DAL_SetZone(z);
                resList.Add(ztmp);
                //consume the random color
                Entity.Zone.NextRandomColor();
            }
            _map.Zones = resList;
            _DAL_Zone = zlist;
            return resList;
        }
        public void DeleteZones(List<Entity.Zone> ZoneLsit)
        {
            List<DAL.Zone> todel = new List<DAL.Zone>();
            foreach(Entity.Zone z in ZoneLsit)
            {
                todel.Add(z.DAL_GetZone());
            }
            //delete database
            DAL.ZoneDA.IZoneDA zoneDA = new DAL.ZoneDA.ZoneDAO();
            zoneDA.DeleteZone(todel);
            //delete memory
            foreach (Entity.Zone z in ZoneLsit)
            {
                _map.Zones.Remove(z);
                _DAL_Zone.Remove(z.DAL_GetZone());
            }
        }
        public void UpdateZones()
        {
            DAL.ZoneDA.IZoneDA zoneDA = new DAL.ZoneDA.ZoneDAO();
            zoneDA.UpdateZone(_DAL_Zone);
        }
        public void InsertZones(List<Entity.Zone> ZoneList)
        {
            List<DAL.Zone> insert = new List<DAL.Zone>();
            foreach(Entity.Zone z in ZoneList)
            {
                insert.Add(z.DAL_GetZone());
            }
            //insert database
            DAL.ZoneDA.IZoneDA zoneDA = new DAL.ZoneDA.ZoneDAO();
            zoneDA.InsertZone(insert);
            //insert memory
            _map.Zones.AddRange(ZoneList);
            _DAL_Zone.AddRange(insert);
        }
    }
}
