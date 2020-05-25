using Models.Classes;
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
    public class ModifySelectedMapItemsViewModels: BaseViewModels
    {
        private List<SingleGridMapItemViewModels> SelectedMapItems = null;
        private Models.Classes.Map _map = null;
        private Window self = null;
        private int _selectedTypeIndex = 0;
        private ObservableCollection<string> types = new ObservableCollection<string>()
        {
            Localiztion.Resource.MapItems_PleaseSelect,
            Localiztion.Resource.MapItems_Unavalible,
            Localiztion.Resource.MapItems_Storage,
            Localiztion.Resource.MapItems_Railway,
            Localiztion.Resource.MapItems_Input,
            Localiztion.Resource.Mapitems_OutPut
        };
        public DelegateCommand ExecuteSaveModifySelectedMapItemCommand { get; private set; }
        public ObservableCollection<string> Types { get { return types; } }
        public bool ApplyToAllLayers { get; set; }

        public ModifySelectedMapItemsViewModels(List<SingleGridMapItemViewModels> SelectedMapItems,
            Models.Classes.Map map,
            Window selfWindow)
        {
            bool hasGoods = false;
            this.SelectedMapItems = SelectedMapItems;
            this.self = selfWindow;
            this._map = map;
            ExecuteSaveModifySelectedMapItemCommand = new DelegateCommand(ExecuteSaveModifySelectedMapItemCommandDo, CanExecuteSaveModifySelectedMapItemCommandDo);
            for(int i = 0; i < this.SelectedMapItems.Count; i++)
            {
                if (!this.SelectedMapItems[i].SingleStorage.Good.IsEmpty())
                {
                    hasGoods = true;
                    break;
                }
            }
            if (hasGoods)
            {
                MessageBox.Show(Localiztion.Resource.ModifySelectedMapItem_Msg_AlertForGoodsInfo);
            }
        }

        public int SelectedTypeIndex
        {
            get { return _selectedTypeIndex; }
            set
            {
                if (!Types[value].Equals(Localiztion.Resource.MapItems_PleaseSelect))
                {
                    Types.Remove(Localiztion.Resource.MapItems_PleaseSelect);
                    value--;
                }
                _selectedTypeIndex = value;
                OnPropertyChanged("SelectedTypeIndex");
                ExecuteSaveModifySelectedMapItemCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteSaveModifySelectedMapItemCommandDo()
        {
            if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_Storage))
            {
                //another window for modify goods types details
                wpfSimulation.Views.ModifyAvaliableGoodsTypesViewModels tw = new wpfSimulation.Views.ModifyAvaliableGoodsTypesViewModels();
                ModifyAvailableGoodsTypesViewModels vm = new ModifyAvailableGoodsTypesViewModels(_map,
                    SelectedMapItems,
                    ApplyToAllLayers,
                    ConfirmTheChanges,
                    tw);
                tw.Owner = self;
                tw.DataContext = vm;
                tw.ShowDialog();
            }
            else
                ConfirmTheChanges();
            self.Close();
        }
        private void ConfirmTheChanges()
        {
            for (int i = 0; i < SelectedMapItems.Count; i++)
            {
                if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_Unavalible))
                    FullfillAllLayers(SelectedMapItems[i], Models.Classes.MapItem.ItemTypes.UNAVAILABLE);
                else if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_Input))
                    FullfillAllLayers(SelectedMapItems[i], Models.Classes.MapItem.ItemTypes.INPUT_POINT);
                else if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.Mapitems_OutPut))
                    FullfillAllLayers(SelectedMapItems[i], Models.Classes.MapItem.ItemTypes.OUTPUT_POINT);
                else if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_Railway))
                    FullfillAllLayers(SelectedMapItems[i], Models.Classes.MapItem.ItemTypes.RAIL);
                else if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_Storage))
                    FullfillAllLayers(SelectedMapItems[i], Models.Classes.MapItem.ItemTypes.EMPTY_STORAGE);
            }
        }

        private void FullfillAllLayers(SingleGridMapItemViewModels item, MapItem.ItemTypes itemType)
        {
            if (!ApplyToAllLayers)
            {
                item.Type = itemType;
            }
            else
            {
                for (int i = 0; i < _map.LayerCount; i++)
                {
                    _map[i, item.SingleStorage.Location.Rack, item.SingleStorage.Location.Column].Type = itemType;
                }
            }
        }

        private bool CanExecuteSaveModifySelectedMapItemCommandDo()
        {
            if (Types[SelectedTypeIndex].Equals(Localiztion.Resource.MapItems_PleaseSelect))
                return false;
            else
                return true;
        }
    }
}
