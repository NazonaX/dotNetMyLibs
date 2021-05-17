using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using wpfSimulation.ViewModels;

namespace wpfSimulation.ViewModels
{
   public  class AddZonesViewModels: BaseViewModels
    {
        private Models.Entity.Map _map = null;
        private Window _self = null;
        private List<Models.Entity.Zone> _addZones = new List<Models.Entity.Zone>();
        private string _textBoxString = "";
        private string _textColorString = "#FFFF0000";

        public DelegateCommand ExecuteAddZonesCommand { get; private set; }
        public DelegateCommand ExecuteRestCommand { get; private set; }

        public AddZonesViewModels(Models.Entity.Map map, Window selfWindow)
        {
            ExecuteAddZonesCommand = new DelegateCommand(ExecuteAddZonesCommandDo);
            ExecuteRestCommand = new DelegateCommand(ExecuteRestCommandDo);
           
            this._map = map;
            this._self = selfWindow;
        }
        public string TextBoxString
        {
            get { return _textBoxString; }
            set
            {
                _textBoxString = value;
                ExecuteAddZonesCommand.RaiseCanExecuteChanged();
                ExecuteAddZonesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("TextBoxString");
            }
        }
        public string TextColorString
        {
            get { return _textColorString; }
            set
            {
                _textColorString = value;
                ExecuteAddZonesCommand.RaiseCanExecuteChanged();
                ExecuteAddZonesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("TextColorString");
            }
        }
        #region Commands

        private void ExecuteRestCommandDo()
        {
            TextColorString = "";
            TextBoxString = "";
        }

    private void ExecuteAddZonesCommandDo()
        {
            Models.Entity.Zone tmp = new Models.Entity.Zone();
            tmp.Name = TextBoxString;
            tmp.Color = TextColorString;//Models.Entity.Zone.NextRandomColor();
            _addZones.Add(tmp);
            using (TransactionScope trans = new TransactionScope())
            {
                Models.Service.IMapSingletonService mapSingletonService = Models.Service.MapSingletonService.Instance;
                mapSingletonService.GetZonesService().InsertZones(_addZones);
                trans.Complete();
                MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.BT_Save);
                if (confirmToDel == MessageBoxResult.OK)
                    _self.Close();
            }
        }
        #endregion
    }
}
