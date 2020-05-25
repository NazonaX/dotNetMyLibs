using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LongestCommomPrefix
    {
        public string LongestCommonPrefix(string[] strs)
        {
            if (strs == null || strs.Length == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            int min = int.MaxValue;
            for(int i = 0; i < strs.Length; i++)
            {
                min = strs[i].Length < min ? strs[i].Length : min;
            }
            if (min == 0)
                return sb.ToString();
            for(int i = 0; i < min; i++)
            {
                bool common = true;
                char ch1 = strs[0][i];
                for(int j = 1; j < strs.Length; j++)
                {
                    common = common && (ch1 == strs[j][i]);
                    if (!common)
                        break;
                }
                if (common)
                    sb.Append(ch1);
                else
                    break;
            }
            return sb.ToString();
        }
    }
}
