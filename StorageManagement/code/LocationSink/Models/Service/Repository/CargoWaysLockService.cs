using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class CargoWaysLockService : ICargoWaysLockService
    {
        private Entity.Map _map = null;
        private List<DAL.CargoWaysLock> _DAL_CargoWaysLocks = null;

        public CargoWaysLockService(Entity.Map map)
        {
            this._map = map;
            this._DAL_CargoWaysLocks = new List<DAL.CargoWaysLock>();
        }
        /// <summary>
        /// O(n^2)
        /// </summary>
        /// <param name="delete"></param>
        public void DeleteCargoWaysLocks(List<CargoWaysLock> delete)
        {
            List<DAL.CargoWaysLock> todel = new List<DAL.CargoWaysLock>();
            foreach(Entity.CargoWaysLock cl in delete)
            {
                todel.Add(cl.DAL_GetCargWaysLock());
            }
            DAL.CargoWayDA.ICargoWaysLockDA cargoWaysLockDA = new DAL.CargoWayDA.CargoWaysLockDAO();
            //database
            cargoWaysLockDA.DeleteCargoWaysLocks(todel);
            //memory
            foreach(Entity.CargoWaysLock cl in delete)
            {
                _DAL_CargoWaysLocks.Remove(cl.DAL_GetCargWaysLock());
                _map.CargoWaysLocks.Remove(cl);
            }
        }

        public void InsertCargoWaysLocks(List<CargoWaysLock> addition)
        {
            List<DAL.CargoWaysLock> toadd = new List<DAL.CargoWaysLock>();
            foreach(Entity.CargoWaysLock cl in addition)
            {
                //for assign real cargowaynumber of cargoway in the cargowaylock to cargowaylock's cargowaynumber
                toadd.Add(cl.DAL_GetCargWaysLock());
            }
            DAL.CargoWayDA.ICargoWaysLockDA cargoWaysLockDA = new DAL.CargoWayDA.CargoWaysLockDAO();
            //database
            cargoWaysLockDA.InsertCargoWaysLocks(toadd);
            //memory
            _DAL_CargoWaysLocks.AddRange(toadd);
            _map.CargoWaysLocks.AddRange(addition);
        }
        /// <summary>
        /// O(m*n), must be loaded after CargoWays loading
        /// </summary>
        /// <returns></returns>
        public List<CargoWaysLock> LoadCargoWaysLocks()
        {
            DAL.CargoWayDA.ICargoWaysLockDA cargoWaysLockDA = new DAL.CargoWayDA.CargoWaysLockDAO();
            List<DAL.CargoWaysLock> loaded = cargoWaysLockDA.GetCargoWaysLocks();
            List<Entity.CargoWaysLock> res = new List<CargoWaysLock>();
            foreach(DAL.CargoWaysLock cl in loaded)
            {
                Entity.CargoWaysLock tmp = new CargoWaysLock();
                tmp.DAL_SetCargoWaysLock(cl);
                res.Add(tmp);
            }
            _DAL_CargoWaysLocks = loaded;
            _map.CargoWaysLocks= res;
            return res;
        }

        public void UpdateCargoWaysLocks()
        {
            DAL.CargoWayDA.ICargoWaysLockDA cargoWaysLockDA = new DAL.CargoWayDA.CargoWaysLockDAO();
            cargoWaysLockDA.UpdateCargoWaysLocks(_DAL_CargoWaysLocks);
        }
    }
}
