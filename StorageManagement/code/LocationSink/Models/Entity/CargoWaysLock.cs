using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class CargoWaysLock
    {
        private DAL.CargoWaysLock _cargowayslock = new DAL.CargoWaysLock();

        //need to notice the cargowaynumber while using repository
        public int CargoWayId
        {
            get { return _cargowayslock.CargoWayId; }
            set { _cargowayslock.CargoWayId = value; }
        }
        public int LockStart
        {
            get { return _cargowayslock.LockStart; }
            set { _cargowayslock.LockStart = value; }
        }
        public int LockEnd
        {
            get { return _cargowayslock.LockEnd; }
            set { _cargowayslock.LockEnd = value; }
        }
        public int InPointMapItemId
        {
            get { return _cargowayslock.InPointMapItemId; }
            set { _cargowayslock.InPointMapItemId = value; }
        }
        public int RailColumn//RailColumn
        {
            get { return _cargowayslock.RailColumn; }
            set { _cargowayslock.RailColumn = value; }
        }
        public int LayerAt
        {
            get { return _cargowayslock.LayerAt; }
            set { _cargowayslock.LayerAt = value; }
        }
        public int RackAt
        {
            get { return _cargowayslock.RackAt; }
            set { _cargowayslock.RackAt = value; }
        }
        public int Id { get { return _cargowayslock.Id; } }
        #region methods
        public void DAL_SetCargoWaysLock(DAL.CargoWaysLock cargowayslock)
        {
            this._cargowayslock = cargowayslock;
        }
        public DAL.CargoWaysLock DAL_GetCargWaysLock()
        {
            return this._cargowayslock;
        }

        #endregion
    }
}
