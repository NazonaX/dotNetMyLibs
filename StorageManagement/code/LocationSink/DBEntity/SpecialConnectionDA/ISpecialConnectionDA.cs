using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SpecialConnectionDA
{
    public interface ISpecialConnectionDA
    {
        List<DAL.SpecialConnection> GetSpecialConnections();
        void InsertSpecialConnections(List<DAL.SpecialConnection> insert);
        void UpdateSpecialConnections(List<DAL.SpecialConnection> update);
        void DeleteSpecialConnections(List<DAL.SpecialConnection> delete);
    }
}
