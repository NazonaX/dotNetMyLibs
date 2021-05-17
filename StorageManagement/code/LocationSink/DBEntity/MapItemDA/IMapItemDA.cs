using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MapItemDA
{
    public interface IMapItemDA
    {
        List<MapItems> GetMapItems();
        void InsertMapItems(List<MapItems> MapItems);
        void UpdateMapItem(List<MapItems> MapItems);
        void DeleteMapItem(List<MapItems> MapItems);
    }
}
