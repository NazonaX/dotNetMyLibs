using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RemoveElement
    {
        public int Solution(int[] nums, int val)
        {
            int index1 = 0;
            int index2 = 0;
            while(index2 != nums.Length)
            {
                if(nums[index2] == val)
                {
                    index2++;
                }
                else
                {
                    nums[index1++] = nums[index2++];
                }
            }
            return index1;
        }
    }
}
