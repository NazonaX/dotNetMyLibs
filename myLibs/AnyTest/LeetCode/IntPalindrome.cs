using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class IntPalindrome
    {
        public bool Solve(int x)
        {
            if (x < 0)
                return false;
            int res = 0;
            int resi = 0;
            int tmp = x;
            while(tmp != 0)
            {
                resi = tmp % 10;
                tmp /= 10;
                res = res * 10 + resi;
            }
            return res == x;
        }
    }
}
