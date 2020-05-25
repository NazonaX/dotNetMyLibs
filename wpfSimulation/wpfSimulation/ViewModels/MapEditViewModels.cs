using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;

namespace wpfSimulation.ViewModels
{
    public class MapEditViewModels : BaseViewModels
    {
        #region properties
        private ObservableCollection<SingleGridMapItemViewModels> gridMapItems = new ObservableCollection<SingleGridMapItemViewModels>();
        private ObservableCollection<LayersViewModels> _Layers = new ObservableCollection<LayersViewModels>();
        private AreaRect _areaRectangle = new AreaRect();
        private Models.Classes.Map _map = null;
        private int _selectedLayer = -1;
        private int _layerCount = -1;
        private int maxRackLength = 0;
        private int maxRackWidth = 0;
        private bool _canGridMapSelect = false;
        private int StatusCounter = 0;//used for control the status string erasing
        private string _statusString = "";
        //for Store requests, store all the sliced set int requests
        private List<Models.Classes.Request> StoreRequests = new List<Models.Classes.Request>();
        //for Take Out requests, store all the sliced take out requests
        private List<Models.Classes.Request> TakeOutRequests = new List<Models.Classes.Request>();

        public DelegateCommand ExecuteCanSelectCommand { get; private set; }
        public DelegateCommand ExecuteCancelSelectCommand { get; private set; }
        //it seems that DelegateCommand need to use "?" for int
        //public DelegateCommand<int?> ExcuteSelectedLayerChangedCommand { get; private set; }
        //Canvas to draw rectangle
        private Point DownPoint = new Point();
        private double AreaThreshold = 10;
        public DelegateCommand<Models.ExParameters> ExcuteAreaCanvasLeftButtonDownCommand { get; private set; }
        public DelegateCommand<Models.ExParameters> ExcuteAreaCanvasLeftButtonUpCommand { get;private set; }
        public DelegateCommand<Models.ExParameters> ExcuteAreaCanvasMoveCommand { get; private set; }
        public DelegateCommand ExecuteSaveMapCommand { get; private set; }
        public DelegateCommand ExecuteModifyStaticGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteModifyMapItemCommand { get; private set; }
        public DelegateCommand ExecuteCreateMapCommand { get; private set; }
        public DelegateCommand ExecuteModifySelectedStorageCommand { get; private set; }
        public DelegateCommand ExecuteModifyMapInformationCommand { get; private set; }
        //TODO:NazonaX ->To add two windows for add requests
        public DelegateCommand ExecuteAddStoreRequestsCommand { get; private set; }
        public DelegateCommand ExecuteAddTakeOutRequestsCommand { get; private set; }

        #endregion
        public MapEditViewModels(Models.Classes.Map map)
        {
            InitialCommands();
            _map = map;
            InitialLayers();
        }
        public MapEditViewModels()
        {
            InitialCommands();
            if (Models.Classes.Map.CheckForFile())
            {
                try
                {
                    _map = Models.Classes.Map.ReadFromFile();
                }
                catch (NullReferenceException ne)
                {
                    _map = new Models.Classes.Map();
                    System.Diagnostics.Debug.WriteLine(ne.Message + "\n" + ne.StackTrace);
                }
            }
            else
            {
                _map = new Models.Classes.Map();
            }
            InitialLayers();
            StatusString = "Ready";
        }

        private void InitialCommands()
        {
            ExecuteModifyMapItemCommand = new DelegateCommand(ExecuteModifyMapItemCommandDo, CanExecuteModifyMapItemCommandDo);
            ExecuteCanSelectCommand = new DelegateCommand(ExecuteCanSelectCommandDo);
            ExecuteCancelSelectCommand = new DelegateCommand(ExecuteCancelSelectCommandDo);
            ExcuteAreaCanvasLeftButtonDownCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonDownCommandDo);
            ExcuteAreaCanvasLeftButtonUpCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonUpCommandDo);
            ExcuteAreaCanvasMoveCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasMoveCommandDo);
            ExecuteSaveMapCommand = new DelegateCommand(ExecuteSaveMapCommandDo);
            ExecuteModifyStaticGoodsTypesCommand = new DelegateCommand(ExecuteModifyStaticGoodsTypesCommandDo);
            ExecuteCreateMapCommand = new DelegateCommand(ExecuteCreateMapCommandDo);
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
            ExecuteModifyMapInformationCommand = new DelegateCommand(ExecuteModifyMapInformationCommandDo);
            ExecuteAddStoreRequestsCommand = new DelegateCommand(ExecuteAddStoreRequestsCommandDo);
            ExecuteAddTakeOutRequestsCommand = new DelegateCommand(ExecuteAddTakeOutRequestsCommandDo);
        }
        /// <summary>
        /// if the _map is empty then set all available and have no values; or initial all
        ///  with _map...Including Layers, _gridMapItems, SelectedLayer, LayerCount, MaxRackLength
        ///  and MaxRackWidth
        /// </summary>
        private void InitialLayers()
        {
            if(_map.IsEmpty())
            {
                _selectedLayer = -1;
                _layerCount = 0;
                MaxRackLength = 0;
                MaxRackWidth = 0;
            }
            else
            {
                GridMapItems.Clear();
                for (int i = 0; i < _map.RackCount; i++)
                    for (int j = 0; j < _map.ColumnCount; j++)
                    {
                        GridMapItems.Add(new SingleGridMapItemViewModels(_map[0, i, j]));
                        //System.Diagnostics.Debug.WriteLine(i + "-" + j + "--" + i * _map.RackCount + j);
                    }
                Layers.Clear();
                for (int i = 0; i < _map.LayerCount; i++)
                    Layers.Add(new LayersViewModels(i + 1));
                SelectedLayer = 0;
                _layerCount = _map.LayerCount;
                Layers[0].IsSelected = true;
                //calculate for the canvas max width and height for visual
                //length for hroizonal
                MaxRackLength = (Models.Classes.Map.Width + Models.Classes.Map.Padding) * _map.ColumnCount
                    + Models.Classes.Map.Padding * 3;
                //width for vertical
                MaxRackWidth = _map.RackCount * (Models.Classes.Map.Width + Models.Classes.Map.Padding)
                    + Models.Classes.Map.Padding * 3;
            }
            
        }

        #region properties' gets sets
        public ObservableCollection<SingleGridMapItemViewModels> GridMapItems
        {
            get { return gridMapItems; }
            private set
            {
                OnPropertyChanged("GridMapItems");
                gridMapItems = value;
            }
        }
        public ObservableCollection<LayersViewModels> Layers
        {
            get { return _Layers; }
            private set
            {
                OnPropertyChanged("Layers");
                _Layers = value;
            }
        }
        public int MaxRackLength
        {
            get
            {
                return maxRackLength;
            }
            set
            {
                maxRackLength = value;
                OnPropertyChanged("MaxRackLength");
            }
        }
        public int MaxRackWidth
        {
            get
            {
                return maxRackWidth;
            }
            set
            {
                maxRackWidth = value;
                OnPropertyChanged("MaxRackWidth");
            }
        }
        public int SelectedLayer
        {
            get
            {
                return _selectedLayer;
            }
            set
            {
                if (value == SelectedLayer)
                    return;
                _selectedLayer = value;
                if (value == -1)
                    return;
                OnPropertyChanged("SelectedLayer");
                //GridMapItems.Clear();
                for (int i = 0; i < _map.RackCount; i++)
                {
                    for (int j = 0; j < _map.ColumnCount; j++)
                    {
                        GridMapItems[i * _map.ColumnCount + j].SingleStorage = _map[_selectedLayer, i, j];
                        //RandomColorType(GridMapItems[i * _map.ColumnCount + j]);
                    }
                }
                OnPropertyChanged("GridMapItems");
            }
        }
        private void RandomColorType(SingleGridMapItemViewModels sgm)
        {
            Models.Classes.MapItem.ItemTypes randomtype;
            Random random = new Random();
            double x = random.NextDouble();
            if (x > 0 && x < 0.2)
                randomtype = Models.Classes.MapItem.ItemTypes.RAIL;
            else if (x >= 0.2 && x < 0.4)
                randomtype = Models.Classes.MapItem.ItemTypes.EMPTY_STORAGE;
            else if (x >= 0.4 && x < 0.6)
                randomtype = Models.Classes.MapItem.ItemTypes.FULL_STORAGE;
            else if (x >= 0.6 && x < 0.8)
                randomtype = Models.Classes.MapItem.ItemTypes.INPUT_POINT;
            else
                randomtype = Models.Classes.MapItem.ItemTypes.OUTPUT_POINT;
            sgm.Type = randomtype;
        }
        public int LayerCount
        {
            get
            {
                return _layerCount;
            }
        }
        public double AreaRectTopPad
        {
            get { return _areaRectangle.TopPad; }
            set
            {
                _areaRectangle.TopPad = value;
                OnPropertyChanged("AreaRectTopPad");
            }
        }
        public double AreaRectLeftPad
        {
            get { return _areaRectangle.LeftPad; }
            set
            {
                _areaRectangle.LeftPad = value;
                OnPropertyChanged("AreaRectLeftPad");
            }
        }
        public double AreaRectWidth
        {
            get { return _areaRectangle.Width; }
            set
            {
                _areaRectangle.Width = value;
                OnPropertyChanged("AreaRectWidth");
            }
        }
        public double AreaRectHeight
        {
            get { return _areaRectangle.Height; }
            set
            {
                _areaRectangle.Height = value;
                OnPropertyChanged("AreaRectHeight");
            }
        }
        public bool CanGridMapSelect
        {
            get { return _canGridMapSelect; }
            set
            {
                _canGridMapSelect = value;
                OnPropertyChanged("CanGridMapSelect");
            }
        }
        public string StatusString {
            get { return _statusString; }
            set {
                _statusString = value;
                OnPropertyChanged("StatusString");
            }
        }
        #endregion

        #region Commands
        private bool CanExecuteModifyMapItemCommandDo()
        {
            return GridMapItems.Any(t => t.IsSelected);
        }

        private void ExecuteModifyMapItemCommandDo()
        {
            List<SingleGridMapItemViewModels> SelectedMapItems = GridMapItems.Where(p => p.IsSelected).ToList();
            wpfSimulation.Views.ModifySelectedMapItemView win = new wpfSimulation.Views.ModifySelectedMapItemView();
            ModifySelectedMapItemsViewModels msmivm = new ModifySelectedMapItemsViewModels(SelectedMapItems, _map, win);
            win.Owner = Application.Current.MainWindow;
            win.DataContext = msmivm;
            win.ShowDialog();
            RefreshGridMapItems();//to refresh the map item color view
        }
        //private void SelectedLayerChangedCommandDo(int? layerIndex)
        //{
        //    if (layerIndex == null || layerIndex == SelectedLayer)
        //        return;
        //    _selectedLayer = (int)layerIndex;
        //    //GridMapItems.Clear();
        //    for (int i = 0; i < _map.RackCount; i++)
        //    {
        //        for(int j = 0; j < _map.ColumnCount; j++)
        //        {
        //            GridMapItems[i * _map.ColumnCount + j].SingleStorage = _map[_selectedLayer, i, j];
        //            string color = "#";
        //            Random random = new Random();
        //            for(int c = 0; c < 3; c++)
        //            {
        //                if (random.NextDouble() > 0.5)
        //                    color += "ff";
        //                else
        //                    color += "00";
        //            }
        //            GridMapItems[i * _map.ColumnCount + j].Color = color;
        //        }
        //    }
        //}
        private void ExcuteAreaCanvasLeftButtonDownCommandDo(Models.ExParameters p)
        {
            //Canvas canvas = p.Parameter as Canvas;
            //MouseButtonEventArgs e = p.EventArgs as MouseButtonEventArgs;
            //Point point = e.GetPosition(canvas);
            //DownPoint = point;
        }
        private void ExcuteAreaCanvasLeftButtonUpCommandDo(Models.ExParameters p)
        {
            if (!CanGridMapSelect)
                return;
            Canvas canvas = p.Parameter as Canvas;
            MouseButtonEventArgs e = p.EventArgs as MouseButtonEventArgs;
            Point point = e.GetPosition(canvas);
            if(!(DownPoint.X == 0 && DownPoint.Y == 0) 
                && Math.Abs(DownPoint.X - point.X) > AreaThreshold && Math.Abs(DownPoint.Y - point.Y) > AreaThreshold)
            {
                //To set Selected ListBox Item
                for (int i = 0; i < GridMapItems.Count; i++)
                {
                    if((GridMapItems[i].TopPad + GridMapItems[i].Height > AreaRectTopPad && GridMapItems[i].TopPad < AreaRectTopPad + AreaRectHeight)
                        && (GridMapItems[i].LeftPad + GridMapItems[i].Width > AreaRectLeftPad && GridMapItems[i].LeftPad < AreaRectLeftPad + AreaRectWidth))
                    {
                        GridMapItems[i].IsSelected = true;
                    }
                    else
                        GridMapItems[i].IsSelected = false;
                }
            }
            ExecuteModifyMapItemCommand.RaiseCanExecuteChanged();
            ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
            AreaRectHeight = 0;
            AreaRectLeftPad = 0;
            AreaRectWidth = 0;
            AreaRectHeight = 0;
            DownPoint = new Point();
            StatusString = "Selecting " + GridMapItems.Where(item => item.IsSelected).ToList().Count + " items...";
        }
        private void ExcuteAreaCanvasMoveCommandDo(Models.ExParameters p)
        {
            if (!CanGridMapSelect)
                return;
            Canvas canvas = p.Parameter as Canvas;
            MouseEventArgs e = p.EventArgs as MouseEventArgs;
            Point point = e.GetPosition(canvas);
            //System.Diagnostics.Debug.WriteLine(point.X + "--" + point.Y + "=================" + DownPoint.X + "--" + DownPoint.Y);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (DownPoint.X == 0 && DownPoint.Y == 0)
                {
                    DownPoint = point;
                }
                AreaRectWidth = Math.Abs(point.X - DownPoint.X);
                AreaRectHeight = Math.Abs(point.Y - DownPoint.Y);
                AreaRectLeftPad = Math.Min(point.X, DownPoint.X);
                AreaRectTopPad = Math.Min(point.Y, DownPoint.Y);
            }
        }
        private void ExecuteCanSelectCommandDo()
        {
            CanGridMapSelect = true;
        }
        private void ExecuteCancelSelectCommandDo()
        {
            CanGridMapSelect = false;
            for (int i = 0; i < GridMapItems.Count; i++)
                GridMapItems[i].IsSelected = false;
            ExecuteModifyMapItemCommand.RaiseCanExecuteChanged();
            ExecuteModifySelectedStorageCommand.RaiseCanExecuteChanged();
        }
        private void ExecuteSaveMapCommandDo()
        {
            try
            {
                _map.SaveToFile();
                MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }
        private void ExecuteModifyStaticGoodsTypesCommandDo()
        {
            wpfSimulation.Views.StaticGoodsTypesView modifyWindow = new wpfSimulation.Views.StaticGoodsTypesView();
            StaticGoodsTypesViewModels msgtvm = new StaticGoodsTypesViewModels(this._map, modifyWindow);
            modifyWindow.Owner = Application.Current.MainWindow;
            modifyWindow.DataContext = msgtvm;
            modifyWindow.ShowDialog();
            RefreshGridMapItems();//refresh the map item view
        }
        private void ExecuteCreateMapCommandDo()
        {
            wpfSimulation.Views.CreateMapView createWindow = new wpfSimulation.Views.CreateMapView();
            CreateMapViewModels cmvm = new CreateMapViewModels(_map, createWindow, RefreshMapLayers);
            createWindow.Owner = Application.Current.MainWindow;
            createWindow.DataContext = cmvm;
            createWindow.ShowDialog();
        }
        private void ExecuteModifySelectedStorageCommandDo()
        {
            Views.ModifySelectedStorageView2 stv = new Views.ModifySelectedStorageView2();
            List<SingleGridMapItemViewModels> SelectedMapItems = GridMapItems.Where(p => p.IsSelected).ToList();
            ModifySelectedStorageViewModels2 mssvm = new ModifySelectedStorageViewModels2(_map, SelectedMapItems, stv);
            stv.Owner = Application.Current.MainWindow;
            stv.DataContext = mssvm;
            stv.ShowDialog();
        }
        private bool CanExecuteModifySelectedStorageCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(p => p.IsSelected).ToList();
            //only when all the selected map items are storages, the command can be execute
            if (selected.Count ==0 || selected.Count > 0 && (selected.Any(p => p.Type != Models.Classes.MapItem.ItemTypes.EMPTY_STORAGE
                 && p.Type != Models.Classes.MapItem.ItemTypes.FULL_STORAGE)))
                return false;
            else
                return true;
        }
        private void ExecuteModifyMapInformationCommandDo()
        {
            Views.ModifyMapInformationView miv = new Views.ModifyMapInformationView();
            ModifyMapInformationViewModels mmivm = new ModifyMapInformationViewModels(_map, miv);
            miv.Owner = Application.Current.MainWindow;
            miv.DataContext = mmivm;
            miv.ShowDialog();
        }
        private void ExecuteAddStoreRequestsCommandDo()
        {
            //TODO:NaonzaX ->to change the constructor of the view model
            //use the same view and inherit the view models, override the command function
            Views.ModifySelectedStorageView2 asrv = new Views.ModifySelectedStorageView2();
            AddStoreRequestsViewModels asrvm = new AddStoreRequestsViewModels(_map, asrv, StoreRequests);
            asrvm.CallBackMessage = new AddStoreRequestsViewModels.StatusShowCallBackFunction(SetStatusStringErasing);
            asrv.Owner = Application.Current.MainWindow;
            asrv.DataContext = asrvm;
            asrv.Show();
        }
        private void ExecuteAddTakeOutRequestsCommandDo()
        {
            //TODO:NazonaX ->to change the constructor of the view model
            Views.AddTakeOutRequestsView atorv = new Views.AddTakeOutRequestsView();
            AddTakeOutRequestsViewModels atorvm = new AddTakeOutRequestsViewModels(_map, atorv, TakeOutRequests);
            atorvm.CallBackMessage = new AddTakeOutRequestsViewModels.StatusShowCallBackFunction(SetStatusStringErasing);
            atorv.Owner = Application.Current.MainWindow;
            atorv.DataContext = atorvm;
            atorv.Show();
        }
        #endregion

        private void RefreshGridMapItems()
        {
            for (int i = 0; i < GridMapItems.Count; i++)
            {
                //maybe the storage will be type none
                GridMapItems[i].RefreshTypeColor();
            }
        }
        private void RefreshMapLayers()
        {
            InitialLayers();
        }
        /// <summary>
        /// for wait for 3s and then erase the Status String
        /// </summary>
        /// <param name="str"></param>
        private void SetStatusStringErasing(string str)
        {
            StatusString = str;
            if (StatusCounter == int.MaxValue)
                StatusCounter = 0;
            else
                StatusCounter++;
            Thread thread = new Thread(delegate () { WaitAndErase(StatusCounter); });
            thread.Start();
        }
        private void WaitAndErase(int statusCounter)
        {
            Thread.Sleep(3000);
            if (StatusCounter == statusCounter)
                StatusString = "Ready";
        }
    }
}
