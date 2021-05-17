using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    /// <summary>
    /// this class is used as a base class
    /// </summary>
    public abstract class BaseClass
    {
        private double scaler = 1;
        private double weight = 1;

        public double Weight { get => weight; set => weight = value; }
        public double Scaler { get => scaler; set => scaler = value; }

        /// <summary>
        /// should be implemented as a method used to change weight and update the scaler
        /// </summary>
        public abstract void changeWeight(double weight);

    }
}
