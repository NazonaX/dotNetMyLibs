using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class CountSay
    {
        public string CountAndSay(int n)
        {
            int i = 1;
            string baseStr = "1";
            int counter = 0;
            StringBuilder sb = new StringBuilder();
            int index1 = 0;char pre = '\0';
            while(i < n)
            {
                for(index1 = 0; index1 < baseStr.Length; index1++)
                {
                    if(pre == '\0')
                    {
                        pre = baseStr[index1];
                        counter++;
                        continue;
                    }
                    else if(baseStr[index1] == pre)
                    {
                        counter++;
                        continue;
                    }
                    else
                    {
                        sb.Append(counter).Append(pre);
                        pre = baseStr[index1];
                        counter = 1;
                    }
                }
                sb.Append(counter).Append(baseStr[index1 - 1]);
                baseStr = sb.ToString();
                sb.Clear();
                pre = '\0';
                counter = 0;
                i++;
            }
            return baseStr;
        }
    }
}
