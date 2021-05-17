using Models.Entity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class ConfirmDialogueViewModels : BaseViewModels
    {
        public delegate void RefreshCallback();
        public delegate void MainStatusCallBack(string msg);

        private List<SingleGridMapItemViewModels> SelectedMapItems = null;
        private Models.Entity.Map _map = null;
        private Window self = null;
        
        private RefreshCallback refreshCallback = null;
        private MainStatusCallBack mainStatusCallBack = null;
        public DelegateCommand ExecuteSaveCommand { get; private set; }

        public ConfirmDialogueViewModels(List<SingleGridMapItemViewModels> SelectedMapItemsss,
            Models.Entity.Map map,
            Window selfWindow, RefreshCallback callback, MainStatusCallBack mainStatusCallBack
            )
        {
          
            this.SelectedMapItems = SelectedMapItemsss;
            this.self = selfWindow;
            this._map = map;
           
            this.refreshCallback = callback;
            this.mainStatusCallBack = mainStatusCallBack;
            ExecuteSaveCommand = new DelegateCommand(ExecuteSaveCommandDo, CanExecuteSaveCommandDo);

        }

        private void ExecuteSaveCommandDo()
        {
           
            List<Models.Entity.Goods> toDeleteList = new List<Models.Entity.Goods>();
            List<Models.Entity.MapItems> toDeleteMIList = new List<MapItems>();
            List<Models.Entity.MapItems> toAddMIList = new List<MapItems>();
            AssignTheChanges(toDeleteList, toDeleteMIList, toAddMIList);
            ConfirmSave(toDeleteList, toAddMIList, toDeleteMIList);
            refreshCallback();
            MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
            StringBuilder sb = new StringBuilder();
            int count = SelectedMapItems.Count;
          
            
            
           
            mainStatusCallBack(sb.ToString());
            self.Close();
        }
        private void ConfirmSave(List<Goods> toDeleteList, List<MapItems> toAddMIList, List<MapItems> toDeleteMIList)
        {
            //do as a transcaction
            using (TransactionScope trans = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.GetMapItemsService().UpdateAllMapItems();
                mapSingletonService.GetMapItemsService().InserSpecialtMapItems(toAddMIList);
                mapSingletonService.GetMapItemsService().DeleteSpecialMapItems(toDeleteMIList);
                mapSingletonService.GetGoodsService().DeleteGoods(toDeleteList);
                trans.Complete();
            }
        }
        private void AssignTheChanges(List<Models.Entity.Goods> toDeleleList,
           List<Models.Entity.MapItems> toDeleteMIList, List<Models.Entity.MapItems> toAddMIList)
        {
            int rail = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_RAIL).Id;
            
            for (int i = 0; i < SelectedMapItems.Count; i++)
            {
                SelectedMapItems[i].SingleStorage.TypeId = rail;
            }
        }
        private bool CanExecuteSaveCommandDo()
        {
            return true;
        }
    }
}
