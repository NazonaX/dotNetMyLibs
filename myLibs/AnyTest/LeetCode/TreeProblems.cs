using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class TreeProblems
    {
        public LeetCode.TreeNode root = new LeetCode.TreeNode(1);
        public TreeProblems()
        {
            LeetCode.TreeNode node = root;
            node.left = new LeetCode.TreeNode(2);
            node.right = new LeetCode.TreeNode(2);
            node = root.right;
            node.left = new LeetCode.TreeNode(4);
            node.right = new LeetCode.TreeNode(3);
            node = root.left;
            node.left = new LeetCode.TreeNode(4);
            node.right = new LeetCode.TreeNode(3);
            node = root.left.right;
            node.left = new LeetCode.TreeNode(8);
            node.right = new LeetCode.TreeNode(9);
            node = root.right.right;
            node.left = new LeetCode.TreeNode(8);
            node.right = new LeetCode.TreeNode(9);
        }


        /// <summary>
        /// 给定一棵树，返回它的中序遍历-inorder traversal,左中右
        /// 尝试使用迭代而非递归
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<int> InorderTraversal(TreeNode root)
        {
            IList<int> res = new List<int>();
            if (root == null)
                return res;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNode tn = root;
            while (tn != null || stack.Count != 0)
            {
                if (tn != null)
                {
                    stack.Push(tn);
                    tn = tn.left;
                }
                else if (tn == null)
                {
                    tn = stack.Pop();
                    res.Add(tn.val);
                    tn = tn.right;
                }
            }
            return res;
        }
        /// <summary>
        /// 非递归先序遍历：preorder traversal，中左右
        /// </summary>
        public IList<int> PreorderTraversal(TreeNode root)
        {
            IList<int> res = new List<int>();
            if (root == null)
                return res;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNode tn = root;
            while (tn != null || stack.Count != 0)
            {
                if (tn != null)
                {
                    res.Add(tn.val);
                    stack.Push(tn);
                    tn = tn.left;
                }
                else
                {
                    tn = stack.Pop();
                    tn = tn.right;
                }
            }
            return res;
        }
        /// <summary>
        /// 非递归后续遍历-postorder traversal，左右中
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<int> PostorderTraversal(TreeNode root)
        {
            IList<int> res = new List<int>();
            if (root == null)
                return res;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            Stack<TreeNode> stack2 = new Stack<TreeNode>();
            TreeNode tn = root;
            while (tn != null || stack.Count != 0)
            {
                if (tn != null)
                {
                    stack.Push(tn);
                    stack2.Push(tn);
                    tn = tn.right;
                }
                else
                {
                    tn = stack.Pop();
                    tn = tn.left;
                }
            }
            while (stack2.Count > 0)
            {
                res.Add(stack2.Pop().val);
            }
            return res;
        }

        /// <summary>
        /// 给定一个整数n，生成包含1~n的所有可能的二搜索叉树
        /// 二叉搜索树：左子节点小于等于当前节点；右子节点大于等于当前节点
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IList<TreeNode> GenerateTrees(int n)
        {
            if (n == 0)
                return new List<TreeNode>();
            return DoRecursionByGenerateTrees(1, n);
        }
        /// <summary>
        /// 使用递归来生成左树和右树，最后拼接并加入结果集合
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private IList<TreeNode> DoRecursionByGenerateTrees(int start, int end)
        {
            IList<TreeNode> res = new List<TreeNode>();
            if (start > end)
            {
                res.Add(null);
                return res;
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    //认为其中包含的重复计算式必要的，因为需要生成新的树节点
                    IList<TreeNode> left = DoRecursionByGenerateTrees(start, i - 1);
                    IList<TreeNode> right = DoRecursionByGenerateTrees(i + 1, end);
                    foreach (TreeNode tl in left)
                    {
                        foreach (TreeNode tr in right)
                        {
                            TreeNode nn = new TreeNode(i);
                            nn.left = tl;
                            nn.right = tr;
                            res.Add(nn);
                        }
                    }
                }
                return res;
            }
        }
        /// <summary>
        /// 给定一个整数，返回1~n所组成的所有可能的二叉搜索树的数量
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int NumTrees(int n)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            return DoRecursionByNumTrees(1, n, dict);
        }
        /// <summary>
        /// 思想跟上面一样，只不过是start>end表示一边子树不存在以及start == end表示一边子树只有一个元素，这两种情况都表示一边的子树只有一种情况
        /// 或者直接使用Catalan数列，满足表达式h(n) = h(1)*h(n-1)+h(2)*h(n-2)+....+h(n-1)*h(1)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int DoRecursionByNumTrees(int start, int end, Dictionary<int, int> dict)
        {
            int num = 0;
            if (start >= end)
            {
                return 1;
            }
            else if (dict.ContainsKey(end - start))
            {
                return dict[end - start];
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    //此处的重复计算可以存储
                    int leftNum = DoRecursionByNumTrees(start, i - 1, dict);
                    int rightNum = DoRecursionByNumTrees(i + 1, end, dict);
                    num += leftNum * rightNum;
                }
                dict.Add(end - start, num);
                return num;
            }
        }

        /// <summary>
        /// 给定一棵二叉树，验证该树是否是一课二叉搜索树
        /// 每个节点的值都大于其左子树的所有值，小于其右子树的所有值
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool IsValidBST(TreeNode root)
        {
            if (root == null)
                return true;
            TreeNode left = root.left;
            TreeNode right = root.right;
            HashSet<int> setForDup = new HashSet<int>();
            setForDup.Add(root.val);
            bool res = (left == null ? true : left.val < root.val ? true && DoRecursionByIsValidBST(left, int.MinValue, root.val, setForDup) : false)
                && (right == null ? true : right.val > root.val ? true && DoRecursionByIsValidBST(right, root.val, int.MaxValue, setForDup) : false);
            return res;
        }
        /// <summary>
        /// 应当设立每一边的最大值和最小值，来帮助判定是否存在隔代异常值
        /// 每个节点的左边小于最小值
        /// 每个节点的右边大于最大值
        /// </summary>
        /// <returns></returns>
        private bool DoRecursionByIsValidBST(TreeNode root, int min, int max, HashSet<int> setForDup)
        {
            if (root.val == int.MinValue && min == int.MinValue && setForDup.Contains(root.val))
            { return false; }
            else if (root.val == int.MaxValue && max == int.MaxValue && setForDup.Contains(root.val))
            { return false; }
            else if (setForDup.Contains(root.val) || root.val < min || root.val > max)
            { return false; }
            setForDup.Add(root.val);
            TreeNode left = root.left;
            TreeNode right = root.right;
            bool res = true;
            if (left == null)
                res = true;
            else
            {
                if (root.val < left.val || left.val < min)
                    return false;
                else
                    res = res && DoRecursionByIsValidBST(left, min, root.val, setForDup);
            }
            if (right == null)
            {
                res = res && true;
            }
            else
            {
                if (root.val > right.val || right.val > max)
                    return false;
                else
                    res = res && DoRecursionByIsValidBST(right, root.val, max, setForDup);
            }
            return res;
        }

        /// <summary>
        /// 给定一棵树，其中两个节点由于错误而置换了，改正这棵BST
        /// 可以利用此处的对象思想代替上题验证BST的int边界值检测
        /// </summary>
        /// <param name="root"></param>
        public void RecoverTree(TreeNode root)
        {
            if (root == null || (root.left == null && root.right == null))
                return;
            while (!DoRecursionByRecoverTree(root.left, null, root) || !DoRecursionByRecoverTree(root.right, root, null))
            { }
        }
        /// <summary>
        /// 设想使用无限递归，直到检测成为一棵BST
        /// 每次检测到错误，交换越界对象和被越界的对象的值
        /// </summary>
        /// <param name="root"></param>
        private bool DoRecursionByRecoverTree(TreeNode root, TreeNode min, TreeNode max)
        {
            if (root == null)
                return true;
            if (min != null && root.val < min.val)
            {
                int tmp = min.val;
                min.val = root.val;
                root.val = tmp;
                return false;
            }
            else if (max != null && root.val > max.val)
            {
                int tmp = max.val;
                max.val = root.val;
                root.val = tmp;
                return false;
            }
            else
            {
                if (DoRecursionByRecoverTree(root.left, min, root))
                    return DoRecursionByRecoverTree(root.right, root, max);
                else
                    return false;
            }
        }


        /// <summary>
        /// 给定两棵二叉树，判断两个树是相似
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool IsSameTree(TreeNode p, TreeNode q)
        {
            Queue<TreeNode> q1 = new Queue<TreeNode>();
            Queue<TreeNode> q2 = new Queue<TreeNode>();
            q1.Enqueue(p);
            q2.Enqueue(q);
            TreeNode t1 = null;
            TreeNode t2 = null;
            while (q1.Count > 0 && q2.Count > 0)
            {
                t1 = q1.Dequeue();
                t2 = q2.Dequeue();
                if (t1 == null && t2 == null)
                    continue;
                if (t1 == null && t2 != null || t1 != null && t2 == null)
                    return false;
                if (t1.val != t2.val)
                    return false;
                q1.Enqueue(t1.left);
                q1.Enqueue(t1.right);
                q2.Enqueue(t2.left);
                q2.Enqueue(t2.right);
            }
            if (q1.Count != q2.Count)
                return false;
            return true;
        }

        /// <summary>
        /// 给定一棵二叉树，判断该树是否是一棵自镜像的，也即左右对称的
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool IsSymmetric(TreeNode root)
        {
            Queue<TreeNode> plainTree = new Queue<TreeNode>();
            Stack<TreeNode> symmeticTree = new Stack<TreeNode>();
            if (root == null)
                return true;
            plainTree.Enqueue(root);
            while (plainTree.Count > 0)
            {
                TreeNode x = plainTree.Dequeue();
                if (x != null)
                {
                    plainTree.Enqueue(x.left);
                    plainTree.Enqueue(x.right);
                    if (PushToStackByIsSymmetic(symmeticTree, symmeticTree.Count, x.left, true))
                        PushToStackByIsSymmetic(symmeticTree, symmeticTree.Count, x.right, true);
                    else
                        PushToStackByIsSymmetic(symmeticTree, symmeticTree.Count, x.right, false);
                }
            }
            if (symmeticTree.Count > 3)
                return false;
            //剩余一个根或者三个元素，验证顶部3个
            if (symmeticTree.Count == 3)
            {
                TreeNode x = symmeticTree.Pop();
                TreeNode y = symmeticTree.Pop();
                if (x == null && y == null
                    || x != null && y != null && x.val == y.val)
                    return true;
                else
                    return false;
            }
            return true;
        }
        private bool PushToStackByIsSymmetic(Stack<TreeNode> symmeticTree, int count, TreeNode x, bool withDelete)
        {
            if (!withDelete)
            {
                symmeticTree.Push(x);
                return false;
            }
            if (count == 0
                || symmeticTree.Peek() == null && x != null
                || symmeticTree.Peek() != null && x == null
                || symmeticTree.Peek() != null && x != null && symmeticTree.Peek().val != x.val)
            {
                symmeticTree.Push(x);
                return false;
            }
            else
            {
                symmeticTree.Pop();
                return true;
            }
        }
        /// <summary>
        /// Best Solution of IsSymmetic
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool IsSymmetric2(TreeNode root)
        {
            if (root == null)
                return true;
            return DoRecursionByIsSymmetic(root.left, root.right);
        }
        private bool DoRecursionByIsSymmetic(TreeNode left, TreeNode right)
        {
            if (left == null && right == null)
                return true;
            else if (left == null || right == null)
                return false;
            else
            {
                return left.val == right.val
                    && DoRecursionByIsSymmetic(left.left, right.right)
                    && DoRecursionByIsSymmetic(left.right, right.left);
            }
        }

        /// <summary>
        /// 给定一棵二叉树，返回它的平整展开，每个层级定义一个list
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<IList<int>> LevelOrder(TreeNode root)
        {
            IList<IList<int>> res = new List<IList<int>>();
            if(root == null)
                return res;
            DoRecursionByLevelOrder(res, 1, root);
            return res;
        }
        private void DoRecursionByLevelOrder(IList<IList<int>> res, int level, TreeNode root)
        {
            if (root == null)
                return;
            if(res.Count < level)
            {
                res.Add(new List<int>());
            }
            res[level - 1].Add(root.val);
            DoRecursionByLevelOrder(res, level + 1, root.left);
            DoRecursionByLevelOrder(res, level + 1, root.right);
        }

        /// <summary>
        /// 给定一棵二叉树，返回它的平整展开，并且是Z字形（zigzag）的层级列表，每个层级对应一个list
        /// 可以使用双栈来进行双边颠倒
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<IList<int>> ZigzagLevelOrder(TreeNode root)
        {
            IList<IList<int>> res = new List<IList<int>>();
            if (root == null)
                return res;
            DoRecursionByZigzagLevelOrder(res, 1, root);
            return res;
        }
        private void DoRecursionByZigzagLevelOrder(IList<IList<int>> res, int level, TreeNode root)
        {
            //上一题思想，可以在奇偶处利用尾插和头插实现，而不需要考虑遍历顺序的问题了
            if (root == null)
                return;
            if(res.Count < level)
            {
                List<int> tmp = new List<int>();
                res.Add(tmp);
            }
            if(level % 2 == 1)
                res[level - 1].Add(root.val);
            else
            {
                res[level - 1].Insert(0, root.val);
            }
            DoRecursionByZigzagLevelOrder(res, level + 1, root.left);
            DoRecursionByZigzagLevelOrder(res, level + 1, root.right);
        }

        /// <summary>
        /// 给定一棵树，返回它的最大深度
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int MaxDepth(TreeNode root)
        {
            int depth = 0;
            if (root != null)
                depth = 1 + Math.Max(MaxDepth(root.left), MaxDepth(root.right));
            return depth;
        }

        /// <summary>
        /// 给定一棵树的先序遍历和中序遍历，返回它们所对应的树
        /// </summary>
        /// <param name="preorder"></param>
        /// <param name="inorder"></param>
        /// <returns></returns>
        public TreeNode BuildTree(int[] preorder, int[] inorder)
        {
            if (preorder == null || inorder == null
                || preorder.Length == 0 || inorder.Length == 0
                || preorder.Length != inorder.Length)
                return null;
            TreeNode res = DoRecursionByBuildTree(preorder, 0, preorder.Length - 1, inorder, 0, inorder.Length - 1);
            return res;
        }
        private TreeNode DoRecursionByBuildTree(int[] preorder, int preFrom, int preTo,
            int[] inorder, int inFrom, int inTo)
        {
            if (preFrom > preTo || inFrom > inTo)
                return null;
            TreeNode root = new TreeNode(preorder[preFrom]);
            if (inFrom == inTo)
                return root;
            int i = 0;
            for(i = inFrom; i <= inTo; i++)
            {
                if (preorder[preFrom] == inorder[i])
                    break;
            }
            int length = i - inFrom;
            if(i != 0)
            {
                root.left = DoRecursionByBuildTree(preorder, preFrom + 1, preFrom + length,
                    inorder, inFrom, i - 1);
            }
            if(i != inorder.Length - 1)
            {
                root.right = DoRecursionByBuildTree(preorder, preFrom + length + 1, preTo,
                    inorder, i + 1, inTo);
            }
            return root;
        }

        /// <summary>
        /// 给定一棵树的中序遍历和后续遍历，返回这棵树
        /// </summary>
        /// <param name="inorder"></param>
        /// <param name="postorder"></param>
        /// <returns></returns>
        public TreeNode BuildTree2(int[] inorder, int[] postorder)
        {
            if (postorder == null || inorder == null
                || postorder.Length == 0 || inorder.Length == 0
                || postorder.Length != inorder.Length)
                return null;
            TreeNode res = DoRecursionByBuildTree2(postorder, postorder.Length - 1, 0, inorder, inorder.Length - 1, 0);
            return res;
        }
        private TreeNode DoRecursionByBuildTree2(int[] postorder, int postFrom, int postTo, int[] inorder, int inFrom, int inTo)
        {
            //更改DoRecursionByBuildTree为逆序版本，因此此处的from > to
            if (postFrom < postTo || inFrom < inTo)
                return null;
            TreeNode root = new TreeNode(postorder[postFrom]);
            if (inFrom == inTo)
                return root;
            int i = 0;
            for (i = inFrom; i >= inTo; i--)
            {
                if (postorder[postFrom] == inorder[i])
                    break;
            }
            int length = inFrom - i;
            if(i != inorder.Length - 1)
            {
                root.right = DoRecursionByBuildTree2(postorder, postFrom - 1, postFrom - length,
                    inorder, inFrom, i + 1);
            }
            if(i != 0)
            {
                root.left = DoRecursionByBuildTree2(postorder, postFrom - length - 1, postTo,
                    inorder, i - 1, inTo);
            }
            return root;
        }

        /// <summary>
        /// 给定一棵树，返回它的从下到上，从左到右的层级列表
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<IList<int>> LevelOrderBottom(TreeNode root)
        {
            IList<IList<int>> res = new List<IList<int>>();
            IList<IList<int>> tmp = new List<IList<int>>();
            DoRecursionByLevelOrderBottom(root, 1, tmp);
            for(int i = tmp.Count - 1; i >= 0; i--)
            {
                res.Add(tmp[i]);
            }
            return res;
        }
        private void DoRecursionByLevelOrderBottom(TreeNode root, int level, IList<IList<int>> tmp)
        {
            if (root == null)
                return;
            if(tmp.Count <= level - 1)
            {
                tmp.Add(new List<int>());
            }
            tmp[level - 1].Add(root.val);
            DoRecursionByLevelOrderBottom(root.left, level + 1, tmp);
            DoRecursionByLevelOrderBottom(root.right, level + 1, tmp);
        }

        /// <summary>
        /// 给定一个升序数组，返回一棵深度平衡的BST
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public TreeNode SortedArrayToBST(int[] nums)
        {
            TreeNode res = null;
            res = DoRecursionBySortedArrayToBST(nums, 0, nums.Length - 1);
            return res;
        }
        private TreeNode DoRecursionBySortedArrayToBST(int[] nums, int start, int end)
        {
            //每次选取中间，并两遍作递归
            if (start > end)
                return null;
            else if (start == end)
                return new TreeNode(nums[start]);
            else
            {
                int mid = (start + end) / 2;
                TreeNode root = new TreeNode(nums[mid]);
                root.left = DoRecursionBySortedArrayToBST(nums, start, mid - 1);
                root.right = DoRecursionBySortedArrayToBST(nums, mid + 1, end);
                return root;
            }
        }

        /// <summary>
        /// 给定一个升序链表，返回它所对应的深度平衡BST
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public TreeNode SortedListToBST(ListNodeClass head)
        {
            TreeNode res = null;
            //快慢指针：一个指针跳一格，另一个指针跳两格，选取中间项，时间复杂度为n^2，空间复杂度为1
            //考虑遍历两遍，形成一个数组方便随机访问，时间复杂度为n，空间复杂度为n
            if (head == null)
                return null;
            int counter = 0;
            ListNodeClass p = head;
            while(p != null)
            {
                counter++;
                p = p.next;
            }
            int[] nums = new int[counter];
            counter = 0;
            p = head;
            while (p != null)
            {
                nums[counter++] = p.val;
                p = p.next;
            }
            res = DoRecursionBySortedArrayToBST(nums, 0, counter - 1);
            return res;
        }

        /// <summary>
        /// 给定一棵二叉树，判断它是否是深度平衡的。
        /// 深度平衡定义为：任何节点的左右子树的深度差不能超过1
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool IsBalanced(TreeNode root)
        {
            if (root == null)
                return true;//认为空树也是深度平衡的
            int leftHeight = DoRecursionByIsBalanced(root.left);
            int rightHeight = DoRecursionByIsBalanced(root.right);
            if (leftHeight == -1 || rightHeight == -1)
                return false;
            if (Math.Abs(leftHeight - rightHeight) <= 1)
                return true;
            else
                return false;
        }
        private int DoRecursionByIsBalanced(TreeNode root)
        {
            if (root == null)
                return 0;
            else
            {
                int left = DoRecursionByIsBalanced(root.left);
                int right = DoRecursionByIsBalanced(root.right);
                if (left == -1 || right == -1)
                    return -1;//stands for some nodes dont meet the requirement
                if (Math.Abs(left - right) > 1)
                    return -1;
                else
                    return Math.Max(left, right) + 1;
            }
        }

        /// <summary>
        /// 给定一棵树，返回它的最小深度
        /// 最小深度定义为：从根节点到最近的叶子节点的距离
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int MinDepth(TreeNode root)
        {
            if (root == null)
                return 0;
            else
            {
                int min = DoRecursionByMinDepth(root, 1);
                return min;
            }
        }
        private int DoRecursionByMinDepth(TreeNode root, int level)
        {
            if (root == null)
                return -1;//stands for not under take
            else if (root.left == null && root.right == null)
                return level;
            else
            {
                int left = DoRecursionByMinDepth(root.left, level + 1);
                int right = DoRecursionByMinDepth(root.right, level + 1);
                if (left == -1)
                    return right;
                else if (right == -1)
                    return left;
                else
                    return Math.Min(left, right);
            }
        }
        public int MinDepthBestSolution(TreeNode root)
        {
            if (root == null)
            {
                return 0;
            }
            if (root.left == null)
            {
                return MinDepthBestSolution(root.right) + 1;
            }
            if (root.right == null)
            {
                return MinDepthBestSolution(root.left) + 1;
            }
            return Math.Min(MinDepthBestSolution(root.left), MinDepthBestSolution(root.right)) + 1;
        }

        /// <summary>
        /// 给定一棵树和一个整型值，返回是否：概述包含一条路径从跟到叶子节点，使其所有路径上的值的和等于给出的整型值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public bool HasPathSum(TreeNode root, int sum)
        {
            if (root == null)
            {
                return false;
            }
            if(root.left == null && root.right == null)
            {
                return sum == root.val;
            }
            return HasPathSum(root.left, sum - root.val) || HasPathSum(root.right, sum - root.val);
        }

        /// <summary>
        /// 给定一棵树和一个数值sum，返回所有该树从root到leaf的、满足和为sum的路径列表
        /// </summary>
        /// <param name="root"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public IList<IList<int>> PathSum(TreeNode root, int sum)
        {
            IList<IList<int>> res = new List<IList<int>>();
            if (root == null)
                return res;
            int localsum = 0;
            List<int> cacheList = new List<int>();
            DoRecursionByPathSum(root, sum, localsum, cacheList, res);
            return res;
        }
        private void DoRecursionByPathSum(TreeNode root, int sum, int localsum, List<int> cacheList, IList<IList<int>> res)
        {
            localsum += root.val;
            cacheList.Add(root.val);
            if(root.left == null && root.right == null)
            {
                if(localsum == sum)
                {
                    List<int> addition = new List<int>(cacheList);
                    res.Add(addition);
                }
            }
            else
            {
                if (root.left != null)
                    DoRecursionByPathSum(root.left, sum, localsum, cacheList, res);
                if (root.right != null)
                    DoRecursionByPathSum(root.right, sum, localsum, cacheList, res);
            }
            cacheList.RemoveAt(cacheList.Count - 1);
        }

        /// <summary>
        /// 给定一棵树，原地展开它，使用右子树作为链表；先序遍历展开
        /// </summary>
        /// <param name="root"></param>
        public void Flatten(TreeNode root)
        {
            while(root != null)
            {
                if(root.left != null)
                {
                    TreeNode leftTmp = root.left;
                    TreeNode leftRight = leftTmp;
                    while(leftRight.right != null)
                    {
                        leftRight = leftRight.right;
                    }
                    root.left = null;
                    leftRight.right = root.right;
                    root.right = leftTmp;
                }
                root = root.right;
            }
        }

        /// <summary>
        /// 给定一棵perfect tree，要求自左向右连接它的同层
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TreeNode2 Connect(TreeNode2 root)
        {
            TreeNode2 doRoot = root;
            Queue<TreeNode2> doQueue = new Queue<TreeNode2>();
            if(root != null)
            {
                doQueue.Enqueue(root);
                int loopNum = 1;
                while(doQueue.Count > 0)
                {
                    TreeNode2 tmp1 = doQueue.Dequeue();
                    for (int i = 1; i < loopNum; i++)
                    {
                        TreeNode2 tmp2 = doQueue.Dequeue();
                        tmp1.next = tmp2;
                        if (tmp1.left != null)//due to the defination
                        {
                            doQueue.Enqueue(tmp1.left);
                            doQueue.Enqueue(tmp1.right);
                        }
                        tmp1 = tmp2;
                    }
                    if (tmp1.left != null)
                    {
                        doQueue.Enqueue(tmp1.left);
                        doQueue.Enqueue(tmp1.right);
                    }
                    loopNum = loopNum << 1;
                }
            }
            return root;
        }
        /// <summary>
        /// 给定一棵二叉树，返回它的自左向右同级连接
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TreeNode2 Connect2(TreeNode2 root)
        {
            Dictionary<int, TreeNode2> layersDict = new Dictionary<int, TreeNode2>();
            DoRecursionByConnect2(root, 1, layersDict);
            return root;
        }
        private void DoRecursionByConnect2(TreeNode2 root, int depth, Dictionary<int, TreeNode2> layersDict)
        {
            if (root == null)
                return;
            if (!layersDict.ContainsKey(depth))
            {
                layersDict.Add(depth, root);
            }
            else
            {
                layersDict[depth].next = root;
                layersDict[depth] = root;
            }
            DoRecursionByConnect2(root.left, depth + 1, layersDict);
            DoRecursionByConnect2(root.right, depth + 1, layersDict);
        }

        /// <summary>
        /// 给定一棵树，求从某一节点到另一个节点经过的和的最大值
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int MaxPathSum(TreeNode root)
        {
            //预计两次遍历，一次用于计算每个节点的返回式值
            //第二次遍历寻找最大值
            if (root == null)
                return 0;
            DoRecursionByMaxPathSumCalulate(root);
            return DoRecursionByMaxPathSumFind(root, root.val);
        }
        private int DoRecursionByMaxPathSumFind(TreeNode root, int max)
        {
            if (root == null)
                return max;
            max = Math.Max(root.val, max);
            int leftMax = DoRecursionByMaxPathSumFind(root.left, max);
            int rightMax = DoRecursionByMaxPathSumFind(root.right, max);
            return Math.Max(root.val, Math.Max(leftMax, rightMax));
        }
        private int DoRecursionByMaxPathSumCalulate(TreeNode root)
        {
            if (root == null)
                return 0;
            else
            {
                int left = DoRecursionByMaxPathSumCalulate(root.left);
                int right = DoRecursionByMaxPathSumCalulate(root.right);
                int max = Math.Max(0, Math.Max(left, right)) + root.val;
                root.val += ((left > 0 ? left : 0) + (right > 0 ? right : 0));
                //此处回溯的值和实际设置的值不一定一致
                //回溯到父节点的值是只选一项的，
                //自己设置的值是可以蔓延到左右子树的
                //后续扫描寻找最大值时可以一边过
                //若左右子树都为0，则不选择，返回自己，赋值自己
                //精髓在于，自己的赋值和回溯返回值是不一样的，这是最骚的
                return max;
            }
        }

        /// <summary>
        /// 给定一棵树，求出所有路径对应数字的总和
        /// 定义：一个路径对应的数字为该路径上从跟到叶子的十进制数，也即如果路径为1-2-3，则数为123
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int SumNumbers(TreeNode root)
        {
            List<int> path = new List<int>();
            if (root == null)
                return 0;
            int res = DoRecursionBySumNumbers(root, path);
            return res;
        }
        private int DoRecursionBySumNumbers(TreeNode root, List<int> path)
        {
            path.Add(root.val);
            int sum = 0;
            if (root.left != null && root.right != null)
                sum = DoRecursionBySumNumbers(root.left, path) + DoRecursionBySumNumbers(root.right, path);
            else if (root.left != null)
                sum = DoRecursionBySumNumbers(root.left, path);
            else if (root.right != null)
                sum = DoRecursionBySumNumbers(root.right, path);
            else
                sum = ListToNumber(path);
            path.RemoveAt(path.Count - 1);
            return sum;
        }
        private int ListToNumber(List<int> path)
        {
            int sum = 0;
            for(int i =0; i <path.Count; i++)
            {
                sum = sum * 10 + path[i];
            }
            return sum;
        }
    }
}
