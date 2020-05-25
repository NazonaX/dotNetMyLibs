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
    public class ModifySelectedStorageViewModels2: BaseViewModels
    {
        private List<SingleGridMapItemViewModels> SelectedStorages = null;
        protected Models.Classes.Map _map = null;
        protected Window _self = null;
        protected string _orderCode = "";
        protected string _goodName = "";
        protected string _goodCode = "";
        protected string _goodSpecification = "";
        protected int _goodCount = 0;
        protected ObservableCollection<string> _goodTypesList = null;
        protected int _selectedGoodType = 0;


        public DelegateCommand ExecuteModifySelectedStorageCommand { get; protected set; }

        public ModifySelectedStorageViewModels2(Models.Classes.Map map, List<SingleGridMapItemViewModels> selectedList,
            Window self)
        {
            this.SelectedStorages = selectedList;
            this._map = map;
            this._self = self;
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
            //add all available good type of selected storages
            Dictionary<string, bool> tmp = new Dictionary<string, bool>();
            for (int i = 0; i < _map.GoodsTypes.Count; i++)
                tmp.Add(_map.GoodsTypes[i], false);
            for(int i = 0; i < SelectedStorages.Count; i++)
                for (int k = 0; k < _map.GoodsTypes.Count; k++)
                    tmp[_map.GoodsTypes[k]] = SelectedStorages[i].SingleStorage.AvailableGoodTypes[_map.GoodsTypes[k]];
            GoodTypesList = new ObservableCollection<string>();
            for (int i = 0; i < _map.GoodsTypes.Count; i++)
            {
                if (tmp[_map.GoodsTypes[i]])
                    GoodTypesList.Add(_map.GoodsTypes[i]);
            }
            GoodTypesList.Insert(0, Localiztion.Resource.MapItems_PleaseSelect);
            SelectedGoodType = 0;
        }
        public ModifySelectedStorageViewModels2(Models.Classes.Map map, Window self)
        {
            this._self = self;
            this._map = map;
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
        }

        #region getters and setters
        public string OrderCode
        {
            get { return _orderCode; }
            set
            {
                _orderCode = value;
                OnPropertyChanged("OrderCode");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }
        public string GoodName
        {
            get { return _goodName; }
            set
            {
                _goodName = value;
                OnPropertyChanged("GoodName");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }
        public string GoodCode
        {
            get { return _goodCode; }
            set
            {
                _goodCode = value;
                OnPropertyChanged("GoodCode");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }
        public string GoodSpecification
        {
            get { return _goodSpecification; }
            set
            {
                _goodSpecification = value;
                OnPropertyChanged("GoodSpecification");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }
        public string GoodCount
        {
            get { return "" + _goodCount; }
            set
            {
                int outer = 0;
                if (Int32.TryParse(value, out outer))
                    _goodCount = outer;
                else
                    _goodCount = 0;
                OnPropertyChanged("GoodCount");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<string> GoodTypesList
        {
            get { return _goodTypesList; }
            set
            {
                _goodTypesList = value;
                OnPropertyChanged("GoodTypesList");
            }
        }
        public int SelectedGoodType
        {
            get { return _selectedGoodType; }
            set
            {
                if (value != 0 && GoodTypesList[0].Equals(Localiztion.Resource.MapItems_PleaseSelect))
                {
                    value--;
                    GoodTypesList.RemoveAt(0);
                }
                _selectedGoodType = value;
                OnPropertyChanged("SelectedGoodType");
                ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
        #region Commands
        virtual protected void ExecuteModifySelectedStorageCommandDo()
        {
            List<string> justOne = new List<string>();
            justOne.Add(GoodTypesList[SelectedGoodType]);
            Models.Classes.Goods good = new Models.Classes.Goods(GoodName, GoodCode, GoodSpecification, OrderCode, _goodCount, new List<string>());
            for(int i = 0; i < SelectedStorages.Count; i++)
            {
                if (SelectedStorages[i].SingleStorage.AvailableGoodTypes[GoodTypesList[SelectedGoodType]])
                {
                    SelectedStorages[i].SingleStorage.Good = good.Copy();
                    SelectedStorages[i].SingleStorage.Good.Types.AddRange(justOne);
                    SelectedStorages[i].Type = Models.Classes.MapItem.ItemTypes.FULL_STORAGE;
                }
            }
            _self.Close();
        }
        protected bool CanExecuteModifySelectedStorageCommandDo()
        {
            if (GoodTypesList[SelectedGoodType].Equals(Localiztion.Resource.MapItems_PleaseSelect) || _orderCode.Equals("") || _goodCode.Equals("")
                || _goodCount == 0 || _goodName.Equals("") || _goodSpecification.Equals(""))
                return false;
            else
                return true;
        }
        
        #endregion
    }
}
