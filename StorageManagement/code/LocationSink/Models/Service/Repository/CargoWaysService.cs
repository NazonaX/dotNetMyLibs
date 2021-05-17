using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class CargoWaysService : ICargoWaysService
    {
        private Entity.Map _map = null;
        private List<DAL.CargoWays> _DAL_CargoWays = null;

        public CargoWaysService(Entity.Map map)
        {
            this._map = map;
            _DAL_CargoWays = new List<DAL.CargoWays>();
        }
        /// <summary>
        /// O(n), normally the cargoways will not be deleted, except for recalculation
        /// </summary>
        public void DeleteAllCargoWays()
        {
            DAL.CargoWayDA.ICargoWaysDA cargoWaysDA = new DAL.CargoWayDA.CargoWaysDAO();
            //database
            cargoWaysDA.DeleteCargoWays(_DAL_CargoWays);
            //memory
            _DAL_CargoWays.Clear();
            _map.CargoWays.Clear();
        }
        /// <summary>
        /// O(n^2)
        /// </summary>
        /// <param name="delete"></param>
        public void DeleteCargoWays(List<CargoWays> delete)
        {
            List<DAL.CargoWays> todel = new List<DAL.CargoWays>();
            HashSet<DAL.CargoWays> has = new HashSet<DAL.CargoWays>();
            foreach (CargoWays c in delete)
            {
                //there may be multiple reference of the same DAL_instance, so we check here
                if (has.Contains(c.DAL_GetCargoWay()))
                    continue;
                todel.Add(c.DAL_GetCargoWay());
                has.Add(c.DAL_GetCargoWay());
            }
            DAL.CargoWayDA.ICargoWaysDA cargoWaysDA = new DAL.CargoWayDA.CargoWaysDAO();
            //database
            cargoWaysDA.DeleteCargoWays(todel);
            //memory
            foreach(Entity.CargoWays c in delete)
            {
                _DAL_CargoWays.Remove(c.DAL_GetCargoWay());
                _map.CargoWays.Remove(c);
            }
        }

        public void InsertCargoWays(List<CargoWays> addition)
        {
            List<DAL.CargoWays> toadd = new List<DAL.CargoWays>();
            HashSet<DAL.CargoWays> has = new HashSet<DAL.CargoWays>();
            foreach(Entity.CargoWays c in addition)
            {
                //there may be multiple reference of the same DAL_instance, so we check here
                if (has.Contains(c.DAL_GetCargoWay()))
                    continue;
                toadd.Add(c.DAL_GetCargoWay());
                has.Add(c.DAL_GetCargoWay());
            }
            DAL.CargoWayDA.ICargoWaysDA cargoWaysDA = new DAL.CargoWayDA.CargoWaysDAO();
            //database
            cargoWaysDA.InsertCargoWays(toadd);
            //memory
            _DAL_CargoWays.AddRange(toadd);
            _map.CargoWays.AddRange(addition);
        }
        /// <summary>
        /// remember mapitem and cargowaylock should be done after load cargoways
        /// </summary>
        /// <returns></returns>
        public List<CargoWays> LoadCargoWays()
        {
            DAL.CargoWayDA.ICargoWaysDA cargoWaysDA = new DAL.CargoWayDA.CargoWaysDAO();
            List<DAL.CargoWays> loaded = cargoWaysDA.GetCargoWays();
            List<Entity.CargoWays> res = new List<CargoWays>();
            foreach(DAL.CargoWays c in loaded)
            {
                Entity.CargoWays tmp = new CargoWays();
                tmp.DAL_SetCargoWay(c);
                tmp.CargoWayNumber = tmp.CargoWayNumber.Trim();
                res.Add(tmp);
            }
            _DAL_CargoWays = loaded;
            _map.CargoWays = res;
            return res;
        }
        /// <summary>
        /// remember to update mapitem and cargowaylock after updating cargoways
        /// </summary>
        public void UpdateCargoWays()
        {
            DAL.CargoWayDA.ICargoWaysDA cargoWaysDA = new DAL.CargoWayDA.CargoWaysDAO();
            cargoWaysDA.UpdateCargoWays(_DAL_CargoWays);
        }
    }
}
