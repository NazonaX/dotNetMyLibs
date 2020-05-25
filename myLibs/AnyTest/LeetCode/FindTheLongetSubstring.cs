using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class FindTheLongetSubstring
    {
        //the normal one
        public int Solve(string str)
        {
            int max = 0;
            int counter = 0;
            Dictionary<char, int> dict = new Dictionary<char, int>();
            for(int i = 0; i < str.Length; i++)
            {
                counter = 1;
                dict.Clear();
                char ch = str[i];
                dict.Add(ch, i);
                for(int j = i + 1; j < str.Length; j++)
                {
                    if (dict.ContainsKey(str[j]))
                        break;
                    else
                    {
                        counter++;
                        dict.Add(str[j], j);
                    }
                }
                if (max < counter)
                    max = counter;
                if (max >= str.Length - i)
                    break;
            }
            return max;
        }

        //the best one
        public int Solve2(string str)
        {
            string tmp = "";
            string max = "";
            foreach (char c in str)
            {
                string s = "" + c;
                if (!tmp.Contains(s))
                    tmp += c;
                else
                {
                    if(max.Length < tmp.Length)
                    {
                        max = tmp;
                    }
                    tmp = tmp.Remove(0, tmp.IndexOf(c) + 1);
                    tmp += c;
                }
            }
            return max.Length > tmp.Length ? max.Length : tmp.Length;
        }
    }
}
