using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RailsDA
{
    public interface IRailsDA
    {
        void InsertRails(List<DAL.Rails> toadd);
        void DeleteRails(List<DAL.Rails> todel);
        void UpdateRails(List<DAL.Rails> rails);
        List<Rails> GetRails();
    }
}
