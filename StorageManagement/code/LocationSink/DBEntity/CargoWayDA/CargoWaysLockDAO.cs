using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CargoWayDA
{
    public class CargoWaysLockDAO : ICargoWaysLockDA
    {
        public void DeleteCargoWaysLocks(List<CargoWaysLock> delete)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(delete);
                context.BulkSaveChanges();
            }
        }

        public List<CargoWaysLock> GetCargoWaysLocks()
        {
            using (DevEntities context = new DevEntities())
            {
                return context.CargoWaysLock.ToList();
            }
        }

        public void InsertCargoWaysLocks(List<CargoWaysLock> addition)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkInsert(addition);
                context.BulkSaveChanges();
            }
        }

        public void UpdateCargoWaysLocks(List<CargoWaysLock> update)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkUpdate(update);
                context.BulkSaveChanges();
            }
        }
    }
}
