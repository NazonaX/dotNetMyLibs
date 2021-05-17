using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace wpfSimulation.ViewModels
{
    public class EditSpecialConnectionViewModels: BaseViewModels
    {
        public delegate void SetMsgCallBack(string msg);

        private Models.Entity.MapItems _item1 = null;
        private Models.Entity.MapItems _item2 = null;
        private Models.Entity.Map _map = null;
        private string _costTime = "";
        private Window _self = null;
        private SetMsgCallBack _setMsgCallBack = null;

        public string CostTime
        {
            get { return "" + _costTime; }
            set
            {
                _costTime = value;
                OnPropertyChanged("CostTime");
            }
        }
        public DelegateCommand ExecuteSaveSpecialConnectionCommand { get; private set; }

        public EditSpecialConnectionViewModels(Models.Entity.Map map, Window self, SetMsgCallBack callback,
            Models.Entity.MapItems item1, Models.Entity.MapItems item2)
        {
            this._map = map;
            this._self = self;
            this._setMsgCallBack = callback;
            this._item1 = item1;
            this._item2 = item2;
            ExecuteSaveSpecialConnectionCommand = new DelegateCommand(ExecuteSaveSpecialConnectionCommandDo);
            Models.Entity.SpecialConnection s = _map.SpecialConnections.SingleOrDefault(sc => sc.MapItemFrom == item1.MapItemID && sc.MapItemTo == item2.MapItemID);
            if (s == null)
                CostTime = "0";
            else
                CostTime = "" + s.TimeCost;
        }

        private void ExecuteSaveSpecialConnectionCommandDo()
        {
            float x = 0;
            float.TryParse(_costTime, out x);
            if(x == 0)
            {
                MessageBox.Show("Illegal input Cost Time. Please check to make sure that it is legal.");
                return;
            }
            if(x != 0)
            {
                //save as undirected graph
                //as a pair of directed vector
                Models.Entity.SpecialConnection s1 = _map.SpecialConnections.SingleOrDefault(sc => sc.MapItemFrom == _item1.MapItemID && sc.MapItemTo == _item2.MapItemID);
                Models.Entity.SpecialConnection s2 = _map.SpecialConnections.SingleOrDefault(sc => sc.MapItemFrom == _item2.MapItemID && sc.MapItemTo == _item1.MapItemID);
                using(TransactionScope trans = new TransactionScope())
                {
                    List<Models.Entity.SpecialConnection> addition = new List<Models.Entity.SpecialConnection>();
                    if (s1 == null)
                    {
                        s1 = new Models.Entity.SpecialConnection()
                        {
                            MapItemFrom = _item1.MapItemID,
                            MapItemTo = _item2.MapItemID,
                            TimeCost = x
                        };
                        addition.Add(s1);
                    }
                    else
                    {
                        s1.TimeCost = x;
                    }
                    if(s2 == null)
                    {
                        s2 = new Models.Entity.SpecialConnection()
                        {
                            MapItemFrom = _item2.MapItemID,
                            MapItemTo = _item1.MapItemID,
                            TimeCost = x
                        };
                        addition.Add(s2);
                    }
                    else
                    {
                        s2.TimeCost = x;
                    }
                    Models.Service.MapSingletonService.Instance.GetSpecialConnectionService().InsertSpecialConnections(addition);
                    Models.Service.MapSingletonService.Instance.GetSpecialConnectionService().UpdateSpecialConnections();
                    trans.Complete();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(addition.Count)
                        .Append(" items has been added. ")
                        .Append(2 - addition.Count)
                        .Append(" items has been updated.");
                    _setMsgCallBack(sb.ToString());
                    MessageBox.Show("Complete!");
                    _self.Close();
                }                   
            }
        }
    }
}
