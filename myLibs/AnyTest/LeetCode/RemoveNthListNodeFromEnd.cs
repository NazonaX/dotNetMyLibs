using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RemoveNthListNodeFromEnd
    {
        public ListNodeClass RemoveNthFromEnd(ListNodeClass head, int n)
        {
            ListNodeClass p = head;
            ListNodeClass pTarget = null;
            ListNodeClass pPre = null;
            int counter = 0;
            while(p.next != null)
            {
                counter++;
                if(counter == n + 1)
                {
                    pPre = head;
                }
                if(counter == n)
                {
                    pTarget = head;
                }
                if(pPre != null)
                    pPre = pPre.next;
                if(pTarget != null)
                    pTarget = pTarget.next;
                p = p.next;
            }
            if (pPre != null && pTarget != null)
                pPre.next = pTarget.next;
            else if (pPre == null && pTarget != null)
                head.next = head.next.next;
            else if (pPre == null && pTarget == null && counter == 0)
                return null;
            else if (pPre == null && pTarget == null)
                head = head.next;
            return head;
        }
    }
}
