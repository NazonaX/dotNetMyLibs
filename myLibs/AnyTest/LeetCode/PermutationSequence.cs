using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class PermutationSequence
    {
        public string GetPermutation(int n, int k)
        {
            StringBuilder sb = new StringBuilder();
            List<int> tmp = new List<int>();
            int summer = 1;
            for (int i = 1; i <= n; i++)
            {
                tmp.Add(i);
                summer *= i;
            }
            summer /= n;
            DoTraceBack(sb, tmp, k - 1, summer, n - 1);
            return sb.ToString();
        }

        private void DoTraceBack(StringBuilder sb, List<int> tmp, int k, int summer, int now)
        {
            int index = (k) / summer;
            int res = (k) % summer;
            sb.Append(tmp[index]);
            tmp.RemoveAt(index);
            if (now == 0)
                return;
            else
                DoTraceBack(sb, tmp, res, summer / now, now - 1);
        }
    }
}
