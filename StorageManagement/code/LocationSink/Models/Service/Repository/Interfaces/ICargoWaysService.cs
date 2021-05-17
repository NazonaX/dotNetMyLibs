using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface ICargoWaysService
    {
        List<Entity.CargoWays> LoadCargoWays();
        void InsertCargoWays(List<Entity.CargoWays> addition);
        void UpdateCargoWays();
        void DeleteCargoWays(List<Entity.CargoWays> delete);
        void DeleteAllCargoWays();
    }
}
