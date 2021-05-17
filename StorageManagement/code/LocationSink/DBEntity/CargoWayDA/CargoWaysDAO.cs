using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CargoWayDA
{
    public class CargoWaysDAO : ICargoWaysDA
    {
        public void DeleteCargoWays(List<CargoWays> delete)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(delete);
                context.BulkSaveChanges();
            }
        }

        public List<CargoWays> GetCargoWays()
        {
            using(DevEntities context = new DevEntities())
            {
                return context.CargoWays.ToList();
            }
        }

        public void InsertCargoWays(List<CargoWays> addition)
        {
            using (DevEntities context = new DevEntities())
            {
                context.BulkInsert(addition);
                context.BulkSaveChanges();
            }
        }

        public void UpdateCargoWays(List<CargoWays> update)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkUpdate(update);
                context.BulkSaveChanges();
            }
        }
    }
}
