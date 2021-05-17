using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ZoneDA
{
    public class ZoneDAO: IZoneDA
    {
        public void DeleteZone(List<Zone> Zones)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkDelete(Zones);
                context.BulkSaveChanges();
            }
        }
        public List<Zone> GetZone()
        {
            using (DevEntities context = new DevEntities())
            {
                return context.Zone.ToList();
            }
        }
        public void InsertZone(List<Zone> Zones)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkInsert(Zones);
                context.BulkSaveChanges();
            }
        }
        public void UpdateZone(List<Zone> Zones)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkUpdate(Zones);
                context.BulkSaveChanges();
            }
        }
    }
}
