using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Combinations
    {

        //too slow, change the method
        public IList<IList<int>> Solution(int[] candidates, int target)
        {
            IList<IList<int>> res = new List<IList<int>>();
            Array.Sort(candidates);
            int length = candidates.Length;
            int[] availables = new int[length];
            int[] reses = new int[length];
            for (int i = 0; i < length; i++)
            {
                availables[i] = target / candidates[i];
            }
            reses[0] = availables[0];
            while (reses[length - 1] <= availables[length - 1])
            {
                int count = 0;
                for (int i = 0; i < length; i++)
                {
                    count += candidates[i] * reses[i];
                    if (count > target)
                        break;
                }
                if (count == target)
                {
                    List<int> tmp = new List<int>();
                    for (int i = 0; i < length; i++)
                    {
                        for (int j = 0; j < reses[i]; j++)
                            tmp.Add(candidates[i]);
                    }
                    res.Add(tmp);
                }
                reses[0]++;
                for (int i = 0; i < length - 1; i++)
                {
                    if (reses[i] > availables[i])
                    {
                        reses[i] = 0;
                        reses[i + 1]++;
                    }
                }
            }
            return res;
        }

        public IList<IList<int>> Solution2(int[] candidates, int target)
        {
            IList<IList<int>> res = new List<IList<int>>();
            Array.Sort(candidates);
            List<int> tmpRes = new List<int>();
            int sum = 0;
            SolveRecord(candidates, target, sum, tmpRes, res, 0);
            return res;
        }

        private void SolveRecord(int[] candidates, int target, int sum, List<int> tmpRes, IList<IList<int>> res,
            int index)
        {
            for(int i = index; i < candidates.Length; i++)
            {
                sum += candidates[i];
                if(sum < target)
                {
                    tmpRes.Add(candidates[i]);
                    SolveRecord(candidates, target, sum, tmpRes, res, i);
                    tmpRes.Remove(candidates[i]);
                    sum -= candidates[i];
                }
                else if(sum == target)
                {
                    tmpRes.Add(candidates[i]);
                    res.Add(new List<int>(tmpRes));
                    tmpRes.Remove(candidates[i]);
                    return;
                }
                else
                    return;
            }
        }

        //Combination Sums two↓
        public IList<IList<int>> CombinationSum2(int[] candidates, int target)
        {
            IList<IList<int>> res = new List<IList<int>>();
            Array.Sort(candidates);
            List<int> tmpRes = new List<int>();
            int sum = 0;
            SolveRecord2(candidates, target, sum, tmpRes, res, 0);
            return res;
        }
        private void SolveRecord2(int[] candidates, int target, int sum, List<int> tmpRes, IList<IList<int>> res,
            int index)
        {
            for (int i = index; i < candidates.Length; )
            {
                sum += candidates[i];
                if (sum < target)
                {
                    tmpRes.Add(candidates[i]);
                    SolveRecord2(candidates, target, sum, tmpRes, res, i + 1);
                    tmpRes.Remove(candidates[i]);
                    sum -= candidates[i];
                }
                else if (sum == target)
                {
                    tmpRes.Add(candidates[i]);
                    res.Add(new List<int>(tmpRes));
                    tmpRes.Remove(candidates[i]);
                    return;
                }
                else
                    return;
                while (i + 1 < candidates.Length && candidates[i] == candidates[i + 1])
                    i++;
                i++;
            }
        }

        /// <summary>
        /// 使用数字1~n，组成k个数字的所有可能组合
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public IList<IList<int>> Combine(int n, int k)
        {
            IList<IList<int>> res = new List<IList<int>>();
            int[] tmp = new int[k];
            DoRecursion(0, k, 1, n, res, tmp);
            return res;
        }

        private void DoRecursion(int nowK, int k, int nowN, int n, IList<IList<int>> res, int[] tmp)
        {
            if(nowK == k)
            {
                List<int> tmpL = new List<int>();
                for (int i = 0; i < k; i++)
                    tmpL.Add(tmp[i]);
                res.Add(tmpL);
            }
            else
            {
                for(int i = nowN; i <= n - k + nowK + 1; i++)
                {
                    tmp[nowK] = i;
                    DoRecursion(nowK + 1, k, i + 1, n, res, tmp);
                }
            }
        }

        /// <summary>
        /// 返回所有传入集合的子集，保证了传入集合不包含重复元素
        /// 借鉴上题recursion算法
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public IList<IList<int>> Subsets(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            res.Add(new List<int>());//必然包含空集
            if(nums.Length == 0)
                return res;
            for(int i = 1; i <= nums.Length; i++)
            {
                //子集长度从1~n
                int[] tmp = new int[i];
                DoRecursionBySubSet(0, i, 0, nums.Length, res, tmp, nums);
            }
            return res;
        }

        private void DoRecursionBySubSet(int v1, int k, int v2, int length, IList<IList<int>> res, int[] tmp, int[] nums)
        {
            if(v1 == k)
            {
                List<int> tmpL = new List<int>();
                for (int i = 0; i < k; i++)
                    tmpL.Add(tmp[i]);
                res.Add(tmpL);
            }
            else
            {
                for(int i = v2; i <= length - k + v1; i++)
                {
                    tmp[v1] = nums[i];
                    DoRecursionBySubSet(v1 + 1, k, i + 1, length, res, tmp, nums);
                }
            }
        }

        public IList<IList<int>> SubsetsBestSolution(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            res.Add(new List<int>());//必然包含空集
            for(int i = 0; i < nums.Length; i++)
            {
                int nowResLength = res.Count;
                for(int j = 0; j < nowResLength; j++)
                {
                    List<int> tmp = new List<int>();
                    tmp.AddRange(res[j]);
                    tmp.Add(nums[i]);
                    res.Add(tmp);
                }
            }
            return res;
        }
    }
}
