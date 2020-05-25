using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class LargeNumberAdd
    {
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int x) { val = x; }
        }

        //forgot what to do
        public ListNode Solve1(ListNode l1, ListNode l2)
        {
            ListNode res = new ListNode((l1.val + l2.val) % 10);
            ListNode now = res;
            int lost = (l1.val + l2.val) / 10;
            l1 = l1.next;
            l2 = l2.next;
            while (l1 != null && l2 != null)
            {
                now.next = new ListNode((l1.val + l2.val + lost) % 10);
                lost = (l1.val + l2.val + lost) / 10;
                now = now.next;
                l1 = l1.next;
                l2 = l2.next;
            }
            if(l1 != null)
            {
                while (l1 != null)
                {
                    now.next = new ListNode((l1.val + lost) % 10);
                    lost = (l1.val + lost) / 10;
                    now = now.next;
                    l1 = l1.next;
                }
            }
            else if(l2 != null)
            {
                while (l2 != null)
                {
                    now.next = new ListNode((l2.val + lost) % 10);
                    lost = (l2.val + lost) / 10;
                    now = now.next;
                    l2 = l2.next;
                }
            }
            if (lost > 0)
                now.next = new ListNode(lost);
            return res;
        }

        //large number add one
        public int[] PlusOne(int[] digits)
        {
            int length = digits.Length;
            int[] res = null;
            int residual = 1;
            for (int i = length - 1; i >=0; i--)
            {
                digits[i] = (digits[i] + residual);
                residual = digits[i] / 10;
                digits[i] = digits[i] % 10;
                if (residual == 0)
                    break;
            }
            if (residual > 0)
            {
                res = new int[length + 1];
                res[0] = residual;
                for (int i = 1; i < length + 1; i++)
                    res[i] = digits[i - 1];
            }
            else
                res = digits;
            return res;
        }

        //two binary string addition
        public string AddBinary(string a, string b)
        {
            StringBuilder sb = new StringBuilder();
            int length1 = a.Length;
            int length2 = b.Length;
            int i, j;
            int add1 = 0, add2 = 0, residual = 0;
            int tmp = 0;
            for(i = length1 - 1, j = length2 - 1; i >=0 && j >=0; i--, j--)
            {
                add1 = a[i] - '0';
                add2 = b[j] - '0';
                tmp = add1 + add2 + residual;
                sb.Insert(0, tmp % 2);
                residual = tmp / 2;
            }
            if(i >= 0)
                for(j = i; j >=0; j--)
                {
                    add1 = a[j] - '0';
                    tmp = add1 + residual;
                    sb.Insert(0, tmp % 2);
                    residual = tmp / 2;
                }
            else if(j >=0)
                for(i = j; i >= 0; i--)
                {
                    add2 = b[i] - '0';
                    tmp = add2 + residual;
                    sb.Insert(0, tmp % 2);
                    residual = tmp / 2;
                }
            if (residual > 0)
                sb.Insert(0, residual);
            return sb.ToString();
        }
    }
}
