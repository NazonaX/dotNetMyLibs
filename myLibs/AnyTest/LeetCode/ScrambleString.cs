using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ScrambleString
    {
        /// <summary>
        /// 给定两个字符串，判断其中一个字符串是否是另一个字符串的扰乱串
        /// 扰乱串：将字符串任意切分成一个二叉树，如果两个字符串的二叉树的最小区别仅仅只几个：
        /// 交换某个节点的两个子节点
        /// 的区别，那么这两个字符串就互为扰乱串
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public bool IsScramble(string s1, string s2)
        {
            //由于是任意切分的二叉树，因此需要遍历每一种情况
            //如果互为扰乱串，那么肯定存在某种切分使得切分后的两遍成为“组”
            //“组”：两个互为扰乱串的切分后的组中的元素一定是相同的
            //“组”对应递归的某种切分
            //因此粗略分析，使用递归--容易造成栈溢出
            if(s1.Length != s2.Length)
            {
                return false;
            }
            if (s1.Equals(s2))
                return true;
            //以下判断字符集合是否一致必须要加，否则会造：
            //非常多余的递归，导致成栈溢出；"great","rgeat"
            char[] chartmp = s1.ToCharArray();
            Array.Sort(chartmp);
            string s1tmp = new string(chartmp);
            chartmp = s2.ToCharArray();
            Array.Sort(chartmp);
            string s2tmp = new string(chartmp);
            if (!s1tmp.Equals(s2tmp))
                return false;
            for(int i = 1; i < s1.Length; i++)
            {
                string s1Left = s1.Substring(0, i);
                string s1Right = s1.Substring(i);
                string s2Left = s2.Substring(0, i);
                string s2Right = s2.Substring(i);
                bool res = IsScramble(s1Left, s2Left) && IsScramble(s1Right, s2Right);
                if (res)
                    return true;
                s1Left = s1.Substring(s1.Length - i);
                s1Right = s1.Substring(0, s1.Length - i);
                res = IsScramble(s1Left, s2Left) && IsScramble(s1Right, s2Right);
                if (res)
                    return true;
            }
            return false;
        }

    }
}
