using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class StaticZonesViewModels : BaseViewModels
    {
        private Models.Entity.Map _map = null;
        private Window _self = null;
        private List<Models.Entity.Zone> _deleteZones = new List<Models.Entity.Zone>();
        private ObservableCollection<ZonesViewModels> _types = null;
        private int _selectedGoodsTypesIndex = 0;
        private string _textBoxString = "";
        private string _textColorString = "";
        public DelegateCommand ExecuteEditGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteDeleteGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteSaveAllCommand { get; private set; }

        public StaticZonesViewModels(Models.Entity.Map map, Window selfWindow)
        {
            ExecuteEditGoodsTypesCommand = new DelegateCommand(ExecuteEditGoodsTypesCommandDo);
            ExecuteDeleteGoodsTypesCommand = new DelegateCommand(ExecuteDeleteGoodsTypesCommandDo, CanExecuteDeleteGoodsTypesCommandDo);
            ExecuteSaveAllCommand = new DelegateCommand(ExecuteSaveAllCommandDo, CanExecuteSaveAllCommandDo);
            this._map = map;
            this._self = selfWindow;
            ZoneTypes = new ObservableCollection<ZonesViewModels>();
            foreach (Models.Entity.Zone z in _map.Zones)
            {
                ZoneTypes.Add(new ZonesViewModels(z));
            }
            Inti();
        }

        public ObservableCollection<ZonesViewModels> ZoneTypes
        {
            get { return _types; }
            set
            {
                _types = value;
                OnPropertyChanged("ZoneTypes");//GoodsTypes
            }
        }
        public int SelectedGoodsTypesIndex
        {
            get { return _selectedGoodsTypesIndex; }
            set
            {
                _selectedGoodsTypesIndex = value;
                //some operations here
                if (_selectedGoodsTypesIndex != -1)
                {
                    TextBoxString = ZoneTypes[_selectedGoodsTypesIndex].ZoneName;
                    TextColorString= ZoneTypes[_selectedGoodsTypesIndex].Zone.Color;
                }
                else
                {
                    TextBoxString = "";
                    TextColorString = "";
                }

                ExecuteEditGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodsTypesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedGoodsTypesIndex");
            }
        }
        public string TextBoxString
        {
            get { return _textBoxString; }
            set
            {
                _textBoxString = value;
                ExecuteEditGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodsTypesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("TextBoxString");
            }
        }
        public string TextColorString
        {
            get { return _textColorString; }
            set
            {
                _textColorString = value;
                ExecuteEditGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodsTypesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("TextColorString");
            }
        }
        /// <summary>
        /// 操作完默认初始化第一条数据
        /// </summary>
        public void Inti()
        {
            _selectedGoodsTypesIndex = 0;
            TextBoxString = ZoneTypes[SelectedGoodsTypesIndex].ZoneName;
            TextColorString = ZoneTypes[SelectedGoodsTypesIndex].Zone.Color;
        }
        #region Commands
        private void ExecuteEditGoodsTypesCommandDo()
        {
            ZoneTypes[SelectedGoodsTypesIndex].ZoneName = TextBoxString;
            ZoneTypes[SelectedGoodsTypesIndex].Zone.Color = TextColorString;
            ExecuteSaveAllCommand.RaiseCanExecuteChanged();
            MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.GoodsTypes_BT_Modify);
        }
        private void ExecuteDeleteGoodsTypesCommandDo()
        {
            _deleteZones.Add(ZoneTypes[SelectedGoodsTypesIndex].Zone);
            ZoneTypes.RemoveAt(SelectedGoodsTypesIndex);
            SelectedGoodsTypesIndex = -1;
            ExecuteSaveAllCommand.RaiseCanExecuteChanged();
            Inti();
        }
        private bool CanExecuteDeleteGoodsTypesCommandDo()
        {
            if (SelectedGoodsTypesIndex == -1)
                return false;
            else
                return true;
        }
        private void ExecuteSaveAllCommandDo()
        {
            using (TransactionScope trans = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.GetZonesService().UpdateZones();
                //need to set all deleted zones' id of certain mapitems
                foreach(Models.Entity.Zone z in _deleteZones)
                {
                    List<Models.Entity.MapItems> delitems = _map.MapItems.Where(mi => mi.ZoneId == z.Id).ToList();
                    foreach(Models.Entity.MapItems mi in delitems)
                    {
                        mi.ZoneId = 0;
                    }
                }
                mapSingletonService.GetMapItemsService().UpdateAllMapItems();
                mapSingletonService.GetZonesService().DeleteZones(_deleteZones);
                trans.Complete();
                MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.ModifySelectedMapItem_GoodsTypes_Complete);
                if (confirmToDel == MessageBoxResult.OK)
                    _self.Close();
            }
        }
        private bool CanExecuteSaveAllCommandDo()
        {
            //check if the GoodsTypes are changed or not
            return true;
        }

        #endregion
    }
}
