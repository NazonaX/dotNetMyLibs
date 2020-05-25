using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class UniquePathes
    {
        public int Solution(int m, int n)
        {
            int res = 0;
            res = DoTraceBack(1, 1, m, n);
            return res;
        }
        //Time limited
        private int DoTraceBack(int v1, int v2, int m, int n)
        {
            if (v1 == m && v2 == n)
                return 1;
            int res = 0;
            if (v1 != m)
                res += DoTraceBack(v1 + 1, v2, m, n);
            if (v2 != n)
                res += DoTraceBack(v1, v2 + 1, m, n);
            return res;
        }

        //Solution 2 for time limited
        public int Solution2(int m, int n)
        {
            if (m == 1 || n == 1)
                return 1;
            double res = 1;
            for(int i = 1; i <= n - 1; i++)
            {
                res = res * (m + i - 1) / (i);
            }
            return (int)res;
        }

        //动态规划
        public int Solution3(int m, int n)
        {
            if (m == 1 || n == 1)
                return 1;
            int[,] matrix = new int[m, n];
            for(int i = 0; i < m; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (i == 0 || j == 0)
                        matrix[i, j] = 1;
                    else
                        matrix[i, j] = matrix[i, j - 1] + matrix[i - 1, j];
                }
            }
            return matrix[m - 1, n - 1];
        }

        //solution with obstacle in the grid
        public int UniquePathsWithObstacles(int[,] obstacleGrid)
        {
            int m = obstacleGrid.GetLength(0);
            int n = obstacleGrid.GetLength(1);
            int[,] matrix = new int[m, n];
            matrix[0, 0] = 1;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if(obstacleGrid[i,j] != 0)
                    {
                        matrix[i, j] = 0;
                        continue;
                    }
                    if (i == 0 && j != 0)
                        matrix[i, j] = matrix[i, j - 1];
                    if (j == 0 && i != 0)
                        matrix[i, j] = matrix[i - 1, j];
                    else if(i != 0 && j != 0)
                        matrix[i, j] = matrix[i, j - 1] + matrix[i - 1, j];
                }
            }
            return matrix[m - 1, n - 1];
        }

        //calculate the min path cost
        public int MinPathSum(int[,] grid)
        {
            int m = grid.GetLength(0);
            int n = grid.GetLength(1);
            int[,] matrix = new int[m, n];
            matrix[0, 0] = grid[0, 0];
            for(int i = 1; i < n; i++)
            {
                matrix[0, i] = matrix[0, i - 1] + grid[0, i];
            }
            for(int i = 1; i < m; i++)
            {
                matrix[i, 0] = matrix[i - 1, 0] + grid[i, 0];
            }
            for(int i = 1; i < m; i++)
            {
                for(int j = 1; j < n; j++)
                {
                    matrix[i, j] = (matrix[i - 1, j] < matrix[i, j - 1] ? matrix[i - 1, j] : matrix[i, j - 1]) + grid[i, j];
                }
            }
            return matrix[m - 1, n - 1];
        }
    }
}
