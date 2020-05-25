using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    class ModifyMapInformationViewModels: BaseViewModels
    {
        private Models.Classes.Map _map = null;
        private Window _self = null;
        private double _gapAlongRack = 0;
        private double _gapAlongColumn = 0;
        private double _gapBetweenLayers = 0;
        private double _PSMaxSpeed = 0;
        private double _PSAcceleration = 0;
        private double _PSDeceleration = 0;
        private double _CSMaxSpeed = 0;
        private double _CSAcceleration = 0;
        private double _CSDeceleration = 0;
        private double _LMaxSpeed = 0;
        private double _LAcceleration = 0;
        private double _LDeceleration = 0;

        public DelegateCommand ExecuteMapInformationSaveCommand { get; private set; }
        public ModifyMapInformationViewModels(Models.Classes.Map map, Window self)
        {
            this._map = map;
            this._self = self;
            ExecuteMapInformationSaveCommand = new DelegateCommand(ExecuteMapInformationSaveCommandDo, CanExecuteMapInformationSaveCommandDo);
            InitialInformations();
        }

        public string GapAlongRack
        {
            get { return _gapAlongRack + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _gapAlongRack = tmp;
                OnPropertyChanged("GapAlongRack");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string GapAlongColumn
        {
            get { return _gapAlongColumn + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _gapAlongColumn = tmp;
                OnPropertyChanged("GapAlongCloumn");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string GapBetweenLayers
        {
            get { return _gapBetweenLayers + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _gapBetweenLayers = tmp;
                OnPropertyChanged("GapBetweenLayers");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string PSMaxSpeed
        {
            get { return _PSMaxSpeed + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _PSMaxSpeed = tmp;
                OnPropertyChanged("PSMaxSpeed");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string PSAcceleration
        {
            get { return _PSAcceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _PSAcceleration = tmp;
                OnPropertyChanged("PSAcceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string PSDeceleration
        {
            get { return _PSDeceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _PSDeceleration = tmp;
                OnPropertyChanged("PSAcceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string CSMaxSpeed
        {
            get { return _CSMaxSpeed + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _CSMaxSpeed = tmp;
                OnPropertyChanged("CSMaxSpeed");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string CSAcceleration
        {
            get { return _CSAcceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _CSAcceleration = tmp;
                OnPropertyChanged("CSAcceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string CSDeceleration
        {
            get { return _CSDeceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _CSDeceleration = tmp;
                OnPropertyChanged("CSDeceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string LMaxSpeed
        {
            get { return _LMaxSpeed + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _LMaxSpeed = tmp;
                OnPropertyChanged("LMaxSpeed");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string LAcceleration
        {
            get { return _LAcceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _LAcceleration = tmp;
                OnPropertyChanged("LAcceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string LDeceleration
        {
            get { return _LDeceleration + ""; }
            set
            {
                double tmp = 0;
                Double.TryParse(value, out tmp);
                _LDeceleration = tmp;
                OnPropertyChanged("LDeceleration");
                ExecuteMapInformationSaveCommand.RaiseCanExecuteChanged();
            }
        }
        
        private void InitialInformations()
        {
            GapAlongColumn = CheckString(_map.GapAlongCloumn);
            GapAlongRack = CheckString(_map.GapAlongRack);
            GapBetweenLayers = CheckString(_map.GapBetweenLayers);
            PSMaxSpeed = CheckString(_map.PSMaxSpeed);
            PSAcceleration = CheckString(_map.PSAcceleration);
            PSDeceleration = CheckString(_map.PSDeceleration);
            CSMaxSpeed = CheckString(_map.CSMaxSpeed);
            CSAcceleration = CheckString(_map.CSAcceleration);
            CSDeceleration = CheckString(_map.CSDeceleration);
            LMaxSpeed = CheckString(_map.LMaxSpeed);
            LAcceleration = CheckString(_map.LAcceleration);
            LDeceleration = CheckString(_map.LDeceleration);
        }
        private string CheckString(double? str)
        {
            if (str == null)
                return "0";
            else
                return str + "";
        }

        private void ExecuteMapInformationSaveCommandDo()
        {
            _map.GapAlongRack = _gapAlongRack;
            _map.GapAlongCloumn = _gapAlongColumn;
            _map.GapBetweenLayers = _gapBetweenLayers;
            _map.PSMaxSpeed = _PSMaxSpeed;
            _map.PSAcceleration = _PSAcceleration;
            _map.PSDeceleration = _PSDeceleration;
            _map.CSMaxSpeed = _CSMaxSpeed;
            _map.CSAcceleration = _CSAcceleration;
            _map.CSDeceleration = _CSDeceleration;
            _map.LMaxSpeed = _LMaxSpeed;
            _map.LAcceleration = _LAcceleration;
            _map.LDeceleration = _LDeceleration;
            MessageBoxResult res = MessageBox.Show(Localiztion.Resource.EditMap_SaveMap_Msg_Complete);
            if(res == MessageBoxResult.OK)
            {
                _self.Close();
            }
        }
        private bool CanExecuteMapInformationSaveCommandDo()
        {
            return _gapAlongColumn != 0 && _gapAlongRack != 0 && _gapBetweenLayers != 0
                && _PSMaxSpeed != 0 && _PSAcceleration != 0 && _PSDeceleration != 0
                && _CSMaxSpeed != 0 && _CSAcceleration != 0 && _CSDeceleration != 0
                && _LMaxSpeed != 0 && _LAcceleration != 0 && _LDeceleration != 0;
        }
    }
}
