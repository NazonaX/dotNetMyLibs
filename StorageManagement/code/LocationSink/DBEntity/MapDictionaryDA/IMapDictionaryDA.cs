using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MapDictionaryDA
{
    public interface IMapDictionary
    {
        //All operations are needed to be batched
        List<MapDictionary> GetMapDictionary();
        void InsertMapDictionary(List<MapDictionary> mapDictionaries);
        void UpdateMapDictionary(List<MapDictionary> mapDictionaries);
        void DeleteMapDictionary(List<MapDictionary> mapDictionaries);
    }
}
