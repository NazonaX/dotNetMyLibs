using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ClimbStairs
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();

        public int CSolution(int n)
        {
            int res = 0;
            res = DoTraceBack(n);
            return res;
        }

        //关于为什么是斐波那契数列
        //猜想：可以这么理解，每次走楼梯可以拆分为走(n-1)步，再加上一步，到达终点；
        //或者走(n-2)步，再加上一个二步，到达终点。因此按照显现的(n-1)和(n-2)的步数总和就是最终答案。
        //也可以拓展出去最大x步，效果一直，(n-1)+(n-2)+....+(n-x)就是最终答案
        private int DoTraceBack(int n)
        {
            if (n == 1 || n == 2)
                return n;
            else
            {
                int x, y;
                if (dict.ContainsKey(n - 1))
                    x = dict[n - 1];
                else
                {
                    x = DoTraceBack(n - 1);
                    dict.Add(n - 1, x);
                }
                if (dict.ContainsKey(n - 2))
                    y = dict[n - 2];
                else
                {
                    y = DoTraceBack(n - 2);
                    dict.Add(n - 2, y);
                }
                return x + y;
            }
        }
    }
}
