using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Utils
{
    public static class Randomer
    {
        private static Random random = new Random();

        public static int GetRandomInt(int max)
        {
            lock (random)
            {
                return random.Next(max);
            }
        }
        public static double GetRandom()
        {
            lock (random)
            {
                return random.NextDouble();
            }
        }

        public static double GetRandomMOPO()
        {
            lock (random)
            {
                return (random.NextDouble() - 0.5) * 2;
            }
        }

        public static double GetRandomByMinMax(double min, double max)
        {
            lock (random)
            {
                return random.NextDouble() * (max - min) + min;
            }
        }

        public static bool[] GetRandomOneHot(double[] target)
        {
            lock (random)
            {
                target = ConvertLargerT0(target);
                bool[] res = new bool[target.Length];
                int i; bool found = false;
                for (i = 1; i < target.Length; i++)
                    target[i] = target[i - 1] + target[i];
                for (i = 0; i < target.Length; i++)
                    target[i] = target[i] / target[target.Length - 1];
                double cursor = random.NextDouble();
                for (i = 0; i < target.Length; i++)
                {
                    if (target[i] > cursor && !found)
                    {
                        res[i] = true;
                        found = true;
                    }
                    else
                        res[i] = false;
                }
                return res;
            }
        }

        private static double[] ConvertLargerT0(double[] target)
        {
            double min = target.Min();
            if (min < 0)
                for (int i = 0; i < target.Length; i++)
                    target[i] = target[i] + min * -1;
            return target;
        }

        public static bool[] GetRandomOneHot(int[] target)
        {
            double[] d = new double[target.Length];
            for (int i = 0; i < target.Length; i++)
                d[i] = target[i];
            return GetRandomOneHot(d);
        }
    }
}
