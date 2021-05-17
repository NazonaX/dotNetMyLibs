//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace wpfSimulation.ViewModels
//{
//    public class AddStoreRequestsViewModels: ModifySelectedStorageViewModels2
//    {
//        private List<Models.Classes.Request> RefStoreRequestList = null;
//        public delegate void StatusShowCallBackFunction(string message);
//        public StatusShowCallBackFunction CallBackMessage = null;

//        public AddStoreRequestsViewModels(Models.Classes.Map map, Window self, 
//            List<Models.Classes.Request> refList)
//            :base(map,self)
//        {
//            this.RefStoreRequestList = refList;
//            //initial the GoodTypeList
//            GoodTypesList = new System.Collections.ObjectModel.ObservableCollection<string>(_map.GoodsTypes);
//            GoodTypesList.Insert(0, Localiztion.Resource.MapItems_PleaseSelect);
//        }

//        protected override void ExecuteModifySelectedStorageCommandDo()
//        {
//            //override the original Command Do
//            List<string> type = new List<string>();
//            type.Add(GoodTypesList[SelectedGoodType]);
//            Models.Classes.Goods goodsToStore = new Models.Classes.Goods(GoodName, GoodCode, GoodSpecification,
//                GoodSpecification, _goodCount, type);
//            RefStoreRequestList.AddRange(Models.Logics.StorageManager.Instance.SetStoreRequests(goodsToStore));
//            CallBackMessage("Add Store Requests Complete, and the total Store Requests' count is " + RefStoreRequestList.Count);
//        }
//    }
//}
