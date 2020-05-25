using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class DivideTwoIntegers
    {
        public int Divide(int dividend, int divisor)
        {
            if (divisor == 1 || dividend == 0)
                return dividend;
            else if (divisor == -1)
            {
                if (dividend == int.MinValue)
                    return int.MaxValue;
                else
                    return -dividend;
            }
            else if (divisor == int.MinValue && dividend != int.MinValue)
                return 0;
            else if (dividend == int.MinValue && divisor == int.MinValue)
                return 1;
            //convert to minus integeres for [-2^32, 2^32 - 1]
            bool isMinus = (dividend < 0 && divisor > 0 || dividend > 0 && divisor < 0) ? true : false;
            bool isMin = dividend == int.MinValue ? true : false;
            dividend = dividend < 0 ?
                dividend == int.MinValue ? int.MaxValue : -dividend 
                : dividend;
            divisor = divisor < 0 ? -divisor : divisor;
            int counter = 0;
            int tmp = 0;int tmp_stand = 0;
            int res = 0;
            while(divisor <= dividend)
            {
                counter = 1;
                tmp = divisor;
                tmp_stand = divisor;
                while(tmp <= dividend && tmp > 0)
                {
                    tmp_stand = tmp;
                    counter <<= 1;
                    tmp <<= 1;
                }
                res += counter >> 1;
                dividend -= tmp_stand;
            }
            if (!isMin) return isMinus ? -res : res;
            else
            {
                if (int.MinValue == (isMinus ? -(res + 1) * divisor : (res + 1) * divisor))
                    res += 1;
                return isMinus ? -res : res;
            }
        }
    }
}
