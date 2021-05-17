using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SpecialConnectionDA
{
    public class SpecialConnectionDAO : ISpecialConnectionDA
    {
        public void DeleteSpecialConnections(List<SpecialConnection> delete)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkDelete(delete);
                context.BulkSaveChanges();
            }
        }

        public List<SpecialConnection> GetSpecialConnections()
        {
            using (DevEntities context = new DevEntities())
            {
                return context.SpecialConnection.ToList();
            }
        }

        public void InsertSpecialConnections(List<SpecialConnection> insert)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkInsert(insert);
                context.BulkSaveChanges();
            }
        }

        public void UpdateSpecialConnections(List<SpecialConnection> update)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkUpdate(update);
                context.BulkSaveChanges();
            }
        }
    }
}
