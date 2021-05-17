using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface ISpecialConnectionService
    {
        List<Entity.SpecialConnection> LoadSpecialConnections();
        void DeleteSpecialConnections(List<Entity.SpecialConnection> delete);
        void InsertSpecialConnections(List<Entity.SpecialConnection> insert);
        void UpdateSpecialConnections();
    }
}
