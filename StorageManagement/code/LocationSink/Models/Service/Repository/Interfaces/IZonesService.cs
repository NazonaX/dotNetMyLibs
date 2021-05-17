using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface IZonesService
    {
        List<Entity.Zone> LoadZones();
        void DeleteZones(List<Entity.Zone> ZoneLsit);
        void InsertZones(List<Entity.Zone> ZoneList);
        void UpdateZones();
    }
}
