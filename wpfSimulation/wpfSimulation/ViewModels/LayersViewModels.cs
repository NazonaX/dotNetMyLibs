using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    public class LayersViewModels: BaseViewModels
    {
        #region for view
        private int _layerNumber;
        private bool _isSelected;
        public LayersViewModels(int layerNumber)
        {
            LayerNumber = layerNumber;
            IsSelected = false;
        }
        public LayersViewModels() { }
        public int LayerNumber
        {
            get { return _layerNumber; }
            set
            {
                _layerNumber = value;
                OnPropertyChanged("LayerNumber");
            }
        }
        public string LayerNumberString {
            get { return "The " + LayerNumber + " layer"; }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion
    }
}
