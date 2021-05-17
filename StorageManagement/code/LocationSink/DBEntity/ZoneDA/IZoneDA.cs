using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ZoneDA
{
    public interface IZoneDA
    {
        List<Zone> GetZone();
        void InsertZone(List<Zone> Zones);
        void UpdateZone(List<Zone> Zones);
        void DeleteZone(List<Zone> Zones);
    }
}
