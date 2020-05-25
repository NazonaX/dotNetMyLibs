using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //[STAThread]
        //public static void Main()
        //{
        //    Models.Classes.Map _map = null;
        //    if (Models.Classes.Map.CheckForFile())
        //    {
        //        try
        //        {
        //            _map = Models.Classes.Map.ReadFromFile();
        //        }
        //        catch (NullReferenceException ne)
        //        {
        //            _map = new Models.Classes.Map();
        //            System.Diagnostics.Debug.WriteLine(ne.Message + "\n" + ne.StackTrace);
        //        }
        //    }
        //    else
        //    {
        //        _map = new Models.Classes.Map();
        //    }
        //    Views.MapEditView mapedit = new Views.MapEditView();
        //    ViewModels.MapEditViewModels mevm = new ViewModels.MapEditViewModels(_map);
        //    mapedit.DataContext = mevm;
        //    Application app = new Application();
        //    app.Run(mapedit);
        //}
    }
}
