using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Sqrt
    {
        public int MySqrt(int x)
        {
            if (x <= 1)
                return x;
            double res = x;
            //res^2 > x --> (x/res)^2 < x
            //类似于二分法，只不过该种二分法更具有适应性
            //此处如果不进行强制类型转换，则会无限迭代下去，精度增加
            while((int)res > (int)(x / res))
            {
                res = (res + x / res) / 2;
            }
            return (int)res;
        }
    }
}
