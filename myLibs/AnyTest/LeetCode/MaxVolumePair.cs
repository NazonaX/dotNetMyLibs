using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MaxVolumePair
    {   
        /// <summary>
        /// O(n^2)
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public int Solve(int[] height)
        {
            int max = int.MinValue;
            int max_length = height.Length - 1;
            int length = height.Length;
            for(int i = 1; i <= max_length; i++)
            {
                for(int j = 0; j + i < length; j++)
                {
                    int volume = i * (height[j] < height[j + i] ? height[j] : height[j + i]);
                    if (volume > max)
                        max = volume;
                }
            }
            return max;
        }

        public int Solve2(int[] height)
        {
            int max = int.MinValue;
            int pre_cursor = 0;int aft_cursor = height.Length - 1;
            while(pre_cursor != aft_cursor)
            {
                max = Math.Max(max, (aft_cursor - pre_cursor) * Math.Min(height[pre_cursor], height[aft_cursor]));
                if (height[pre_cursor] < height[aft_cursor])
                    pre_cursor++;
                else
                    aft_cursor--;
            }
            return max;
        }
    }
}
