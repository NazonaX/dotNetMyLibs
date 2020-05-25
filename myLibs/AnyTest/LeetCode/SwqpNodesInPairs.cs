using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SwqpNodesInPairs
    {
        public ListNodeClass SwapPairs(ListNodeClass head)
        {
            ListNodeClass resHead = null;
            ListNodeClass p1 = null;
            ListNodeClass p2 = null;
            ListNodeClass p = null;
            //for the first
            p1 = head;
            if (p1 == null)
                return null;
            p2 = p1.next;
            if (p2 == null)
                return head;
            resHead = p2;
            p1.next = p2.next;
            p2.next = p1;
            p = p1;
            //for loop
            while (true)
            {
                if (p1.next == null)
                    break;
                p1 = p1.next;
                if (p1.next == null)
                    break;
                p2 = p1.next;

                p1.next = p2.next;
                p2.next = p1;
                p.next = p2;
                p = p1;
            }
            return resHead;
        }

        //almost the same speed as mine
        public ListNodeClass BestSolution(ListNodeClass head)
        {
            if (head != null && head.next != null)
            {
                ListNodeClass nextHead = head.next.next;
                var temp = head;
                head = head.next;
                head.next = temp;

                if (nextHead != null)
                {
                    temp.next = SwapPairs(nextHead);
                }
                else
                {
                    temp.next = null;
                }
            }
            return head;
        }
    }
}
