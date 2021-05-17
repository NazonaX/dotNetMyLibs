using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.TypesDA
{
    public interface ITypesDA
    {
        List<Types> GetTypes();
        void InsertTypes(List<Types> insertList);
        void UpdateTypes(List<Types> updateList);
        void DeleteTypes(List<Types> delList);

    }
}
