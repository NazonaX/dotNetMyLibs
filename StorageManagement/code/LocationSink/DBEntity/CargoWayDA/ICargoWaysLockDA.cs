using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CargoWayDA
{
    public interface ICargoWaysLockDA
    {
        List<CargoWaysLock> GetCargoWaysLocks();
        void InsertCargoWaysLocks(List<CargoWaysLock> addition);
        void UpdateCargoWaysLocks(List<CargoWaysLock> update);
        void DeleteCargoWaysLocks(List<CargoWaysLock> delete);
    }
}
