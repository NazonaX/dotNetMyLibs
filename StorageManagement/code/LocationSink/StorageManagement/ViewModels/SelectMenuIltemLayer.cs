using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    public class SelectMenuIltemLayer : BaseViewModels
    {

        public DelegateCommand<Object> ExecuteLayerCommand { get; set; }
        public string Text { get; set; }
        public int Data { get; set; }
        private bool _checked = false;
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value;
                OnPropertyChanged("Checked"); }
        }
    }
}
