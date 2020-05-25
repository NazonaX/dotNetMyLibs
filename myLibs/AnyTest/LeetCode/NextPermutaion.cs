using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class NextPermutaion
    {
        public void Solution(int[] nums)
        {
            if (nums == null || nums.Length < 2)
                return;
            bool change = false;int length = nums.Length;
            int tmp = 0;int minIndex = -1;
            for(int i = length - 1; i > 0 ; i--)
            {
                if(nums[i] > nums[i - 1])
                {
                    minIndex = i;
                    while (minIndex + 1 < length && nums[minIndex + 1] > nums[i - 1])
                        minIndex++;
                    tmp = nums[i - 1];
                    nums[i - 1] = nums[minIndex];
                    nums[minIndex] = tmp;
                    Array.Sort(nums, i, length - i);
                    change = true;
                    break;
                }
            }
            if (!change)
            {
                for(int i = 0; i < length / 2; i++)
                {
                    tmp = nums[i];
                    nums[i] = nums[length - i - 1];
                    nums[length - i - 1] = tmp;

                }
            }
        }
    }
}
