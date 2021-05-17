using System;
using System.Collections.Generic;

namespace Classes
{
    public class Generation
    {
        private static double NORMAL_MIU = 0;
        private static double NORMAL_DELTA = 0.05;
        public static int GENERATION_INDEX = 0;
        public static int GENERATION_NUMBER = 200;
        public static int GENERATION_WINNER_NUMBER = 10;
        public static int GENERATION_ITER_COUNT = 1000;
        public static double GENERATION_DIEOUT_RATE = 0.6;

        private Random r1 = new Random(Guid.NewGuid().GetHashCode());
        private Random r2 = new Random(Guid.NewGuid().GetHashCode());

        private List<Dimensions> dimensionsGenerations;

        public Generation(int DimensionCount)
        {
            dimensionsGenerations = new List<Dimensions>();
            dimensionsGenerations.Add(new Dimensions(DimensionCount));
        }

        public List<Dimensions> DimensionsGenerations { get => dimensionsGenerations; set => dimensionsGenerations = value; }

        /// <summary>
        /// used to get Normal number
        /// </summary>
        public double getNormalNumber()
        {
            double u1 = r1.NextDouble();
            double u2 = r2.NextDouble();
            double noise = (NORMAL_MIU + Math.Sqrt(NORMAL_DELTA) * Math.Sqrt((-2) * (Math.Log(u1) / Math.Log(Math.E))) * Math.Cos(2 * Math.PI * u2));
            System.Diagnostics.Debug.WriteLine("The 0-1 noise is: " + noise);
            return noise;
        }

    }
}