using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LongetEchoSubstring
    {
        //string assignment will cost a lot of time like s+=char
        public string Solve(string s)
        {
            double cursor = 0;
            string longest = "";
            int indexLeft = -1;
            int indexRight = -1;
            int longestLength = 0;
            int length = 0;
            int longestCL = 0;
            int longestCR = 0;
            while (cursor < s.Length)
            {
                indexLeft = -1;
                indexRight = -1;
                if (cursor % 1 == 0)
                {
                    indexLeft = (int)(cursor);
                    indexRight = (int)(cursor);
                }
                else
                {
                    indexLeft = (int)(cursor - 0.5);
                    indexRight = (int)(cursor + 0.5);
                }
                int min = indexLeft + 1 < s.Length - indexRight ? indexLeft + 1 : s.Length - indexRight;
                if (longestLength > longestLength + 2 * min)
                    break;
                while(indexLeft >= 0 && indexRight < s.Length)
                {
                    if (s[indexLeft] == s[indexRight])
                    {
                        indexLeft--;
                        indexRight++;
                    }
                    else
                        break;
                }
                indexRight--;
                indexLeft++;
                length = indexRight - indexLeft + 1;
                if (length > longestLength)
                {
                    longestLength = length;
                    longestCL = indexLeft;
                    longestCR = indexRight;
                }
                cursor += 0.5;
            }
            return s.Substring(longestCL, longestLength);
        }

        public string longestPalindrome(string s)
        {
            if (s == null || s.Length < 1) return "";
            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int len1 = expandAroundCenter(s, i, i);
                int len2 = expandAroundCenter(s, i, i + 1);
                int len = Math.Max(len1, len2);
                if (len > end - start)
                {
                    start = i - (len - 1) / 2;
                    end = i + len / 2;
                }
            }
            return s.Substring(start, end - start + 1);
        }

        private int expandAroundCenter(string s, int left, int right)
        {
            int L = left, R = right;
            while (L >= 0 && R < s.Length && s[L] == s[R])
            {
                L--;
                R++;
            }
            return R - L - 1;
        }

    }
}
