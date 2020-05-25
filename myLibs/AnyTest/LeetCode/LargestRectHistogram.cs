using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LargestRectHistogram
    {
        /// <summary>
        /// 单调栈方法
        /// 给定一个数组，求出连续的矩形的最大面积
        /// etc. input [2,1,5,6,2,3]: --> return 10
        /// </summary>
        /// <param name="heights"></param>
        /// <returns></returns>
        public int Solution(int[] heights)
        {
            int res = int.MinValue;
            int length = heights.Length;
            if (length == 0)
                return 0;
            else if (length == 1)
                return heights[0];
            int[] LeftGE = new int[length];
            int[] RigthGE = new int[length];
            //利用单调栈对左高和右高进行填充
            Stack<int> tmpStk = new Stack<int>();
            for(int i = 0; i < length; i++)
            {
                //小于栈顶则压入，保证栈内自顶向下是递减的
                while(tmpStk.Count > 0 && heights[tmpStk.Peek()] > heights[i])
                {
                    int top = tmpStk.Pop();
                    RigthGE[top] = i - top - 1;
                }
                tmpStk.Push(i);//压入下标，用于计算宽度
            }
            while(tmpStk.Count > 0)
            {
                int top = tmpStk.Pop();
                RigthGE[top] = length - top - 1;
            }
            for (int i = length - 1; i >= 0; i--)
            {
                while (tmpStk.Count > 0 && heights[tmpStk.Peek()] > heights[i])
                {
                    int top = tmpStk.Pop();
                    LeftGE[top] = top - i - 1;
                }
                tmpStk.Push(i);
            }
            while (tmpStk.Count > 0)
            {
                int top = tmpStk.Pop();
                LeftGE[top] = top;
            }
            for(int i = 0; i < length; i++)
            {
                int val = heights[i] * (LeftGE[i] + RigthGE[i] + 1);
                if (res < val)
                    res = val;
            }
            return res;
        }
    }
}
