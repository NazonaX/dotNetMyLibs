using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LengthOfTheLastWorld
    {
        public int LengthOfLastWord(string s)
        {
            int index = -1;
            s = s.Trim();
            for (int i = 0; i < s.Length; i++)
                if (s[i] == ' ')
                    index = i;
            return s.Length == 0 ? 0 : s.Length - index - 1;
        }
    }
}
