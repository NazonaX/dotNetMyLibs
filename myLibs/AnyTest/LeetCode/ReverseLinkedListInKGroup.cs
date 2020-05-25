using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ReverseLinkedListInKGroup
    {
        public ListNodeClass ReverseKGroup(ListNodeClass head, int k)
        {
            if (head == null)
                return null;
            head = Reverse(head, k);
            return head;
        }
        private ListNodeClass Reverse(ListNodeClass p , int k)
        {
            int counter = 0;
            ListNodeClass[] tmps = new ListNodeClass[k];
            ListNodeClass pHead = p;
            while (p != null)
            {
                tmps[counter] = p;
                counter++;
                p = p.next;
                if (counter == k)
                    break;
            }
            if(counter < k)
            {
                return pHead;
            }
            for(int i = k -1; i > 0; i--)
            {
                tmps[i].next = tmps[i - 1];
            }
            tmps[0].next = Reverse(p, k);
            return tmps[k - 1];
        }
    }
}
