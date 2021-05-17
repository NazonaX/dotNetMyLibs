using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class SpecialConnectionService : ISpecialConnectionService
    {
        private Entity.Map _map = new Entity.Map();
        List<DAL.SpecialConnection> _DAL_SpecialConnections = null;

        public SpecialConnectionService(Entity.Map map)
        {
            this._map = map;
            this._DAL_SpecialConnections = new List<DAL.SpecialConnection>();
        }

        #region implement methods
        public List<SpecialConnection> LoadSpecialConnections()
        {
            DAL.SpecialConnectionDA.ISpecialConnectionDA specialConnectionDA = new DAL.SpecialConnectionDA.SpecialConnectionDAO();
            List<DAL.SpecialConnection> tmp = specialConnectionDA.GetSpecialConnections();
            List<Entity.SpecialConnection> res = new List<SpecialConnection>();
            foreach(DAL.SpecialConnection sc in tmp)
            {
                Entity.SpecialConnection tsc = new SpecialConnection();
                tsc.DAL_SetSpecialConnection(sc);
                tsc.MapItemFromEntity = _map.FastFinder[sc.MapItemFrom];
                tsc.MapItemToEntity = _map.FastFinder[sc.MapItemTo];
                res.Add(tsc);
            }
            this._DAL_SpecialConnections = tmp;
            this._map.SpecialConnections = res;
            return res;
        }
        public void DeleteSpecialConnections(List<SpecialConnection> delete)
        {
            List<DAL.SpecialConnection> todel = new List<DAL.SpecialConnection>();
            foreach(Entity.SpecialConnection esc in delete)
            {
                todel.Add(esc.DAL_GetSpecialConnection());
            }
            //delete database
            DAL.SpecialConnectionDA.ISpecialConnectionDA specialConnectionDA = new DAL.SpecialConnectionDA.SpecialConnectionDAO();
            specialConnectionDA.DeleteSpecialConnections(todel);
            //delete memory
            foreach (Entity.SpecialConnection esc in delete)
            {
                _map.SpecialConnections.Remove(esc);
                _DAL_SpecialConnections.Remove(esc.DAL_GetSpecialConnection());
            }
        }
        public void InsertSpecialConnections(List<SpecialConnection> insert)
        {
            List<DAL.SpecialConnection> addition = new List<DAL.SpecialConnection>();
            foreach(Entity.SpecialConnection tsc in insert)
            {
                addition.Add(tsc.DAL_GetSpecialConnection());
                tsc.MapItemFromEntity = _map.FastFinder[tsc.MapItemFrom];
                tsc.MapItemToEntity = _map.FastFinder[tsc.MapItemTo];
            }
            //insert into database
            DAL.SpecialConnectionDA.ISpecialConnectionDA specialConnectionDA = new DAL.SpecialConnectionDA.SpecialConnectionDAO();
            specialConnectionDA.InsertSpecialConnections(addition);
            //insert into memory
            _map.SpecialConnections.AddRange(insert);
            _DAL_SpecialConnections.AddRange(addition);
        }
        public void UpdateSpecialConnections()
        {
            DAL.SpecialConnectionDA.ISpecialConnectionDA specialConnectionDA = new DAL.SpecialConnectionDA.SpecialConnectionDAO();
            specialConnectionDA.UpdateSpecialConnections(_DAL_SpecialConnections);
        }
        #endregion
    }
}
