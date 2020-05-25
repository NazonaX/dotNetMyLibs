using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapAndSimulation.Map;

namespace MapAndSimulation
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Map.Map map = new Map.Map();
            if (Map.Map.CheckForMapFile())
            {
                map = (Map.Map)Map.Map.ReadExistedMap();
                Utils.Logger.WriteMsgAndLog("Read map data from existed file...");
            }
            else
            {
                map = new Map.Map(NumOfLayers: 3, Size: new int[] { 10, 20 },
                    MainPath: new int[] { 4, 7 }, Elevator: new int[] { 20 });
                Utils.Logger.WriteMsgAndLog("Creating a new map...");
            }
            Application.Run(new Form1(map));
        }
    }
}
