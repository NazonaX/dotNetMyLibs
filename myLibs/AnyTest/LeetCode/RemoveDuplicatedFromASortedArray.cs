using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RemoveDuplicatedFromASortedArray
    {
        public int RemoveDuplicates(int[] nums)
        {
            int index1 = 0;
            int index2 = 1;
            if (nums == null || nums.Length < 2)
                return nums.Length;
            while (index2 != nums.Length)
            {
                if (nums[index1] == nums[index2])
                {
                    index2++;
                }
                else
                {
                    nums[++index1] = nums[index2];
                    index2++;
                }
            }
            return index1 + 1;
        }

        /// <summary>
        /// 删除一个已排序的数组，使得重复元素最多出现2次，inplace，额外内存消耗为O(1)
        /// 返回删除后数组长度
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int RemoveDuplicates2(int[] nums)
        {
            int length = nums.Length;
            if (length <= 2)
                return length;
            int index1 = 0;
            int index2 = index1 + 1;
            int left = index2;
            int assignLeft = 0;
            while (index2 < length)
            {
                if (nums[index1] != nums[index2])
                {
                    nums[assignLeft++] = nums[index1];
                    index1++;
                    index2 = index1 + 1;
                }
                else
                {
                    left = index2;
                    while (index2 + 1 < length && nums[index2] == nums[index2 + 1])
                    {
                        index2++;
                    }
                    for (; index1 <= left; index1++)
                    {
                        nums[assignLeft++] = nums[index1];
                    }
                    index1 = index2 + 1;
                    index2 = index1 + 1;
                }
            }
            if (index1 < length)
                nums[assignLeft++] = nums[index1];
            return assignLeft;
        }
        /// <summary>
        /// 最优美写法，思想是默认就是目标解，然后选取长度之后的一个值，向前进行比较
        /// 想不到
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int RemoveDuplicates2BestSoluion(int[] nums)
        {
            if (nums.Length <= 2)
                return nums.Length;
            int newLength = 2;
            for(int i = 2; i < nums.Length; i++)
            {
                if(nums[i] != nums[newLength - 1] || nums[i] != nums[newLength - 2])
                {
                    nums[newLength++] = nums[i];
                }
            }
            return newLength;
        }

        /// <summary>
        /// 给定一个已排序的链表，删除所有含有重复项的元素
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public ListNodeClass DeleteDuplicatesByLinkedList(ListNodeClass head)
        {
            if (head == null || head.next == null)
                return head;
            ListNodeClass p = null;
            ListNodeClass headP = new ListNodeClass(0);
            ListNodeClass head2 = headP;
            ListNodeClass p2 = null;

            p = head;
            while (true)
            {
                if (p == null)
                {
                    headP.next = null;
                    break;
                }
                p2 = p.next;
                if (p2 == null)
                {
                    headP.next = p;
                    headP = headP.next;
                    headP.next = null;
                    break;
                }
                if (p.val == p2.val)
                {
                    while (p2.next != null && p2.next.val == p.val)
                        p2 = p2.next;
                    p = p2.next;
                }
                else
                {
                    headP.next = p;
                    headP = headP.next;
                    p = p2;
                }
            }
            return head2.next;
        }
        // 上述和下述链表算法都是基于生成一个默认头的做法，然后返回默认头之后的链
        // 可以节省寻找头的麻烦
        /// <summary>
        /// 给定一个排序链表，删除所有重复元素，仅留一个元素
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public ListNodeClass DeleteDuplicatesByLinkedList2(ListNodeClass head)
        {
            if (head == null || head.next == null)
                return head;
            ListNodeClass head2 = new ListNodeClass(0);
            ListNodeClass headP = head2;
            ListNodeClass p = head;
            for(; ; )
            {
                while (p.next != null && p.val == p.next.val)
                    p = p.next;
                headP.next = p;
                headP = headP.next;
                p = p.next;
                if (p == null)
                    break;
            }
            headP.next = null;
            return head2.next;
        }

    }
}
