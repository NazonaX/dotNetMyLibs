using Models.Entity;
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
    public class ModifySelectedMapItemsTypesViewModels : BaseViewModels
    {
        public delegate void RefreshCallback();
        public delegate void MainStatusCallBack(string msg);

        private List<SingleGridMapItemViewModels> SelectedMapItems = null;
        private Models.Entity.Map _map = null;
        private Window self = null;
        private int _selectedTypeIndex = 0;
        private ObservableCollection<Models.Entity.Types> types = new ObservableCollection<Models.Entity.Types>();
        private RefreshCallback refreshCallback = null;
        private MainStatusCallBack mainStatusCallBack = null;
        private int SelectedLayer = 0;

        public DelegateCommand ExecuteSaveModifySelectedMapItemCommand { get; private set; }
        public ObservableCollection<Models.Entity.Types> Types { get { return types; } }
        public bool ApplyToAllLayers { get; set; }

        public ModifySelectedMapItemsTypesViewModels(List<SingleGridMapItemViewModels> SelectedMapItems,
            Models.Entity.Map map,
            Window selfWindow, RefreshCallback callback, MainStatusCallBack mainStatusCallBack,
            int SelectedLayer)
        {
            bool hasGoods = false;
            this.SelectedMapItems = SelectedMapItems;
            this.self = selfWindow;
            this._map = map;
            this.Types.AddRange(_map.Types);
            this.refreshCallback = callback;
            this.mainStatusCallBack = mainStatusCallBack;
            this.SelectedLayer = SelectedLayer;
            ExecuteSaveModifySelectedMapItemCommand = new DelegateCommand(ExecuteSaveModifySelectedMapItemCommandDo, CanExecuteSaveModifySelectedMapItemCommandDo);
            for (int i = 0; i < this.SelectedMapItems.Count; i++)
            {
                if (Models.Service.MapSingletonService.Instance.HasGood(SelectedMapItems[i].SingleStorage))
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
                _selectedTypeIndex = value;
                OnPropertyChanged("SelectedTypeIndex");
                ExecuteSaveModifySelectedMapItemCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteSaveModifySelectedMapItemCommandDo()
        {
            ////no need to select good types
            ////so the ModifyAvailableGoodsTypes__s are not necessary any more, you can just ignore them
            List<Models.Entity.Goods> toDeleteList = new List<Models.Entity.Goods>();
            List<Models.Entity.MapItems> toDeleteMIList = new List<MapItems>();
            List<Models.Entity.MapItems> toAddMIList = new List<MapItems>();
            AssignTheChanges(toDeleteList, toDeleteMIList, toAddMIList);
            ConfirmSave(toDeleteList, toAddMIList, toDeleteMIList);
            refreshCallback();
            MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
            StringBuilder sb = new StringBuilder();
            int count = SelectedMapItems.Count;
            if (ApplyToAllLayers)
            {
                sb.Append("All layers ");
                count *= _map.LayerCount;
            }
            else
            {
                sb.Append("Current layer ");
            }
            sb.Append("has been affected. All ").Append(count).Append(" items has be defined as ")
                .Append(Types[SelectedTypeIndex].Name);
            mainStatusCallBack(sb.ToString());
            self.Close();
        }

        private void ConfirmSave(List<Goods> toDeleteList, List<MapItems> toAddMIList, List<MapItems> toDeleteMIList)
        {
            //do as a transcaction
            using(TransactionScope trans = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.GetMapItemsService().InserSpecialtMapItems(toAddMIList);
                mapSingletonService.GetMapItemsService().DeleteSpecialMapItems(toDeleteMIList);
                mapSingletonService.GetGoodsService().DeleteGoods(toDeleteList);
                mapSingletonService.GetMapItemsService().UpdateAllMapItems();
                trans.Complete();
            }
        }

        private void AssignTheChanges(List<Models.Entity.Goods> toDeleleList,
            List<Models.Entity.MapItems> toDeleteMIList, List<Models.Entity.MapItems> toAddMIList)
        {
            int Unavailabe = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_UNAVAILABLE).Id;
            int Input = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_INPUT_POINT).Id;
            int Output = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_OUTPUT_POINT).Id;
            int Lifter = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_LIFTER).Id;
            for (int i = 0; i < SelectedMapItems.Count; i++)
            {
                if (!ApplyToAllLayers)
                {
                    //for current layer
                    if (SelectedMapItems[i].SingleStorage.Rack < 0 || SelectedMapItems[i].SingleStorage.Rack >= _map.RackCount
                        || SelectedMapItems[i].SingleStorage.Column < 0 || SelectedMapItems[i].SingleStorage.Column >= _map.ColumnCount)
                    {
                        Models.Entity.MapItems tmp = _map.SpecialMapItems.SingleOrDefault(item => item.Layer == SelectedLayer && item.Rack == SelectedMapItems[i].SingleStorage.Rack && item.Column == SelectedMapItems[i].SingleStorage.Column);
                        if (Types[SelectedTypeIndex].Id == Unavailabe && SelectedMapItems[i].SingleStorage.MapItemID != 0)
                        {
                            //DELETE
                            if (tmp != null)
                                toDeleteMIList.Add(tmp);
                        }
                        else if(Types[SelectedTypeIndex].Id == Input || Types[SelectedTypeIndex].Id == Output || Types[SelectedTypeIndex].Id == Lifter)
                        {
                            //UPDATE or ADD
                            SelectedMapItems[i].SingleStorage.Layer = SelectedLayer;
                            SelectedMapItems[i].SingleStorage.TypeId = Types[SelectedTypeIndex].Id;
                            if (tmp == null)
                                toAddMIList.Add(SelectedMapItems[i].SingleStorage);
                        }
                        continue;
                    }
                    //dont forget to reset these items, maybe need to recalculate the logics cargoways
                    SelectedMapItems[i].SingleStorage.TypeId = Types[SelectedTypeIndex].Id;
                    SelectedMapItems[i].SingleStorage.ZoneId = 0;
                    if(Models.Service.MapSingletonService.Instance.HasGood(SelectedMapItems[i].SingleStorage))
                    {
                        //add to to delete Good Lists
                        toDeleleList.Add(_map.Goods.Single(g => g.MapItemsId == SelectedMapItems[i].SingleStorage.MapItemID));
                    }
                }
                else
                {
                    //for each layers
                    for (int j = 0; j < _map.LayerCount; j++)
                    {
                        if (SelectedMapItems[i].SingleStorage.Rack < 0 || SelectedMapItems[i].SingleStorage.Rack >= _map.RackCount
                            || SelectedMapItems[i].SingleStorage.Column < 0 || SelectedMapItems[i].SingleStorage.Column >= _map.ColumnCount)
                        {
                            Models.Entity.MapItems tmp = _map.SpecialMapItems.SingleOrDefault(item => item.Layer == j && item.Rack == SelectedMapItems[i].SingleStorage.Rack && item.Column == SelectedMapItems[i].SingleStorage.Column);
                            if (Types[SelectedTypeIndex].Id == Unavailabe)
                            {
                                //DELETE
                                if (tmp != null)
                                    toDeleteMIList.Add(tmp);
                            }
                            else if (Types[SelectedTypeIndex].Id == Input || Types[SelectedTypeIndex].Id == Output || Types[SelectedTypeIndex].Id == Lifter)
                            {
                                //UPDATE or ADD
                                //new one
                                if (tmp == null)
                                {
                                    MapItems newone = new MapItems();
                                    newone.Layer = j;
                                    newone.Rack = SelectedMapItems[i].SingleStorage.Rack;
                                    newone.Column = SelectedMapItems[i].SingleStorage.Column;
                                    newone.TypeId = Types[SelectedTypeIndex].Id;
                                    toAddMIList.Add(newone);
                                }
                                else
                                    tmp.TypeId = Types[SelectedTypeIndex].Id;
                            }
                            continue;
                        }
                        _map[j, SelectedMapItems[i].SingleStorage.Rack, SelectedMapItems[i].SingleStorage.Column].TypeId = Types[SelectedTypeIndex].Id;
                        _map[j, SelectedMapItems[i].SingleStorage.Rack, SelectedMapItems[i].SingleStorage.Column].ZoneId = 0;
                        if (Models.Service.MapSingletonService.Instance.HasGood(_map[j, SelectedMapItems[i].SingleStorage.Rack, SelectedMapItems[i].SingleStorage.Column]))
                        {
                            //add to to delete Good Lists
                            toDeleleList.Add(_map.Goods.Single(g => g.MapItemsId == _map[j, SelectedMapItems[i].SingleStorage.Rack, SelectedMapItems[i].SingleStorage.Column].MapItemID));
                        }
                    }
                }
            }
        }

        private bool CanExecuteSaveModifySelectedMapItemCommandDo()
        {
            return true;
        }
    }
}
