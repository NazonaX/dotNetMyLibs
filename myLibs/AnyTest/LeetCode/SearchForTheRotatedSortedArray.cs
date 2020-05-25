using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SearchForTheRotatedSortedArray
    {
        public int Search(int[] nums, int target)
        {
            if (nums == null || nums.Length < 1)
                return -1;
            if (nums.Length == 1)
                return nums[0] == target ? 0 : -1;
            int length = nums.Length;
            int indexLeft = 0;
            int indexRight = length - 1;
            int indexMid = length / 2;
            //find the rotated point
            while (indexLeft < indexRight)
            {
                if (nums[indexLeft] > nums[indexMid])
                    indexRight = indexMid;
                else
                    indexLeft = indexMid;
                indexMid = (indexRight + indexLeft) / 2;
                if (indexLeft == indexMid)
                    break;
            }
            int rotatedIndex = nums[0] <= nums[length / 2] && nums[length / 2] <= nums[length - 1] ?
                    0 : indexLeft + 1;
            if (rotatedIndex != 0)
            {
                if (target < nums[rotatedIndex] || target > nums[rotatedIndex - 1])
                    return -1;
                //to decide which the target belongs to
                if (target >= nums[0])
                {
                    //left part
                    indexLeft = 0;
                    indexRight = rotatedIndex - 1;
                }
                else
                {
                    //right part
                    indexLeft = rotatedIndex;
                    indexRight = length - 1;
                }
            }
            else
            {
                if (nums[0] > target || nums[length - 1] < target)
                    return -1;
                indexLeft = 0;
                indexRight = length - 1;
            }
            indexMid = (indexLeft + indexRight) / 2;
            while (indexLeft < indexRight)
            {
                if (nums[indexMid] == target)
                    return indexMid;
                else if (nums[indexMid] >= target)
                    indexRight = indexMid;
                else
                    indexLeft = indexMid;
                indexMid = (indexLeft + indexRight) / 2;
                if (indexMid == indexLeft || indexMid == indexRight)
                {
                    if (nums[indexLeft] == target)
                        return indexLeft;
                    else if (nums[indexRight] == target)
                        return indexRight;
                    else
                        return -1;
                }
            }
            if (indexLeft == indexRight && nums[indexRight] == target)
                return indexRight;
            return -1;
        }

        /// <summary>
        /// 在已排序的数组中寻找目标数，该数组可能包含重复数字，并且旋转点未知
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool SearchWithDuplicatedNumbers(int[] nums, int target)
        {
            if (nums.Length == 0)
                return false;
            //二分不适用，考虑(1,3,1,1,1,1) and (1,1,1,1,1,3,1)分不清左右
            //应当使用线性查找旋转点
            //首先找到旋转点
            int length = nums.Length;
            int start = 0;
            int end = length - 1;
            int mid = (start + end) / 2;
            int zeroPoint = 0;
            while (zeroPoint + 1 < length && nums[zeroPoint] <= nums[zeroPoint + 1])
            {
                if (nums[zeroPoint++] == target)
                    return true;
            }
            zeroPoint++;
            zeroPoint = zeroPoint == length ? 0 : zeroPoint;
            if (target < nums[zeroPoint] || (zeroPoint == 0 && target > nums[length - 1])
                || (zeroPoint != 0 && target > nums[zeroPoint - 1]))
                return false;
            if (target <= nums[length - 1])
            {
                start = zeroPoint;
                end = length - 1;
            }
            else
            {
                start = 0;
                end = zeroPoint - 1;
            }
            mid = (start + end) / 2;
            while (start < mid)
            {
                if (nums[mid] == target)
                    return true;
                if (nums[mid] < target)
                {
                    start = mid;
                    mid = (start + end) / 2;
                }
                else
                {
                    end = mid;
                    mid = (start + end) / 2;
                }
            }
            if (end < start)
                return false;
            else if (nums[start] == target || nums[end] == target)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 遇到重复就用线性直到重复消失
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool SearchWithDuplicatedNumbersBestSolution(int[] nums, int target)
        {
            if (nums.Length == 0)
                return false;
            int start = 0;
            int end = nums.Length - 1;
            int mid = (start + end) / 2;
            while(start < mid)
            {
                if(nums[mid] == nums[end])
                {
                    while (mid != end && nums[end] == nums[end - 1])
                        end--;
                    if(mid == end)
                    {
                        end = mid;
                    }
                    else
                    {
                        start = mid;
                    }
                    mid = (start + end) / 2;
                }
                else
                {
                    if (target == nums[mid])
                        return true;
                    if(nums[mid] < nums[end])
                    {
                        
                        if (target > nums[mid])
                        {
                            //哪边有序使用哪边进行换边判断，此处为右
                            if (target <= nums[end])
                                start = mid;
                            else
                                end = mid;
                        }
                        else if (target < nums[mid])
                        {
                            end = mid;
                        }
                    }
                    else
                    {
                        if(target > nums[mid])
                        {
                            start = mid;
                        }
                        else
                        {
                            ////哪边有序使用哪边进行换边判断，此处为左
                            if (target < nums[start])
                                start = mid;
                            else
                                end = mid;
                        }
                    }
                    mid = (start + end) / 2;
                }
            }
            if (target == nums[start] || target == nums[end])
                return true;
            else
                return false;
        }
    }
}
