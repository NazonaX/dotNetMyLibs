using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Sort
    {
        //不使用库函数对数组进行排序，传入的数组只会包含0、1、2
        //通常做法是扫描两遍存储0、1、2的数量，然后在扫描一遍进行赋值
        //能否使用一遍扫描，并且空间使用固定的方法
        public void SortColors(int[] nums)
        {
            int start = 0, end = nums.Length - 1;
            int Left = 0, Right = end;
            for (; start <= end;)
            {
                if (nums[start] == 0)
                {
                    start++;
                    nums[Left++] = 0;
                    continue;
                }
                if (nums[end] == 2)
                {
                    end--;
                    nums[Right--] = 2;
                    continue;
                }
                else if (nums[start] == 2 && nums[end] == 0)
                {
                    nums[Left] = 0;
                    nums[Right] = 2;
                    end--; Right--;
                    start++; Left++;
                    continue;
                }
                else if (nums[start] == 2 && nums[end] != 0)
                {
                    nums[Right] = 2;
                    end--; Right--;
                    start++;
                }
                else if (nums[start] != 2 && nums[end] == 0)
                {
                    nums[Left] = 0;
                    end--;
                    start++; Left++;
                }
                else
                {
                    start++;
                    end--;
                }
            }
            for (int i = Left; i <= Right; i++)
                nums[i] = 1;
        }

        public void SortColors2(int[] nums)
        {
            int start = 0;
            int end = nums.Length - 1;
            for (int i = 0; i <= end && start < end; i++)
            {
                if (nums[i] == 0)
                {
                    Swap(nums, i, start);
                    start++;
                }
                else if (nums[i] == 2)
                {
                    Swap(nums, i, end);
                    end--;
                    i--;
                }
            }
        }

        private void Swap(int[] nums, int i, int index)
        {
            if (i == index)
                return;
            int tmp = nums[i];
            nums[i] = nums[index];
            nums[index] = tmp;
        }
    }
}
