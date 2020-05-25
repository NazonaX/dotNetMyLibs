using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class FindIntervalOfSortedArray
    {
        public int[] SearchRange(int[] nums, int target)
        {
            int[] res = new int[2];
            res[0] = -1;res[1] = -1;
            if (nums == null || nums.Length == 0)
                return res;
            int length = nums.Length;
            int indexLeft = 0;
            int indexRight = length - 1;
            int indexMid = (indexLeft + indexRight) / 2;
            //find rigth part
            while(indexLeft < indexRight)
            {
                if (nums[indexMid] >= target)
                    indexRight = indexMid;
                else
                    indexLeft = indexMid;
                indexMid = (indexLeft + indexRight) / 2;
                if (indexMid == indexLeft)
                    break;
            }
            if (nums[indexLeft] == target)
                res[0] = indexLeft;
            else if (nums[indexRight] == target)
                res[0] = indexRight;
            else if(indexLeft == indexRight || indexLeft == indexRight - 1)
            {
                return res;
            }
            indexLeft = 0;
            indexRight = length - 1;
            indexMid = (indexLeft + indexRight) / 2;
            while(indexLeft < indexRight)
            {
                if (nums[indexMid] <= target)
                    indexLeft = indexMid;
                else
                    indexRight = indexMid;
                indexMid = (indexLeft + indexRight) / 2;
                if (indexMid == indexLeft)
                    break;
            }
            if (nums[indexRight] == target)
                res[1] = indexRight;
            else if (nums[indexLeft] == target)
                res[1] = indexLeft;
            return res;
        }
    }
}
