using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SpiralMatrix
    {
        public IList<int> SpiralOrder(int[,] matrix)
        {
            IList<int> res = new List<int>();
            int i = 0;int j = 0;
            int rank1 = matrix.GetLength(0);
            int rank2 = matrix.GetLength(1);
            int boundary = rank1 < rank2 ? rank1 : rank2;
            boundary = boundary % 2 == 1 ? boundary / 2 + 1 : boundary / 2;
            for(int bound = 0; bound < boundary ;bound ++ )
            {
                int upper = 0 + bound;
                int bottom = rank1 - bound - 1;
                int left = 0 + bound;
                int right = rank2 - 1 - bound;
                for (j = left; j <= right; j++)
                    res.Add(matrix[upper, j]);
                for (i = upper + 1; i <= bottom; i++)
                    res.Add(matrix[i, right]);
                if(upper != bottom)
                    for (j = right - 1; j >= left; j--)
                        res.Add(matrix[bottom, j]);
                if(left != right)
                    for (i = bottom - 1; i > upper; i--)
                        res.Add(matrix[i, left]);
            }
            return res;
        }

        public int[,] GenerateMatrix(int n)
        {
            int[,] res = new int[n, n];int counter = 1;
            for (int bound = 0; bound < n; bound++)
            {
                int upper = 0 + bound;
                int bottom = n - bound - 1;
                int left = 0 + bound;
                int right = n - 1 - bound;
                int j = 0, i = 0;
                for (j = left; j <= right; j++)
                    res[upper, j] = counter++;
                for (i = upper + 1; i <= bottom; i++)
                    res[i, right] = counter++;
                if (upper != bottom)
                    for (j = right - 1; j >= left; j--)
                        res[bottom, j] = counter++;
                if (left != right)
                    for (i = bottom - 1; i > upper; i--)
                        res[i, left] = counter++;
            }
            return res;
        }
    }
}
