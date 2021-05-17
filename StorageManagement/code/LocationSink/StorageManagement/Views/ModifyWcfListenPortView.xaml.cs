using System.Windows;
using wpfSimulation.ViewModels;

namespace wpfSimulation.Views
{
    /// <summary>
    /// ModifyWcfListenPortView.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyWcfListenPortView : Window
    {
        public ModifyWcfListenPortView()
        {
            InitializeComponent();
            this.DataContext = new ModifyWcfListenPortViewModel();
        }
    }
}
