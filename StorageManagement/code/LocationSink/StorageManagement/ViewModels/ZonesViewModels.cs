using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    /// <summary>
    /// box Models.Entity.Zone as a view model to bind data
    /// used in StaticGoodsTypesViewModels
    /// </summary>
    public class ZonesViewModels: BaseViewModels
    {
        private Models.Entity.Zone _zone = null;

        public ZonesViewModels(Models.Entity.Zone zone)
        {
            this._zone = zone;
        }
        public Models.Entity.Zone Zone
        {
            get { return _zone; }
        }
        public int ZoneID
        {
            get { return _zone.Id; }
        }

        public string ZoneName
        {
            get { return _zone.Name; }
            set
            {
                _zone.Name = value;
                OnPropertyChanged("ZoneName");
            }
        }
    }
}
