using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace GenticAlg
{
    public class DataGenerater
    {

        public static int TOTALNUM = 200;
        public static double WEIGHT1 = 3;
        public static double WEIGHT2 = 6.8;
        public static double WEIGHT3 = -198.6;
        public static double WEIGHT4 = -4.1;
        public static double BIASIS = 98.7;

        public static double NORMAL_MIU = 0;
        public static double NORMAL_DELTA = 1;

        public static TwoP1Example getTestData()
        {
            Random u1 = new Random(Guid.NewGuid().GetHashCode()); Random u2 = new Random(Guid.NewGuid().GetHashCode());
            TwoP1Example twoP1 = new TwoP1Example();
            for (int i = 0; i < TOTALNUM; i++)
            {
                double x1 = (u1.NextDouble() - 0.5);
                double x2 = (u2.NextDouble() - 0.5);
                double x3 = (u1.NextDouble() - 0.5);
                double x4 = (u2.NextDouble() - 0.5);
                double noise = GetZTFB(u1.NextDouble(), u2.NextDouble(), NORMAL_MIU, NORMAL_DELTA);
                //System.Diagnostics.Debug.WriteLine("noise:: " + noise);
                //double y = BIASIS + WEIGHT1 * x1 + WEIGHT2 * x2 + noise;
                double y = Method.CalculateMethod(new Object[] { BIASIS, WEIGHT1, WEIGHT2, WEIGHT3, WEIGHT4 }, new Object[] { x1, x2, x3, x4});
                twoP1.D1x.Add(x1);
                twoP1.D2x.Add(x2);
                twoP1.D3x.Add(x3);
                twoP1.D4x.Add(x4);
                twoP1.Y.Add(y);
            }
            return twoP1;
        }
        private static double GetZTFB(double u1, double u2, double e, double d)
        {
            double result = 0;
            try
            {
                result = e + Math.Sqrt(d) * Math.Sqrt((-2) * (Math.Log(u1) / Math.Log(Math.E))) * Math.Cos(2 * Math.PI * u2);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

    }
}
