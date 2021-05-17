using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class TwoP1Example
    {
        private static int dimensionCount = 5;

        private List<Generation> generations;
        private List<double> d1x;
        private List<double> d2x;
        private List<double> d3x;
        private List<double> d4x;
        private List<double> y;

        private double prevAverageLoss = 0;

        private List<double> loss;

        public TwoP1Example()
        {
            generations = new List<Generation>();
            generations.Add(new Generation(dimensionCount));
            loss = new List<double>();
            d1x = new List<double>();
            d2x = new List<double>();
            d3x = new List<double>();
            d4x = new List<double>();
            y = new List<double>();
        }
        
        public List<double> D1x { get => d1x; set => d1x = value; }
        public List<double> D2x { get => d2x; set => d2x = value; }

        public double getAverageLoss()
        {
            if (loss.Count == 0)
                return 0;
            else
                return loss.Average();
        }

        public List<double> Y { get => y; set => y = value; }
        public List<Generation> Generations { get => generations; }
        public List<double> D3x { get => d3x; set => d3x = value; }
        public List<double> D4x { get => d4x; set => d4x = value; }

        /// <summary>
        /// to generate a new generation. 
        /// For now is stored in Generations[0], each generation will overwite the former in order to save memory
        /// </summary>
        /// <returns></returns>
        public int generateNewGeneration()
        {
            Generation gen = generations[0];
            List<Dimensions> newGeneration = new List<Dimensions>();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < Generation.GENERATION_NUMBER; i++)
            {
                int LuckyIndex = random.Next(1, gen.DimensionsGenerations.Count) -1;
                Dimensions tmp = gen.DimensionsGenerations[LuckyIndex].Clone();
                for (int j = 0; j < tmp.DimensionCount; j++)
                {
                    tmp.Thetas[j].changeWeight(gen.getNormalNumber());
                }
                newGeneration.Add(tmp);
            }
            gen.DimensionsGenerations = newGeneration;
            Generation.GENERATION_INDEX++;
            generations[0] = gen;
            return gen.DimensionsGenerations.Count;
        }

        /// <summary>
        /// to calculate the loss or cost
        /// </summary>
        /// <returns></returns>
        public int calculateLoss()
        {
            Generation gen = Generations[0];
            loss.Clear();
            for(int i = 0; i < gen.DimensionsGenerations.Count; i++)
            {
                Dimensions dims = gen.DimensionsGenerations[i];
                double biasis = dims.Thetas[0].Weight;
                double weight1 = dims.Thetas[1].Weight;
                double weight2 = dims.Thetas[2].Weight;
                double weight3 = dims.Thetas[3].Weight;
                double weight4 = dims.Thetas[4].Weight;
                double sum = 0;
                for(int j = 0; j < y.Count; j++)
                {
                    sum += Math.Pow(y[j] - Method.CalculateMethod(new Object[] { biasis, weight1, weight2, weight3, weight4}, 
                        new Object[] { d1x[j], d2x[j], d3x[j], d4x[j] }), 2);
                }
                sum /= (2 * y.Count);
                loss.Add(sum);
            }
            return loss.Count;
        }

        /// <summary>
        /// to select the best ones in the generations, the count is 10 by default
        /// </summary>
        /// <returns></returns>
        public int selectBest()
        {
            List<double> top = null;
            if (prevAverageLoss != 0)
                top = loss.Where(tmp => tmp < prevAverageLoss).OrderBy(tmp => tmp).Take(Generation.GENERATION_WINNER_NUMBER).ToList();
            else
                top = loss.OrderBy(tmp => tmp).Take(Generation.GENERATION_WINNER_NUMBER).ToList();
            if (top != null)
            {
                List<Dimensions> topGeneration = new List<Dimensions>();
                for (int i = 0; i < top.Count; i++)
                {
                    int index = loss.FindIndex(tmp => tmp == top[i]);
                    topGeneration.Add(generations[0].DimensionsGenerations[index].Clone());
                }
                generations[0].DimensionsGenerations.Clear();
                generations[0].DimensionsGenerations = topGeneration;
            }
            loss = loss.OrderBy(tmp => tmp).Take((int)(loss.Count * Generation.GENERATION_DIEOUT_RATE)).ToList();
            prevAverageLoss = getAverageLoss();
            return generations[0].DimensionsGenerations.Count;
        }
    }
}
