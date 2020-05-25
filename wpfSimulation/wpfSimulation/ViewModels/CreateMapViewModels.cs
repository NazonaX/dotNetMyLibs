using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class CreateMapViewModels: BaseViewModels
    {
        public delegate void CreateCallBackHandler();

        private CreateCallBackHandler CallBackFunction;
        private Models.Classes.Map _map = null;
        private Window self = null;
        private int _layerCount = 0;
        private int _rackCount = 0;
        private int _columnCount = 0;

        public DelegateCommand ExecuteConfirmAndCreateMapCommand { get; private set; }

        public CreateMapViewModels(Models.Classes.Map map, Window selfWindow, Action callback)
        {
            _map = map;
            self = selfWindow;
            CallBackFunction = new CreateCallBackHandler(callback);
            ExecuteConfirmAndCreateMapCommand = new DelegateCommand(ExecuteConfirmAndCreateMapCommandDo, CanExecuteConfirmAndCreateMapCommandDo);
        }

        public bool CanCreate = false;

        public string LayerCountString
        {
            get { if (_layerCount != 0) return _layerCount + ""; else return ""; }
            set
            {
                try
                {
                    _layerCount = Int32.Parse(value);
                    OnPropertyChanged("LayerCountString");
                    ExecuteConfirmAndCreateMapCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e)
                {
                    CanCreate = false;
                    _layerCount = 0;
                }
            }
        }
        public string RackCountString
        {
            get { if (_rackCount != 0) return _rackCount + ""; else return ""; }
            set
            {
                try
                {
                    _rackCount = Int32.Parse(value);
                    OnPropertyChanged("RackCountString");
                    ExecuteConfirmAndCreateMapCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e)
                {
                    CanCreate = false;
                    _rackCount = 0;
                }
            }
        }
        public string ColumnCountString
        {
            get { if (_columnCount != 0) return _columnCount + ""; else return ""; }
            set
            {
                try
                {
                    _columnCount = Int32.Parse(value);
                    OnPropertyChanged("ColumnCountString");
                    ExecuteConfirmAndCreateMapCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e)
                {
                    CanCreate = false;
                    _columnCount = 0;
                }
            }
        }

        #region Commands
        private bool CanExecuteConfirmAndCreateMapCommandDo()
        {
            return !(_layerCount == 0) && !(_rackCount == 0) && !(_columnCount == 0);
        }
        private void ExecuteConfirmAndCreateMapCommandDo()
        {
            _map.SetMapFromScratch(_layerCount, _rackCount, _columnCount);
            MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.CreateMap_Complete);
            if(confirmToDel == MessageBoxResult.OK)
            {
                CallBackFunction();
                self.Close();
            }
        }
        #endregion
    }
}
