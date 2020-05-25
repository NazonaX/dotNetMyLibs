using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RegexMatch
    {
        public bool Solve(string s, string p)
        {
            if (p.Length == 0 && s.Length == 0)
                return true;
            if (p.Length == 0 && s.Length != 0)
                return false;
            if(p.Length == 1)
            {
                if (s.Length != 1)
                    return false;
                else if (!p.Equals("."))
                    return s.Equals(p);
                else
                    return true;
            }
            if(s.Length == 0)
            {
                if (p.Length % 2 != 0)
                    return false;
                bool canMatch = true;
                for (int i = 1; i < p.Length; i += 2)
                    canMatch = canMatch && (p[i] == '*');
                return canMatch;
            }
            char ch1 = p[0];
            char ch2 = p[1];
            if(p[1] == '*')
            {
                bool hasMatch = false;
                int iter = 0;
                while (iter <= s.Length)
                {
                    string sub = "";
                    hasMatch = true;
                    if (iter != 0)
                        sub = s.Substring(0, iter);
                    if (ch1 != '.')
                    {
                        for (int i = 0; i < iter; i++)
                            hasMatch = hasMatch && sub[i] == (ch1);
                    }
                    else
                        hasMatch = true;
                    hasMatch = hasMatch && Solve(s.Substring(iter, s.Length - iter), p.Substring(2, p.Length - 2));
                    if (hasMatch)
                        break;
                    iter++;
                }
                return hasMatch;
            }
            else
            {
                if(p[0] != '.')
                    return (p[0] == s[0]) && Solve(s.Substring(1, s.Length - 1), p.Substring(1, p.Length - 1));
                else
                    return Solve(s.Substring(1, s.Length - 1), p.Substring(1, p.Length - 1));
            }
        }
    }
}
