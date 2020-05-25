using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class DeepCopyListWithRandomPointer
    {
        public class Node
        {
            public int val;
            public Node next;
            public Node random;

            public Node() { }
            public Node(int _val, Node _next, Node _random)
            {
                val = _val;
                next = _next;
                random = _random;
            }
        }
        public Node TestHead = new Node();
        private Node p2 = new Node();
        private Node p3 = new Node();
        private Node p4 = new Node();
        private Node p5 = new Node();

        public DeepCopyListWithRandomPointer()
        {
            TestHead.val = 1;
            TestHead.next = p2;
            TestHead.random = p3;
            p2.val = 2;
            p2.next = p3;
            p2.random = null;
            p3.val = 3;
            p3.next = p4;
            p3.random = p4;
            p4.val = 4;
            p4.next = p5;
            p4.random = p2;
            p5.val = 5;
        }

        /// <summary>
        /// 给定一个拥有随机指针的列表，返回它的深拷贝
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public Node CopyRandomList(Node head)
        {
            Node phead = new Node();
            Node p = phead;
            Node _p = head;
            while(_p != null)
            {
                p.val = _p.val;
                if (_p.next != null)
                    p.next = new Node();
                p = p.next;
                _p = _p.next;
            }
            _p = head;
            p = phead;
            while(_p != null)
            {
                Node t = _p.random;
                if( t!= null)
                {
                    //从头开始找到目标
                    Node find = phead;
                    Node _find = head;
                    while(_find != null)
                    {
                        if(_find == t)
                        {
                            p.random = find;
                        }
                        find = find.next;
                        _find = _find.next;
                    }
                }
                _p = _p.next;
                p = p.next;
            }
            return phead;
        }


    }
}
