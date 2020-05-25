using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RotateImageInPlace
    {
        public void Rotate(int[,] matrix)
        {
            int length = matrix.GetLength(0);
            int x = 0, y = 0;
            int tmp1 = 0;int tmp2 = 0;
            for (int i = 0; i < length / 2; i++)
            {
                for(int j = i; j < length - 1 - i; j++)
                {
                    x = i;
                    y = j;
                    tmp2 = matrix[x, y];
                    for (int k = 0; k < 4; k++)
                    {
                        tmp1 = matrix[y, length - 1 - x];
                        matrix[y, length - 1 - x] = tmp2;
                        tmp2 = tmp1;
                        tmp1 = x;
                        x = y;
                        y = length - 1 - tmp1;
                    }
                }
            }
        }
    }
}
