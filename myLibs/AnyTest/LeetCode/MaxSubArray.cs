using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MaxSubArray
    {
        public int MySolution(int[] nums)
        {
            int res = nums[0];
            int sum = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                if (sum > 0)
                    sum += nums[i];
                else
                    sum = nums[i];
                res = sum > res ? sum : res;
            }
            return res;
        }
    }
}
