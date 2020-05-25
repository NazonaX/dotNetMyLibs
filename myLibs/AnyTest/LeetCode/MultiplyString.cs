using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MultiplyString
    {
        public string Multiply(string num1, string num2)
        {
            if (num1.Equals("0") || num2.Equals("0"))
                return "0";
            StringBuilder sb = new StringBuilder();
            int length1 = num1.Length;
            int length2 = num2.Length;
            int[] resByte = new int[length1 + length2];
            int[] byteNum1 = new int[length1];
            int[] byteNum2 = new int[length2];
            for(int i = 0; i < length1; i++)
            {
                byteNum1[i] = (num1[i] - '0');
            }
            for(int i = 0; i < length2; i++)
            {
                byteNum2[i] = (num2[i] - '0');
            }
            int x = 0;int y = 0;
            int addition = 0;
            int pos = 0;
            int[] tmp = new int[length1 + 1];
            for(int i = length2 - 1; i >= 0; i--)
            {
                //multiply
                tmp[0] = 0;
                y = 0;
                for(int j = length1 -1; j >=0; j--)
                {
                    x = byteNum2[i] * byteNum1[j] + y;
                    tmp[j + 1] = x % 10;
                    y = x / 10;
                }
                if (y != 0)
                    tmp[0] = y;
                //add
                y = 0;
                for(int j = 0; j <= length1; j++)
                {
                    addition = resByte[length1 + length2 - pos - 1 - j] + tmp[length1 - j] + y;
                    resByte[length1 + length2 - pos - 1 - j] = addition % 10;
                    y = addition / 10;
                }
                if (y != 0)
                    resByte[length2 - pos - 1] += y;
                pos++;
            }
            if (resByte[0] != 0)
                sb.Append(resByte[0]);
            for(int i = 1; i < resByte.Length; i++)
            {
                sb.Append(resByte[i]);
            }
            return sb.ToString();
        }
    }
}
