using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class StrStr
    {
        public int Solution(string haystack, string needle)
        {
            if (needle == null || needle.Equals(""))
                return 0;
            if (needle.Length > haystack.Length)
                return -1;
            int length = haystack.Length;
            int length2 = needle.Length;
            int j = 0;int k = 0;int i = 0;
            for(i = 0; i < length && k < length2 && j < length; )
            {
                if(haystack[j] == needle[k])
                {
                    k++;
                    j++;
                    continue;
                }
                else
                {
                    j = ++i;
                    k = 0;
                    continue;
                }
            }
            if (k == length2)
                return i;
            else
                return -1;
        }
    }
}
