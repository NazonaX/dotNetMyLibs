using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ValidBracket
    {
        public bool IsValid(string s)
        {
            Stack<char> stk = new Stack<char>();
            int l = s.Length;
            stk.Push('#');
            bool Valid = false;
            for(int i = 0; i < l; i++)
            {
                if (s[i] == '(' || s[i] == '{' || s[i] == '[')
                {
                    stk.Push(s[i]);
                    continue;
                }
                char c = stk.Pop();
                if(c == '{' && s[i] != '}'
                    || c == '(' && s[i] != ')'
                    || c == '[' && s[i] != ']'
                    || c =='#')
                {
                    stk.Push(s[i]);
                    break;
                }
            }
            if (stk.Pop() == '#')
                Valid = true;
            return Valid;
        }
    }
}
