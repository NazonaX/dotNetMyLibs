using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class RailsService : IRailsService
    {
        private Entity.Map _map = null;
        private List<DAL.Rails> _DAL_Rails = null;

        public RailsService(Models.Entity.Map _map)
        {
            this._map = _map;
            this._DAL_Rails = new List<DAL.Rails>();
        }

        public void DeleteRails(List<Rails> todel)
        {
            List<DAL.Rails> dal_todel = new List<DAL.Rails>();
            foreach(Models.Entity.Rails r in todel)
            {
                dal_todel.Add(r.DAL_GetRail());
            }
            //database
            DAL.RailsDA.IRailsDA railsDA = new DAL.RailsDA.RailsDAO();
            railsDA.DeleteRails(dal_todel);
            //memory
            foreach(Models.Entity.Rails r in todel)
            {
                _map.Rails.Remove(r);
                _DAL_Rails.Remove(r.DAL_GetRail());
            }
        }

        public void InsertRails(List<Rails> additions)
        {
            List<DAL.Rails> dal_add = new List<DAL.Rails>();
            foreach(Models.Entity.Rails r in additions)
            {
                dal_add.Add(r.DAL_GetRail());
            }
            //database
            DAL.RailsDA.IRailsDA railsDA = new DAL.RailsDA.RailsDAO();
            railsDA.InsertRails(dal_add);
            //memory
            _DAL_Rails.AddRange(dal_add);
            _map.Rails.AddRange(additions);
        }

        public List<Rails> LoadRails()
        {
            DAL.RailsDA.IRailsDA railsDA = new DAL.RailsDA.RailsDAO();
            List<DAL.Rails> dal_rails = railsDA.GetRails();
            List<Models.Entity.Rails> entity_rails = new List<Rails>();
            foreach(DAL.Rails r in dal_rails)
            {
                Models.Entity.Rails tmp = new Rails();
                tmp.DAL_SetRail(r);
                tmp.RailNumber = tmp.RailNumber.Trim();
                entity_rails.Add(tmp);
            }
            //memory
            _map.Rails = entity_rails;
            _DAL_Rails = dal_rails;
            return entity_rails;
        }

        public void UpdateRails()
        {
            DAL.RailsDA.IRailsDA railsDA = new DAL.RailsDA.RailsDAO();
            railsDA.UpdateRails(_DAL_Rails);
        }
    }
}
