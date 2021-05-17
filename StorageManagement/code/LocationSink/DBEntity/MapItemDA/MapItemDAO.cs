using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MapItemDA
{
    public class MapItemDAO : IMapItemDA
    {
        public void DeleteMapItem(List<MapItems> MapItems)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(MapItems);
                context.BulkSaveChanges();
            }
        }

        public List<MapItems> GetMapItems()
        {
            using (DevEntities context = new DevEntities())
            {
                return context.MapItems.ToList();
            }
        }

        public void InsertMapItems(List<MapItems> MapItems)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkInsert(MapItems);
                context.BulkSaveChanges();
            }
        }

        public void UpdateMapItem(List<MapItems> MapItems)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkUpdate(MapItems);
                context.BulkSaveChanges();
            }
        }
    }
}
