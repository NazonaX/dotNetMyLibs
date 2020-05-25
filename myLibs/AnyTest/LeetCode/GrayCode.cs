using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class GrayCode
    {
        /// <summary>
        /// 给定位数，输出所有gray code
        /// gray code：相邻两个数字的二进制编码仅相差1位
        /// n位gray code共有2^n个数字
        /// etc. n = 2::0,1,3,2
        /// 0: 00
        /// 1: 01
        /// 3: 11
        /// 2: 10
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IList<int> Solution(int n)
        {
            IList<int> res = new List<int>();
            //利用递归，推出规律：n的gray编码是n-1的gray编码的镜面对称总长，后半镜面加上2^(n-1)即可
            DoRecursion(res, n);
            return res;
        }
        private void DoRecursion(IList<int> res, int n)
        {
            if(n == 0)
            {
                res.Add(0);
            }
            else
            {
                DoRecursion(res, n - 1);
                int length = res.Count;
                int addition = 1 << (n - 1);
                for(int i = length - 1; i >= 0; i--)
                {
                    res.Add(res[i] + addition);
                }
            }
        }
        public IList<int> BestSolution(int n)
        {
            IList<int> res = new List<int>();
            //格雷码的正向转换和逆向转换都可以使用以下位移+异或运算
            //具体原理不明。猜测：异或运算为同0异1，遇到进位可以有效避免连续的数值丢失。
            //而右位移对应的公式的数位交叉异或，高位补0。
            for(int i = 0; i < Math.Pow(2, n); i++)
            {
                res.Add((i >> 1) ^ i);
            }
            return res;
        }
    }
}
