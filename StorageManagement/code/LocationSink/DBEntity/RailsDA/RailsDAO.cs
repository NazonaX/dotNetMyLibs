using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RailsDA
{
    public class RailsDAO : IRailsDA
    {
        public void DeleteRails(List<Rails> todel)
        {
            using(DAL.DevEntities context = new DevEntities())
            {
                context.BulkDelete(todel);
                context.BulkSaveChanges();
            }
        }

        public List<Rails> GetRails()
        {
            using(DAL.DevEntities context = new DevEntities())
            {
                return context.Rails.ToList();
            }
        }

        public void InsertRails(List<Rails> toadd)
        {
            using(DAL.DevEntities context = new DevEntities())
            {
                context.BulkInsert(toadd);
                context.BulkSaveChanges();
            }
        }

        public void UpdateRails(List<Rails> rails)
        {
            using(DAL.DevEntities context = new DevEntities())
            {
                context.BulkUpdate(rails);
                context.BulkSaveChanges();
            }
        }
    }
}
