using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class DecodeWays
    {
        /// <summary>
        /// 规定字母A-Z表示为数字1-26
        /// 输入一串字符串包含数字0~9，
        /// 输出数字字符串代表的字母组合的可能性的个数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int NumDecodings(string s)
        {
            //分割串，然后求出每串的斐波那契数
            //分割表示为n位1or2，尾位：1-0~9or2-0~6，使能够凑成一串可以任意两两组合的子串
            //其他情况的子串则肯定只能组成1中情况
            //注意0的位置，遇到0需要单独与前一个字符做判断，0和大于2的数都将直接不符合条件
            if (s == null || s == "" || s.Length > 0 && s[0] == '0')
                return 0;
            Dictionary<int, int> febnaciDict = new Dictionary<int, int>();
            febnaciDict.Add(0, 1);
            febnaciDict.Add(1, 1);
            int start = 0;
            bool start_ = true;
            int res = 1;
            for(int i = 0; i < s.Length; i++)
            {
                if(s[i] == '1' || s[i] == '2')
                {
                    if (start_)
                    {
                        start = i;
                        start_ = false;
                    }
                    continue;
                }
                else if(s[i] == '0')
                {
                    if (i - 1 < 0)
                        return 0;
                    else if (s[i - 1] - '0' == 0 || s[i - 1] - '0' > 2)
                        return 0;
                    if(!start_)
                        res *= GetFebNaciNumber(febnaciDict, i - start - 1);
                    start = 0;
                    start_ = true;
                }
                else if(!start_)
                {
                    if(i - 1 >= 0)
                    {
                        int x = s[i - 1] - '1' + 1;
                        int y = s[i] - '1' + 1;
                        int end = 0;
                        if(x == 1)
                        {
                            end = i;
                        }
                        else if(x == 2)
                        {
                            if (y >= 0 && y <= 6)
                                end = i;
                            else
                                end = i - 1;
                        }
                        res *= GetFebNaciNumber(febnaciDict, end - start + 1);
                    }
                    start = 0;
                    start_ = true;
                }
            }
            if (!start_)
                res *= GetFebNaciNumber(febnaciDict, s.Length - start);
            return res;
        }

        private int GetFebNaciNumber(Dictionary<int,int> febdict,  int v)
        {
            if(febdict.ContainsKey(v))
            {
                return febdict[v];
            }
            else
            {
                int x = GetFebNaciNumber(febdict, v - 1);
                int y = GetFebNaciNumber(febdict, v - 2);
                if (!febdict.ContainsKey(v - 1))
                    febdict.Add(v - 1, x);
                if (!febdict.ContainsKey(v - 2))
                    febdict.Add(v - 2, y);
                return x + y;
            }
        }
    }
}
