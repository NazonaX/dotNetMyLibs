using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface IMapItemsService
    {
        List<Entity.MapItems> LoadMapItems();
        void InsertMapItems(List<Entity.MapItems> MapItemList);
        void DeleteSpecialMapItems(List<Entity.MapItems> MapItemList);
        void InserSpecialtMapItems(List<Entity.MapItems> MapItemList);
        void UpdateSpecialMapItems();
        void DeleteAllMapItems();
        void UpdateAllMapItems();

    }
}
