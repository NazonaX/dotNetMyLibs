using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LinkedListProblems
    {
        /// <summary>
        /// 给定一个链表，返回一个list，使得所有小于x的节点，在大于等于x的节点的左边
        /// </summary>
        /// <param name="head"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public ListNodeClass Partition(ListNodeClass head, int x)
        {
            ListNodeClass headL = new ListNodeClass(0);
            ListNodeClass headGE = new ListNodeClass(0);
            ListNodeClass pL = headL;
            ListNodeClass pGE = headGE;
            if (head == null || head.next == null)
                return head;
            while(head != null)
            {
                if(head.val < x)
                {
                    pL.next = head;
                    pL = pL.next;
                }
                else
                {
                    pGE.next = head;
                    pGE = pGE.next;
                }
                head = head.next;
            }
            pL.next = headGE.next;
            pGE.next = null;
            return headL.next;
        }

        /// <summary>
        /// 给定一个链表和两个数字，翻转介于这两个数字之间的数值
        /// 申明两个头结点用于存储传入的head和逆向拼接的head，为了应对头转换
        /// 存储p1位起始位置前一个元素，p2为逆向拼接的尾元素，最后拼接p1、逆向链表和结束位置的迭代p即可
        /// </summary>
        /// <param name="head"></param>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public ListNodeClass ReverseBetween(ListNodeClass head, int m, int n)
        {
            if (head == null)
                return head;
            int counter = 0;
            ListNodeClass headtmp = new ListNodeClass(0);
            headtmp.next = head;
            head = headtmp;
            m++;
            n++;
            ListNodeClass p = head;
            ListNodeClass head2 = new ListNodeClass(0);
            ListNodeClass p1 = null;
            ListNodeClass p2 = null;
            ListNodeClass tmp = null;
            while(counter < n && p != null){
                counter++;
                if(counter == m - 1)
                {
                    p1 = p;
                }
                if(counter == m)
                {
                    p2 = p;
                }
                if(counter >= m)
                {
                    tmp = head2.next;
                    head2.next = p;
                    p = p.next;
                    head2.next.next = tmp;
                    continue;
                }
                p = p.next;
            }
            p1.next = head2.next;
            p2.next = p;
            return head.next;
        }

    }
}
