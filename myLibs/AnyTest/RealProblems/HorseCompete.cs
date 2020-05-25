using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.RealProblems
{
    public static class HorseCompete
    {
        public static void Main(string[] args)
        {
            int hourseNum = 0;
            int.TryParse(Console.ReadLine(), out hourseNum);
            double res = 0;
            for (int i = 1; i <= hourseNum; i++)
                res += 1.0 / i;
            Console.WriteLine(res.ToString("0.0000"));
            Console.ReadKey();
        }
    }
}
