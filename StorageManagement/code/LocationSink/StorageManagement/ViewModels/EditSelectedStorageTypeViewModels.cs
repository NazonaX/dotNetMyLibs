using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class EditSelectedStorageTypeViewModels : BaseViewModels
    {
        public delegate void RefreshCallBack();
        public delegate void SetStatusStringErasing(string str);

        private ObservableCollection<Models.Entity.Zone> _zones = new ObservableCollection<Models.Entity.Zone>();
        private int _selectedZoneIndex = -1;
        private bool _applyToAllLayers = false;
        private Window _self = null;
        private Models.Entity.Map _map = null;
        private List<SingleGridMapItemViewModels> _selectedMapItems = null;
        private RefreshCallBack _refreshCallBack = null;
        private SetStatusStringErasing _setMessageCallBack = null;

        public ObservableCollection<Models.Entity.Zone> Zones
        {
            get { return _zones; }
            set
            {
                _zones = value;
                OnPropertyChanged("Zones");
            }
        }
        public int SelectedZoneIndex
        {
            get { return _selectedZoneIndex; }
            set
            {
                _selectedZoneIndex = value;
            }
        }
        public bool ApplyToAllLayers
        {
            get { return _applyToAllLayers; }
            set { _applyToAllLayers = value; }
        }
        public DelegateCommand ExecuteSaveEditSelectedStorageZoneTypeCommand { get; private set; }

        public EditSelectedStorageTypeViewModels(Models.Entity.Map map, Window self, List<SingleGridMapItemViewModels> SelectedMapItemsss,
            RefreshCallBack refreshCallbackFunc, SetStatusStringErasing msgCallBackFunc)
        {
            this._map = map;
            this._self = self;
            this._selectedMapItems = SelectedMapItemsss;
            this._refreshCallBack = refreshCallbackFunc;
            this._setMessageCallBack = msgCallBackFunc;
            this.Zones.AddRange(this._map.Zones);
            if (this.Zones.Count != 0)
                SelectedZoneIndex = 0;
            ExecuteSaveEditSelectedStorageZoneTypeCommand = new DelegateCommand(ExecuteSaveEditSelectedStorageZoneTypeCommandDo);
        }

        private void ExecuteSaveEditSelectedStorageZoneTypeCommandDo()
        {
            if (SelectedZoneIndex == -1)
                return;
            int totalCount = 0;
            int setCount = 0;
            if (!ApplyToAllLayers)
            {
                List<SingleGridMapItemViewModels> tmp = _selectedMapItems.Where(mi => Models.Service.MapSingletonService.Instance.IsStorage(mi.SingleStorage)).ToList();
                foreach (SingleGridMapItemViewModels s in tmp)
                    s.SingleStorage.ZoneId = Zones[SelectedZoneIndex].Id;
                totalCount = _selectedMapItems.Count;
                setCount = tmp.Count;
            }
            else
            {
                totalCount = _selectedMapItems.Count * _map.LayerCount;
                //just search for mapitems excluding special ones
                foreach(SingleGridMapItemViewModels s in _selectedMapItems)
                {
                    List<Models.Entity.MapItems> tmp = _map.MapItems.Where(mi => mi.Rack == s.SingleStorage.Rack
                                                                            && mi.Column == s.SingleStorage.Column
                                                                            && Models.Service.MapSingletonService.Instance.IsStorage(mi)).ToList();
                    foreach (Models.Entity.MapItems i in tmp)
                        i.ZoneId = Zones[SelectedZoneIndex].Id;
                    setCount += tmp.Count;
                }
            }
            Models.Service.MapSingletonService.Instance.GetMapItemsService().UpdateAllMapItems();
            StringBuilder sb = new StringBuilder();
            sb.Append(setCount).Append(" MapItems set as Storage-Zone-").Append(Zones[SelectedZoneIndex].Name).Append(". ")
                .Append(totalCount - setCount).Append(" MapItems have been blocked as illegle items..");
            _setMessageCallBack(sb.ToString());
            _refreshCallBack();
            _self.Close();
        }
    }
}
