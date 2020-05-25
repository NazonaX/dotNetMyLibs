using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MergeLinkedList
    {
        public ListNodeClass MergeTwoLists(ListNodeClass l1, ListNodeClass l2)
        {
            if (l1 == null && l2 != null)
                return l2;
            else if (l1 != null && l2 == null)
                return l1;
            else if (l1 == null && l2 == null)
                return null;
            ListNodeClass head = null;
            ListNodeClass p = null;
            head = l1.val <= l2.val ? l1 : l2;
            l1 = l1 == head ? l1.next : l1;
            l2 = l2 == head ? l2.next : l2;
            p = head;
            while(l1 != null && l2 != null)
            {
                if(l1.val <= l2.val)
                {
                    p.next = l1;
                    l1 = l1.next;
                }
                else
                {
                    p.next = l2;
                    l2 = l2.next;
                }
                p = p.next;
            }
            if(l2 == null)
            {
                p.next = l1;
            }
            else if(l1 == null)
            {
                p.next = l2;
            }
            return head;
        }
    }
}
