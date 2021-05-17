using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class StaticGoodsTypesViewModels: BaseViewModels
    {
        private Models.Classes.Map _map = null;
        private ObservableCollection<string> _goodsTypes = null;
        public int _selectedGoodsTypesIndex = 0;

        public StaticGoodsTypesViewModels(Models.Classes.Map map)
        {
            this._map = map;
            GoodsTypes = new ObservableCollection<string>(map.GoodsTypes);
        }

        public ObservableCollection<string> GoodsTypes {
            get { return _goodsTypes; }
            set
            {
                _goodsTypes = value;
                OnPropertyChanged("GoodsTypes");
            }
        }
        public int SelectedGoodsTypesIndex
        {
            get { return _selectedGoodsTypesIndex; }
            set
            {
                _selectedGoodsTypesIndex = value;
                //some operations here

                OnPropertyChanged("SelectedGoodsTypesIndex");
            }
        }
    }
}
