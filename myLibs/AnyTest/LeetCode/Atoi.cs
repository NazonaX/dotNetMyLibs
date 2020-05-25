using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Atoi
    {
        public int Solve(string str)
        {
            str = str.TrimStart();
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < str.Length; i++)
            {
                if (i == 0 && (str[i] == '-' || str[i] == '+'))
                    sb.Append(str[i]);
                else if (str[i] >= '0' && str[i] <= '9')
                    sb.Append(str[i]);
                else
                    break;
            }
            try
            {
                return int.Parse(sb.ToString());
            }
            catch(OverflowException ofe)
            {
                if (sb.ToString().Contains("-"))
                    return int.MinValue;
                else
                    return int.MaxValue;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
    }
}
