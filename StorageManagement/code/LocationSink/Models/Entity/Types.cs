using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class Types
    {
        private DAL.Types _types = new DAL.Types();

        public int Id
        {
            get { return _types.Id; }
            private set { _types.Id = value; }
        }
        public string Name
        {
            get { return _types.Name; }
            set { _types.Name = value; }
        }
        public string Color
        {
            get { return _types.Color; }
            set { _types.Color = value; }
        }

        #region methods
        public void DAL_SetTypes(DAL.Types t)
        {
            this._types = t;
        }
        public DAL.Types DAL_GetTypes()
        {
            return this._types;
        }
        #endregion
    }
}
