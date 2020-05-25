using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SearchInsertPosition
    {
        public int SearchInsert(int[] nums, int target)
        {
            if (nums == null || nums.Length == 0)
                return 0;
            int indexLeft = 0;
            int length = nums.Length;
            int indexRight = length - 1;
            int indexMid = (indexLeft + indexRight) / 2;
            while(indexLeft != indexMid)
            {
                if (nums[indexMid] == target)
                    return indexMid;
                else if (nums[indexMid] > target)
                    indexRight = indexMid;
                else if (nums[indexMid] < target)
                    indexLeft = indexMid;
                indexMid = (indexLeft + indexRight) / 2;
            }
            if (nums[indexLeft] == target)
                return indexLeft;
            else if (nums[indexRight] == target)
                return indexRight;
            else if (nums[indexLeft] > target)
                return indexLeft;
            else if (nums[indexLeft] < target && nums[indexRight] > target)
                return indexLeft + 1;
            else
                return indexRight + 1;
        }
    }
}
