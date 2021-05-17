using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CargoWayDA
{
    public interface ICargoWaysDA
    {
        List<CargoWays> GetCargoWays();
        void InsertCargoWays(List<CargoWays> addition);
        void UpdateCargoWays(List<CargoWays> update);
        void DeleteCargoWays(List<CargoWays> delete);
    }
}
