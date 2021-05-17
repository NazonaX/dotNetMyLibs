using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSimulation.ViewModels
{
    public class SelectComboxList : BaseViewModels
    {

        public string Text { get; set; }
        public int Data { get; set; }
        public DelegateCommand<Object> ExecuteLayerCommand { get; set; }
        
    }
}
