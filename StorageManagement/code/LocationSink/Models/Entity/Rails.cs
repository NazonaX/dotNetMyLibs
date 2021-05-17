using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class Rails
    {
        private DAL.Rails _rail = new DAL.Rails();

        public int Id
        {
            get { return _rail.Id; }
        }
        public int RailColumn
        {
            get { return _rail.RailColumn; }
            set { _rail.RailColumn = value; }
        }
        public int RailLayer
        {
            get { return _rail.RailLayer; }
            set { _rail.RailLayer = value; }
        }
        public string RailNumber
        {
            get { return _rail.RailNumber; }
            set { _rail.RailNumber = value; }
        }

        #region public methods
        public DAL.Rails DAL_GetRail()
        {
            return _rail;
        }
        public void DAL_SetRail(DAL.Rails rail)
        {
            _rail = rail;
        }
        #endregion

    }
}
