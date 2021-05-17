using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models.Entity;

namespace Models.Service.Repository
{
    public interface IMapDictionaryService
    {
        Entity.Map GetMap();

        void DeleteMap(Entity.Map Map);

        void InsertMap(Entity.Map Map);

        void UpdateMap(Entity.Map Map);

    }
}
