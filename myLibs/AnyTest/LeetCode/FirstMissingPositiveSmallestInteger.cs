using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class FirstMissingPositiveSmallestInteger
    {
        public int FirstMissingPositive(int[] nums)
        {
            int smallest = 1;
            if (nums == null || nums.Length == 0)
                return smallest;
            int length = nums.Length;
            int nextNum = 0;
            int nextIndex = 0;
            int nowIndex = 0;
            for(int i = 0; i < length; i++)
            {
                if (nums[i] == int.MinValue)
                    continue;
                else
                {
                    nextIndex = i;nextNum = nums[nextIndex];
                    bool has = false;
                    while(nextIndex >=0 && nextIndex < length && nextNum != int.MinValue)
                    {
                        nowIndex = nextIndex;
                        nextIndex = nextNum - 1;
                        if (nextIndex >= 0 && nextIndex < length)
                        {
                            if (nums[nextIndex] == nextIndex + 1)
                            {
                                if(nextIndex == i)
                                    has = true;
                                break;
                            }
                            nextNum = nums[nextIndex];
                            nums[nextIndex] = nextIndex + 1;
                            if (nextIndex == i)
                                has = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if(!has)
                        nums[i] = int.MinValue;
                }
            }
            bool found = false;
            for(int i = 0; i < length; i++)
            {
                if(nums[i] == int.MinValue)
                {
                    smallest = i + 1;
                    found = true;
                    break;
                }
            }
            if (!found)
                smallest = nums.Length + 1;
            return smallest;
        }
    }
}
