using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace GenticAlg
{
    class Program
    {
        static void Main(string[] args)
        {
            TwoP1Example twoP1Example = DataGenerater.getTestData();
            for(int i = 0; i < Generation.GENERATION_ITER_COUNT; i++)
            { 
                twoP1Example.generateNewGeneration();
                Console.WriteLine("the " + Generation.GENERATION_INDEX + "'s generation's count is " + twoP1Example.Generations[0].DimensionsGenerations.Count);
                twoP1Example.calculateLoss();
                Console.WriteLine("the " + Generation.GENERATION_INDEX + "'s generation's average loss is " + twoP1Example.getAverageLoss());
                twoP1Example.selectBest();
                Console.WriteLine("the " + Generation.GENERATION_INDEX + "'s generation's lived average loss is " + twoP1Example.getAverageLoss());
                Console.WriteLine("the " + Generation.GENERATION_INDEX + "'s generation's best thetas are:");
                int total = 0;
                if (Generation.GENERATION_WINNER_NUMBER > twoP1Example.Generations[0].DimensionsGenerations.Count)
                    total = twoP1Example.Generations[0].DimensionsGenerations.Count;
                else
                    total = Generation.GENERATION_WINNER_NUMBER;
                if(total == 0)
                {
                    Console.Write("Generation DIE OUT....");
                }
                for (int j = 0; j < total; j++)
                {
                    for(int k = 0; k < twoP1Example.Generations[0].DimensionsGenerations[j].DimensionCount; k++)
                    {
                        Console.Write("theta" + k + " = " + twoP1Example.Generations[0].DimensionsGenerations[j].Thetas[k].Weight.ToString("0.0000")+"..");
                    }
                    Console.WriteLine("");
                }
            }
            Console.ReadKey();
        }
    }
}
