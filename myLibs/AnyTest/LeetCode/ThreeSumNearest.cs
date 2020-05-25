using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ThreeSumNearest
    {
        public int ThreeSumClosest(int[] nums, int target)
        {
            int res = 0;
            if (nums == null || nums.Length < 3)
                return 0;
            Array.Sort(nums);
            int index1 = 0;
            int index2 = 0;
            int length = nums.Length;
            int index3 = 0;
            int minGap = int.MaxValue;
            int gap = 0;
            while(index1 <= length - 3)
            {
                index2 = index1 + 1;
                index3 = length - 1;
                while(index2 < index3)
                {
                    gap = (nums[index1] + nums[index2] + nums[index3]) - target;
                    if (Math.Abs(gap) < minGap)
                    {
                        res = nums[index1] + nums[index2] + nums[index3];
                        minGap = Math.Abs(gap);
                    }
                    if (gap < 0)
                    {
                        while (index2 < index3 && nums[index2] == nums[index2 + 1]) index2++;
                        index2++;
                    }
                    else if (gap >= 0)
                    {
                        while (index2 < index3 && nums[index3] == nums[index3 - 1]) index3--;
                        index3--;
                    }
                }
                while (index1 <= length - 3 && nums[index1] == nums[index1 + 1]) index1++;
                index1++;
            }
            return res;
        }
    }
}
