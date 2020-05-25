using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    public class AreaRect: BaseViewModels
    {
        private double _topPad;
        private double _leftPad;
        private double _width;
        private double _height;

        public AreaRect() { TopPad = 0;LeftPad = 0;Width = 0;Height = 0; }
        public AreaRect(double tp, double lp, double w, double h)
        {
            TopPad = tp;
            LeftPad = lp;
            Width = w;
            Height = h;
        }

        public double TopPad
        {
            get { return _topPad; }
            set
            {
                _topPad = value;
                OnPropertyChanged("TopPad");
            }
        }
        public double LeftPad
        {
            get { return _leftPad; }
            set
            {
                _leftPad = value;
                OnPropertyChanged("LeftPad");
            }
        }
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        public bool IsNull()
        {
            if (_leftPad == 0 && _topPad == 0 && _width == 0 && _height == 0)
                return true;
            else
                return false;
        }
        public void SetNull()
        {
            LeftPad = 0;
            TopPad = 0;
            Width = 0;
            Height = 0;
        }
    }
}
