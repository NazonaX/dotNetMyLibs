using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface ITypesService
    {
        List<Entity.Types> LoadTypes();
        void DeleteTypes(List<Entity.Types> detList);
        void InsertTypes(List<Entity.Types> addList);
        void UpdateTypes();
    }
}
