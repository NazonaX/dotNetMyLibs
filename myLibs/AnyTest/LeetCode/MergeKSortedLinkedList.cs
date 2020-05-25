using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MergeKSortedLinkedList
    {
        public ListNodeClass MergeKLists(ListNodeClass[] lists)
        {
            ListNodeClass head = new ListNodeClass(0);
            ListNodeClass p = head;
            ListNodeClass pMin = null;
            bool end = false;
            int length = lists.Length;
            int index = 0;
            while (!end)
            {
                pMin = new ListNodeClass(int.MaxValue);
                end = true;
                for(int i = 0; i < length; i++)
                {
                    if(lists[i] != null && pMin.val >= lists[i].val)
                    {
                        pMin = lists[i];
                        index = i;
                        end = false;
                    }
                }
                if (!end)
                {
                    p.next = pMin;
                    p = p.next;
                    lists[index] = lists[index].next;
                }
            }
            return head.next;
        }

        //really best algorithm
        //the idea is to merge two each time, Merge sort by deviding to two
        //4 times faster than mine
        public ListNodeClass SolutionBest(ListNodeClass[] lists)
        {
            if (lists == null || lists.Length == 0)
                return null;
            return MergeLists2(lists, 0, lists.Length - 1);
        }

        private ListNodeClass MergeLists2(ListNodeClass[] lists, int start, int end)
        {
            if (start == end)
            {
                return lists[start];
            }
            else
            {
                var middle = (start + end) / 2;
                var left = MergeLists2(lists, start, middle);
                var right = MergeLists2(lists, middle + 1, end);
                return MergeTwoLists2(left, right);
            }
        }

        private ListNodeClass MergeTwoLists2(ListNodeClass l1, ListNodeClass l2)
        {
            if (l1 == null && l2 == null)
                return null;
            if (l1 == null && l2 != null)
                return l2;
            if (l1 != null && l2 == null)
                return l1;
            var head = new ListNodeClass(0);
            var temp = head;
            var p1 = l1;
            var p2 = l2;
            while (true)
            {
                if (p1 == null)
                {
                    temp.next = p2;
                    break;
                }
                if (p2 == null)
                {
                    temp.next = p1;
                    break;
                }

                if (p1.val < p2.val)
                {
                    temp.next = p1;
                    temp = temp.next;
                    p1 = p1.next;
                }
                else
                {
                    temp.next = p2;
                    temp = temp.next;
                    p2 = p2.next;
                }
            }
            return head.next;
        }
    }
}
