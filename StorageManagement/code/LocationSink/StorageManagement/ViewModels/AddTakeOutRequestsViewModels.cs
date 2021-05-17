//using Models.Classes;
//using Prism.Commands;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;

//namespace wpfSimulation.ViewModels
//{
//    public class AddTakeOutRequestsViewModels: BaseViewModels
//    {
//        private Models.Classes.Map _map = null;
//        private Window _self = null;
//        private List<Models.Classes.Request> RefTakeOutRequestList = null;
//        public delegate void StatusShowCallBackFunction(string message);
//        public StatusShowCallBackFunction CallBackMessage = null;

//        private List<string> AllPGCList = new List<string>();
//        private ObservableCollection<string> _possibleGoodCodes = new ObservableCollection<string>();
//        private string _selectedPGC = Goods.Empty;
//        private List<string> AllPOCList = new List<string>();
//        private ObservableCollection<string> _possibleOrderCodes = new ObservableCollection<string>();
//        private string _selectedPOC = Goods.Empty;
//        private List<string> AllPGNList = new List<string>();
//        private ObservableCollection<string> _possibleGoodNames = new ObservableCollection<string>();
//        private string _selectedPGN = Goods.Empty;
//        private List<string> AllPSList = new List<string>();
//        private ObservableCollection<string> _possibleSpecification = new ObservableCollection<string>();
//        private string _selectedPS = Goods.Empty;
//        private List<string> AllPGTList = new List<string>();
//        private ObservableCollection<string> _possibleGoodTypes = new ObservableCollection<string>();
//        private string _selectedPGT = Goods.Empty;
//        private int _possibleGoodCounts = 0;
//        private List<Models.Classes.Goods> TempAllGoods = new List<Models.Classes.Goods>();
//        List<Models.Classes.Goods> Tmp = null;

//        public DelegateCommand ExecuteAddTakeOutRequestsCommand { get; private set; }
//        public DelegateCommand ExecuteResetCommand { get; private set; }

//        public AddTakeOutRequestsViewModels(Models.Classes.Map map, Window self, List<Models.Classes.Request> refList)
//        {
//            this._map = map;
//            this._self = self;
//            this.RefTakeOutRequestList = refList;
//            ExecuteAddTakeOutRequestsCommand = new DelegateCommand(ExecuteAddTakeOutRequestsCommandDo, CanExecuteAddTakeOutRequestsCommandDo);
//            ExecuteResetCommand = new DelegateCommand(ExecuteResetCommandDo);
//            //to initial all the observable collection
//            for(int i=0; i< _map.LayerCount; i++)
//            {
//                for(int j = 0; j< _map.RackCount; j++)
//                {
//                    for(int k = 0; k < _map.ColumnCount; k++)
//                    {
//                        Models.Classes.Goods g = _map[i, j, k].Good;
//                        if (!g.IsEmpty())
//                        {
//                            TempAllGoods.Add(g);
//                            if (!Models.Classes.Goods.IsStringNullOrEmpty(g.GoodsCode) && !PossibleGoodCodes.Contains(g.GoodsCode))
//                                PossibleGoodCodes.Add(g.GoodsCode);
//                            if (!Models.Classes.Goods.IsStringNullOrEmpty(g.GoodsName) && !PossibleGoodNames.Contains(g.GoodsName))
//                                PossibleGoodNames.Add(g.GoodsName);
//                            if (!Models.Classes.Goods.IsStringNullOrEmpty(g.Specification) && !PossibleSpecification.Contains(g.Specification))
//                                PossibleSpecification.Add(g.Specification);
//                            if (!Models.Classes.Goods.IsStringNullOrEmpty(g.OrderCode) && !PossibleOrderCodes.Contains(g.OrderCode))
//                                PossibleOrderCodes.Add(g.OrderCode);
//                            if (g.Types.Count > 0)
//                            {
//                                for (int m = 0; m < g.Types.Count; m++)
//                                    if (!PossibleGoodTypes.Contains(g.Types[m]))
//                                        PossibleGoodTypes.Add(g.Types[m]);
//                            }
//                        }
//                    }
//                }
//            }
//            PossibleGoodNames.Insert(0, Models.Classes.Goods.Empty);
//            PossibleGoodCodes.Insert(0, Models.Classes.Goods.Empty);
//            PossibleOrderCodes.Insert(0, Models.Classes.Goods.Empty);
//            PossibleSpecification.Insert(0, Models.Classes.Goods.Empty);
//            PossibleGoodTypes.Insert(0, Models.Classes.Goods.Empty);
//            AllPGCList = ((ObservableCollection<string>)Utils.IOOps.CopyMemory(PossibleGoodCodes)).ToList();
//            AllPGNList = ((ObservableCollection<string>)Utils.IOOps.CopyMemory(PossibleGoodNames)).ToList();
//            AllPGTList = ((ObservableCollection<string>)Utils.IOOps.CopyMemory(PossibleGoodTypes)).ToList();
//            AllPOCList = ((ObservableCollection<string>)Utils.IOOps.CopyMemory(PossibleOrderCodes)).ToList();
//            AllPSList = ((ObservableCollection<string>)Utils.IOOps.CopyMemory(PossibleSpecification)).ToList();
            
//        }

//        #region getters and setters
//        public ObservableCollection<string> PossibleGoodCodes
//        {
//            get { return _possibleGoodCodes; }
//            set
//            {
//                _possibleGoodCodes = value;
//                OnPropertyChanged("PossibleGoodCodes");
//            }
//        }
//        public ObservableCollection<string> PossibleOrderCodes
//        {
//            get { return _possibleOrderCodes; }
//            set
//            {
//                _possibleOrderCodes = value;
//                OnPropertyChanged("PossibleOrderCodes");
//            }
//        }
//        public ObservableCollection<string> PossibleGoodNames
//        {
//            get { return _possibleGoodNames; }
//            set
//            {
//                _possibleGoodNames = value;
//                OnPropertyChanged("PossibleGoodNames");
//            }
//        }
//        public ObservableCollection<string> PossibleSpecification
//        {
//            get { return _possibleSpecification; }
//            set
//            {
//                _possibleSpecification = value;
//                OnPropertyChanged("PossibleSpecification");
//            }
//        }
//        public ObservableCollection<string> PossibleGoodTypes
//        {
//            get { return _possibleGoodTypes; }
//            set
//            {
//                _possibleGoodTypes = value;
//                OnPropertyChanged("PossibleGoodTypes");
//            }
//        }
//        public string TakeGoodCounts
//        {
//            get { return "" + _possibleGoodCounts; }
//            set
//            {
//                int outer = 0;
//                int.TryParse(value, out outer);
//                _possibleGoodCounts = outer;
//                OnPropertyChanged("TakeGoodCounts");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
//        public string SelectedPGC
//        {
//            get { return _selectedPGC; }
//            set
//            {
//                _selectedPGC = value;
//                OnPropertyChanged("SelectedPGC");
//                ReloadAllPossibleLists("PGC");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
//        public string SelectedPOC
//        {
//            get { return _selectedPOC; }
//            set
//            {
//                _selectedPOC = value;
//                OnPropertyChanged("SelectedPOC");
//                ReloadAllPossibleLists("POC");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
//        public string SelectedPGN
//        {
//            get { return _selectedPGN; }
//            set
//            {
//                _selectedPGN = value;
//                OnPropertyChanged("SelectedPGN");
//                ReloadAllPossibleLists("PGN");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
//        public string SelectedPS
//        {
//            get { return _selectedPS; }
//            set
//            {
//                _selectedPS = value;
//                OnPropertyChanged("SelectedPS");
//                ReloadAllPossibleLists("PS");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
//        public string SelectedPGT
//        {
//            get { return _selectedPGT; }
//            set
//            {
//                _selectedPGT = value;
//                OnPropertyChanged("SelectedPGT");
//                ReloadAllPossibleLists("PGT");
//                ExecuteAddTakeOutRequestsCommand.RaiseCanExecuteChanged();
//            }
//        }
        
//        #endregion

//        #region Commands
//        private void ExecuteAddTakeOutRequestsCommandDo()
//        {
//            int TotalStorageCount = 0;
//            foreach(Goods g in Tmp)
//            {
//                TotalStorageCount += g.GoodsCount;
//            }
//            bool isIllegal = (TotalStorageCount < _possibleGoodCounts ? true : false)
//                || (0 > _possibleGoodCounts ? true : false);
//            _possibleGoodCounts = TotalStorageCount < _possibleGoodCounts ? TotalStorageCount : _possibleGoodCounts;
//            _possibleGoodCounts = 0 > _possibleGoodCounts ? 0 : _possibleGoodCounts;
//            TakeGoodCounts = "" + _possibleGoodCounts;
//            StringBuilder sb = new StringBuilder();
//            sb.Append(Localiztion.Resource.ModifySelectedStorage_MB_ConfirmTakeOut).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_GoodCode).Append(SelectedPGC).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_GoodName).Append(SelectedPGN).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_SetGoodTypes).Append(SelectedPGT).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_GoodSpecification).Append(SelectedPS).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_OrderCode).Append(SelectedPOC).Append("\r\n")
//                .Append(Localiztion.Resource.ModifySelectedStorage_LB_GoodCount).Append(TakeGoodCounts);
//            if (isIllegal)
//            {
//                MessageBox.Show(Localiztion.Resource.AddTakeOutRequest_IllegalCount_MSG + "\r\n0~" + TakeGoodCounts);
                
//            }
//            MessageBoxResult res = MessageBox.Show(sb.ToString(), Localiztion.Resource.BT_Confirm, MessageBoxButton.OKCancel);
//            if(res == MessageBoxResult.OK)
//            {
//                List<string> justOne = new List<string>();
//                justOne.Add(SelectedPGT);
//                List<Request> requests = Models.Logics.StorageManager.Instance.SetTakeOutRequests(
//                    new Goods(SelectedPGN, SelectedPGC, SelectedPS, SelectedPOC, _possibleGoodCounts, justOne));
//                RefTakeOutRequestList.AddRange(requests);
//                CallBackMessage("Add TakeOut Requests Complete, and the total TakeOut Requests' count is " + RefTakeOutRequestList.Count);
//            }
//        }
//        private bool CanExecuteAddTakeOutRequestsCommandDo()
//        {
//            return SelectedPGC != Goods.Empty && SelectedPGN != Goods.Empty
//                && SelectedPGT != Goods.Empty && SelectedPOC != Goods.Empty
//                && SelectedPS != Goods.Empty && _possibleGoodCounts != 0;
//        }
//        private void ExecuteResetCommandDo()
//        {
//            SelectedPGC = Goods.Empty;
//            SelectedPGN = Goods.Empty;
//            SelectedPGT = Goods.Empty;
//            SelectedPOC = Goods.Empty;
//            SelectedPS = Goods.Empty;
//            SelectedPGC = Goods.Empty;//Do another selected again for reset the last one
//        }
//        #endregion
//        private void ReloadAllPossibleLists(string CommandFrom)
//        {
//            //to reload all the possible lists
//            //keep the selected items and Empty item
//            List<string> justOne = new List<string>();
//            justOne.Add(SelectedPGT);
//            Models.Classes.Goods g = new Models.Classes.Goods(SelectedPGN,
//                SelectedPGC,
//                SelectedPS,
//                SelectedPOC,
//                _possibleGoodCounts,
//                justOne);
//            Tmp = TempAllGoods.Where(good => good.PartialEquals(g)).ToList();
//            //Reload all possible lists
//            if (CommandFrom.Equals("PGC"))
//            {
//                ReloadPossibleListsFromAllListsByGood(AllPGNList, Tmp, g, "GN");
//                ReloadPossibleListsFromAllListsByGood(AllPGTList, Tmp, g, "GT");
//                ReloadPossibleListsFromAllListsByGood(AllPOCList, Tmp, g, "OC");
//                ReloadPossibleListsFromAllListsByGood(AllPSList, Tmp, g, "S");
//            }
//            else if (CommandFrom.Equals("PGN"))
//            {
//                ReloadPossibleListsFromAllListsByGood(AllPGCList, Tmp, g, "GC");
//                ReloadPossibleListsFromAllListsByGood(AllPGTList, Tmp, g, "GT");
//                ReloadPossibleListsFromAllListsByGood(AllPOCList, Tmp, g, "OC");
//                ReloadPossibleListsFromAllListsByGood(AllPSList, Tmp, g, "S");
//            }
//            else if (CommandFrom.Equals("PGT"))
//            {
//                ReloadPossibleListsFromAllListsByGood(AllPGCList, Tmp, g, "GC");
//                ReloadPossibleListsFromAllListsByGood(AllPGNList, Tmp, g, "GN");
//                ReloadPossibleListsFromAllListsByGood(AllPOCList, Tmp, g, "OC");
//                ReloadPossibleListsFromAllListsByGood(AllPSList, Tmp, g, "S");
//            }
//            else if (CommandFrom.Equals("POC"))
//            {
//                ReloadPossibleListsFromAllListsByGood(AllPGCList, Tmp, g, "GC");
//                ReloadPossibleListsFromAllListsByGood(AllPGNList, Tmp, g, "GN");
//                ReloadPossibleListsFromAllListsByGood(AllPGTList, Tmp, g, "GT");
//                ReloadPossibleListsFromAllListsByGood(AllPSList, Tmp, g, "S");
//            }
//            else if (CommandFrom.Equals("PS"))
//            {
//                ReloadPossibleListsFromAllListsByGood(AllPGCList, Tmp, g, "GC");
//                ReloadPossibleListsFromAllListsByGood(AllPGNList, Tmp, g, "GN");
//                ReloadPossibleListsFromAllListsByGood(AllPGTList, Tmp, g, "GT");
//                ReloadPossibleListsFromAllListsByGood(AllPOCList, Tmp, g, "OC");
//            }
//        }

//        private void ReloadPossibleListsFromAllListsByGood(List<string> allPList, 
//            List<Models.Classes.Goods> goods,
//            Goods g,
//            string type)
//        {
//            //int deleteCounter = possible.Count;
//            //for (int i = 1; i < deleteCounter; i++)
//            //{
//            //    possible.RemoveAt(1); //will throw exception here and dont know why
//            //}
//            ObservableCollection<string> possible = null;
//            if (type.Equals("GC"))
//            {
//                if (!g.GoodsCode.Equals(Goods.Empty))
//                    PossibleGoodCodes = new ObservableCollection<string>(AllPGCList
//                        .Where(p => p.Equals(Goods.Empty)
//                            || p.Equals(g.GoodsCode))
//                        .ToList());
//                else
//                    PossibleGoodCodes = new ObservableCollection<string>(AllPGCList
//                        .Where(p => p.Equals(Goods.Empty)).ToList());
//                possible = PossibleGoodCodes;
//            }
//            else if (type.Equals("GN"))
//            {
//                if (!g.GoodsName.Equals(Goods.Empty))
//                    PossibleGoodNames = new ObservableCollection<string>(AllPGNList
//                        .Where(p => p.Equals(Goods.Empty)
//                            || p.Equals(g.GoodsName))
//                        .ToList());
//                else
//                    PossibleGoodNames = new ObservableCollection<string>(AllPGNList
//                        .Where(p => p.Equals(Goods.Empty)).ToList());
//                possible = PossibleGoodNames;
//            }
//            else if (type.Equals("GT"))
//            {
//                if (!(g.Types.Count == 1 && g.Types[0].Equals(Goods.Empty)))
//                    PossibleGoodTypes = new ObservableCollection<string>(AllPGTList
//                        .Where(p => p.Equals(Goods.Empty)
//                            || g.Types.Contains(p))
//                        .ToList());
//                else
//                    PossibleGoodTypes = new ObservableCollection<string>(AllPGTList
//                        .Where(p => p.Equals(Goods.Empty)).ToList());
//                possible = PossibleGoodTypes;
//            }
//            else if (type.Equals("OC"))
//            {
//                if (!g.OrderCode.Equals(Goods.Empty))
//                    PossibleOrderCodes = new ObservableCollection<string>(AllPOCList
//                        .Where(p => p.Equals(Goods.Empty)
//                            || p.Equals(g.OrderCode))
//                        .ToList());
//                else
//                    PossibleOrderCodes = new ObservableCollection<string>(AllPOCList
//                        .Where(p => p.Equals(Goods.Empty)).ToList());
//                possible = PossibleOrderCodes;
//            }
//            else if (type.Equals("S"))
//            {
//                if (!g.Specification.Equals(Goods.Empty))
//                    PossibleSpecification = new ObservableCollection<string>(AllPSList
//                        .Where(p => p.Equals(Goods.Empty)
//                            || p.Equals(g.Specification))
//                        .ToList());
//                else
//                    PossibleSpecification = new ObservableCollection<string>(AllPSList
//                        .Where(p => p.Equals(Goods.Empty)).ToList());
//                possible = PossibleSpecification;
//            }
//            for (int i = 0; i < allPList.Count; i++)
//            {
//                bool has = false;
//                if (type.Equals("GC"))
//                {
//                    has = goods.Any(tmp => tmp.GoodsCode.Equals(allPList[i]));
//                }
//                else if (type.Equals("GN"))
//                {
//                    has = goods.Any(tmp => tmp.GoodsName.Equals(allPList[i]));
//                }
//                else if (type.Equals("GT"))
//                {
//                    has = goods.Any(tmp => tmp.Types.Contains(allPList[i]));
//                }
//                else if (type.Equals("OC"))
//                {
//                    has = goods.Any(tmp => tmp.OrderCode.Equals(allPList[i]));
//                }
//                else
//                {
//                    has = goods.Any(tmp => tmp.Specification.Equals(allPList[i]));
//                }
//                if (!possible.Contains(allPList[i]) && has)
//                    possible.Add(allPList[i]);
//            }
//        }
//    }
//}
