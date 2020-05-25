using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class StaticGoodsTypesViewModels: BaseViewModels
    {
        private Models.Classes.Map _map = null;
        private Window self = null;
        private ObservableCollection<string> _goodsTypes = null;
        public int _selectedGoodsTypesIndex = 0;
        private string _textBoxString = "123";
        private List<int> Actions = new List<int>();
        private Dictionary<int, string> ActionDetails = new Dictionary<int, string>();
        private enum ActionTypes { ADD, MODIFY, DELETE}

        public DelegateCommand ExecuteAddGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteModifyGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteDeleteGoodsTypesCommand { get; private set; }
        public DelegateCommand ExecuteSaveAllCommand { get; private set; }

        public StaticGoodsTypesViewModels(Models.Classes.Map map, Window selfWindow)
        {
            ExecuteAddGoodsTypesCommand = new DelegateCommand(ExecuteAddGoodsTypesCommandDo, CanExecuteAddGoodsTypesCommandDo);
            ExecuteDeleteGoodsTypesCommand = new DelegateCommand(ExecuteDeleteGoodsTypesCommandDo, CanExecuteDeleteGoodsTypesCommandDo);
            ExecuteModifyGoodsTypesCommand = new DelegateCommand(ExecuteModifyGoodsTypesCommandDo, CanExecuteModifyGoodsTypesCommandDo);
            ExecuteSaveAllCommand = new DelegateCommand(ExecuteSaveAllCommandDo, CanExecuteSaveAllCommandDo);

            this._map = map;
            self = selfWindow;
            GoodsTypes = new ObservableCollection<string>(map.GoodsTypes);
            TextBoxString = map.GoodsTypes[0];
        }

        public ObservableCollection<string> GoodsTypes {
            get { return _goodsTypes; }
            set
            {
                _goodsTypes = value;
                OnPropertyChanged("GoodsTypes");
            }
        }
        public int SelectedGoodsTypesIndex
        {
            get { return _selectedGoodsTypesIndex; }
            set
            {
                _selectedGoodsTypesIndex = value;
                //some operations here
                if (_selectedGoodsTypesIndex != -1)
                    TextBoxString = GoodsTypes[_selectedGoodsTypesIndex];
                else
                    TextBoxString = "";
                ExecuteAddGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteModifyGoodsTypesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedGoodsTypesIndex");
            }
        }
        public string TextBoxString
        {
            get { return _textBoxString; }
            set
            {
                _textBoxString = value;
                ExecuteAddGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteDeleteGoodsTypesCommand.RaiseCanExecuteChanged();
                ExecuteModifyGoodsTypesCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("TextBoxString");
            }
        }

        private void AddAction(ActionTypes type, string target, string source)
        {
            if(type == ActionTypes.ADD)
            {
                ActionDetails.Add(Actions.Count, "ADD<-SPLIT->" + target);
            }
            else if(type == ActionTypes.MODIFY)
            {
                ActionDetails.Add(Actions.Count, "MODIFY<-SPLIT->" + source + "<-SPLIT->" + target);
            }
            else if(type == ActionTypes.DELETE)
            {
                ActionDetails.Add(Actions.Count, "DELETE<-SPLIT->" + target);
            }
            Actions.Add(Actions.Count);
        }
        #region Commands
        private void ExecuteAddGoodsTypesCommandDo()
        {
            //_map.AddGoodsTypes(TextBoxString);
            //GoodsTypes = new ObservableCollection<string>(_map.GoodsTypes);
            GoodsTypes.Add(TextBoxString);
            AddAction(ActionTypes.ADD, TextBoxString, "");
            SelectedGoodsTypesIndex = GoodsTypes.Count - 1;
            ExecuteSaveAllCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteAddGoodsTypesCommandDo()
        {
            if (TextBoxString.Equals(""))
                return false;
            for(int i = 0; i < GoodsTypes.Count; i++)
            {
                if (GoodsTypes[i].Equals(TextBoxString))
                    return false;
            }
            return true;
        }
        private void ExecuteModifyGoodsTypesCommandDo()
        {
            //_map.ModifyGoodsTypes(original: _map.GoodsTypes[_selectedGoodsTypesIndex], target: TextBoxString);
            int tmpIndex = SelectedGoodsTypesIndex;
            string tmpstr = TextBoxString;
            AddAction(ActionTypes.MODIFY, TextBoxString, GoodsTypes[SelectedGoodsTypesIndex]);
            SelectedGoodsTypesIndex = -1;
            //GoodsTypes = new ObservableCollection<string>(_map.GoodsTypes);
            GoodsTypes.RemoveAt(tmpIndex);
            GoodsTypes.Insert(tmpIndex, tmpstr);
            SelectedGoodsTypesIndex = tmpIndex;
            ExecuteSaveAllCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteModifyGoodsTypesCommandDo()
        {
            //the same logic as check for add command
            return CanExecuteAddGoodsTypesCommandDo();
        }
        private void ExecuteDeleteGoodsTypesCommandDo()
        {
            //_map.DeleteGoodsTypes(TextBoxString);
            //GoodsTypes = new ObservableCollection<string>(_map.GoodsTypes);
            AddAction(ActionTypes.DELETE, GoodsTypes[SelectedGoodsTypesIndex], "");
            GoodsTypes.RemoveAt(SelectedGoodsTypesIndex);
            SelectedGoodsTypesIndex = -1;
            ExecuteSaveAllCommand.RaiseCanExecuteChanged();
        }
        private bool CanExecuteDeleteGoodsTypesCommandDo()
        {
            if (_selectedGoodsTypesIndex != -1)
                return true;
            else return false;
        }
        private void ExecuteSaveAllCommandDo()
        {
            for(int i = 0; i < Actions.Count; i++)
            {
                string command = ActionDetails[Actions[i]];
                string[] commands = command.Split(new string[] { "<-SPLIT->" }, StringSplitOptions.None);
                if (commands[0].Equals("ADD"))
                {
                    _map.AddGoodsTypes(commands[1]);
                }
                else if (commands[0].Equals("MODIFY"))
                {
                    _map.ModifyGoodsTypes(commands[1], commands[2]);
                }
                else if (commands[0].Equals("DELETE"))
                {
                    _map.DeleteGoodsTypes(commands[1]);
                }
            }
            _map.SaveGoodsTypesToFile();
            _map.SaveToFile();
            MessageBoxResult confirmToDel = MessageBox.Show(Localiztion.Resource.GoodsTypes_Msg_SaveComplete);
            if (confirmToDel == MessageBoxResult.OK)
                self.Close();
        }
        private bool CanExecuteSaveAllCommandDo()
        {
            //check if the GoodsTypes are changed or not
            return Actions.Count > 0;
        }
        #endregion
    }
}
