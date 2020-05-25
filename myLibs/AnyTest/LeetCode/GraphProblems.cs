using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class GraphProblems
    {
        /// <summary>
        /// 给定一个图的节点，返回它的深拷贝
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node CloneGraph(Node node)
        {
            //需要考虑图的回环问题
            Dictionary<Node, Node> conn = new Dictionary<Node, Node>();
            Node deepCopy = new Node();
            DoRecursionByCloneGraph(deepCopy, node, conn);
            return deepCopy;
        }
        private void DoRecursionByCloneGraph(Node deepCopy, Node node, Dictionary<Node, Node> conn)
        {
            deepCopy.val = node.val;
            deepCopy.neighbors = new List<Node>();
            conn.Add(node, deepCopy);
            foreach(Node n in node.neighbors)
            {
                if (conn.ContainsKey(n))
                {
                    deepCopy.neighbors.Add(conn[n]);
                }
                else
                {
                    Node t = new Node();
                    DoRecursionByCloneGraph(t, n, conn);
                    deepCopy.neighbors.Add(t);
                }
            }
        }
    }
}
