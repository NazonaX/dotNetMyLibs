using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Pow
    {
        public double MyPow(double x, int n)
        {
            if (n == 0)
                return 1;
            double res = 1;
            for(int i = n; i != 0; i /= 2)
            {
                //每次折一半，奇数多出来的1乘在res上，x自身累乘
                if (i % 2 != 0)//初始奇数和最后的1
                    res *= x;
                x *= x;
            }
            return n > 0 ? res : 1 / res;
        }
    }
}
