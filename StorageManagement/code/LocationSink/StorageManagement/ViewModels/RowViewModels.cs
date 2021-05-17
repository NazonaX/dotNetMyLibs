using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    public class RowViewModels:BaseViewModels
    {
        private Models.Entity.MapItems _item = null;

        #region for view
        private int _width = 0;
        private int _height = 0;
        private int _topPad = 0;
        private int _leftPad = 0;
        private bool isSelected = false;
        public string _text = "";
        #endregion

        public RowViewModels(int w, int p, Models.Entity.MapItems mapitem,
           int i, int j)
        {
            _item = mapitem;
            Width = w;
            Height = w;
            TopPad = i* (Width + p) + p;
            LeftPad = j* (Width + p) + p;

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
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }

        }

        public int LeftPad
        {
            get { return _leftPad; }
            set { _leftPad = value; OnPropertyChanged("LeftPad"); }
        }
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        public int Rack { get { return _item.Rack; } }
        public int Column { get { return _item.Column; } }

        #endregion

    }
}
