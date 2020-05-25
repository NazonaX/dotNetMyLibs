using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class JumpII
    {
        public int Jump(int[] nums)
        {
            if (nums.Length == 1)
                return 0;
            int length = nums.Length;
            int[] jumped = new int[length];
            for (int i = 1; i < length; i++)
                jumped[i] = int.MaxValue;
            for(int i = 0; i < length; i++)
            {
                int tmp = nums[i];
                for (int j = 1; j <= tmp && j + i <= length - 1; j++)
                {
                    jumped[j + i] = jumped[j + i] > jumped[i] + 1 ? jumped[i] + 1 : jumped[i + j];
                }
            }
            return jumped[length - 1];
        }

        public int BestSolution(int[] nums)
        {
            if (nums.Length <= 2)
            {
                return nums.Length - 1;
            }
            int start = 0, end = 0, i = 1;
            for (; ; i++)
            {
                int count = 0;
                //在一段区间内选出步长能够到达数组最远处的值，然后再继续更新start和end，
                //end为最远处，start为原始end+1
                for (int j = start; j <= end && j < nums.Length; j++)
                {
                    count = Math.Max(count, j + nums[j]);
                }
                if (count >= nums.Length - 1)
                {
                    break;
                }
                else
                {
                    start = end + 1;
                    end = count;
                }
            }
            return i;
        }

        public bool CanJump(int[] nums)
        {
            if (nums.Length == 1)
                return true;
            int start = 0;int end = 0;
            int count = 0;int length = nums.Length;
            while (true)
            {
                for(int i = start; i <= end; i++)
                {
                    count = i + nums[i] < count ? count : nums[i] + i;
                }
                if (count == end)
                    return false;
                else if (count >= length - 1)
                    return true;
                else
                {
                    start = end + 1;
                    end = count;
                    count = 0;
                }
            }
        }
    }
}
