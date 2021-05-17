using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using wpfSimulation.Views;
using WCFService;
using Models.Entity;
using Models.Service.Repository;

namespace wpfSimulation.ViewModels
{
    public class MapEditViewModels : BaseViewModels
    {
        #region properties
        private Models.Entity.Map _map = null;
        private int _padGridNumber = 1;

        public ObservableCollection<SelectMenuIltemLayer> SelectMenuIltemLayer { get; set; }
        public ObservableCollection<SelectComboxList> SelectComboxList { get; set; }

        private ObservableCollection<SingleGridMapItemViewModels> _gridMapItems = new ObservableCollection<SingleGridMapItemViewModels>();
        private ObservableCollection<ColumnViewModels> _columnMapItems = new ObservableCollection<ColumnViewModels>();
        private ObservableCollection<RowViewModels> _rowMapItems = new ObservableCollection<RowViewModels>();

        private ObservableCollection<LayersViewModels> _Layers = new ObservableCollection<LayersViewModels>();

       
        private AreaRect _areaRectangle = new AreaRect();
        private int _selectedLayer = -1;
        private int _layerCount = -1;
        private int maxRackLength = 0;
        private int maxRackWidth = 0;
        private int maxColumnWidth = 0;
        private int maxRowWidth = 0;
        private bool _canGridMapSelect = true;
        private int StatusCounter = 0;//used for control the status string erasing
        private string _statusString = "";
        private string _wcfStatusString = "WCF服务关闭（点击开启）";//WCF测试

        private int typeId = 0;

        //for Store requests, store all the sliced set int requests
        //private List<Models.Classes.Request> StoreRequests = new List<Models.Classes.Request>();
        //for Take Out requests, store all the sliced take out requests
        //private List<Models.Classes.Request> TakeOutRequests = new List<Models.Classes.Request>();

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
        public DelegateCommand ExecuteModifyStaticZonesCommand { get; private set; }

        public DelegateCommand ExecuteAddZonesCommand { get; private set; }
        public DelegateCommand ExecuteModifyMapItemCommand { get; private set; }
        public DelegateCommand ExecuteCreateMapCommand { get; private set; }
        public DelegateCommand ExecuteModifySelectedStorageCommand { get; private set; }
        public DelegateCommand ExecuteModifyMapInformationCommand { get; private set; }
        public DelegateCommand ExecuteRefreshAllMapsCommand { get; private set; }
        public DelegateCommand ExecuteDeleteSelectedGoods { get; private set; }
        public DelegateCommand ExecuteEditSelectedStorageZonesCommand { get; private set; }
        public DelegateCommand ExecuteConnectTwoMapItemsCommand { get; private set; }
        public DelegateCommand ExecuteDeleteSpecialConnectionCommand { get; private set; }
        public DelegateCommand ExecuteInitializeBlankCargowaysCommand { get; private set; }
        public DelegateCommand ExecuteLoadCargowaysCommand { get; private set; }
        public DelegateCommand ExecuteMarkCargowayCommand { get; private set; }

        public DelegateCommand ExecuteSetWholeColumnAsRailCommand { get; private set; }

        public DelegateCommand ExecuteWcfServiceTestStartCommand { get; private set; }
        public DelegateCommand ExecuteWcfServiceTestCloseCommand { get; private set; }
        public DelegateCommand ExecuteChangeWcfStatusCommand { get; private set; }

        public DelegateCommand ExecuteTestAddStorageCommand { get; private set; }
        #endregion

        public MapEditViewModels()
        {
          
            Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
            _map = mapSingletonService.GetMap();//to keep a reference, may be null
            typeId = Models.Service.MapSingletonService.Instance.Type_GetStorageId();

            InitialCommands();
            InitialLayers();
            StatusString = "Ready";
            if (_map.CargoWays != null && _map.CargoWays.Count > 0)
                ExecuteLoadCargowaysCommandDo();
        }

        private void InitialCommands()
        {
            //used commands
            ExecuteCreateMapCommand = new DelegateCommand(ExecuteCreateMapCommandDo);
            ExecuteSaveMapCommand = new DelegateCommand(ExecuteSaveMapCommandDo);
            ExecuteCanSelectCommand = new DelegateCommand(ExecuteCanSelectCommandDo);
            ExecuteCancelSelectCommand = new DelegateCommand(ExecuteCancelSelectCommandDo);
            ExecuteModifyMapInformationCommand = new DelegateCommand(ExecuteModifyMapInformationCommandDo, CanExecuteModifyMapInformationCommandDo);

            ExcuteAreaCanvasLeftButtonDownCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonDownCommandDo);
            ExcuteAreaCanvasLeftButtonUpCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonUpCommandDo);
            ExcuteAreaCanvasMoveCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasMoveCommandDo);

            ExecuteModifyMapItemCommand = new DelegateCommand(ExecuteModifyMapItemCommandDo, CanExecuteModifyMapItemCommandDo);
            ExecuteConnectTwoMapItemsCommand = new DelegateCommand(ExecuteConnectTwoMapItemsCommandDo, CanExecuteConnectTwoMapItemsCommandDo);
            ExecuteModifyStaticZonesCommand = new DelegateCommand(ExecuteModifyStaticZonesCommandDo);
            ExecuteAddZonesCommand = new DelegateCommand(ExecuteAddZonesCommandDo);

            ExecuteEditSelectedStorageZonesCommand = new DelegateCommand(ExecuteEditSelectedStorageZonesCommandDo, CanExecuteEditSelectedStorageZonesCommandDo);

            //unused commands
            //can be used to set goods by hand
            ExecuteModifySelectedStorageCommand = new DelegateCommand(ExecuteModifySelectedStorageCommandDo, CanExecuteModifySelectedStorageCommandDo);
            //can be used to refresh the map, as reloading map from database
            ExecuteRefreshAllMapsCommand = new DelegateCommand(ExecuteRefreshAllMapsCommandDo);
            //can be used to delete goods by hand
            ExecuteDeleteSelectedGoods = new DelegateCommand(ExecuteDeleteSelectedGoodsDo, CanExecuteDeleteSelectedGoodsDo);
            //can be used to delete the special connection between two grids which are connected
            ExecuteDeleteSpecialConnectionCommand = new DelegateCommand(ExecuteDeleteSpecialConnectionCommandDo, CanExecuteDeleteSpecialConnectionCommandDo);
            //can be used to initialize blank cargoway, all infomations will be recalculated and be stored in the database
            ExecuteInitializeBlankCargowaysCommand = new DelegateCommand(ExecuteInitializeBlankCargowaysCommandDo);
            //can be used to load cargoways with goods and cargowaylocks, and to buid up indexer
            ExecuteLoadCargowaysCommand = new DelegateCommand(ExecuteLoadCargowaysCommandDo, CanExecuteLoadCargowaysCommandDo);
            ExecuteMarkCargowayCommand = new DelegateCommand(ExecuteMarkCargowayCommandDo, CanExecuteMarkCargowayCommandDo);
            ExecuteSetWholeColumnAsRailCommand = new DelegateCommand(ExecuteSetWholeColumnAsRailCommandDo, CanExecuteSetWholeColumnAsRailCommandDo);

            ExecuteWcfServiceTestStartCommand = new DelegateCommand(ExecuteWcfServiceTestStartCommandDo);
            ExecuteWcfServiceTestCloseCommand = new DelegateCommand(ExecuteWcfServiceTestCloseCommandDo);
            ExecuteChangeWcfStatusCommand = new DelegateCommand(ExecuteChangeWcfStatusCommandDo);

            ExecuteTestAddStorageCommand = new DelegateCommand(ExecuteTestAddStorageCommandDo);
        }

        


        /// <summary>
        /// if the _map is empty then set all available and have no values; or initial all
        ///  with _map...Including Layers, _gridMapItems, SelectedLayer, LayerCount, MaxRackLength
        ///  and MaxRackWidth;
        ///  use _padGrids to add extra rows and columns for special mapitems
        /// </summary>
        private void InitialLayers()
        {
            GridMapItems.Clear();
            Layers.Clear();
            MaxRackLength = 0;
            MaxRackWidth = 0;
            if (_map == null)
            {
                return;
            }
            else
            {
                //wrap an outer of _padGridNumber width with unavailable mapitems
                for (int i = 0; i <_map.RackCount + 2 * _padGridNumber; i++)
                {
                    for (int j = 0; j <_map.ColumnCount + 2 * _padGridNumber; j++)
                    {                      
                        if (i < _padGridNumber || i >= _map.RackCount + _padGridNumber
                            || j < _padGridNumber || j >= _map.ColumnCount + _padGridNumber)
                        {
                                                    
                            Models.Entity.MapItems tmp = new Models.Entity.MapItems();
                            tmp.Rack = i - _padGridNumber;
                            tmp.Column = i - _padGridNumber;
                            GridMapItems.Add(new SingleGridMapItemViewModels(SingleGridMapItemViewModels.Rect_Width ,
                                SingleGridMapItemViewModels.Rect_Padding , tmp, i, j,typeId,_map.RackCount,_map.ColumnCount));
                        }
                        else 
                        {
                            GridMapItems.Add(new SingleGridMapItemViewModels(SingleGridMapItemViewModels.Rect_Width ,
                                SingleGridMapItemViewModels.Rect_Padding, _map[0, i - _padGridNumber, j - _padGridNumber], i, j,typeId, _map.RackCount, _map.ColumnCount));
                        }
                    }
                }
                //initialize the column and row bar on the left and bottom of the window
                for (int i = 0; i < _map.ColumnCount + 2 * _padGridNumber; i++)
                {
                    Models.Entity.MapItems tmp = new Models.Entity.MapItems();
                    tmp.Rack = 0;
                    tmp.Column = i - _padGridNumber;
                    if (i != 0 && i != _map.ColumnCount + 2 * _padGridNumber - 1)
                    {
                        ColumnMapItems.Add(new ColumnViewModels(SingleGridMapItemViewModels.Rect_Width, SingleGridMapItemViewModels.Rect_Padding, tmp, 0, i)
                        {
                            Text = i.ToString()
                        });
                    }
                }
                for (int j = 0; j < _map.RackCount + 2 * _padGridNumber; j++)
                {
                    Models.Entity.MapItems tmp = new Models.Entity.MapItems();
                    tmp.Rack = j - _padGridNumber;
                    tmp.Column = 0;
                    if (j != 0 && j != _map.RackCount + 2 * _padGridNumber - 1)
                    {
                        RowMapItems.Add(new RowViewModels(SingleGridMapItemViewModels.Rect_Width, SingleGridMapItemViewModels.Rect_Padding, tmp, j, 0)
                        {
                            Text = j.ToString()
                        });
                    }
                }

                //menu->Layer selection
                SelectMenuIltemLayer = new ObservableCollection<SelectMenuIltemLayer>();
                for (int i = 0; i < _map.LayerCount; i++)
                {
                    SelectMenuIltemLayer t = new SelectMenuIltemLayer()
                    {
                        Data = i + 1,
                        ExecuteLayerCommand = new DelegateCommand<Object>(functest),
                        Text = "第" + (i + 1) + "层",
                        Checked = false
                    };
                    SelectMenuIltemLayer.Add(t);
                }
                //menu Combox
                SelectComboxList = new ObservableCollection<SelectComboxList>();
                for (int i = 0; i < _map.LayerCount; i++)
                {
                    SelectComboxList s = new SelectComboxList()
                    {
                        Data = i + 1,
                        ExecuteLayerCommand = new DelegateCommand<Object>(functest1),
                        Text = "第" + (i + 1) + "层"

                    };
                    SelectComboxList.Add(s);
                }
                SelectedLayer = 0;
                _layerCount = _map.LayerCount;
                
                //calculate for the canvas max width and height for visual
                //Conver 90 Anti-clock
                //length for hroizonal
                MaxRackLength = (SingleGridMapItemViewModels.Rect_Width + SingleGridMapItemViewModels.Rect_Padding) * (_map.ColumnCount + 2 * _padGridNumber)
                    + SingleGridMapItemViewModels.Rect_Padding * 3;
                //width for vertical
                MaxRackWidth = (SingleGridMapItemViewModels.Rect_Width + SingleGridMapItemViewModels.Rect_Padding) * (_map.RackCount + 2 * _padGridNumber)
                    + SingleGridMapItemViewModels.Rect_Padding * 3;
                MaxColumnWidth = SingleGridMapItemViewModels.Rect_Width + SingleGridMapItemViewModels.Rect_Padding * 3;
                MaxRowWidth = SingleGridMapItemViewModels.Rect_Width + SingleGridMapItemViewModels.Rect_Padding * 3;
            }
            
        }



#region properties' gets sets
        public ObservableCollection<SingleGridMapItemViewModels> GridMapItems
        {
            get { return _gridMapItems; }
            private set
            {
                _gridMapItems = value;
                OnPropertyChanged("GridMapItems");
            }
        }
        public ObservableCollection<ColumnViewModels> ColumnMapItems
        {
            get { return _columnMapItems; }
            private set
            {
                _columnMapItems = value;
                OnPropertyChanged("ColumnMapItems");
            }

        }
        public ObservableCollection<RowViewModels> RowMapItems
        {
            get { return _rowMapItems; }
            private set
            {
                _rowMapItems = value;
                OnPropertyChanged("RowMapItems");
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
        public int MaxRowWidth
        {
            get
            {
                return maxRowWidth;
            }
            set
            {
                maxRowWidth = value;
                OnPropertyChanged("MaxRowWidth");
            }

        }
        public int MaxColumnWidth
        {
            get
            {
                return maxColumnWidth;
            }
            set
            {
                maxColumnWidth = value;
                OnPropertyChanged("MaxColumnWidth");
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
                _selectedLayer = value;
                if (value == -1)
                    return;
                OnPropertyChanged("SelectedLayer");

                for (int i = 0; i < _map.LayerCount; i++)
                {

                    SelectMenuIltemLayer[i].Checked = false;


                }
                SelectMenuIltemLayer[SelectedLayer].Checked = true;

                int afterRack = _map.RackCount + 2 * _padGridNumber;
                int afterColumn = _map.ColumnCount + 2 * _padGridNumber;
                for (int i = 0; i < afterRack; i++)
                {
                    for (int j = 0; j < afterColumn; j++)
                    {
                        
                        if(i < _padGridNumber || i >= _map.RackCount + _padGridNumber
                            || j < _padGridNumber || j >= _map.ColumnCount + _padGridNumber)
                        {
                            Models.Entity.MapItems tmp = _map.SpecialMapItems.SingleOrDefault(item => item.Layer == SelectedLayer
                                                                                                 && item.Rack == i - _padGridNumber
                                                                                                 && item.Column == j - _padGridNumber);
                            Models.Entity.MapItems newone = new Models.Entity.MapItems();
                            newone.Rack = i - _padGridNumber;
                            newone.Column = j - _padGridNumber;
                            GridMapItems[i * afterColumn + j].SingleStorage = tmp == null ? newone : tmp;
                        }
                        else
                        {
                            GridMapItems[i * afterColumn + j].SingleStorage = _map[_selectedLayer, i - _padGridNumber, j - _padGridNumber];
                        }
                    }
                }

                OnPropertyChanged("GridMapItems");
            }
        }
        public int LayerCount
        {
            get{ return _layerCount; }
            private set { _layerCount = value; }
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
        public string WcfStatusString
        {
            get { return _wcfStatusString; }
            set
            {
                _wcfStatusString = value;
                OnPropertyChanged("WcfStatusString");
            }
        }

        public ColumnViewModels ColumnSelected
        {
            set
            {
                value.IsSelected = false;
                //System.Diagnostics.Debug.WriteLine(value.Rack + "--" + value.Column);
                foreach (SingleGridMapItemViewModels item in GridMapItems)
                {
                    if (item.SingleStorage.Rack < 0 || item.SingleStorage.Rack >= _map.RackCount
                        || item.SingleStorage.Column < 0 || item.SingleStorage.Column >= _map.ColumnCount)
                        item.IsSelected = false;
                    else if (item.SingleStorage.Column == value.Column)
                        item.IsSelected = true;
                    else
                        item.IsSelected = false;
                }
            }
            get
            { return null; }
        }
        public RowViewModels RowSelected
        {
            set
            {
                value.IsSelected = false;
                //System.Diagnostics.Debug.WriteLine(value.Rack + "--" + value.Column);
                foreach (SingleGridMapItemViewModels item in GridMapItems)
                {
                    if (item.SingleStorage.Rack < 0 || item.SingleStorage.Rack >= _map.RackCount
                        || item.SingleStorage.Column < 0 || item.SingleStorage.Column >= _map.ColumnCount)
                        item.IsSelected = false;
                    else if (item.SingleStorage.Rack == value.Rack)
                        item.IsSelected = true;
                    else
                        item.IsSelected = false;
                }
            }
            get
            { return null; }
        }
        #endregion

        #region Commands

        private void ExecuteCreateMapCommandDo()
        {
            if(_map != null)
            {
                //alert for delete now existance
                MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.CreateMap_Alert_ConfirmDelete,
                    Localiztion.Resource.Alert, MessageBoxButton.OKCancel);
                if (confirmToDel == MessageBoxResult.OK)
                {
                    //delete map and create a new one
                    Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                    mapSingletonService.DeleteMap();
                    _map = null;//set to null and initial the view
                    InitialLayers();
                }
                else
                    return;
            }
            wpfSimulation.Views.CreateMapView createWindow = new wpfSimulation.Views.CreateMapView();
            CreateMapViewModels cmvm = new CreateMapViewModels(_map, createWindow, RefreshMapLayers);
            createWindow.Owner = Application.Current.MainWindow;
            createWindow.DataContext = cmvm;
            createWindow.ShowDialog();
        }
        private void ExecuteSaveMapCommandDo()
        {
            try
            {
                //for now is just save base map infos
                //the zones and goods should be saved at other sides!!
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.SaveMap();
                MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
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
            ExecuteDeleteSelectedGoods.RaiseCanExecuteChanged();
            ExecuteSetWholeColumnAsRailCommand.RaiseCanExecuteChanged();
            ExecuteMarkCargowayCommand.RaiseCanExecuteChanged();
            ExecuteSetWholeColumnAsRailCommand.RaiseCanExecuteChanged();
        }
        private void ExcuteAreaCanvasLeftButtonDownCommandDo(Models.ExParameters p)
        {
            //nothing happens
        }
        private void ExcuteAreaCanvasLeftButtonUpCommandDo(Models.ExParameters p)
        {
            if (!CanGridMapSelect)
                return;
            Canvas canvas = p.Parameter as Canvas;
            MouseButtonEventArgs e = p.EventArgs as MouseButtonEventArgs;
            Point point = e.GetPosition(canvas);
            if (!(DownPoint.X == 0 && DownPoint.Y == 0)
                && Math.Abs(DownPoint.X - point.X) > AreaThreshold && Math.Abs(DownPoint.Y - point.Y) > AreaThreshold)
            {
                //To set Selected ListBox Item
                for (int i = 0; i < GridMapItems.Count; i++)
                {
                    if ((GridMapItems[i].TopPad + GridMapItems[i].Height > AreaRectTopPad && GridMapItems[i].TopPad < AreaRectTopPad + AreaRectHeight)
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
            ExecuteDeleteSelectedGoods.RaiseCanExecuteChanged();
            ExecuteEditSelectedStorageZonesCommand.RaiseCanExecuteChanged();
            ExecuteConnectTwoMapItemsCommand.RaiseCanExecuteChanged();
            ExecuteDeleteSpecialConnectionCommand.RaiseCanExecuteChanged();
            ExecuteSetWholeColumnAsRailCommand.RaiseCanExecuteChanged();
            ExecuteMarkCargowayCommand.RaiseCanExecuteChanged();
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
        private bool CanExecuteModifyMapItemCommandDo()
        {
            return GridMapItems.Any(t => t.IsSelected);
        }
        private void ExecuteModifyMapItemCommandDo()
        {
            List<SingleGridMapItemViewModels> SelectedMapItems = GridMapItems.Where(p => p.IsSelected).ToList();
            wpfSimulation.Views.ModifySelectedMapItemView win = new wpfSimulation.Views.ModifySelectedMapItemView();
            ModifySelectedMapItemsTypesViewModels msmivm = new ModifySelectedMapItemsTypesViewModels(SelectedMapItems, _map, win, RefreshGridMapItems, SetStatusStringErasing, SelectedLayer);
            win.Owner = Application.Current.MainWindow;
            win.DataContext = msmivm;
            win.ShowDialog();
        }
        private bool CanExecuteModifySelectedStorageCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(p => p.IsSelected).ToList();
            //only when all the selected map items are storages, the command can be execute
            if (selected.Count == 0 || selected.Any(p => !Models.Service.MapSingletonService.Instance.IsStorage(p.SingleStorage)))
                return false;
            else
                return true;
        }
        private void ExecuteModifySelectedStorageCommandDo()
        {
            Views.ModifySelectedStorageView2 stv = new Views.ModifySelectedStorageView2();
            List<SingleGridMapItemViewModels> SelectedMapItems = GridMapItems.Where(p => p.IsSelected).ToList();
            ModifySelectedStorageViewModels2 mssvm = new ModifySelectedStorageViewModels2(_map, SelectedMapItems, stv, RefreshGridMapItems, SetStatusStringErasing);
            stv.Owner = Application.Current.MainWindow;
            stv.DataContext = mssvm;
            stv.ShowDialog();
        }
        private void ExecuteModifyStaticZonesCommandDo()
        {
            //Renamed as Static Zones View, please be careful about that
            wpfSimulation.Views.StaticGoodsTypesView modifyWindow = new wpfSimulation.Views.StaticGoodsTypesView();
            StaticZonesViewModels msgtvm = new StaticZonesViewModels(_map, modifyWindow);
            modifyWindow.Owner = Application.Current.MainWindow;
            modifyWindow.DataContext = msgtvm;
            modifyWindow.ShowDialog();
            RefreshGridMapItems();//refresh the map item view
        }
        //增加于20190507
        private void ExecuteAddZonesCommandDo()
        {
            wpfSimulation.Views.AddZonesView AddZonesWindow = new wpfSimulation.Views.AddZonesView();
            AddZonesViewModels addvm = new AddZonesViewModels(_map, AddZonesWindow);
            AddZonesWindow.Owner = Application.Current.MainWindow;
            AddZonesWindow.DataContext = addvm;
            AddZonesWindow.ShowDialog();
            RefreshGridMapItems();
        }
        private bool CanExecuteModifyMapInformationCommandDo()
        {
            return _map != null;
        }
        private void ExecuteModifyMapInformationCommandDo()
        {
            Views.ModifyMapInformationView miv = new Views.ModifyMapInformationView();
            ModifyMapInformationViewModels mmivm = new ModifyMapInformationViewModels(_map, miv, SetStatusStringErasing);
            miv.Owner = Application.Current.MainWindow;
            miv.DataContext = mmivm;
            miv.ShowDialog();
        }
        private bool CanExecuteDeleteSelectedGoodsDo()
        {
            return CanExecuteModifySelectedStorageCommandDo();
        }
        private void ExecuteDeleteSelectedGoodsDo()
        {
            List<Models.Entity.Goods> deleList = new List<Models.Entity.Goods>();
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(p => p.IsSelected).ToList();
            using(TransactionScope scope = new TransactionScope())
            {
                foreach (SingleGridMapItemViewModels s in selected)
                {
                    Goods gt = _map.Goods.SingleOrDefault(g => g.MapItemsId == s.SingleStorage.MapItemID);
                    if (gt != null)
                    {
                        deleList.Add(gt);
                    }
                }
                Models.Service.MapSingletonService.Instance.GetGoodsService().DeleteGoods(deleList);
                Models.Service.MapSingletonService.Instance.GetMapItemsService().UpdateAllMapItems();
                scope.Complete();
                SetStatusStringErasing("Delete " + deleList.Count + " goods complete.");
            }
            RefreshGridMapItems();
        }
        private void ExecuteRefreshAllMapsCommandDo()
        {
            RefreshMapLayers();
        }
        private bool CanExecuteEditSelectedStorageZonesCommandDo()
        {
            return !GridMapItems.Where(mi => mi.IsSelected == true).Any(mi => !Models.Service.MapSingletonService.Instance.IsStorage(mi.SingleStorage));
        }
        private void ExecuteEditSelectedStorageZonesCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(mi => mi.IsSelected == true).ToList();
            HashSet<int> hasCargoway = new HashSet<int>();
            foreach (SingleGridMapItemViewModels singleSelected in selected)
            {
                if (hasCargoway.Contains(singleSelected.SingleStorage.CargowayId))
                    continue;
                hasCargoway.Add(singleSelected.SingleStorage.CargowayId);
                foreach (SingleGridMapItemViewModels sgvm in GridMapItems.Where(gmi => gmi.SingleStorage.CargowayId == singleSelected.SingleStorage.CargowayId).ToList())
                {
                    sgvm.IsSelected = true;
                }
            }
            List<SingleGridMapItemViewModels> SelectedMapItemsss = GridMapItems.Where(p => p.IsSelected).ToList();
            Views.EditSelectedStorageTypeView window = new Views.EditSelectedStorageTypeView();
            ViewModels.EditSelectedStorageTypeViewModels vm = new EditSelectedStorageTypeViewModels(_map, window, SelectedMapItemsss,
                RefreshGridMapItems, SetStatusStringErasing);
            window.DataContext = vm;
            window.ShowDialog();
        }
        private bool CanExecuteConnectTwoMapItemsCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(s => s.IsSelected).ToList();
            if (selected.Count != 2)
                return false;
            Models.Entity.MapItems item1 = selected[0].SingleStorage;
            Models.Entity.MapItems item2 = selected[1].SingleStorage;
            int Input = Models.Service.MapSingletonService.Instance.Type_GetInputId();
            int Output = Models.Service.MapSingletonService.Instance.Type_GetOutputId();
            int Lifter = Models.Service.MapSingletonService.Instance.Type_GetLifterId();
            int Rail = Models.Service.MapSingletonService.Instance.Type_GetRailId();
            if ((item1.TypeId == Input || item1.TypeId == Output) && item2.TypeId == Rail//io points and rail can be connected
                || (item2.TypeId == Input || item2.TypeId == Output) && item1.TypeId == Rail//reverse
                || (item1.TypeId == Input || item1.TypeId == Output) && item2.TypeId == Lifter//io points and lifter can be connected
                || (item2.TypeId == Input || item2.TypeId == Output) && item1.TypeId == Lifter//reverse
                || item1.TypeId == Lifter && item2.TypeId == Rail//rail and lifter cn be connected
                || item2.TypeId == Lifter && item1.TypeId == Rail)//reverse
                return true;
            else
                return false;
        }
        private void ExecuteConnectTwoMapItemsCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(s => s.IsSelected).ToList();
            Window self = new Views.EditSpecialConnectionView();
            ViewModels.EditSpecialConnectionViewModels vm = new EditSpecialConnectionViewModels(_map, self, SetStatusStringErasing,
                selected[0].SingleStorage, selected[1].SingleStorage);
            self.DataContext = vm;
            self.ShowDialog();
        }
        private bool CanExecuteDeleteSpecialConnectionCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(s => s.IsSelected).ToList();
            if (selected.Count != 2)
                return false;
            Models.Entity.MapItems item1 = selected[0].SingleStorage;
            Models.Entity.MapItems item2 = selected[1].SingleStorage;
            return _map.SpecialConnections.Any(scw => scw.MapItemFrom == item1.MapItemID && scw.MapItemTo == item2.MapItemID
                                        || scw.MapItemTo == item1.MapItemID && scw.MapItemFrom == item2.MapItemID);
                
        }
        private void ExecuteDeleteSpecialConnectionCommandDo()
        {
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(s => s.IsSelected).ToList();
            Models.Entity.MapItems item1 = selected[0].SingleStorage;
            Models.Entity.MapItems item2 = selected[1].SingleStorage;
            List<Models.Entity.SpecialConnection> todel = new List<Models.Entity.SpecialConnection>();
            Models.Entity.SpecialConnection sc1 = _map.SpecialConnections.SingleOrDefault(s => s.MapItemFrom == item1.MapItemID && s.MapItemTo == item2.MapItemID);
            Models.Entity.SpecialConnection sc2 = _map.SpecialConnections.SingleOrDefault(s => s.MapItemFrom == item2.MapItemID && s.MapItemTo == item1.MapItemID);
            if (sc1 != null)
                todel.Add(sc1);
            if (sc2 != null)
                todel.Add(sc2);
            //delete pair
            Models.Service.MapSingletonService.Instance.GetSpecialConnectionService().DeleteSpecialConnections(todel);
            MessageBox.Show("Complete.");
            SetStatusStringErasing("Delete " + todel.Count + " Special Connections.");
        }
        
        /// <summary>
        /// 调用该流程之后，特殊连接、货物、货道锁都会删除，货道信息重置，各个栅格点的所属货道id重置
        /// </summary>
        private void ExecuteInitializeBlankCargowaysCommandDo()
        {
            using(TransactionScope trans = new TransactionScope())
            {
                //delete cargoways and cargowaysnumber first
                Models.Service.MapSingletonService.Instance.GetCargoWaysService().DeleteAllCargoWays();
                Models.Service.MapSingletonService.Instance.GetCargoWaysLockService().DeleteCargoWaysLocks(new List<Models.Entity.CargoWaysLock>(_map.CargoWaysLocks));
                Models.Service.MapSingletonService.Instance.GetGoodsService().DeleteGoods(_map.Goods.Where(g => g.CargoWayLockId != 0).ToList());
                Models.Service.MapSingletonService.Instance.GetSpecialConnectionService().DeleteSpecialConnections(new List<SpecialConnection>(_map.SpecialConnections));
                //initialize new cargoways
                //in this method, the new cargoway will be set in the certain mapitem, so we need to update mapitems after save cargoways
                List<Models.Entity.CargoWays> newCWL = Models.Service.MapSingletonService.Instance.GetMapLogicsService().InitializeCargoPaths();
                List<Models.Entity.Rails> newRL = Models.Service.MapSingletonService.Instance.GetMapLogicsService().InitializeRails();
                //save cargoways and update mapitems
                Models.Service.MapSingletonService.Instance.GetCargoWaysService().InsertCargoWays(newCWL);
                Models.Service.MapSingletonService.Instance.GetRailsService().InsertRails(newRL);
                //set cargowaysId to MapItems
                foreach (Models.Entity.CargoWays cw in _map.CargoWays)
                {
                    int i = cw.LayerAt;
                    int j = cw.RackAt;
                    for (int k = cw.LeftRailColumn + 1; k < cw.RightRailColumn; k++)
                        _map[i, j, k].CargowayId = cw.Id;
                }
                Models.Service.MapSingletonService.Instance.GetMapItemsService().UpdateAllMapItems();
                trans.Complete();
                SetStatusStringErasing("Blank Cargoway initialization complete...");
            }
            RefreshGridMapItems();
            ExecuteLoadCargowaysCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteLoadCargowaysCommandDo()
        {
            return _map.CargoWays != null && _map.CargoWays.Count > 0;
        }
        private void ExecuteLoadCargowaysCommandDo()
        {
            if( _map.CSAcceleration == 0 || _map.CSDeceleration == 0 || _map.CSMaxSpeed == 0
                || _map.PSAcceleration == 0 || _map.PSDeceleration == 0 || _map.PSMaxSpeed == 0
                || _map.LAcceleration == 0 || _map.LDeceleration == 0 || _map.LMaxSpeed == 0
                || _map.GapAlongCloumn == 0 || _map.GapAlongRack == 0 || _map.GapBetweenLayers == 0)
            {
                SetStatusStringErasing("You need to set Map Basic Infos completely first...");
            }
            else
            {
                Models.Service.MapSingletonService.Instance.GetMapLogicsService().InitializeIndexer();
                SetStatusStringErasing("Load Cargoways complete...");
            }
        }
  
        private void ExecuteMarkCargowayCommandDo()
        {
            if (_map.CargoWays == null || _map.CargoWays.Count == 0)
            {
                SetStatusStringErasing("No Cargoways Infos for now, please do the initialization...");
                return;
            }
            List<SingleGridMapItemViewModels> selected = GridMapItems.Where(gmi => gmi.IsSelected).ToList();

            HashSet<int> hasCargoway = new HashSet<int>();
            foreach (SingleGridMapItemViewModels singleSelected in selected)
            {
                if (hasCargoway.Contains(singleSelected.SingleStorage.CargowayId))
                    continue;
                hasCargoway.Add(singleSelected.SingleStorage.CargowayId);
                foreach (SingleGridMapItemViewModels sgvm in GridMapItems.Where(gmi => gmi.SingleStorage.CargowayId == singleSelected.SingleStorage.CargowayId).ToList())
                {
                    sgvm.IsSelected = true;
                }
            }
        }
        private bool CanExecuteMarkCargowayCommandDo()
        {
            return GridMapItems.Any(t => t.IsSelected);

        }
        

        private void ExecuteSetWholeColumnAsRailCommandDo()
        {
            
            List<SingleGridMapItemViewModels> SelectedMapItem = GridMapItems.Where(p => p.IsSelected).ToList();
            HashSet<int> mapItemRack = new HashSet<int>();
            foreach (SingleGridMapItemViewModels selected in SelectedMapItem)
            {
                mapItemRack.Add(selected.SingleStorage.Column);
                foreach (SingleGridMapItemViewModels sgvm in GridMapItems.Where(g => g.SingleStorage.Column == selected.SingleStorage.Column).ToList())
                {
                    sgvm.IsSelected = true;
                }

            }
            List<SingleGridMapItemViewModels> SelectedMapItemsss = GridMapItems.Where(p => p.IsSelected).ToList();
            string MessageBoxText = "是否确认修改整列为轨道";
            string caption =" ";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(MessageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    List<Models.Entity.Goods> toDeleteList = new List<Models.Entity.Goods>();
                    List<Models.Entity.MapItems> toDeleteMIList = new List<MapItems>();
                    List<Models.Entity.MapItems> toAddMIList = new List<MapItems>();
                    //AssignTheChanges(toDeleteList, toDeleteMIList, toAddMIList);
                    int rail = _map.Types.SingleOrDefault(z => z.Name == Models.Service.MapSingletonService.ItemTypesString.ITEM_TYPE_RAIL).Id;

                    for (int i = 0; i < SelectedMapItemsss.Count-1; i++)
                    {
                        SelectedMapItemsss[i].SingleStorage.TypeId = rail;
                    }
                    ConfirmSave(toDeleteList, toAddMIList, toDeleteMIList);
                   
                    MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
                                                                     
                    break;
                case MessageBoxResult.No:
                                     
                    break;


            }
            RefreshGridMapItems();
            
        }

        private bool CanExecuteSetWholeColumnAsRailCommandDo()
        {
            return GridMapItems.Any(t => t.IsSelected);
        }


        //private void ExecuteAddStoreRequestsCommandDo()
        //{
        //    //TODO:NaonzaX ->to change the constructor of the view model
        //    //use the same view and inherit the view models, override the command function
        //    Views.ModifySelectedStorageView2 asrv = new Views.ModifySelectedStorageView2();
        //    AddStoreRequestsViewModels asrvm = new AddStoreRequestsViewModels(_map, asrv, StoreRequests);
        //    asrvm.CallBackMessage = new AddStoreRequestsViewModels.StatusShowCallBackFunction(SetStatusStringErasing);
        //    asrv.Owner = Application.Current.MainWindow;
        //    asrv.DataContext = asrvm;
        //    asrv.Show();
        //}
        //private void ExecuteAddTakeOutRequestsCommandDo()
        //{
        //    //TODO:NazonaX ->to change the constructor of the view model
        //    Views.AddTakeOutRequestsView atorv = new Views.AddTakeOutRequestsView();
        //    AddTakeOutRequestsViewModels atorvm = new AddTakeOutRequestsViewModels(_map, atorv, TakeOutRequests);
        //    atorvm.CallBackMessage = new AddTakeOutRequestsViewModels.StatusShowCallBackFunction(SetStatusStringErasing);
        //    atorv.Owner = Application.Current.MainWindow;
        //    atorv.DataContext = atorvm;
        //    atorv.Show();
        //}

        #region test Logics Algorithm
        private void ExecuteTestAddStorageCommandDo()
        {
            Utils.IOOps.DeleteFile(Utils.Logger.LogPath);
            Models.Service.LogicsOrder randomOrder = new Models.Service.LogicsOrder();
            randomOrder.Mixable = false;
            randomOrder.DifferLayersFirst = true;
            randomOrder.BarCode = "1234";
            randomOrder.GoodBatch = "12F";
            randomOrder.GoodModel = "Rhoder";
            randomOrder.GoodName = "Amiya";
            randomOrder.ProductId = "8848-T";
            Models.Entity.MapItems m = _map.SpecialMapItems.Where(mi => mi.TypeId == Models.Service.MapSingletonService.Instance.Type_GetInputId()).ToList()[0];
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10);
            StringBuilder sb = new StringBuilder();
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            List<Models.Service.LockLocations> res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(12);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(10); randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(12);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(12); randomOrder.AddUnit(10);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(12);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(10);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(12);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11); randomOrder.AddUnit(12);
            randomOrder.AddUnit(10); randomOrder.AddUnit(11);
            sb.Append(randomOrder.Units.Count).Append("\r\n");
            res = Models.Service.MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(randomOrder, m);
            foreach (Models.Service.LockLocations ll in res)
            {
                sb.Append(ll.Layer + "--" + ll.Rack + "--" + ll.RailColumn + " : " + ll.LockStart + "--" + ll.LockEnd).Append("\r\n");
            }
            sb.Append("-----------------------\r\n");
            Utils.Logger.WriteMsgAndLog(sb.ToString());
        }
#endregion

#region 开启测试WCF服务Command
        private void ExecuteWcfServiceTestStartCommandDo()
        {
            ModifyWcfListenPortView childWindow = new ModifyWcfListenPortView();
            var childWinDialogResult = childWindow.ShowDialog();
            if (childWinDialogResult==true)
            {
                WcfStatusString = "WCF服务开启（点击关闭）";
            }
        }
#endregion

#region 关闭测试WCF服务Command
        private void ExecuteWcfServiceTestCloseCommandDo()
        {
            WCFServiceHost.GetInstance().HostClose();
            WcfStatusString = "WCF服务关闭（点击开启）";
        }
#endregion

#region 改变WCF服务状态

        private void ExecuteChangeWcfStatusCommandDo()
        {
            if (WCFServiceHost.GetInstance().host == null || WCFServiceHost.GetInstance().host.State == CommunicationState.Closed)
            {
                //开启WCF服务
                ExecuteWcfServiceTestStartCommandDo();
            }
            else
            {
                //关闭WCF服务
                ExecuteWcfServiceTestCloseCommandDo();
            }
        }

#endregion

#endregion

#region other methods
        private void RefreshGridMapItems()
        {
            SelectedLayer = SelectedLayer;
        }
        private void RefreshMapLayers()
        {
            Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
            _map = mapSingletonService.RefreshMap();
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
        private void ConfirmSave(List<Goods> toDeleteList, List<MapItems> toAddMIList, List<MapItems> toDeleteMIList)
        {
            //do as a transcaction
            using (TransactionScope trans = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.GetMapItemsService().InserSpecialtMapItems(toAddMIList);
                mapSingletonService.GetMapItemsService().DeleteSpecialMapItems(toDeleteMIList);
                mapSingletonService.GetGoodsService().DeleteGoods(toDeleteList);
                mapSingletonService.GetMapItemsService().UpdateAllMapItems();
                trans.Complete();
            }
        }
#endregion

        //TODO::data base first --> code first
        //TODO::database modification
        //NOTIFICATION:: The z.entityframework should be updated monthly, or will expire
        private void functest(Object data)
        {
            SelectedLayer = (int)data - 1;
            //for (int i = 0; i < _map.LayerCount; i++)
            //{
                
            //     SelectMenuIltemLayer[i].Checked = false;

      
            //}
            //SelectMenuIltemLayer[SelectedLayer].Checked = true;
            

        }
        private void functest1(Object data)
        {


        }
       
       
    }
}
