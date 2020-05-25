using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class StringProblems
    {
        /// <summary>
        /// 给定一个只包含数字的字符串，返回所有可能的组成ip地址的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IList<string> RestoreIpAddresses(string s)
        {
            IList<string> res = new List<string>();
            string[] tmps = new string[4];
            StringBuilder sb = new StringBuilder();
            DoRecursionByRestoreIPAddress(res, s, 0, 0, tmps, sb);
            return res;
        }
        private void DoRecursionByRestoreIPAddress(IList<string> res, string s, int nowIndex, int dotIndex,
            string[] tmps , StringBuilder sb)
        {
            if(dotIndex == 4)
            {
                if (nowIndex != s.Length)
                    return;
                sb.Clear();
                for(int i = 0; i < 4; i++)
                {
                    sb.Append(tmps[i]).Append('.');
                }
                sb.Remove(sb.Length - 1, 1);
                res.Add(sb.ToString());
            }
            else
            {
                for(int i = 0; nowIndex + i < s.Length && i < 3; i++)
                {
                    string str = s.Substring(nowIndex, i + 1);
                    //判断0的情况，包括前导0、多重0等情况
                    if (str.Length > 1 && str[0] == '0')
                        continue;
                    int x = 0;
                    if(int.TryParse(str, out x) && x <= 255)
                    {
                        tmps[dotIndex] = str;
                        DoRecursionByRestoreIPAddress(res, s, nowIndex + i + 1, dotIndex + 1, tmps, sb);
                    }
                }
            }
        }

        /// <summary>
        /// 给定三个字符串，判断s3是否是s1和s2交错形成的字符串
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <returns></returns>
        public bool IsInterleave(string s1, string s2, string s3)
        {
            int length1 = s1.Length;
            int length2 = s2.Length;
            int length3 = s3.Length;
            if (length3 != (length1 + length2))
                return false;
            else if (length1 == 0)
                return s2 == s3;
            else if (length2 == 0)
                return s1 == s3;
            bool[,] res = new bool[length1 + 1, length2 + 1];
            for(int i = 1; i <= length1; i++)
            {
                res[i, 0] = s1[i - 1] == s3[i - 1];
                if (!res[i, 0])
                    break;
            }
            for(int i = 1; i <= length2; i++)
            {
                res[0, i] = s2[i - 1] == s3[i - 1];
                if (!res[0, i])
                    break;
            }
            for(int i = 1; i <= length1; i++)
            {
                for(int j = 1; j <= length2; j++)
                {
                    int s3Index = i + j;
                    res[i, j] = (s1[i - 1] == s3[s3Index - 1] && res[i - 1, j]) 
                        || (s2[j - 1] == s3[s3Index - 1] && res[i, j - 1]);
                }
            }
            return res[length1, length2];
        }

        /// <summary>
        /// 给定两个字符串，返回s中可能组成t的不同子序列的数量
        /// 例如：s=rabbbit，t=rabbit，则返回3。因为s中去掉三个b中的任何一个b都能构成t
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int NumDistinct(string s, string t)
        {
            int length1 = s.Length;
            int length2 = t.Length;
            int[,] matrix = new int[length2 + 1, length1 + 1];
            if (length1 == 0 || length2 == 0)
                return 0;
            for(int i = 0; i < length1; i++)
            {
                if (t[0] == s[i])
                    matrix[0, i] = 1;
            }
            for(int i = 1; i <= length2; i++)
            {
                for(int j = 1; j <= length1; j++)
                {
                    if(j < i)
                    {
                        matrix[i, j] = 0;
                    }
                    else
                    {
                        if(s[j - 1] == t[i - 1])
                        {
                            matrix[i, j] = matrix[i - 1, j - 1] + matrix[i , j - 1];
                        }
                        else
                        {
                            matrix[i, j] = matrix[i, j - 1];
                        }
                    }
                }
            }
            return matrix[length2, length1];
        }

        /// <summary>
        /// 给定一个字符串，无视大小写和符号，判断它是否是回文
        /// 回文-palindrome：顺序、逆序读都是一样的
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsPalindrome(string s)
        {
            int standardGap = Math.Abs('a' - 'A');
            int front = 0;
            int back = s.Length - 1;
            while (true)
            {
                if (front >= back)
                {
                    break;
                }
                if (s[front] < 'a' && s[front] > 'Z' || s[front] > 'z' || s[front] < 'A')
                {
                    front++;
                }
                else if (s[back] < 'a' && s[back] > 'Z' || s[back] > 'z' || s[back] < 'A')
                {
                    back--;
                }
                else
                {
                    int gap = Math.Abs(s[front] - s[back]);
                    if (gap != 0 && gap != standardGap)
                        return false;
                    front++;
                    back--;
                }
            }
            return true;
        }

        /// <summary>
        /// 给定一个起始单词、结束单词和一个单词列表
        /// 返回从起始单词到结束单词的最短变化列表，约束：每次变化只能变化一个字母，并且变化后的单词出现在单词列表中
        /// 假设：给定的所有单词都是等长、不为空、并且起始单词和结束单词不同
        /// </summary>
        /// <param name="beginWord"></param>
        /// <param name="endWord"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        public IList<IList<string>> FindLadders(string beginWord, string endWord, IList<string> wordList)
        {
            //利用队列实现一个广度优先搜索，存储到目标节点的最小路径值
            //首先创建一个邻接矩阵，存储每个单词的可达单词
            if (!wordList.Contains(endWord))
                return new List<IList<string>>();
            Dictionary<string, List<string>> neighbour = new Dictionary<string, List<string>>();
            Dictionary<string, int> pathNumber = new Dictionary<string, int>();
            Dictionary<int, List<string>> pathes = new Dictionary<int, List<string>>();
            for (int i = 0; i < wordList.Count; i++)
            {
                neighbour.Add(wordList[i], new List<string>());
                pathNumber.Add(wordList[i], -1);
            }
            for(int i = 0; i < wordList.Count - 1; i++)
            {
                for(int j = i + 1; j < wordList.Count; j++)
                {
                    if(StepOne(wordList[i], wordList[j]))
                    {
                        neighbour[wordList[i]].Add(wordList[j]);
                        neighbour[wordList[j]].Add(wordList[i]);
                    }
                }
            }
            if (!wordList.Contains(beginWord))
            {
                pathNumber.Add(beginWord, 0);
                neighbour.Add(beginWord, new List<string>());
                for (int i = 0; i < wordList.Count; i++)
                {
                    if(StepOne(wordList[i], beginWord))
                    {
                        neighbour[beginWord].Add(wordList[i]);
                    }
                }
            }
            pathNumber[beginWord] = 0;
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(beginWord);
            while(queue.Count > 0)
            {
                string current = queue.Dequeue();
                foreach(string s in neighbour[current])
                {
                    if(pathNumber[s] == -1)
                        queue.Enqueue(s);
                    int steps = pathNumber[current] + 1;
                    if (pathNumber[s] == -1)
                        pathNumber[s] = steps;
                    else
                        pathNumber[s] = steps < pathNumber[s] ? steps : pathNumber[s];
                }
            }
            IList<IList<string>> res = new List<IList<string>>();
            List<string> constructor = new List<string>();
            constructor.Add(beginWord);
            SetPathes(pathes, pathNumber, wordList, beginWord);
            DoRecursionByFindLadders(res, constructor, 1, pathNumber[endWord], pathes, beginWord, endWord, neighbour);
            return res;
        }
        private void DoRecursionByFindLadders(IList<IList<string>> res, List<string> constructor, 
            int step, int minSteps, Dictionary<int, List<string>> pathes, string beginWord, string endWord,
            Dictionary<string, List<string>> neighbour)
        {
            if (step > minSteps)
                return;
            if (step == minSteps && pathes[step].Contains(endWord) && neighbour[beginWord].Contains(endWord))
            {
                constructor.Add(endWord);
                res.Add(new List<string>(constructor));
                constructor.RemoveAt(constructor.Count - 1);
            }
            else if(step < minSteps)
            {
                foreach(string s in pathes[step])
                {
                    if (!neighbour[beginWord].Contains(s))
                        continue;
                    constructor.Add(s);
                    DoRecursionByFindLadders(res, constructor, step + 1, minSteps,
                        pathes, s, endWord, neighbour);
                    constructor.RemoveAt(constructor.Count - 1);
                }
            }
        }
        private void SetPathes(Dictionary<int, List<string>> pathes, Dictionary<string, int> pathNumber,
            IList<string> wordList, string beginWord)
        {
            for(int i = 0; i < wordList.Count; i++)
            {
                int path = pathNumber[wordList[i]];
                if (!pathes.ContainsKey(path))
                    pathes.Add(path, new List<string>());
                pathes[path].Add(wordList[i]);
            }
            if (!wordList.Contains(beginWord))
            {
                if (!pathes.ContainsKey(pathNumber[beginWord]))
                    pathes.Add(pathNumber[beginWord], new List<string>());
                pathes[pathNumber[beginWord]].Add(beginWord);
            }
        }
        private bool StepOne(string str1, string str2)
        {
            int step = 0;
            for(int k = 0; k < str1.Length; k++)
            {
                if (str1[k] != str2[k])
                    step++;
            }
            return step == 1;
        }

        /// <summary>
        /// 给定一个字符串，要求对该字符串进行分割，分割之后的所有子串都是回文串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IList<IList<string>> PartitionPalindrome(string s)
        {
            IList<IList<string>> res = new List<IList<string>>();
            List<string> contributer = new List<string>();
            if(s == "")
            {
                return res;
            }
            //考虑使用暴递归法
            DoRecursionByPartitionPalindrome(res, s, contributer, 0, s.Length - 1);
            return res;
        }
        private void DoRecursionByPartitionPalindrome(IList<IList<string>> res, string s, List<string> contributer, int start, int end)
        {
            int index_left = start;
            int index_right = end;
            if(index_left > index_right)
            {
                res.Add(new List<string>(contributer));
                return;
            }
            while (index_left <= index_right)
            {
                bool isPalindrome = true;
                for (int i = index_left, j = index_right; i < j; j--, i++)
                {
                    if(s[i] != s[j])
                    {
                        isPalindrome = false;
                        break;
                    }
                }

                if (isPalindrome)
                {
                    contributer.Add(s.Substring(index_left, index_right - index_left + 1));
                    DoRecursionByPartitionPalindrome(res, s, contributer, index_right + 1, end);
                    contributer.RemoveAt(contributer.Count - 1);
                }
                index_right--;
            }
        }
        /// <summary>
        /// 给定一个字符串，要求返回最小的切割数，使得所有子串是回文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int MinCutToSubPalindrome(string s)
        {
            //有可能本身就是回文串，这样只需要0刀
            //利用上述算法，求出所有，扫描最小，O(n^3)会超时
            //考虑首先进行顺序遍历获取能够获取到的所有回文的cut
            //对结果进行扫描，约束是所有cut的长度和为s.length并且首位相接,O(n^2),字典爆炸
            //动态规划依次从长度为1到n
            if (s == "" || IsPalindrome2(s))
                return 0;
            for(int i = 1; i <= s.Length; i++)
            {
                if(IsPalindrome2(s.Substring(0, i)) && IsPalindrome2(s.Substring(i, s.Length - i)))
                {
                    return 1;
                }
            }
            int[,] matrix = new int[s.Length, s.Length];
            for(int l = 1; l <= s.Length; l++)
            {
                //每l长度判断，从1长开始包括自身的切分
                for(int i = 0; i < s.Length - l + 1; i++)
                {
                    int j = i + l - 1;
                    matrix[i, j] = int.MaxValue;//先赋值最大，断言一个串的切分长度必定有长度，假设极限不超过int.max
                    if(IsPalindrome2(s.Substring(i, l))) { 
                        matrix[i, j] = 0;
                    }
                    for(int k = i; k < j; k++)
                    {
                        matrix[i, j] = Math.Min(matrix[i, j], 1 + matrix[i, k] + matrix[k + 1, j]);
                    }
                }
            }
            return matrix[0,s.Length - 1];
        }
        private bool IsPalindrome2(string s)
        {
            for(int i = 0, j = s.Length - 1; i <= j; i++, j--)
            {
                if(s[i] != s[j])
                {
                    return false;
                }
            }
            return true;
        }
        public int MinCutBestSolution(string s)
        {
            //表示是否是回文的矩阵，ij表示从i到j
            bool[,] dp = new bool[s.Length, s.Length];
            //cut记录的是形成的回文子串的数量，长度为s.length + 1，其中最后一位是保留的0个回文子串；其余用作计算
            int[] cut = new int[s.Length + 1];
            for (int i = s.Length - 1; i >= 0; i--)
            {
                cut[i] = int.MaxValue;
                for (int j = i; j < s.Length; j++)
                {
                    if (s[i] == s[j] && (j - i <= 1 || dp[i + 1, j - 1]))
                    {
                        dp[i, j] = true;
                        //如果形成回文串，那么赋值为该回文情况下，包含其他回文的总数量，为最小值
                        cut[i] = Math.Min(cut[i], 1 + cut[j + 1]);
                    }
                }
            }
            return cut[0] - 1;
        }

    }
}
