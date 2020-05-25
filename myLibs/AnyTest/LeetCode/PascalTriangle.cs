using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class PascalTriangle
    {
        /// <summary>
        /// 返回指定层数的杨辉三角
        /// </summary>
        /// <param name="numRows"></param>
        /// <returns></returns>
        public IList<IList<int>> Generate(int numRows)
        {
            IList<IList<int>> res = new List<IList<int>>();
            if (numRows < 1)
                return res;
            res.Add(new List<int>() { 1 });
            for(int i = 2; i <= numRows; i++)
            {
                int[] tmp = new int[i];
                tmp[0] = 1;
                tmp[i - 1] = 1;
                for(int j = 1; j < i - 1; j++)
                {
                    tmp[j] = res[i - 2][j - 1] + res[i - 2][j];
                }
                res.Add(new List<int>(tmp));
            }
            return res;
        }
        /// <summary>
        /// 返回杨辉三角的指定层数
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public IList<int> GetRow(int rowIndex)
        {
            List<int> res = new List<int>();
            if (rowIndex == 0)
                return new List<int>(new int[] { 1 });
            int[] pre = new int[] { 1, 1 };
            for(int i = 3; i <= rowIndex + 1; i++)
            {
                int[] tmp = new int[i];
                tmp[0] = 1;
                tmp[i - 1] = 1;
                for(int j = 1; j < i - 1; j++)
                {
                    tmp[j] = pre[j - 1] + pre[j];
                }
                pre = tmp;
            }
            res.AddRange(pre);
            return res;
        }

        /// <summary>
        /// 给定一个三角形的数组列表，返回它的自顶到底路径的最小和
        /// 路径：该路径从定开始，每次向下移动只能移动到相邻的位置，例如0下标，只能移动到0或1下标
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public int MinimumTotal(IList<IList<int>> triangle)
        {
            int depth_length = triangle.Count;
            int res = DoRecursionByMinimumTotal(triangle, depth_length, 0, 0, 0);
            return res;
        }
        private int DoRecursionByMinimumTotal(IList<IList<int>> triangle, int depth_length, int depth, int index, int sum)
        {
            if(depth == depth_length)
            {
                return sum;
            }
            else
            {
                sum += triangle[depth][index];
                return Math.Min(DoRecursionByMinimumTotal(triangle, depth_length, depth + 1, index, sum),
                    DoRecursionByMinimumTotal(triangle, depth_length, depth + 1, index + 1, sum));
            }
        }
        //best solution
        //逆向思维，自底向上进行最小和的收束
        //翻转求解思路
        public int MinimumTotalBestSolution(IList<IList<int>> triangle)
        {
            int depth_length = triangle.Count;
            for(int i = depth_length - 2; i >= 0; i--)
            {
                for(int j = 0; j <= i; j++)
                {
                    triangle[i][j] = Math.Min(triangle[i + 1][j], triangle[i + 1][j + 1]) + triangle[i][j];
                }
            }
            return triangle[0][0];
        }


    }
}
