using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SubSet
    {
        /// <summary>
        /// 给定一个可能带有重复数值的数组，返回所有可能的子集
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public IList<IList<int>> SubsetsWithDup(int[] nums)
        {
            IList<IList<int>> res = new List<IList<int>>();
            Array.Sort(nums);
            res.Add(new List<int>());
            res.Add(new List<int>()
            {
                nums[0]
            });
            if (nums.Length < 2)
                return res;
            int pre_index = 1;
            int pre_num = nums[0];
            for(int i = 1; i < nums.Length; i++)
            {
                int length = res.Count;
                int j = nums[i] == pre_num ? pre_index : 0;
                for (; j < length; j++)
                {
                    List<int> newone = new List<int>();
                    newone.AddRange(res[j]);
                    newone.Add(nums[i]);
                    res.Add(newone);
                }
                pre_num = nums[i];
                pre_index = length;
            }
            return res;
        }
    }
}
