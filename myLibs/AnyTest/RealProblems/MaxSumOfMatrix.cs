using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.RealProblems
{
    public static class MaxSumOfMatrix
    {
        public static void Main(string[] args)
        {
            int N = 0;
            int D = 0;

            int i = 0; int j = 0;
            string firsLine = Console.ReadLine();
            if (!(int.TryParse(firsLine.Split(' ')[0], out N)
                && int.TryParse(firsLine.Split(' ')[1], out D)))
                return;
            int[,] matrix = new int[N, N];
            for(i = 0; i < N; i++)
            {
                string lines = Console.ReadLine();
                string[] strs = lines.Split(' ');
                for(j = 0; j < N; j++)
                {
                    int.TryParse(strs[j], out matrix[i, j]);
                }
            }
            int max = int.MinValue;
            int sum = 0;
            //lines
            for(i = 0; i < N; i++)
            {
                sum = 0;
                for(j = 0; j < D; j++)
                {
                    sum += matrix[i, j];
                }
                max =max < sum ? sum : max;
                for(j = D; j < N; j++)
                {
                    sum = sum - matrix[i, j - D] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            //columns
            for(j = 0; j < N; j++)
            {
                sum = 0;
                for(i = 0; i < D; i++)
                {
                    sum += matrix[i, j];
                }
                max = max < sum ? sum : max;
                for(i = D; i < N; i++)
                {
                    sum = sum - matrix[i - D, j] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            //LT-RB
            sum = 0;int counter = 0;
            for(int k = 0; k <= N - D; k++)
            {
                sum = 0;counter = 0;
                for (i = 0, j = k; counter < D; i++, j++, counter++)
                    sum += matrix[i, j];
                max = max < sum ? sum : max;
                for(;i < N && j < N; i++, j++)
                {
                    sum = sum - matrix[i - D, j - D] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            for(int k = 1; k <= N - D; k++)
            {
                sum = 0;counter = 0;
                for (i = k, j = 0; counter < D; i++, j++, counter++)
                    sum += matrix[i, j];
                max = max < sum ? sum : max;
                for (; i < N && j < N; i++, j++)
                {
                    sum = sum - matrix[i - D, j - D] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            //RT-LB
            for(int k = N -1; k >= D - 1; k--)
            {
                sum = 0; counter = 0;
                for (i = 0, j = k; counter < D; i++, j--, counter++)
                    sum += matrix[i, j];
                max = max < sum ? sum : max;
                for (; i < N && j >= 0; i++, j--)
                {
                    sum = sum - matrix[i - D, j + D] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            for (int k = 1; k <= N - D; k++)
            {
                sum = 0; counter = 0;
                for (i = k, j = N - 1; counter < D; i++, j--, counter++)
                    sum += matrix[i, j];
                max = max < sum ? sum : max;
                for (; i < N; i++, j--)
                {
                    sum = sum - matrix[i - D, j + D] + matrix[i, j];
                    max = max < sum ? sum : max;
                }
            }
            Console.WriteLine(max);
            Console.ReadKey();
        }
    }
}
