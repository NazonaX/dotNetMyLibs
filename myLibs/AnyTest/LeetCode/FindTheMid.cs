using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class FindTheMid
    {
        public double Solve(int[] num1, int[] num2)
        {
            int length1 = num1.Length;
            int length2 = num2.Length;
            if (length1 > length2)
                return FindTheMidAlg(num2, num1, length2, length1);
            else
                return FindTheMidAlg(num1, num2, length1, length2);
        }
        //the time complexity is log(m+n).
        private double FindTheMidAlg(int[] num1, int[] num2, int length1, int length2)
        {
            int index = (length1 + length2 - 2) / 2;
            int start = 0, end = length1 + 1;
            double res = 0;
            if (length1 == 0)
                return length2 % 2 == 1 ? num2[length2 / 2] * 1.0 : (num2[length2 / 2 - 1] + num2[length2 / 2]) / 2.0;
            while (true)
            {
                int i = (start + end) / 2 - 1 + (start + end) % 2;
                //int addition = (end - start + 1) % 2;
                //i = i - addition;
                int j = index - i;
                int maxLeft = -1; 
                int minRight = -1;
                bool rightDir = false;
                int? num1Left = null;
                int? num2Left = null;
                int? num1Right = null;
                int? num2Right = null;
                if (i != -1 && i != length1)
                    num1Left = num1[i];
                if (i + 1 != length1)
                    num1Right = num1[i + 1];
                if (j != 0)
                    num2Left = num2[j - 1];
                if (j != length2)
                    num2Right = num2[j];
                if (num1Left != null && num2Left != null)
                {
                    if (num1Left > num2Left)
                    {
                        rightDir = false;
                        maxLeft = num1Left ?? int.MinValue;
                    }
                    else
                    {
                        rightDir = true;
                        maxLeft = num2Left ?? int.MinValue;
                    }
                }
                else if (num1Left != null)
                {
                    rightDir = false;
                    maxLeft = num1Left ?? int.MinValue;
                }
                else
                {
                    rightDir = true;
                    maxLeft = num2Left ?? int.MinValue;
                }
                if (num1Right != null && num2Right != null)
                {
                    if (num1Right < num2Right)
                        minRight = num1Right ?? int.MaxValue;
                    else
                        minRight = num2Right ?? int.MaxValue;
                }
                else if (num1Right != null)
                    minRight = num1Right ?? int.MaxValue;
                else
                    minRight = num2Right ?? int.MaxValue;

                if (maxLeft > minRight)
                {
                    if (rightDir)
                        start = (start + end) / 2;
                    else
                        end = (start + end) / 2;
                }
                else if (maxLeft <= minRight)
                {
                    if ((length1 + length2) % 2 == 1)
                        res = minRight * 1.0;
                    else
                        res = (maxLeft + minRight) / 2.0;
                    break;
                }
            }
            return res;
        }
    }
}
