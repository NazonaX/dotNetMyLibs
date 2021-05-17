using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Method
    {
        /// <summary>
        /// define the calculate model here
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static double CalculateMethod(Object[] thetas, Object[] xs)
        {
            double result = 0;
            if((double)xs[0] > 0 && (double)xs[1] > 0 && (double)xs[2] > 0)
                result = (double)thetas[0] + (double)xs[0] * (double)thetas[1] + (double)xs[1] * (double)thetas[2]
                    + (double)xs[2] * (double)thetas[3] + (double)xs[3] * Math.Pow((double)thetas[4],2);
            else if((double)xs[0] < 0 && (double)xs[1] < 0 && (double)xs[2] > 0)
                result = (double)thetas[0] + (double)xs[0] * (double)thetas[1] - (double)xs[1] * (double)thetas[2]
                    - (double)xs[2] * (double)thetas[3] - (double)xs[3] * Math.Pow((double)thetas[4], 2);
            else
                result = (double)thetas[0] + (double)xs[0] * (double)thetas[1] - (double)xs[1] * (double)thetas[2]
                    - (double)xs[2] * (double)thetas[3] + (double)xs[3] * Math.Pow((double)thetas[4], 2);
            return result;
        }
    }
}
