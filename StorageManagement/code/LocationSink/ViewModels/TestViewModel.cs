using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using ViewModels;

namespace ViewModels
{
    public class TestViewModel : BaseViewModels
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

        public DelegateCommand ExecuteModifyCommand { get; private set; }
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
        public DelegateCommand ExecuteSaveMapCommand { get; set; }
        public DelegateCommand ExecuteModifyStaticGoodsTypesCommand { get; set; }
        public DelegateCommand ExecuteModifyMapItemCommand { get; set; }
    #endregion
        public TestViewModel()
        {
            ExecuteModifyCommand = new DelegateCommand(ModifyCommandDo, CanModifyCommandDo);
            ExecuteCanSelectCommand = new DelegateCommand(ExecuteCanSelectCommandDo);
            ExecuteCancelSelectCommand = new DelegateCommand(ExecuteCancelSelectCommandDo);
            //ExcuteSelectedLayerChangedCommand = new DelegateCommand<int?>(SelectedLayerChangedCommandDo,canSelectedLayerChangedCommandDo);
            ExcuteAreaCanvasLeftButtonDownCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonDownCommandDo);
            ExcuteAreaCanvasLeftButtonUpCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasLeftButtonUpCommandDo);
            ExcuteAreaCanvasMoveCommand = new DelegateCommand<Models.ExParameters>(ExcuteAreaCanvasMoveCommandDo);
            ExecuteSaveMapCommand = new DelegateCommand(ExecuteSaveMapCommandDo);
            if (Models.Classes.Map.CheckForFile())
            {
                _map = Models.Classes.Map.ReadFromFile();
                if (_map.MapItems == null)
                    _map.SetMapFromScratch(10, 20, 30);
            }
            else
            {
                _map = new Models.Classes.Map(10, 20, 30);
            }
            _selectedLayer = 0;
            _layerCount = _map.LayerCount;
            gridMapItems.Clear();
            for (int i = 0; i < _map.RackCount * _map.ColumnCount; i++)
                gridMapItems.Add(new SingleGridMapItemViewModels());
            for (int i = 0; i < _map.RackCount; i++)
                for (int j = 0; j < _map.ColumnCount; j++)
                {
                    gridMapItems[i * _map.ColumnCount + j].SingleStorage = _map[SelectedLayer, i, j];
                    //System.Diagnostics.Debug.WriteLine(i + "-" + j + "--" + i * _map.RackCount + j);
                }
            for (int i = 0; i < _map.LayerCount; i++)
                _Layers.Add(new LayersViewModels(i + 1));
            _Layers[0].IsSelected = true;
            //calculate for the canvas max width and height for visual
            MaxRackLength = (Models.Classes.Map.Width + Models.Classes.Map.Padding) * _map.ColumnCount
                + Models.Classes.Map.Padding * 3;
            MaxRackWidth = _map.RackCount * (Models.Classes.Map.Width + Models.Classes.Map.Padding)
                + Models.Classes.Map.Padding * 3;

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
                OnPropertyChanged("MaxRackNumber");
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
                OnPropertyChanged("SelectedLayer");
                //GridMapItems.Clear();
                for (int i = 0; i < _map.RackCount; i++)
                {
                    for (int j = 0; j < _map.ColumnCount; j++)
                    {
                        GridMapItems[i * _map.ColumnCount + j].SingleStorage = _map[_selectedLayer, i, j];
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
                        GridMapItems[i * _map.ColumnCount + j].Type = randomtype;
                    }
                }
            }
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
        #endregion

        #region Commands
        private bool CanModifyCommandDo()
        {
            return gridMapItems.Any(t => t.IsSelected);
        }

        private void ModifyCommandDo()
        {
            
            throw new NotImplementedException();
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
                for (int i = 0; i < gridMapItems.Count; i++)
                {
                    if((gridMapItems[i].TopPad + gridMapItems[i].Height > AreaRectTopPad && gridMapItems[i].TopPad < AreaRectTopPad + AreaRectHeight)
                        && (gridMapItems[i].LeftPad + gridMapItems[i].Width > AreaRectLeftPad && gridMapItems[i].LeftPad < AreaRectLeftPad + AreaRectWidth))
                    {
                        gridMapItems[i].IsSelected = true;
                    }
                    else
                        gridMapItems[i].IsSelected = false;
                }
            }
            ExecuteModifyCommand.RaiseCanExecuteChanged();
            AreaRectHeight = 0;
            AreaRectLeftPad = 0;
            AreaRectWidth = 0;
            AreaRectHeight = 0;
            DownPoint = new Point();
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
            for (int i = 0; i < gridMapItems.Count; i++)
                gridMapItems[i].IsSelected = false;
            ExecuteModifyCommand.RaiseCanExecuteChanged();
        }
        private void ExecuteSaveMapCommandDo()
        {
            try
            {
                _map.SaveToFile();
                MessageBox.Show("Map Save Complete!");
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }
        private void ExecuteModifyStaticGoodsTypesCommandDo()
        {
            StaticGoodsTypesViewModels msgtvm = new StaticGoodsTypesViewModels(this._map);
            
        }
        #endregion
    }
}
