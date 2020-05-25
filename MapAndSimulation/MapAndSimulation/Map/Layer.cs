using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Map
{
    [Serializable]
    public class Layer
    {
        private List<Rack> rows;
        private int layerNum;
        /// <summary>
        /// used to create a layer of a map
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="MainPath"></param>
        /// <param name="Elevator"></param>
        public Layer(int NumofLayer, int[] Size, int[] MainPath, int[] Elevator)
        {
            LayerNum = NumofLayer;
            rows = new List<Rack>();
            for (int i = 0; i < Size[0]; i++)
                rows.Add(new Rack(NumofLayer, i + 1, Size));
            //set main path
            foreach(int i in MainPath)
            {
                int x = i - 1;
                for (int j = 0; j < rows[x].Values.Count; j++)
                    rows[x].Values[j] = -1;
                rows[x].IsMainPath = true;
            }
            //set elevator
            foreach(int ele in Elevator)
            {
                int x = ele - 1;
                for(int i = 0; i < rows.Count; i++)
                {
                    rows[i].Values[x] = -2;
                }
            }
        }

        public List<Rack> Values{ get => rows; set => rows = value; }
        public int LayerNum { get => layerNum; set => layerNum = value; }
    }
}
