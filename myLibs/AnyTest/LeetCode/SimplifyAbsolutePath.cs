using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{

    public class SimplifyAbsolutePath
    {
        public string SimplifyPath(string path)
        {
            StringBuilder sb = new StringBuilder();
            Stack<string> stack = new Stack<string>();
            string[] strs = path.Split('/');
            for(int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == "" || strs[i] == ".")
                    continue;
                else if (strs[i] == "..")
                    if (stack.Count == 0)
                        continue;
                    else
                        stack.Pop();
                else
                    stack.Push(strs[i]);
            }
            int stackCount = stack.Count;
            if (stackCount > 0)
                for (int i = 0; i < stackCount; i++)
                {
                    sb.Insert(0, stack.Pop());
                    sb.Insert(0, "/");
                }
            else
                sb.Append('/');
            return sb.ToString();
        }
    }
}
