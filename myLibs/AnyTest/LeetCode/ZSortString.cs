using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    class ZSortString
    {
        public string Solve(string s, int numRows)
        {
            int groupLength = numRows > 1 ? numRows * 2 - 2 : 1;
            int groupNum = 1;int iter = 1;
            StringBuilder sb = new StringBuilder();
            while(iter <= numRows)
            {
                if(iter == 1 || iter == numRows)
                {
                    groupNum = 0;
                    int index = groupNum * groupLength + iter - 1;
                    while(index < s.Length)
                    {
                        sb.Append(s[index]);
                        index = ++groupNum * groupLength + iter - 1;
                    }
                }
                else
                {
                    groupNum = 0;
                    int index1 = groupNum * groupLength + iter - 1;
                    int index2 = (groupNum * groupLength + numRows) * 2 - index1 - 2;
                    while (index1 < s.Length || index2 < s.Length)
                    {
                        if(index1 < s.Length)
                            sb.Append(s[index1]);
                        if (index2 < s.Length)
                            sb.Append(s[index2]);
                        index1 = ++groupNum * groupLength + iter - 1;
                        index2 = (groupNum * groupLength + numRows) * 2 - index1 - 2;
                    }
                }
                iter++;
            }
            return sb.ToString();
        }
    }
}
