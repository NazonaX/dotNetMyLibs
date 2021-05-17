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
    public class ModifySelectedStorageViewModels2 : BaseViewModels
    {
        public delegate void RefreshGridColorCallBack();
        public delegate void MainStatusCallBack(string msg);

        private List<SingleGridMapItemViewModels> SelectedStorages = null;
        private RefreshGridColorCallBack refreshGridColorCallBack = null;
        private MainStatusCallBack mainStatusCallBack = null;
        protected Models.Entity.Map _map = null;
        protected Window _self = null;
        protected string _goodModel = "";
        protected string _goodName = "";
        protected string _goodBatch = "";
        protected int _goodCount = 0;


        public DelegateCommand ExecuteModifySelectedStorageCommand { get; protected set; }

        public ModifySelectedStorageViewModels2(Models.Entity.Map map, List<SingleGridMapItemViewModels> selectedList,
            Window self, RefreshGridColorCallBack refreshGridColorCallBack, MainStatusCallBack mainStatusCallBack)
        {
            this.SelectedStorages = selectedList;
            this._map = map;
            this._self = self;
            this.refreshGridColorCallBack = refreshGridColorCallBack;
            this.mainStatusCallBack = mainStatusCallBack;
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
        }

        #region getters and setters
        public string GoodModel
        {
            get { return _goodModel; }
            set
            {
                _goodModel = value;
                OnPropertyChanged("GoodModel");
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
        public string GoodBatch
        {
            get { return _goodBatch; }
            set
            {
                _goodBatch = value;
                OnPropertyChanged("GoodBatch");
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

        #endregion

        #region Commands6
        virtual protected void ExecuteModifySelectedStorageCommandDo()
        {
            //first to pick all the MapItems as a list
            //then create a new list of goods
            //create goods
            //save goods--> transaction
            //save map-- transaction
            //over
            List<Models.Entity.Goods> addList = new List<Models.Entity.Goods>();
            List<Models.Entity.Goods> deleteList = new List<Models.Entity.Goods>();
            for(int i = 0; i < SelectedStorages.Count; i++)
            {
                addList.Add(new Models.Entity.Goods()
                {
                    Name = GoodName,
                    Batch = GoodBatch,
                    Model = GoodModel,
                    Count = _goodCount,
                    MapItemsId = SelectedStorages[i].SingleStorage.MapItemID,
                    CargoWayLockId = 0,
                    //TODO::set GoodId and BarCode interfaces
                    ProductId = "",
                    BarCode = ""
                });
            }
            using(TransactionScope scope = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                for(int i = 0; i < SelectedStorages.Count; i++)
                {
                    Models.Entity.Goods todel = _map.Goods.SingleOrDefault(g => g.MapItemsId == SelectedStorages[i].SingleStorage.MapItemID);
                    if (todel != null && todel.Id != 0)
                        deleteList.Add(todel);
                }
                //先删除，后赋值保存
                mapSingletonService.GetGoodsService().DeleteGoods(deleteList);
                mapSingletonService.GetGoodsService().InsertGoods(addList);
                //存储栅格状态
                mapSingletonService.GetMapItemsService().UpdateAllMapItems();
                scope.Complete();
                refreshGridColorCallBack();
                StringBuilder sb = new StringBuilder();
                sb.Append("Delete ").Append(deleteList.Count).Append(" goods. And add ")
                    .Append(addList.Count).Append(" new goods.");
                mainStatusCallBack(sb.ToString());
                _self.Close();
            }
        }
        protected bool CanExecuteModifySelectedStorageCommandDo()
        {
            if (GoodBatch.Equals("") || GoodModel.Equals("")
                || _goodCount == 0 || GoodName.Equals(""))
                return false;
            else
                return true;
        }

        #endregion
    }
}
