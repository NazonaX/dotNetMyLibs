using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class ModifyAvailableGoodsTypesViewModels: BaseViewModels
    {
        public delegate void CallBackHandler();

        private Models.Classes.Map _map = null;
        private Window self = null;
        private bool isApplyToAllLayers = false;
        private List<SingleGridMapItemViewModels> selectedMapItems = null;
        private ObservableCollection<string> _goodsTypes = null;
        private ObservableCollection<string> _availableTypes = null;
        private int _selectedGoodsTypeIndex = -1;
        private int _selectedAvailableGoodsTypeIndex = -1;
        private CallBackHandler CallBackFunction;
        private bool Saved = false;
        private bool FromClosing = false;

        public DelegateCommand ExecuteAddAvailableGoodsTypes { get; private set; }
        public DelegateCommand ExecuteDeleteAvailableGoodsTypes { get; private set; }
        public DelegateCommand ExecuteConfirm { get; private set; }
        public DelegateCommand<Models.ExParameters> ExecuteClosingCommand { get; private set; }

        public ModifyAvailableGoodsTypesViewModels(Models.Classes.Map map, 
            List<SingleGridMapItemViewModels> selectedMapItems,
            bool isApplyToAllLayers,
            Action callback,
            Window selfWindow)
        {
            this._map = map;
            this.self = selfWindow;
            this.isApplyToAllLayers = isApplyToAllLayers;
            this.selectedMapItems = selectedMapItems;
            this.CallBackFunction = new CallBackHandler(callback);
            //initial Commands
            ExecuteAddAvailableGoodsTypes = new DelegateCommand(ExecuteAddAvailableGoodsTypesDo, CanExecuteAddAvailableGoodsTypesDo);
            ExecuteDeleteAvailableGoodsTypes = new DelegateCommand(ExecuteDeleteAvailableGoodsTypesDo, CanExecuteDeleteAvailableGoodsTypesDo);
            ExecuteConfirm = new DelegateCommand(ExecuteConfirmDo, CanExecuteConfirmDo);
            ExecuteClosingCommand = new DelegateCommand<Models.ExParameters>(ExecuteClosingCommandDo);
            //intitial ObservableCollections
            GoodsTypes = new ObservableCollection<string>(_map.GoodsTypes);
            GoodsTypes.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            AvailableGoodsTypes = new ObservableCollection<string>();
            SelectedAvailableGoodsType = -1;
            SelectedGoodsType = -1;
        }

        #region setters & getters
        public ObservableCollection<string> GoodsTypes
        {
            get { return _goodsTypes; }
            set
            {
                _goodsTypes = value;
                OnPropertyChanged("GoodsTypes");
            }
        }
        public int SelectedGoodsType
        {
            get { return _selectedGoodsTypeIndex; }
            set
            {
                _selectedGoodsTypeIndex = value;
                OnPropertyChanged("SelectedGoodsType");
                ExecuteAddAvailableGoodsTypes.RaiseCanExecuteChanged();
                ExecuteDeleteAvailableGoodsTypes.RaiseCanExecuteChanged();
                ExecuteConfirm.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<string> AvailableGoodsTypes
        {
            get { return _availableTypes; }
            set
            {
                _availableTypes = value;
                OnPropertyChanged("AvailableGoodsTypes");
            }
        }
        public int SelectedAvailableGoodsType
        {
            get { return _selectedAvailableGoodsTypeIndex; }
            set
            {
                _selectedAvailableGoodsTypeIndex = value;
                OnPropertyChanged("SelectedAvailableGoodsType");
                ExecuteAddAvailableGoodsTypes.RaiseCanExecuteChanged();
                ExecuteDeleteAvailableGoodsTypes.RaiseCanExecuteChanged();
                ExecuteConfirm.RaiseCanExecuteChanged();
            }
        }
        #endregion
        #region Commands
        public void ExecuteAddAvailableGoodsTypesDo()
        {
            if (!GoodsTypes[SelectedGoodsType].Equals(Localiztion.Resource.GoodsTypes_LST_All)){
                AvailableGoodsTypes.Add(GoodsTypes[SelectedGoodsType]);
                GoodsTypes.RemoveAt(SelectedGoodsType);
            }
            else
            {
                int counter = GoodsTypes.Count;
                for (int i = 1; i < counter; i++)
                {
                    AvailableGoodsTypes.Add(GoodsTypes[i]);
                }
                for (int i = 1; i < counter; i++)
                {
                    GoodsTypes.RemoveAt(1);
                }
            }
            if (GoodsTypes.Count == 1 && GoodsTypes[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                GoodsTypes.RemoveAt(0);
            if (AvailableGoodsTypes.Count > 0 && !AvailableGoodsTypes[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                AvailableGoodsTypes.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            SelectedGoodsType = -1;
            Saved = false;
        }
        public bool CanExecuteAddAvailableGoodsTypesDo()
        {
            if (SelectedGoodsType == -1 || SelectedGoodsType >= GoodsTypes.Count || SelectedGoodsType < 0)
                return false;
            else
                return true;
        }
        public void ExecuteDeleteAvailableGoodsTypesDo()
        {
            if (!AvailableGoodsTypes[SelectedAvailableGoodsType].Equals(Localiztion.Resource.GoodsTypes_LST_All))
            {
                GoodsTypes.Add(AvailableGoodsTypes[SelectedAvailableGoodsType]);
                AvailableGoodsTypes.RemoveAt(SelectedAvailableGoodsType);
            }
            else
            {
                int counter = AvailableGoodsTypes.Count;
                for (int i = 1; i < counter; i++)
                {
                    GoodsTypes.Add(AvailableGoodsTypes[i]);
                }
                for (int i = 1; i < counter; i++)
                {
                    AvailableGoodsTypes.RemoveAt(1);
                }
            }
            if (AvailableGoodsTypes.Count == 1 && AvailableGoodsTypes[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                AvailableGoodsTypes.RemoveAt(0);
            if (GoodsTypes.Count > 0 && !GoodsTypes[0].Equals(Localiztion.Resource.GoodsTypes_LST_All))
                GoodsTypes.Insert(0, Localiztion.Resource.GoodsTypes_LST_All);
            SelectedAvailableGoodsType = -1;
            Saved = false;
        }
        public bool CanExecuteDeleteAvailableGoodsTypesDo()
        {
            if (SelectedAvailableGoodsType == -1 || SelectedAvailableGoodsType >= AvailableGoodsTypes.Count || SelectedAvailableGoodsType < 0)
                return false;
            else
                return true;
        }
        public void ExecuteConfirmDo()
        {
            //1. Apply to all layers
            //2. add all Available goods types to selected items except ALL
            for(int i = 0; i < selectedMapItems.Count; i++)
            {
                if (!isApplyToAllLayers)
                {
                    selectedMapItems[i].SingleStorage.ResetAvailableGoodTypes();
                    for (int j = 1; j < AvailableGoodsTypes.Count; j++)
                    {
                        selectedMapItems[i].SingleStorage.AvailableGoodTypes[AvailableGoodsTypes[j]] = true;
                    }
                }
                else
                {
                    for(int k = 0; k < _map.LayerCount; k++)
                    {
                        _map[k, selectedMapItems[i].SingleStorage.Location.Rack, selectedMapItems[i].SingleStorage.Location.Column]
                            .ResetAvailableGoodTypes();
                        for (int j = 1; j < AvailableGoodsTypes.Count; j++)
                        {
                            _map[k, selectedMapItems[i].SingleStorage.Location.Rack, selectedMapItems[i].SingleStorage.Location.Column]
                               .AvailableGoodTypes[AvailableGoodsTypes[j]] = true;
                        }
                    }
                }
            }
            Saved = true;
            CallBackFunction();
            ExecuteConfirm.RaiseCanExecuteChanged();
            MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.ModifySelectedMapItem_GoodsTypes_Complete);
            if (!FromClosing && confirmToDel == MessageBoxResult.OK)
            {
                self.Close();
            }
            else
            {
                FromClosing = false;
            }
        }
        public bool CanExecuteConfirmDo()
        {
            if (AvailableGoodsTypes.Count == 0 || Saved)
                return false;
            else
                return true;
        }
        private void ExecuteClosingCommandDo(Models.ExParameters parameters)
        {
            if (Saved)
                return;
            CancelEventArgs cancelEventArgs = parameters.EventArgs as CancelEventArgs;
            cancelEventArgs.Cancel = true;
            
            if (AvailableGoodsTypes.Count == 0)
            {
                //ask for quit
                MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.ModifySelectedMapItem_AskForQuit,
                    Localiztion.Resource.Alert, MessageBoxButton.OKCancel);
                if (confirmToDel == MessageBoxResult.OK)
                {
                    cancelEventArgs.Cancel = false;
                }
                else
                    return;
            }
            else
            {
                //ask for save
                MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.ModifySelectedMapItem_AskForSave,
                    Localiztion.Resource.Alert, MessageBoxButton.YesNoCancel);
                if (confirmToDel == MessageBoxResult.Yes)
                {
                    FromClosing = true;//control not to close the window to ensure the unhandle exception
                    ExecuteConfirmDo();
                    cancelEventArgs.Cancel = false;
                }
                else if(confirmToDel == MessageBoxResult.Cancel)
                    return;
                else
                    cancelEventArgs.Cancel = false;
            }
        }
        #endregion
    }
}
