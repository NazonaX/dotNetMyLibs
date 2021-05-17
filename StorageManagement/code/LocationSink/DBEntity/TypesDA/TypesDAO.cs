using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.TypesDA
{
    public class TypesDAO : ITypesDA
    {
        public void DeleteTypes(List<Types> delList)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(delList);
                context.BulkSaveChanges();
            }
        }

        public List<Types> GetTypes()
        {
            using(DevEntities context = new DevEntities())
            {
                return context.Types.ToList();
            }
        }

        public void InsertTypes(List<Types> insertList)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkInsert(insertList);
                context.BulkSaveChanges();
            }
        }

        public void UpdateTypes(List<Types> updateList)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkUpdate(updateList);
                context.BulkSaveChanges();
            }
        }
    }
}
