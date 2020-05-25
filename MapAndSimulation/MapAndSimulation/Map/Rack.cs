using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Map
{
    [Serializable]
    public class Rack
    {
        private List<int> line;
        private List<Good> goods; 
        private int layerNum;
        private int rowNum;
        private bool isMainPath;
        /// <summary>
        /// used to create a rack
        /// </summary>
        /// <param name="Size">here we use Size[1] to define the row length</param>
        public Rack(int NumofLayer, int NumofRow, int[] Size)
        {
            layerNum = NumofLayer;
            rowNum = NumofRow;
            line = new List<int>();
            goods = new List<Good>();
            for (int i = 0; i < Size[1]; i++)
            {
                line.Add(0);
                goods.Add(new Good());
            }
            isMainPath = false;
        }

        public List<int> Values { get => line; set => line = value; }
        public bool IsMainPath { get => isMainPath; set => isMainPath = value; }
        public int RowNum { get => rowNum; set => rowNum = value; }
        public int LayerNum { get => layerNum; set => layerNum = value; }
        public List<Good> Goods { get => goods; set => goods = value; }
    }
}
