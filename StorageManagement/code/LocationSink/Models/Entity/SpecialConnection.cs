using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class SpecialConnection
    {
        private DAL.SpecialConnection _specialConnection = new DAL.SpecialConnection();

        #region public properties
        public int Id
        {
            get { return _specialConnection.Id; }
        }
        public int MapItemFrom
        {
            get { return _specialConnection.MapItemFrom; }
            set { _specialConnection.MapItemFrom = value; }
        }
        public int MapItemTo
        {
            get { return _specialConnection.MapItemTo; }
            set { _specialConnection.MapItemTo = value; }
        }
        public Entity.MapItems MapItemFromEntity
        {
            get;set;
        }
        public Entity.MapItems MapItemToEntity
        {
            get;set;
        }
        /// <summary>
        /// set will cast double to float
        /// </summary>
        public float TimeCost
        {
            get { return _specialConnection.TimeCost; }
            set { _specialConnection.TimeCost = value; }
        }
        #endregion

        #region DAL getter and setter
        public DAL.SpecialConnection DAL_GetSpecialConnection()
        {
            return _specialConnection;
        }
        public void DAL_SetSpecialConnection(DAL.SpecialConnection s)
        {
            _specialConnection = s;
        }
        #endregion
    }
}
