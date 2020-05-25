using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapAndSimulation
{
    public partial class Form1 : Form
    {
        Map.Map map;
        public Form1(Map.Map map)
        {
            this.map = map;
            InitializeComponent();
            InitialVisualMap();
            //Label lab1 = new Label();
            //lab1.AutoSize = false;
            //lab1.Width = 10;
            //lab1.Height = 10;
            //lab1.Location = new Point(10 + 8 + 3, 8);
            //lab1.BackColor = Color.Silver;
            //lab1.Click += Lab1_Click;
            //LayerControlPanel.TabPages[0].Controls.Add(lab1);
        }

        private void InitialVisualMap()
        {
            foreach(Map.Layer layer in map.Layers)
            {
                TabPage tp = new TabPage("Layer" + layer.LayerNum);
                int row = 1;int column = 1;
                int gap = 3;int size = 10;
                foreach(Map.Rack rack in layer.Values)
                {
                    foreach(int x in rack.Values)
                    {
                        Color color = Color.Silver;
                        if (x == 1)
                            color = Color.Black;
                        else if (x == -1)
                            color = Color.Red;
                        else if (x == -2)
                            color = Color.Purple;
                        Label lab = ControlGenerater.LabelGenerater.Generate(width: size,
                            height: size, location: new Point(gap * column + (column - 1) * size, gap * row + (row - 1) * size),
                            color: color,
                            name:"label-"+rack.LayerNum+"-"+rack.RowNum+"-"+column);
                        //set event handler
                        lab.MouseClick += Lab_Click;
                        tp.Controls.Add(lab);
                        column++;
                    }
                    row++;
                    column = 1;
                }
                LayerControlPanel.TabPages.Add(tp);
            }
        }

        private void Lab_Click(object sender, EventArgs e)
        {
            Label obj = (Label)sender;
            int layer = 0, row = 0, column = 0;
            string[] namesplit = obj.Name.Split('-');
            layer = int.Parse(namesplit[1]);
            row = int.Parse(namesplit[2]);
            column = int.Parse(namesplit[3]);
            Map.Good good = map.GetGoodAt(layer, row, column);
            //Map.Rack rack = map.Layers.Find(ly => ly.LayerNum == layer)
            //    .Values.Find(rck => rck.LayerNum == layer && rck.RowNum == row) ?? null;
            if (good == null)
            {
                Utils.Logger.WriteMsgAndLog("Label: " + obj.Name + " not found...", MsgWindow);
                return;
            }
            if(Mode_Select.Checked)
            {
                //show the message of the selected position of rack
                GoodName.Text = good.GoodName;
                GoodOrderNo.Text = good.OrderNumber;
                GoodSpecification.Text = good.Specification;
            }else if (Mode_Add.Checked)
            {
                //add a good into the selected rack
                if (good.GoodName != "")
                {
                    Utils.Logger.WriteMsgAndLog("Selected rack " + obj.Name + " has a good so far...", MsgWindow);
                    return;
                }
                if (map.GetStatusAt(layer,row, column) == 0)
                {
                    if (GoodName.Text == "" || GoodOrderNo.Text == "" || GoodSpecification.Text == "")
                    {
                        Utils.Logger.WriteMsgAndLog("All infomation of Good is needed...", MsgWindow);
                        return;
                    }
                    obj.BackColor = Color.Black;
                    map.SetStatusAt(1, layer, row, column);
                    good.GoodName = GoodName.Text;
                    good.OrderNumber = GoodOrderNo.Text;
                    good.Specification = GoodSpecification.Text;
                    Utils.Logger.WriteMsgAndLog("Convert " + obj.Name + " from 0 to 1\n--->"
                        + GoodName.Text + "-" + GoodOrderNo.Text + "-" + GoodSpecification.Text, MsgWindow);
                }
                else
                {
                    Utils.Logger.WriteMsgAndLog("Clicked " + obj.Name + " and nothing happen...", MsgWindow);
                }
            }
            else if (Mode_Delete.Checked)
            {
                //delete the good info of the selected rack
                if (map.GetStatusAt(layer, row, column) == 1)
                {
                    obj.BackColor = Color.Silver;
                    map.SetStatusAt(0, layer, row, column);
                    good.GoodName = "";
                    good.OrderNumber = "";
                    good.Specification = "";
                    Utils.Logger.WriteMsgAndLog("Convert " + obj.Name + " from 1 to 0", MsgWindow);
                }
            }
            
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            //try
            //{
            //    map.WriteMapToFile();
            //}catch(Exception ex)
            //{
            //    Utils.Logger.WriteMsgAndLog(ex.Message, MsgWindow);
            //}
            //base.OnFormClosed(e);
        }

        private async void Btn_Evalutate_Click(object sender, EventArgs e)
        {
            map.WriteMapToFile();
            Utils.Logger.WriteMsgAndLog("Map writting done...", MsgWindow);
            int NumOfAGVs = int.Parse(Txt_NumOfAGVs.Text);
            Map.Solution solution = new Map.Solution(NumOfAGVs);
            //initial missions to test
            List<Mission.Mission> missions = new List<Mission.Mission>();
            //set test order num are:9527, 9983 and 7752
            //except for 1111
            //** must set the orderNum, others could be empty
            //and the Solve function will extend the simple missions to detailed missions
            Mission.Mission m = new Mission.Mission();
            m.TargetGood.OrderNumber = "9527";
            m.Type = Mission.Mission.MissionType.TAKE_OUT;
            missions.Add(m);
            m = new Mission.Mission();
            m.TargetGood.OrderNumber = "9983";
            m.Type = Mission.Mission.MissionType.TAKE_OUT;
            missions.Add(m);
            m = new Mission.Mission();
            m.TargetGood.OrderNumber = "7752";
            m.Type = Mission.Mission.MissionType.TAKE_OUT;
            missions.Add(m);

            Utils.Logger.WriteMsgAndLog("Total Mission random iter count is: " + Map.Solution.MAX_RANDOM_MISSION);
            Utils.Logger.WriteMsgAndLog("Total calculating iter count is: " + Map.Solution.NUM_OF_ITER);
            Utils.Logger.WriteMsgAndLog("Number of particles is: " + Map.Solution.PARTICLE_NUM);
            Utils.Logger.WriteMsgAndLog("Now doing calculation...Please wait...");
            Mission.AGV[] res = await Task.Run(() => solution.Solve(map, missions));
            int agvCounter = 1;
            StringBuilder sb = new StringBuilder();
            foreach (Mission.AGV a in res)
            {
                sb.Append("AGV No." + agvCounter + "'s mission situations...\r\n");
                foreach (Mission.Mission mt in a.Missions)
                {
                    sb.Append(mt.Name);
                    sb.Append("---->from: " + mt.FromPosition.ToString() + "\r\n");
                    sb.Append("---->to: " + mt.TargetPosition.ToString() + "\r\n");
                    sb.Append("---->Taking main path: " + mt.MainPathToTake + "\r\n");
                    sb.Append("---->Taking elevator: " + mt.ElevatorToTake + "\r\n");
                    sb.Append("---->WaitCost: " + mt.WaitedTime + "\r\n");
                    sb.Append("---->Cost: " + mt.CostTime + "\r\n");
                }
                agvCounter++;
            }
            sb.Append("The Total EvaluateTimeCost is: " + solution.CostTime + "\r\n");
            Utils.Logger.WriteMsgAndLog(sb.ToString(), MsgWindow);



        }

    }
}
