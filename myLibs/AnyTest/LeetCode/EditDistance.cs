using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class EditDistance
    {
        //从小到大构建的动态分配
        /// <summary>
        /// 当min(i,j)=0时，lev(i,j)=max(i,j)，一个字符串的长度为0，编辑距离自然是另一个字符串的长度
        /// 当a[i]=b[j] 时，lev(i, j)=lev(i−1, j−1)，比如xxcz和xyz的距离=xxc和xy的距离
        /// 否则，lev(i, j)为如下三项的最小值：
        /// lev(i−1, j)+1(在a中删除ai)，比如xxc和xyz的距离=xx和xyz的距离+1，表示删除其中一个的末尾，时最后相等
        /// lev(i, j−1)+1(在a中插入bj)，比如xxc和xyz的距离=xxcz和xyz的距离+1=xxc和xy的距离+1，同上
        /// lev(i−1, j−1)+1(在a中把ai替换bj)，比如xxc和xyz的距离=xxz和xyz的距离+1=xx和xy的距离+1，表示直接修改其中一个，使相等
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        public int MinDistance(string word1, string word2)
        {
            int length1 = word1.Length;
            int length2 = word2.Length;
            int[,] matrix = new int[length1 + 1, length2 + 1];
            for(int i = 0; i < length1 + 1; i++)
            {
                for(int j = 0; j < length2 + 1; j++)
                {
                    if (j == 0 || i == 0)
                        matrix[i, j] = Math.Max(i, j);
                    else
                    {
                        if (word1[i - 1] == word2[j - 1])
                            matrix[i, j] = matrix[i - 1, j - 1];
                        else
                            matrix[i, j] = 1 + Math.Min(matrix[i - 1, j - 1], Math.Min(matrix[i, j - 1], matrix[i - 1, j]));
                    }
                }
            }
            return matrix[length1, length2];
        }
    }
}
