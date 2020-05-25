using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ReverseInt
    {
        public int Solve(int x)
        {
            StringBuilder sb = new StringBuilder();
            bool minus = x < 0 ? true : false;
            int res = 0;
            while(x != 0)
            {
                res = x % 10;
                x /= 10;
                sb.Append(Math.Abs(res));
            }
            if (minus)
                sb.Insert(0, '-');
            try
            {
                return int.Parse(sb.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// faster with no double calculate and string operation
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int Solve2(int x)
        {
            int res = 0;
            int resi = 0;
            int before = 0;
            while (x != 0)
            {
                resi = x % 10;
                x /= 10;
                before = res;
                res = res * 10 + resi;
                if (before != 0 && res / before < 10)
                    return 0;
            }
            return res;
        }
    }
}
