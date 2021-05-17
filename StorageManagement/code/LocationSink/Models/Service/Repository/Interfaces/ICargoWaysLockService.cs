using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface ICargoWaysLockService
    {
        List<Entity.CargoWaysLock> LoadCargoWaysLocks();
        void InsertCargoWaysLocks(List<Entity.CargoWaysLock> addition);
        void DeleteCargoWaysLocks(List<Entity.CargoWaysLock> delete);
        void UpdateCargoWaysLocks();
    }
}
