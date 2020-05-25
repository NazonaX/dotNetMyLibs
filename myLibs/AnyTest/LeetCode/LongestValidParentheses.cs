using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LongestValidParentheses
    {
        public int Solution(string s)
        {
            int res = 0;
            int length = s.Length;
            int[] vars = new int[length];
            int start = 0; int end = 0;
            for(int i = 0; i < length;)
            {
                if(s[i] == '(')
                {
                    int counter = 0;
                    start = i;
                    end = i;
                    while(end < length)
                    {
                        if (s[end] == '(')
                            vars[end] = ++counter;
                        else if (s[end] == ')')
                            vars[end] = --counter;
                        end++;
                    }
                    end = start;
                    for(int j = start; j < length; j++)
                    {
                        if (vars[j] < 0)
                            break;
                        else if (vars[j] == 0)
                            end = j;
                    }
                    i = end + 1;
                    if (start != end && res < end - start + 1)
                        res = end - start + 1;
                    continue;
                }
                i++;
            }
            return res;
        }
        public int Solution2(string s)
        {
            if (s == null || s.Length < 2)
                return 0;
            int res = 0;
            int length = s.Length;
            int[] vars = new int[length];
            bool[] canStart = new bool[length];
            vars[0] = 0;canStart[0] = s[0] == '(' ? true : false;
            int i = 0;
            for (i = 1; i < length; i++)
            {
                canStart[i] = s[i] == '(' ? true : false;
                vars[i] = s[i] == '(' ? vars[i - 1] + 1 : vars[i - 1] - 1;
            }
            for(i = 0; i < length;)
            {
                if (canStart[i])
                {
                    int startVal = vars[i];
                    int endIndex = i;
                    int k = endIndex;
                    int threshold = startVal - 1;
                    while (k < length)
                    {
                        if (vars[k] >= threshold)
                        {
                            if (vars[k] == threshold)
                                endIndex = k;
                            k++;
                        }
                        else
                            break;
                    }
                    if(endIndex != i)
                        res = res < endIndex - i + 1 ? endIndex - i + 1 : res;
                    i = endIndex + 1;
                }
                else
                    i++;
            }
            return res;
        }

        public int BestSolution(string s)
        {
            int num = 0;
            int left = 0;
            int currentValue = 0;
            int N = s.Length;
            int[] sInt = new int[N];
            for (int i = 0; i < N; i++)
            {
                if (s[i] == '(')
                {
                    left++;
                }
                else
                {
                    if (left > 0)
                    {
                        int temp = 0;
                        for (int j = i; j >= 0; j--)
                        {
                            if (temp == 2) break;
                            if (sInt[j] == 0)
                            {
                                sInt[j] = 1;
                                temp++;
                            }
                        }
                        left--;
                    }
                }

            }
            for (int i = 0; i < N; i++)
            {
                if (sInt[i] == 1)
                    currentValue++;
                else
                {
                    num = num > currentValue ? num : currentValue;
                    currentValue = 0;
                }
            }
            num = num > currentValue ? num : currentValue;
            return num;
        }
    }
}
