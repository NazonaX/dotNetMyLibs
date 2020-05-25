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
    public class ModifySelectedStorageViewModels: BaseViewModels
    {
        private List<SingleGridMapItemViewModels> SelectedStorages = null;
        private Models.Classes.Map _map = null;
        private Window _self = null;
        private string _orderCode = "";
        private string _goodName = "";
        private string _goodCode = "";
        private string _goodSpecification = "";
        private int _goodCount = 0;
        private ObservableCollection<string> _goodTypesList = null;
        private int _selectedGoodType = -1;
        private ObservableCollection<string> _setTypesList = null;
        private int _selectedSetType = -1;


        public DelegateCommand ExecuteModifySelectedStorageCommand { get; private set; }
        public DelegateCommand ExecuteAddGoodTypeCommand { get; private set; }
        public DelegateCommand ExecuteDeleteGoodTypeCommand { get; private set; }

        public ModifySelectedStorageViewModels(Models.Classes.Map map, List<SingleGridMapItemViewModels> selectedList,
            Window self)
        {
            this.SelectedStorages = selectedList;
            this._map = map;
            _self = self;
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
            ExecuteAddGoodTypeCommand = new DelegateCommand(ExecuteAddGoodTypeCommandDo, CanExecuteAddGoodTypeCommandDo);
            ExecuteDeleteGoodTypeCommand = new DelegateCommand(ExecuteDeleteGoodTypeCommandDo, CanExecuteDeleteGoodTypeCommandDo);
            GoodTypesList = new ObservableCollection<string>(_map.GoodsTypes);
            GoodTypesList.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            SetTypesList = new ObservableCollection<string>();
            SelectedGoodType = -1;
            SelectedSetType = -1;
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
                _selectedGoodType = value;
                OnPropertyChanged("SelectedGoodType");
                ExecuteAddGoodTypeCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodTypeCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<string> SetTypesList
        {
            get { return _setTypesList; }
            set
            {
                _setTypesList = value;
                OnPropertyChanged("SetTypesList");
            }
        }
        public int SelectedSetType
        {
            get { return _selectedSetType; }
            set
            {
                _selectedSetType = value;
                OnPropertyChanged("SelectedSetType");
                ExecuteAddGoodTypeCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodTypeCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion
        #region Commands
        private void ExecuteModifySelectedStorageCommandDo()
        {
            Models.Classes.Goods good = new Models.Classes.Goods(GoodName, GoodCode, GoodSpecification, OrderCode, _goodCount, SetTypesList.ToList());
            for(int i = 0; i < SelectedStorages.Count; i++)
            {
                SelectedStorages[i].SingleStorage.Good = good.Copy();
                SelectedStorages[i].Type = Models.Classes.MapItem.ItemTypes.FULL_STORAGE;
            }
            _self.Close();
        }
        private bool CanExecuteModifySelectedStorageCommandDo()
        {
            if (SetTypesList.Count == 0 || _orderCode.Equals("") || _goodCode.Equals("")
                || _goodCount == 0 || _goodName.Equals("") || _goodSpecification.Equals(""))
                return false;
            else
                return true;
        }
        private void ExecuteAddGoodTypeCommandDo()
        {
            if (!GoodTypesList[SelectedGoodType].Equals(Localiztion.Resource.GoodsTypes_LST_All))
            {
                SetTypesList.Add(GoodTypesList[SelectedGoodType]);
                GoodTypesList.RemoveAt(SelectedGoodType);
            }
            else
            {
                int counter = GoodTypesList.Count;
                for (int i = 1; i < counter; i++)
                {
                    SetTypesList.Add(GoodTypesList[i]);
                }
                for (int i = 1; i < counter; i++)
                {
                    GoodTypesList.RemoveAt(1);
                }
            }
            if (GoodTypesList.Count == 1 && GoodTypesList[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                GoodTypesList.RemoveAt(0);
            if (SetTypesList.Count > 0 && !SetTypesList[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                SetTypesList.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            SelectedGoodType = -1;
            ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteAddGoodTypeCommandDo()
        {
            if (SelectedGoodType == -1 || SelectedGoodType >= GoodTypesList.Count || SelectedGoodType < 0)
                return false;
            else
                return true;
        }
        private void ExecuteDeleteGoodTypeCommandDo()
        {
            if (!SetTypesList[SelectedSetType].Equals(Localiztion.Resource.GoodsTypes_LST_All))
            {
                GoodTypesList.Add(SetTypesList[SelectedSetType]);
                SetTypesList.RemoveAt(SelectedSetType);
            }
            else
            {
                int counter = SetTypesList.Count;
                for (int i = 1; i < counter; i++)
                {
                    GoodTypesList.Add(SetTypesList[i]);
                }
                for (int i = 1; i < counter; i++)
                {
                    SetTypesList.RemoveAt(1);
                }
            }
            if (SetTypesList.Count == 1 && SetTypesList[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                SetTypesList.RemoveAt(0);
            if (GoodTypesList.Count > 0 && !GoodTypesList[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                GoodTypesList.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            SelectedSetType = -1;
            ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteDeleteGoodTypeCommandDo()
        {
            if (SelectedSetType == -1 || SelectedSetType >= SetTypesList.Count || SelectedSetType < 0)
                return false;
            else
                return true;
        }
        #endregion
    }
}
