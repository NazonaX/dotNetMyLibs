using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RotateList
    {
        public ListNodeClass RotateRight(ListNodeClass head, int k)
        {
            if(head == null || head.next == null)
                return head;
            int counter = 1;
            ListNodeClass p = head;
            ListNodeClass tail = null;
            ListNodeClass tail_pre = null;
            p = head;
            while (p.next != null)
            {
                tail_pre = p;
                p = p.next;
                counter++;
            }
            counter = k % counter;
            for (int i = 0; i < counter; i++)
            {
                tail = p;
                tail.next = head;
                head = tail;
                tail_pre.next = null;
                if (i == counter - 1)
                    break;
                else
                {
                    p = head;
                    while (p.next != null)
                    {
                        tail_pre = p;
                        p = p.next;
                    }
                }
            }
            return head;
        }
        //Best solution is:
        //存储到一个等长的list，然后计算开头起始位置，利用的也是余数
        //从起始位置开始重新赋值，并loop到一遍起始位置
    }
}
