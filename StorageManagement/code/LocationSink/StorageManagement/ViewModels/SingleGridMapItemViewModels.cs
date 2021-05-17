using Models.Service;
using Models.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Entity.MapItems;


namespace wpfSimulation.ViewModels
{
    public class SingleGridMapItemViewModels: BaseViewModels
    {
        private Models.Entity.MapItems _item = null;

        #region for view
        private int _width = 0;
        private int _height = 0;
        private int _topPad = 0;
        private int _leftPad = 0;
        private bool isSelected = false;
        public static int Rect_Padding = 4;
        public static int Rect_Width = 15;
        private int  _strokeThickness=1;
        private string _fillColor= "#50514f";
        private int typeID=0;
        private int Rockt = 0;
        private int Colmt = 0;
        
        #endregion

        public Models.Entity.MapItems SingleStorage {
            get { return _item; }
            set {
                _item = value;
                IsSelected = false;
                if ((_item.Column == -1|| _item.Column == Colmt||_item.Rack == -1 || _item.Rack == Rockt))
                {
                    StrokeThicknessNum = 1;
                    OnPropertyChanged("Color");
                    FillColor = Color;         
                }
                else
                {
                    if (_item.TypeId == typeID)
                    {
                        StrokeThicknessNum = 5;
                        if (_item.Status == MapItemStatus.STATUS_EMPTY)
                            FillColor = "#ffffff";
                        else if (_item.Status == MapItemStatus.STATUS_LOCK)
                            FillColor = "#000000";
                        else
                            FillColor = "#808080";
                        OnPropertyChanged("Color");
                    }
                    else
                    {
                        StrokeThicknessNum = 1;
                        OnPropertyChanged("Color");
                        FillColor = Color;
                    }
                }
            }
        }
       
        public SingleGridMapItemViewModels(int w, int p, Models.Entity.MapItems mapitem,
            int i, int j,int typeid,int Rockcont,int colmcont)
        {
            SingleStorage = mapitem;
            Width = w;
            Height = w;
            TopPad = i * (Width + p) + p;
            LeftPad = j * (Width + p) + p;
             typeID = typeid;
             Rockt = Rockcont;
             Colmt = colmcont;
    }

        #region pulic property
        public int Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }

        }
        public int TopPad
        {
            get { return _topPad; }
            set { _topPad = value; OnPropertyChanged("TopPad"); }
        }
       
        
        public int LeftPad
        {
            get { return _leftPad; }
            set { _leftPad = value; OnPropertyChanged("LeftPad"); }
        }
        public int StrokeThicknessNum
        {
            get { return _strokeThickness; }
            set
            {
                _strokeThickness = value;
                OnPropertyChanged("StrokeThicknessNum");
            }
        }
        public string FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                OnPropertyChanged("FillColor");
            }
        }
        public string Color
        {
            get
            {
                return Models.Service.MapSingletonService.Instance.GetColor(_item);
            }
        }
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public int TypeId
        {
            get { return _item.TypeId; }
            set
            {
                _item.TypeId = value;
                OnPropertyChanged("Color");
            }
        }
        public int ZoneId
        {
            get { return _item.ZoneId; }
            set
            {
                _item.ZoneId = value;
                OnPropertyChanged("Color");
            }
        }
        #endregion

        public void RefreshTypeColor()
        {
            OnPropertyChanged("Color");
        }
       

    }
}
