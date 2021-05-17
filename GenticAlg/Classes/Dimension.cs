using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    /// <summary>
    /// the implement of baseclass
    /// </summary>
    public class Dimension : BaseClass
    {
        /// <summary>
        /// to add a moise number to the weight
        /// </summary>
        /// <param name="noise"></param>
        public override void changeWeight(double noise)
        {
            //for now we dont change scaler
            this.Weight = this.Weight + noise;
        }

    }
}
