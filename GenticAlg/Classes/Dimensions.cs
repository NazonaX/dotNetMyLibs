using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    /// <summary>
    /// the dimensions of real dimension
    /// </summary>
    public class Dimensions
    {
        private List<Dimension> thetas;
        private int dimensionNumber;

        public Dimensions(int dimensionNumber)
        {
            Thetas = new List<Dimension>(dimensionNumber);
            this.dimensionNumber = dimensionNumber;
            for (int i = 0; i < dimensionNumber; i++)
                thetas.Add(new Dimension());
        }

        public Dimensions Clone()
        {
            Dimensions tmp = new Dimensions(dimensionNumber);
            for(int i = 0; i < dimensionNumber; i++)
            {
                tmp.thetas[i].Weight = this.thetas[i].Weight;
                tmp.thetas[i].Scaler = this.thetas[i].Scaler;
            }
            return tmp;
        }

        public List<Dimension> Thetas { get => thetas; set => thetas = value; }
        public int DimensionCount { get => dimensionNumber; }
    }
}
