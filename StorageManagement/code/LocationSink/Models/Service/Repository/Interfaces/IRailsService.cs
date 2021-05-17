using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface IRailsService
    {
        void InsertRails(List<Models.Entity.Rails> additions);
        void DeleteRails(List<Models.Entity.Rails> todel);
        void UpdateRails();
        List<Models.Entity.Rails> LoadRails();
    }
}
