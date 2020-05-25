using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class TreeNode2
    {
        public int val;
        public TreeNode2 left;
        public TreeNode2 right;
        public TreeNode2 next;

        public TreeNode2() { }
        public TreeNode2(int _val, TreeNode2 _left, TreeNode2 _right, TreeNode2 _next)
        {
            val = _val;
            left = _left;
            right = _right;
            next = _next;
        }
    }
}
