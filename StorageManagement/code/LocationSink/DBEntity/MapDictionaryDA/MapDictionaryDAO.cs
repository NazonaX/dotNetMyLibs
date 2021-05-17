using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MapDictionaryDA
{
    public class MapDictionaryDAO : IMapDictionary
    {
        public void DeleteMapDictionary(List<MapDictionary> mapDictionaries)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(mapDictionaries);
                context.BulkSaveChanges();
            }
        }
        public List<MapDictionary> GetMapDictionary()
        {
            using(DevEntities context = new DevEntities())
            {
                return context.MapDictionary.ToList();
            }
        }
        public void InsertMapDictionary(List<MapDictionary> mapDictionaries)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkInsert(mapDictionaries);
                context.BulkSaveChanges();
            }
        }
        public void UpdateMapDictionary(List<MapDictionary> mapDictionaries)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkUpdate(mapDictionaries);
                context.BulkSaveChanges();
            }
        }

    }
}
