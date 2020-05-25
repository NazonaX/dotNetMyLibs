using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MergeSortedArray
    {
        /// <summary>
        /// 给定两个有序数组nums1和nums2，nums1长度为m，nums2长度为n
        /// 保证nums1的长度>=m+n，也即保证nums1的长度能够容纳nums2作为额外数据
        /// 将nums2整合到nums1中，保证有序
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="m"></param>
        /// <param name="nums2"></param>
        /// <param name="n"></param>
        public void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int index1 = 0;
            int index2 = 0;
            if (n == 0)
                return;
            for(; index1 < m; )
            {
                if(nums1[index1] <= nums2[0])
                {
                    index1++;
                }
                else
                {
                    int tmp = nums1[index1];
                    nums1[index1] = nums2[0];
                    nums2[0] = tmp;
                    index2 = 0;
                    while(index2 + 1 < n && nums2[index2] > nums2[index2 + 1])
                    {
                        tmp = nums2[index2];
                        nums2[index2] = nums2[index2 + 1];
                        nums2[index2 + 1] = tmp;
                        index2++;
                    }
                }
            }
            for(index1 = m, index2 = 0; index1 < m + n && index2 < n; index1++, index2++)
            {
                nums1[index1] = nums2[index2];
            }
        }
    }
}
