using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Permutation1
    {
        public IList<IList<int>> Permute(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            Dictionary<int, bool> hasDict = new Dictionary<int, bool>();
            int[] resTmp = new int[nums.Length];
            int position = 0;
            DoTraceBack(resTmp, position, nums, res, hasDict);
            return res;
        }

        private void DoTraceBack(int[] resTmp, int position, int[] nums, IList<IList<int>> res, Dictionary<int, bool> hasDict)
        {
            if(position == nums.Length)
            {
                List<int> tmplist = new List<int>();
                for (int i = 0; i < resTmp.Length; i++)
                    tmplist.Add(resTmp[i]);
                res.Add(tmplist);
            }
            else
            {
                for(int i = 0; i < nums.Length; i++)
                {
                    if (hasDict.ContainsKey(nums[i]))
                        continue;
                    else
                    {
                        resTmp[position] = nums[i];
                        hasDict.Add(nums[i], true);
                        DoTraceBack(resTmp, position + 1, nums, res, hasDict);
                        hasDict.Remove(nums[i]);
                    }
                }
            }
        }

        public IList<IList<int>> PermuteUnique(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            int[] resTmp = new int[nums.Length];
            bool[] has = new bool[nums.Length];
            int position = 0;
            Array.Sort(nums);
            DoTraceBack2(resTmp, position, nums, res, has);
            return res;
        }
        private void DoTraceBack2(int[] resTmp, int position, int[] nums, IList<IList<int>> res, bool[] has)
        {
            if (position == nums.Length)
            {
                List<int> tmplist = new List<int>();
                for (int i = 0; i < resTmp.Length; i++)
                    tmplist.Add(resTmp[i]);
                res.Add(tmplist);
            }
            else
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    if (has[i])
                        continue;
                    else
                    {
                        resTmp[position] = nums[i];
                        has[i] = true;
                        DoTraceBack2(resTmp, position + 1, nums, res, has);
                        has[i] = false;
                        while (i + 1 < nums.Length && nums[i] == nums[i + 1])
                            i++;
                    }
                }
            }
        }
    }
}
