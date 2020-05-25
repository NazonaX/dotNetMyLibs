using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SubString
    {
        /// <summary>
        /// 找到包含t所有字母的s中的最小子串
        /// 找不到则返回""空串
        /// 名为滑动窗口法
        /// 若使用数组，则注意复数代表的含义“无该符号值”或者“该符号值有重复并且多余”
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public string MinWindow(string s, string t)
        {
            if(t.Length > s.Length || s.Length == 0 || t.Length == 0)
                return "";
            int indexLeft = 0;
            int indexRight = 0;
            int start = -1;
            int end = -1;
            int lengthS = s.Length;
            int lengthT = t.Length;
            int min = lengthS + 1;
            int counter = 0;
            Dictionary<char, int> dict = new Dictionary<char, int>();
            for(int i = 0; i < lengthT; i++)
            {
                if (dict.ContainsKey(t[i]))
                    dict[t[i]]++;
                else
                    dict.Add(t[i], 1);
            }
            while(indexLeft <= lengthS - lengthT && indexRight < lengthS)
            {
                if(dict.ContainsKey(s[indexRight]))
                {
                    dict[s[indexRight]]--;
                    if (dict[s[indexRight]] >= 0)
                        counter++;
                }
                if(counter == lengthT)
                {
                    //右移左限
                    while (!dict.ContainsKey(s[indexLeft]) || dict[s[indexLeft]] < 0)
                    {
                        if (dict.ContainsKey(s[indexLeft]))//范围内多余的值
                            dict[s[indexLeft]]++;
                        indexLeft++;
                    }
                    if(min > (indexRight - indexLeft))
                    {
                        min = indexRight - indexLeft;
                        start = indexLeft;
                        end = indexRight;
                    }
                    //当前左向右移动一位，该左必定是有效字符值
                    counter--;
                    dict[s[indexLeft]]++;
                    indexLeft++;
                }
                indexRight++;
            }
            return end == -1 ? "" : s.Substring(start, end - start + 1);
        }
    }
}
