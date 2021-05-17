using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class Goods
    {
        #region private property
        private DAL.Goods _good = new DAL.Goods();
        #endregion

        #region public property
        public int Id
        {
            get { return _good.Id; }
            private set { _good.Id = value; }
        }
        public string Name
        {
            get { return _good.Name; }
            set { _good.Name = value; }
        }
        public string Model
        {
            get { return _good.Model; }
            set { _good.Model = value; }
        }
        public string Batch
        {
            get { return _good.Batch; }
            set { _good.Batch = value; }
        }
        public string Model_Batch
        {
            get { return Model + "_" + Batch; }
        }
        public int Count
        {
            get { return _good.Count; }
            set { _good.Count = value; }
        }
        public string ProductId
        {
            get { return _good.ProductId; }
            set { _good.ProductId = value; }
        }
        public string BarCode
        {
            get { return _good.BarCode; }
            set { _good.BarCode = value; }
        }
        public int MapItemsId
        {
            get { return _good.MapItemsId; }
            set { _good.MapItemsId = value; }
        }
        public int CargoWayLockId
        {
            get { return _good.CargoWayLockId; }
            set { _good.CargoWayLockId = value; }
        }
        #endregion

        #region method
        public void DAL_SetGood(DAL.Goods good)
        {
            this._good = good;
        }
        public DAL.Goods DAL_GetGood()
        {
            return _good;
        }
        #endregion

    }
}
